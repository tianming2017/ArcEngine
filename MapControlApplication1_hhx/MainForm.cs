using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;

using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.DataSourcesGDB;

using ESRI.ArcGIS.Analyst3D;

using System.Collections.Generic;


namespace MapControlApplication1_hhx
{
    public sealed partial class MainForm : Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        private IWorkspace workspace = null;        
        private ILayer TOCRightLayer = null;    //用于存储TOC右键选中图层
        private Color m_FromColor = Color.Red;  //初始化色块颜色（左）
        private Color m_ToColor = Color.Blue;   //初始化色块颜色（右）
        private bool filterBtnFirstClick = true;//自定义  用来标记清空文件夹的
        private bool fClip = false;             //记录是否处于裁剪状态
        private bool fExtraction = false;       //记录是否处于Extraction裁剪状态
        private bool fLineOfSight = false;      //记录是否处于通视分析状态
        private bool fVisibility = false;       //记录是否处于视域分析状态
        private bool fTIN = false;              //记录是否处于TIN构建状态
        private ITinEdit TinEdit = null;

        #endregion

        #region class constructor
        public MainForm()
        {
            InitializeComponent();
            //色带初始化
            m_FromColor = Color.Red;    //初始化色块颜色（左）
            m_ToColor = Color.Blue;     //初始化色块颜色（右）
            //！！！下面这句话以后取消注释！！！
            RefreshColors(m_FromColor,m_ToColor);

        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;

            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;
        }

        #region Main Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            //execute New Document command
            ICommand command = new CreateNewDocument();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            //execute Open Document command
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            //execute Save Document command
            if (m_mapControl.CheckMxFile(m_mapDocumentName))
            {
                //create a new instance of a MapDocument
                IMapDocument mapDoc = new MapDocumentClass();
                mapDoc.Open(m_mapDocumentName, string.Empty);

                //Make sure that the MapDocument is not readonly
                if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                {
                    MessageBox.Show("Map document is read only!");
                    mapDoc.Close();
                    return;
                }

                //Replace its contents with the current map
                mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                //save the MapDocument in order to persist it
                mapDoc.Save(mapDoc.UsesRelativePaths, false);

                //close the MapDocument
                mapDoc.Close();
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            //execute SaveAs Document command
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(m_mapControl.Object);
            command.OnClick();
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        #region map event handlers
        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            //get the current document name from the MapControl
            m_mapDocumentName = m_mapControl.DocumentFilename;

            //if there is no MapDocument, diable the Save menu and clear the statusbar
            if (m_mapDocumentName == string.Empty)
            {
                menuSaveDoc.Enabled = false;
                statusBarXY.Text = string.Empty;
            }
            else
            {
                //enable the Save manu and write the doc name to the statusbar
                menuSaveDoc.Enabled = true;
                statusBarXY.Text = System.IO.Path.GetFileName(m_mapDocumentName);
            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            try
            {
                //如果是处于裁剪状态
                if (fClip)
                {
                    //获取屏幕显示 有关的 接口对象（与ActiveView有关）
                    IScreenDisplay screenDisplay = axMapControl1.ActiveView.ScreenDisplay;
                    //rubberBand橡皮筋接口 用来绘制图形的
                    IRubberBand rubberBand = new RubberEnvelopeClass();
                    //获取绘制的几何图形IGeometry接口,应该是下面这句话 产生了自定义的选框
                    IGeometry geometry = rubberBand.TrackNew(screenDisplay, null);
                    //IEnvelope接口获取几何图形的 包络范围矩形
                    IEnvelope cutEnv = null;
                    cutEnv = geometry.Envelope;
                    //获取选中的图层,进行裁剪操作
                    int layerIndex = cmb_TransformLayer.SelectedIndex;
                    ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                    IRasterLayer rasterLayer = layer as IRasterLayer;
                    IRaster raster = rasterLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;

                    //创建栅格图像变换操作接口的对象
                    ITransformationOp transop = new RasterTransformationOpClass();
                    //定义输出地理数据集的对象
                    IGeoDataset outdataset = null;
                    outdataset = transop.Clip(geoDataset, cutEnv);

                    IRasterDataset rasterDataset = outdataset as IRasterDataset;
                    IRasterLayer resRasterLayer = new RasterLayerClass();
                    resRasterLayer.CreateFromDataset(rasterDataset);
                    ILayer resLayer = resRasterLayer as ILayer;
                    resLayer.Name = "裁剪Clip";
                    axMapControl1.Map.AddLayer(resLayer);
                    axMapControl1.ActiveView.Extent = resLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                    MessageBox.Show("图像变换成功！", "提示");

                    fClip = false;
                }
                else if (fExtraction)//2017/11/23 裁剪出来的图层亮度会改变 是因为 针对裁剪区域中的高程值 重新拉伸0-255
                {
                    //获取选择的图层栅格数据
                    int indexLayer = cmb_ExtractionRasterLayer.SelectedIndex;
                    ILayer layer = axMapControl1.Map.get_Layer(indexLayer);
                    //获取栅格图层的栅格对象
                    if (layer is IRasterLayer)
                    {
                        //获取选中的栅格图层 及其 数据
                        IRasterLayer rstLayer = layer as IRasterLayer;
                        IRaster raster = rstLayer.Raster;
                        IGeoDataset pGeoDataset = raster as IGeoDataset;
                        //鼠标点击屏幕绘制多边形
                        //[震惊] 一句话实现polygon绘制
                        IPolygon pPolygon = axMapControl1.TrackPolygon() as IPolygon;
                        //创建栅格数据 裁剪分析 操作相关的 类对象
                        IExtractionOp pExtractionOp = new RasterExtractionOpClass();
                        //执行裁剪分析操作得到的结果数据集
                        IGeoDataset pGeoOutput = pExtractionOp.Polygon(pGeoDataset, pPolygon, true);

                        IRasterLayer resLayer = new RasterLayerClass();
                        resLayer.CreateFromRaster(pGeoOutput as IRaster);
                        resLayer.Name = "Extraction";
                        axMapControl1.Map.AddLayer(resLayer as ILayer);
                        axMapControl1.ActiveView.Refresh();
                        axTOCControl1.Update();

                        //更新 图层 和 波段 下拉框
                        iniCmbItems();
                    }
                    fExtraction = false;
                }
                else if (fLineOfSight)//2017/11/24 通视分析 ISurface getLineOfSight
                {

                    int layerIndex = cmb_LineOfSightLayer.SelectedIndex;
                    ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                    if (layer is IRasterLayer)
                    {
                        IRasterLayer pRasterLayer = layer as IRasterLayer;
                        //创建栅格表面分析的对象 并设置 处理栅格数据对象
                        IRasterSurface pRasterSurface = new RasterSurfaceClass();
                        pRasterSurface.PutRaster(pRasterLayer.Raster, 0);
                        //接口转换 ISurface
                        ISurface pSurface = pRasterSurface as ISurface;
                        //地图上跟踪绘制直线 得到几何对象
                        IPolyline pPolyline = axMapControl1.TrackLine() as IPolyline;
                        IPoint pPoint = null;//output
                        IPolyline pVPolyline = null;//output Visible
                        IPolyline pInPolyline = null;//output Invisible
                        //设置参数
                        object pRef = 0.13;
                        bool pBool = true;
                        //获取DEM的高程
                        double pZ1 = pSurface.GetElevation(pPolyline.FromPoint);
                        double PZ2 = pSurface.GetElevation(pPolyline.ToPoint);
                        //创建IPoint对象 赋值XYZ值
                        IPoint pPoint1 = new PointClass();
                        IPoint pPoint2 = new PointClass();
                        pPoint1.X = pPolyline.FromPoint.X;
                        pPoint1.Y = pPolyline.FromPoint.Y;
                        pPoint1.Z = pZ1;
                        pPoint2.X = pPolyline.ToPoint.X;
                        pPoint2.Y = pPolyline.ToPoint.Y;
                        pPoint2.Z = PZ2;
                        ////?我都傻了 为什么不直接拿来用呢? 因为这样拿到的Z值 应该是0吧?
                        ////pPoint1 = pPolyline.FromPoint;
                        ////pPoint2 = pPolyline.ToPoint;

                        //调用ISurface接口的getLineOfSight方法得到通视范围
                        pSurface.GetLineOfSight(pPoint1, pPoint2, out pPoint, out pVPolyline, out pInPolyline, out pBool, false, false, ref pRef);
                        //element has geometry? symbol?. symbol has color

                        //add by hhx 每次运行前先清空 上一次的track图形
                        axMapControl1.ActiveView.GraphicsContainer.DeleteAllElements();

                        if (pVPolyline != null)
                        {
                            //如果可视范围不为null 则进行渲染和显示
                            IElement pLineElementV = new LineElementClass();
                            pLineElementV.Geometry = pVPolyline;
                            ILineSymbol pLineSymbolV = new SimpleLineSymbolClass();
                            pLineSymbolV.Width = 2;
                            IRgbColor pColorV = new RgbColorClass();
                            pColorV.Green = 255;
                            pLineSymbolV.Color = pColorV;
                            ILineElement pLineV = pLineElementV as ILineElement;
                            pLineV.Symbol = pLineSymbolV;
                            axMapControl1.ActiveView.GraphicsContainer.AddElement(pLineElementV, 0);
                        }
                        //渲染和显示非可视范围直线
                        if (pInPolyline != null)
                        {
                            IRgbColor rgbColor = new RgbColorClass();
                            rgbColor.Red = 255;
                            ILineSymbol lineSymbol = new SimpleLineSymbolClass();
                            lineSymbol.Color = rgbColor;
                            lineSymbol.Width = 2;
                            ILineElement lineElement = new LineElementClass();
                            lineElement.Symbol = lineSymbol;
                            IElement element = lineElement as IElement;
                            element.Geometry = pInPolyline;

                            axMapControl1.ActiveView.GraphicsContainer.AddElement(element, 1);

                        }

                        //对视图范围进行局部刷新
                        axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

                    }
                    fLineOfSight = false;
                }
                else if (fVisibility)//视域分析
                {
                    int layerIndex = cmb_VisibilityLayer.SelectedIndex;
                    ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                    if (layer is IRasterLayer)
                    {
                        IRasterLayer rstLayer = layer as IRasterLayer;
                        IRaster raster = rstLayer.Raster;
                        IGeoDataset pGeoDataset = raster as IGeoDataset;
                        //工作空间 modeify by hhx 2017/11/24
                        //假如workspace是有SDEWorkspaceFactory 创建的。则它既可以转换成IRasterWorkspace,也可以转换成IFeatureWorkspace
                        //如果栅格图像是直接从文件中打开的,我是用RasterWorkspaceFactory创建的workspace
                        //已经尝试直接用workspace替换 [从文件打开]代码中的 myWorkspace,并运行下面
                        //发现接口转换失败.说明:RasterWorkspaceFactory创建的Workspace无法转换到IfeatureWorkspace
                        IFeatureWorkspace fcw = null;
                        if (workspace is IFeatureWorkspace)//true when connect DB before,workspace is create by SDEWorkspaceFactory
                        {
                            fcw = workspace as IFeatureWorkspace;//所以这句成立的前提是  SDE连接了Oracle数据库
                        }
                        else//load image from file directly,NOT connect DB,workspace = null
                        {
                            //后面的操作跟GDB有关,所以工厂 不能 是ShapefileWorkspaceFactoryClass
                            //FileGDBWorkspaceFactoryClass,AccessWorkspaceFactoryClass都可以
                            //.gdb是一个文件夹 .mdb是一个文件
                            //【注意!】open gdb or mdb 之前先创建它.....额
                            string outputFolder = @"d:\hhx\visibility";
                            string tempGDB = "temp.gdb";
                            //DeleteDir(outputFolder);
                            //如果workspace不存在 再来创建它 不要每次都删掉重新创建 ? AE中的ContainsWorkspace不符合我的要求
                            //直接用C# File 判断
                            if (!Directory.Exists(outputFolder + "\\" + tempGDB))
                            {
                                //使用Geoprocessor来创建地理数据库 参考mdb的创建
                                ESRI.ArcGIS.Geoprocessor.Geoprocessor geoProcessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();

                                CreateFileGDB createFileGDB = new CreateFileGDB();
                                createFileGDB.out_folder_path = outputFolder;
                                createFileGDB.out_name = tempGDB;
                                geoProcessor.Execute(createFileGDB, null);

                            }
                            IWorkspaceFactory tmpworkspaceFactory = new FileGDBWorkspaceFactoryClass();
                            //临时工作空间 会在此文件夹下面产生一个 临时文件 
                            IWorkspace tmpworkspace = tmpworkspaceFactory.OpenFromFile(outputFolder + "\\" + tempGDB, 0);//有了才可以打开

                            fcw = tmpworkspace as IFeatureWorkspace;//all work for here
                        }

                        //创建要素类的字段集合
                        IFields fields = new FieldsClass();
                        IFieldsEdit fieldsEdit = fields as IFieldsEdit;
                        //添加OID字段
                        IField oidField = new FieldClass();
                        IFieldEdit oidFieldEdit = oidField as IFieldEdit;
                        oidFieldEdit.Name_2 = "OID";
                        oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                        fieldsEdit.AddField(oidField);

                        //创建(定义)几何字段 IGeometryDefEdit has ISpatialReference
                        IGeometryDef geometryDef = new GeometryDefClass();
                        IGeometryDefEdit geometryDefEdit = geometryDef as IGeometryDefEdit;
                        geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;
                        ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                        ISpatialReference spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRProjCS_NAD1983UTM_20N);
                        ISpatialReferenceResolution spatialReferenceResolution = spatialReference as ISpatialReferenceResolution;//look here
                        spatialReferenceResolution.ConstructFromHorizon();
                        spatialReferenceResolution.SetDefaultXYResolution();
                        ISpatialReferenceTolerance spatialReferenceTolerance = spatialReference as ISpatialReferenceTolerance;//look here
                        spatialReferenceTolerance.SetDefaultXYTolerance();
                        geometryDefEdit.SpatialReference_2 = spatialReference;//geometryDefEdit has ISpatialReference

                        //添加几何字段
                        IField geometryField = new FieldClass();
                        IFieldEdit geometryFieldEdit = geometryField as IFieldEdit;
                        geometryFieldEdit.Name_2 = "Shape";
                        geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                        geometryFieldEdit.GeometryDef_2 = geometryDef;//geometryFieldEdit has IGeometryDef. notice type_2 
                        fieldsEdit.AddField(geometryField);

                        //创建name字段
                        IField nameField = new FieldClass();
                        IFieldEdit nameFieldEdit = nameField as IFieldEdit;
                        nameFieldEdit.Name_2 = "Name";
                        nameFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        nameFieldEdit.Length_2 = 20;
                        fieldsEdit.AddField(nameField);

                        //利用IFieldChecker 创建验证字段集合
                        IFieldChecker fieldChecker = new FieldCheckerClass();
                        IEnumFieldError enumFieldError = null;//output
                        IFields validatedFields = null;//output
                        fieldChecker.ValidateWorkspace = fcw as IWorkspace;//....
                        fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                        //创建要素类,在GDB或SDB中 没有后缀名
                        IFeatureClass featureClass = fcw.CreateFeatureClass("visibility_featureclass1363", validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");

                        //鼠标点击屏幕绘制点
                        IPoint pt;
                        pt = axMapControl1.ToMapPoint(e.x, e.y);
                        //创建要素
                        IFeature feature = featureClass.CreateFeature();
                        feature.Shape = pt;
                        //应用适当的子类型到要素中
                        ISubtypes subtypes = featureClass as ISubtypes;
                        IRowSubtypes rowSubtypes = feature as IRowSubtypes;
                        if (subtypes.HasSubtype)
                        {
                            rowSubtypes.SubtypeCode = 3;
                        }
                        //初始化要素的所有默认设置
                        rowSubtypes.InitDefaultValues();
                        //实现保存
                        feature.Store();

                        //IFeatureClass 转换 IGeodataset
                        IGeoDataset geoDataset = featureClass as IGeoDataset;
                        //创建栅格数据 表面分析操作相关的 类对象
                        ISurfaceOp surfaceOp = new RasterSurfaceOpClass();
                        //执行视域分析操作 得到 结果数据集
                        IGeoDataset pGeoOutput = surfaceOp.Visibility(pGeoDataset, geoDataset, esriGeoAnalysisVisibilityEnum.esriGeoAnalysisVisibilityObservers);
                        //[震惊!]为了得到用户在屏幕上 点了一个 点的geoDataset 写了这么多代码?有其他方法吗?
                        //删除刚刚创建的要素类
                        IDataset dataset = featureClass as IDataset;
                        dataset.Delete();

                        //加载和显示结果
                        IRasterLayer resRasterLayer = new RasterLayerClass();
                        resRasterLayer.CreateFromRaster(pGeoOutput as IRaster);
                        axMapControl1.Map.AddLayer(resRasterLayer as ILayer);
                        axMapControl1.ActiveView.Refresh();
                        axTOCControl1.Update();

                        iniCmbItems();
                    }
                    fVisibility = false;
                }//end else if
                else if (fTIN)
                {
                    int layerIndex = cmb_CreateTinLayer.SelectedIndex;
                    ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                    if (layer is IRasterLayer)
                    {
                        IRasterLayer pRasterLayer = layer as IRasterLayer;
                        //为构建TIN 添加point节点
                        IPoint point = new PointClass();
                        point = axMapControl1.ToMapPoint(e.x, e.y);
                        //创建IRasterSurface对象 用putRaster方法设置处理对象 转换ISurface接口 用getElevation方法获取point点高程值,将高程值赋值给point的Z属性
                        IRasterSurface rasterSurface = new RasterSurfaceClass();
                        rasterSurface.PutRaster(pRasterLayer.Raster, 0);
                        ISurface surface = rasterSurface as ISurface;
                        double pointZ = surface.GetElevation(point);
                        point.Z = pointZ;
                        //添加point到TINEdit中
                        TinEdit.AddPointZ(point, 0);

                        //高亮画点
                        //获取map和activeview
                        IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
                        IActiveView pActiveView = pGra as IActiveView;
                        //创建和设置element
                        IMarkerElement pMarkerElement = new MarkerElementClass();
                        //创建和设置symbol
                        IMarkerSymbol pMarkSym = new SimpleMarkerSymbolClass();
                        IRgbColor rgbColor = new RgbColorClass();
                        //设置rgbColor的red green blue属性值
                        //将rgbcolor赋值给pmarkerSymol的Color属性值
                        //设置pmarkerSym的size属性值
                        //将pmarkSym赋值给pMarkerElemnt的symbol属性
                        rgbColor.Red = 255;
                        rgbColor.Green = 255;
                        rgbColor.Blue = 0;
                        pMarkSym.Color = rgbColor;//yellow
                        pMarkSym.Size = 2;
                        pMarkerElement.Symbol = pMarkSym;
                        //这里的pMarkerElement 有点像renderer

                        //设置geometry
                        IElement pElement;
                        pElement = pMarkerElement as IElement;
                        pElement.Geometry = point;
                        //添加点marker 刷新activeView
                        pGra.AddElement(pElement, 0);
                        pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                    }
                }//end else if
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //mapcontrol的双击事件 表示结束构建TIN的操作
        private void axMapControl1_OnDoubleClick(object sender, IMapControlEvents2_OnDoubleClickEvent e)
        {
            try
            {
                if (fTIN == true)
                {
                    //还原tin标记变量的值
                    fTIN = false;
                    //创建TIN图层对象 设置name属性
                    ITinLayer tinLayer = new TinLayerClass();
                    tinLayer.Name = "TIN";
                    //设置dataset为TINedit对象
                    tinLayer.Dataset = TinEdit as ITin;
                    //添加图层 刷新视图显示
                    axMapControl1.Map.AddLayer(tinLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    //更新combobox的图层列表
                    iniCmbItems();
                    //我觉得画完就把点清除掉比较好
                    //获取map, 清除所有的element的标记marker
                    IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
                    pGra.DeleteAllElements();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        //连接Oracle数据库
        private void menuConnectDB_Click(object sender, EventArgs e)
        {
            //SDE连接数据库参数设置
            IPropertySet propertySet = new PropertySetClass();
            propertySet.SetProperty("SERVER", "localhost");
            propertySet.SetProperty("INSTANCE", "sde:oracle11g:localhost/orcl");//主机名或端口SC-201705151353
            propertySet.SetProperty("DATABASE", "sde1363");
            propertySet.SetProperty("USER","sde");
            propertySet.SetProperty("PASSWORD", "oracle");
            propertySet.SetProperty("VERSION", "sde.DEFAULT");
            propertySet.SetProperty("AUTHENTICATION_MODE", "DBMS");

            //指定SDE工作空间工厂
            Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.SdeWorkspaceFactory");
            IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            //根据SDE连接参数设置打开SDE工作空间
            workspace = workspaceFactory.Open(propertySet, 0);

            //清除栅格目录下拉框里面的选项
            cmb_LoadRstCatalog.Items.Clear();
            cmb_LoadRstCatalog.Items.Add("非栅格目录（工作空间）");
            cmb_ImportRstCatalog.Items.Clear();
            cmb_ImportRstCatalog.Items.Add("非栅格目录（工作空间）");
            //20171117 新增
            cmb_MosaicRstCatalog.Items.Clear();
            cmb_MosaicRstCatalog.Items.Add("临时栅格目录");



            //获取数据库中的栅格目录，去除SDE前缀
            IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
            IDatasetName datasetName = enumDatasetName.Next();
            while (datasetName != null)
            {
                cmb_LoadRstCatalog.Items.Add(datasetName.Name.Substring(datasetName.Name.LastIndexOf('.') + 1));
                cmb_ImportRstCatalog.Items.Add(datasetName.Name.Substring(datasetName.Name.LastIndexOf('.') + 1));
                //2017 11 17 add
                cmb_MosaicRstCatalog.Items.Add(datasetName.Name.Substring(datasetName.Name.LastIndexOf('.') + 1));
                datasetName = enumDatasetName.Next();
            }
            //设置下拉框默认选项为 非栅格目录（工作空间）
            if (cmb_LoadRstCatalog.Items.Count > 0) { cmb_LoadRstCatalog.SelectedIndex = 0; }
            if (cmb_ImportRstCatalog.Items.Count > 0) { cmb_ImportRstCatalog.SelectedIndex = 0; }
            //2017 11 17 add
            if (cmb_MosaicRstCatalog.Items.Count > 0) { cmb_MosaicRstCatalog.SelectedIndex = 0; }

        }

        //从文件打开 栅格影像 并且 显示出来的代码
        //2017/11/25 modify
        //RasterWorkspaceFactoryClass确实可以打开影像 而且方便
        private void menuAccessFile_Click(object sender, EventArgs e)
        {
            #region use RasterWorkspaceFactoryClass
            //打开文件选择对话框，设置对话框属性
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imag file(*.img)|*.img|Tiff file(*.tif)|*.tif|Flt file|*.flt|All(*.*)|*.*";
            openFileDialog.Title = "打开影像数据";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                FileInfo fileinfo = new FileInfo(fileName);
                //打开工作空间 用IRasterWorkspace 的方法打开数据集.依据这个数据集生成图层.
                IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                IWorkspace myworkspace = workspaceFactory.OpenFromFile(fileinfo.DirectoryName, 0);
                if (myworkspace is IRasterWorkspace)
                {
                    IRasterWorkspace myRasterWorkspace = myworkspace as IRasterWorkspace;
                    IRasterDataset myRasterDataset = myRasterWorkspace.OpenRasterDataset(fileinfo.Name);

                    IRasterLayer myRasterLayer = new RasterLayerClass();
                    myRasterLayer.CreateFromDataset(myRasterDataset);
                    ILayer layer = myRasterLayer as ILayer;
                    axMapControl1.Map.AddLayer(layer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                }
                else
                {
                    MessageBox.Show("打不开此文件!");
                }
            }
            #endregion

        }

        //当选择的栅格目录发生变化，则相应的栅格图像列表也发生变化
        private void cmb_LoadRstCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh_cmb_LoadRstDataset();
            #region obsolete code
            //string rstCatalogName = cmb_LoadRstCatalog.SelectedItem.ToString();
            //IEnumDatasetName enumDatasetName = null;
            //IDatasetName datasetName = null;
            //if (cmb_LoadRstCatalog.SelectedIndex == 0)
            //{
            //    //清除栅格图像下拉框里面的选项
            //    cmb_LoadRstDataset.Items.Clear();
            //    //获取非栅格目录（工作空间）中的栅格图像
            //    enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            //    datasetName = enumDatasetName.Next();
            //    while (datasetName != null)
            //    {
            //        cmb_LoadRstDataset.Items.Add(datasetName.Name.Substring(datasetName.Name.LastIndexOf('.') + 1));
            //        datasetName = enumDatasetName.Next();
            //    }
            //    //设置下拉框 默认选中 第一个
            //    if (cmb_LoadRstDataset.Items.Count > 0) 
            //    { 
            //        cmb_LoadRstDataset.SelectedIndex = 0; 
            //    }
            //}
            //else
            //{ 
            //    //接口转换IRasterWorkspaceEx
            //    IRasterWorkspaceEx workspaceEx = (IRasterWorkspaceEx)workspace;
            //    //获取栅格目录
            //    IRasterCatalog rasterCatalog = workspaceEx.OpenRasterCatalog(rstCatalogName);
            //    //接口转换IFeatureClass
            //    IFeatureClass featureClass = (IFeatureClass)rasterCatalog;
            //    //接口转换ITable
            //    ITable pTable = featureClass as ITable;
            //    //执行查询获取指针
            //    ICursor cursor = pTable.Search(null, true) as ICursor;
            //    IRow pRow = null;
            //    //清除下拉框的选项
            //    cmb_LoadRstDataset.Items.Clear();
            //    cmb_LoadRstDataset.Text = "";
            //    //循环遍历读取每一条记录
            //    while ((pRow = cursor.NextRow()) != null)
            //    {
            //        int idxName = pRow.Fields.FindField("NAME");
            //        cmb_LoadRstDataset.Items.Add(pRow.get_Value(idxName).ToString());
            //    }
            //    //设置默认选项
            //    if (cmb_LoadRstDataset.Items.Count > 0)
            //    {
            //        cmb_LoadRstDataset.SelectedIndex = 0;               
            //    }
            //    //释放内存空间
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            //}
            #endregion
        }

        //自己新增
        private void Refresh_cmb_LoadRstDataset()
        {
            string rstCatalogName = cmb_LoadRstCatalog.SelectedItem.ToString();
            IEnumDatasetName enumDatasetName = null;
            IDatasetName datasetName = null;
            if (cmb_LoadRstCatalog.SelectedIndex == 0)
            {
                //清除栅格图像下拉框里面的选项
                cmb_LoadRstDataset.Items.Clear();
                //获取非栅格目录（工作空间）中的栅格图像
                enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
                datasetName = enumDatasetName.Next();
                while (datasetName != null)
                {
                    cmb_LoadRstDataset.Items.Add(datasetName.Name.Substring(datasetName.Name.LastIndexOf('.') + 1));
                    datasetName = enumDatasetName.Next();
                }
                //设置下拉框 默认选中 第一个
                if (cmb_LoadRstDataset.Items.Count > 0)
                {
                    cmb_LoadRstDataset.SelectedIndex = 0;
                }
            }
            else
            {
                //接口转换IRasterWorkspaceEx
                IRasterWorkspaceEx workspaceEx = (IRasterWorkspaceEx)workspace;
                //获取栅格目录
                IRasterCatalog rasterCatalog = workspaceEx.OpenRasterCatalog(rstCatalogName);
                //接口转换IFeatureClass
                IFeatureClass featureClass = (IFeatureClass)rasterCatalog;
                //接口转换ITable
                ITable pTable = featureClass as ITable;
                //执行查询获取指针
                ICursor cursor = pTable.Search(null, true) as ICursor;
                IRow pRow = null;
                //清除下拉框的选项
                cmb_LoadRstDataset.Items.Clear();
                cmb_LoadRstDataset.Text = "";
                //循环遍历读取每一条记录
                while ((pRow = cursor.NextRow()) != null)
                {
                    int idxName = pRow.Fields.FindField("NAME");
                    cmb_LoadRstDataset.Items.Add(pRow.get_Value(idxName).ToString());
                }
                //设置默认选项
                if (cmb_LoadRstDataset.Items.Count > 0)
                {
                    cmb_LoadRstDataset.SelectedIndex = 0;
                }
                //释放内存空间
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
            }
        }


        /// <summary>
        /// 【创建栅格目录】，默认采用WGS84投影
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_create_Click(object sender, EventArgs e)
        {
            if (txb_create.Text.Trim() == "")
            {
                MessageBox.Show("请输入栅格目录名称！");
            }
            else
            {
                string rasCatalogName = txb_create.Text.Trim();
                IRasterWorkspaceEx rasterWorkspaceEx = workspace as IRasterWorkspaceEx;
                //定义空间参考 采用WGS84投影
                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference spatialReference = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                spatialReference.SetDomain(-180, 180, -90, 90);
                //判断栅格目录是否存在
                IEnumDatasetName enumDatasetName = workspace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
                IDatasetName datasetName = enumDatasetName.Next();
                bool isExist = false;
                while (datasetName != null)
                {
                    if (datasetName.Name.Substring(datasetName.Name.LastIndexOf('.') + 1) == rasCatalogName)
                    {
                        isExist = true;
                        MessageBox.Show("栅格目录已经存在！");
                        txb_create.Focus();
                        return;           
                    }
                    datasetName = enumDatasetName.Next();
                }
                //若不存在，则创建新的栅格目录
                if (isExist == false)
                {
                    //创建栅格目录字段集
                    IFields fields = CreateFields("RASTER", "SHAPE", spatialReference, spatialReference);
                    rasterWorkspaceEx.CreateRasterCatalog(rasCatalogName, fields, "SHAPE","RASTER","DEFAULTS");
                    /////debug
                    //IRasterCatalog rasterCatalog = rasterWorkspaceEx.CreateRasterCatalog(rasCatalogName, fields, "SHAPE", "RASTER", "DEFAULTS");
                    //MessageBox.Show(rasterCatalog.RasterSpatialReference.Name.ToString());



                    //将新创建的栅格目录添加到下拉列表中，并设置为当前栅格目录
                    cmb_LoadRstCatalog.Items.Add(rasCatalogName);
                    cmb_LoadRstCatalog.SelectedIndex = cmb_LoadRstCatalog.Items.Count - 1;
                    cmb_ImportRstCatalog.Items.Add(rasCatalogName);
                    cmb_ImportRstCatalog.SelectedIndex = cmb_ImportRstCatalog.Items.Count - 1;
                    cmb_LoadRstDataset.Items.Clear();
                    cmb_LoadRstDataset.Text = "";
                }
                MessageBox.Show("栅格目录创建成功！");    
            }
        }

        /// <summary>
        /// 创建栅格目录所需的【字段集】
        /// </summary>
        /// <param name="rasterFIdName">Raster字段名称</param>
        /// <param name="shapeFIdName">Shape字段名称</param>
        /// <param name="rasterSF">Raster字段的空间参考</param>
        /// <param name="shapeSF">Shape字段的空间参考</param>
        /// <returns></returns>
        private IFields CreateFields(string rasterFIdName, string shapeFIdName, ISpatialReference rasterSF, ISpatialReference shapeSF)
        {
            IFields fields = new FieldsClass();
            IFieldsEdit fieldsEdit = fields as IFieldsEdit;

            IField field;
            IFieldEdit fieldEdit;

            //添加OID字段，注意字段type
            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "ObjectID";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            fieldsEdit.AddField(field);

            //添加name字段，注意字段type
            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "NAME";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            fieldsEdit.AddField(field);

            //添加raster字段，注意字段type
            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = rasterFIdName;
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;

            //IRasterDef接口定义栅格字段
            IRasterDef rasterDef = new RasterDefClass();
            rasterDef.SpatialReference = rasterSF;
            ((IFieldEdit2)fieldEdit).RasterDef = rasterDef;
            fieldsEdit.AddField(field);

            //添加shape字段，注意字段type
            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = shapeFIdName;
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            //IGeometryDef和IGeometryDefEdit接口定义和编辑几何字段
            IGeometryDef geometryDef = new GeometryDefClass();
            IGeometryDefEdit geometryDefEdit = geometryDef as IGeometryDefEdit;
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            geometryDefEdit.SpatialReference_2 = shapeSF;
            ((IFieldEdit2)fieldEdit).GeometryDef_2 = geometryDef;
            fieldsEdit.AddField(field);

            //添加xml（元数据）字段，注意字段type
            field = new FieldClass();
            fieldEdit = field as IFieldEdit;
            fieldEdit.Name_2 = "METADATA";
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeBlob;
            fieldsEdit.AddField(field);

            return fields;
        }

        //4 【加载】栅格图像
        //根据选定的栅格目录和栅格图像来加载相应的图像
        private void btn_load_Click(object sender, EventArgs e)
        {
            if (cmb_LoadRstCatalog.SelectedIndex == 0)
            {
                string rstDatasetName = cmb_LoadRstDataset.SelectedItem.ToString();
                //接口转换IRasterWorkspaceEx
                IRasterWorkspaceEx workspaceEx = (IRasterWorkspaceEx)workspace;
                //获取栅格数据集
                IRasterDataset rasterDataset = workspaceEx.OpenRasterDataset(rstDatasetName);
                //利用栅格目录项创建栅格图层
                IRasterLayer rasterLayer = new RasterLayerClass();
                rasterLayer.CreateFromDataset(rasterDataset);
                ILayer layer = rasterLayer as ILayer;

                //将图层加载到MapControl中，并缩放到当前图层
                axMapControl1.AddLayer(layer);
                axMapControl1.ActiveView.Extent = layer.AreaOfInterest;
                axMapControl1.ActiveView.Refresh();
                axTOCControl1.Update();
            }
            else
            {
                string rstCatalogName = cmb_LoadRstCatalog.SelectedItem.ToString();
                string rstDatasetName = cmb_LoadRstDataset.SelectedItem.ToString();
                //接口转换IRasterWorkspaceEx
                IRasterWorkspaceEx workspaceEx = (IRasterWorkspaceEx)workspace;
                //获取栅格目录
                IRasterCatalog rasterCatalog = workspaceEx.OpenRasterCatalog(rstCatalogName);
                //接口转换IFeatureClass
                IFeatureClass featureClass = (IFeatureClass)rasterCatalog;
                //接口转换ITable
                ITable ptable = featureClass as ITable;
                //查询条件过滤器QueryFilterClass
                IQueryFilter qf = new QueryFilterClass();
                qf.SubFields = "OBJECTID";
                qf.WhereClause = "NAME='" + rstDatasetName + "'";
                //执行查询获取指针
                ICursor cursor = ptable.Search(qf, true) as ICursor;
                IRow pRow = null;
                int rstOID = 0;
                //判断读取第一行记录
                if ((pRow = cursor.NextRow()) != null)
                {
                    int idxfId = pRow.Fields.FindField("OBJECTID");
                    rstOID = int.Parse(pRow.get_Value(idxfId).ToString());
                    //获取检索到的栅格目录项
                    IRasterCatalogItem rasterCatalogItem = (IRasterCatalogItem)featureClass.GetFeature(rstOID);
                    //利用栅格目录项创建栅格图层
                    IRasterLayer rasterLayer = new RasterLayerClass();
                    rasterLayer.CreateFromDataset(rasterCatalogItem.RasterDataset);
                    ILayer layer = rasterLayer as ILayer;

                    //将图层加载在MapControl中，并缩放到当前图层
                    axMapControl1.AddLayer(layer);
                    axMapControl1.ActiveView.Extent = layer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                }
                //释放内存空间
                System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);          
            }
            iniCmbItems();//////第2次课后增加
        }

        //点击栅格图像弹出【文件选择对话框】，选择要导入的栅格图像
        private void txb_importRstDataset_MouseDown(object sender, MouseEventArgs e)
        {
            //打开文件选择对话框，设置对话框属性
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imag file(*.img)|*.img|Tiff file(*.tif)|*.tif|Flt file|*.flt|All(*.*)|*.*";
            openFileDialog.Title = "打开影像数据";
            openFileDialog.Multiselect = false;
            string fileName = "";
            //如果对话框已成功选择文件，将文件路径信息填写到输入框内
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                txb_importRstDataset.Text = fileName;
            }
        }

        //点击导入，把选择的栅格图像【导入】工作空间或指定的栅格目录
        private void btn_import_Click(object sender, EventArgs e)
        {
            //获取栅格图像的路径和文件名字
            string fileName = txb_importRstDataset.Text;
            if (fileName == "")
            {
                MessageBox.Show("请先输入要导入的栅格图像");
                return;
            }
            FileInfo fileInfo = new FileInfo(fileName);
            string filePath = fileInfo.DirectoryName;
            string file = fileInfo.Name;
            string strOutName = file.Substring(0, file.LastIndexOf('.'));
            //根据路径和文件名字获取栅格数据集
            if (cmb_ImportRstCatalog.SelectedIndex == 0)
            {
                //判断是否有重名现象
                IWorkspace2 workspace2 = workspace as IWorkspace2;
                //如果名称已存在
                if (workspace2.get_NameExists(esriDatasetType.esriDTRasterDataset, strOutName))
                {
                    DialogResult result;
                    result = MessageBox.Show(this, "名为 " + strOutName + " 的栅格文件在数据库中已存在！" + "\n是否覆盖？", "相同文件名", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    //如果选择确认删除，则覆盖原栅格数据
                    if (result == DialogResult.Yes)
                    {
                        IRasterWorkspaceEx rstWorkspaceEx = workspace as IRasterWorkspaceEx;
                        IDataset datasetDel = rstWorkspaceEx.OpenRasterDataset(strOutName) as IDataset;
                        //调用IDataset接口的Delete接口实现已存在栅格数据集的删除
                        datasetDel.Delete();
                        datasetDel = null;
                    }
                    else if (result == DialogResult.No)
                    {
                        MessageBox.Show("工作空间已存在同名栅格数据集，不覆盖不能导入！");
                        return;
                    }
                }
                //根据选择的栅格图像的路径打开栅格工作空间
                IWorkspaceFactory rstWorkspaceFactoryImport = new RasterWorkspaceFactoryClass();
                IRasterWorkspace rstWorkspaceImport = (IRasterWorkspace)rstWorkspaceFactoryImport.OpenFromFile(filePath, 0);
                IRasterDataset rstDatasetImport = null;
                //检测选择文件的路径是不是有效的栅格工作空间
                if (!(rstWorkspaceImport is IRasterWorkspace))
                {
                    MessageBox.Show("文件路径不是有效的栅格工作空间");
                    return;
                }
                //根据选择的栅格图像的名字获取栅格数据集
                rstDatasetImport = rstWorkspaceImport.OpenRasterDataset(file);
                ///////debug///////////
                //IGeoDataset geoDataSet = rstDatasetImport as IGeoDataset;
                //ISpatialReference spatialReference = geoDataSet.SpatialReference;
                //MessageBox.Show(spatialReference.Name);
                //MessageBox.Show(((geoDataSet as IGeoDatasetSchemaEdit).CanAlterSpatialReference).ToString());



                //用IRasterDataset接口的CreateDefaultRaster方法创建空白的栅格对象
                IRaster raster = rstDatasetImport.CreateDefaultRaster();
                //IRasterProps是和栅格属性定义有关的接口
                IRasterProps rasterProp = raster as IRasterProps;

                //IRasterStorageDef接口和栅格储存参数有关
                IRasterStorageDef storageDef = new RasterStorageDefClass();
                //指定压缩类型
                storageDef.CompressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                //设置cellsize
                IPnt pnt = new PntClass();
                pnt.SetCoords(rasterProp.MeanCellSize().X, rasterProp.MeanCellSize().Y);
                storageDef.CellSize = pnt;
                //设置栅格数据集的原点，在最左上角的一点位置
                IPoint origin = new PointClass();
                origin.PutCoords(rasterProp.Extent.XMin, rasterProp.Extent.YMax);
                storageDef.Origin = origin;

                //接口转换为和栅格存储有关的ISaveAs2
                ISaveAs2 saveAs2 = (ISaveAs2)rstDatasetImport;

                //接口转换为和栅格存储属性定义有关的IRasterStorageDef2
                IRasterStorageDef2 rasterStorageDef2 = (IRasterStorageDef2)storageDef;
                //指定压缩质量，瓦片高度和宽度
                rasterStorageDef2.CompressionQuality = 100;
                rasterStorageDef2.Tiled = true;
                rasterStorageDef2.TileHeight = 128;
                rasterStorageDef2.TileWidth = 128;
                //最后调用ISaveAs2接口的SaveAsRasterDataset方法实现栅格数据的存储
                //指定存储名字，工作空间，存储格式和相关存储属性
                //关键！关键！关键！
                ///debug
                //MessageBox.Show((saveAs2.CanSaveAs("GRID")).ToString());
                saveAs2.SaveAsRasterDataset(strOutName, workspace, "GRID", rasterStorageDef2);

                //显示导入成功的消息
                MessageBox.Show("导入成功！");
            }
            else
            {
                //string rasterCatalogName = comboBox3.Text;
                string rasterCatalogName = cmb_ImportRstCatalog.SelectedItem.ToString();
                //打开栅格工作空间
                IWorkspaceFactory pRasterWsFac = new RasterWorkspaceFactoryClass();
                IWorkspace pWs = pRasterWsFac.OpenFromFile(filePath, 0);
                if (!(pWs is IRasterWorkspace))
                {
                    MessageBox.Show("文件路径不是有效的栅格工作空间");
                    return;
                }
                IRasterWorkspace pRasterWs = pWs as IRasterWorkspace;
                //获取栅格数据集
                IRasterDataset pRasterDs = pRasterWs.OpenRasterDataset(file);
                //创建栅格对象
                IRaster raster = pRasterDs.CreateDefaultRaster();
                IRasterProps rasterProp = raster as IRasterProps;

                //设置栅格储存参数
                IRasterStorageDef storageDef = new RasterStorageDefClass();
                storageDef.CompressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                //设置cellsize
                IPnt pnt = new PntClass();
                pnt.SetCoords(rasterProp.MeanCellSize().X, rasterProp.MeanCellSize().Y);
                storageDef.CellSize = pnt;
                //设置栅格数据集的原点，在最左上角一点的位置
                IPoint origin = new PointClass();
                origin.PutCoords(rasterProp.Extent.XMin, rasterProp.Extent.YMax);
                storageDef.Origin = origin;

                //在Raster Catalog添加栅格
                //打开对应的Raster Catalog
                IRasterCatalog pRasterCatalog = ((IRasterWorkspaceEx)workspace).OpenRasterCatalog(rasterCatalogName);
                //将需要导入的Raster Catalog转换成为Feature Class
                IFeatureClass pFeatureClass = (IFeatureClass)pRasterCatalog;
                //名字所在列的索引号
                int nameIndex = pRasterCatalog.NameFieldIndex;
                //栅格数据所在列的索引号
                int rasterIndex = pRasterCatalog.RasterFieldIndex;

                IFeatureBuffer pBuffer = null;
                IFeatureCursor pFeatureCursor = pFeatureClass.Insert(false);
                //创建IRasterValue接口的对象——IFeatureBuffer对象的rasterIndex需要使用
                IRasterValue pRasterValue = new RasterValueClass();
                //设置IRasterValue的RasterDataset
                pRasterValue.RasterDataset = (IRasterDataset)pRasterDs;
                //存储参数设定
                pRasterValue.RasterStorageDef = storageDef;
                pBuffer = pFeatureClass.CreateFeatureBuffer();
                //设置RasterBuffer对象的rasterIndex和nameIndex
                pBuffer.set_Value(rasterIndex, pRasterValue);
                pBuffer.set_Value(nameIndex, strOutName);
                //通过cursor实现feature的insert操作
                pFeatureCursor.InsertFeature(pBuffer);
                pFeatureCursor.Flush();
                //释放内存资源
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pBuffer);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pRasterValue);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                //显示成功信息
                MessageBox.Show("导入成功！");     
            }
            //自己新增，当导入成功后，加载框内容应该刷新吧
            int importIndex = this.cmb_ImportRstCatalog.SelectedIndex;
            this.cmb_LoadRstCatalog.SelectedIndex = importIndex;
            Refresh_cmb_LoadRstDataset();
            //设置默认选项
            if (cmb_LoadRstDataset.Items.Count > 0)
            {
                cmb_LoadRstDataset.SelectedIndex = cmb_LoadRstDataset.Items.Count-1;
            }
        }

        /// <summary>
        /// TOCControl的鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            try
            {
                //获取当前鼠标点击位置的相关信息
                esriTOCControlItem itemType = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap basicMap = null;
                ILayer layer = null;
                object unk = null;
                object data = null;
                //将以上定义的接口作为引用传入函数中，获取多个返回值
                this.axTOCControl1.HitTest(e.x, e.y, ref itemType, ref basicMap, ref layer, ref unk, ref data);

                //如果是 鼠标右击 并且 点击位置是图层，则弹出右击功能框
                if (e.button == 2)
                {
                    if (itemType == esriTOCControlItem.esriTOCControlItemLayer)
                    {
                        //设置TOC选择图层
                        this.TOCRightLayer = layer;
                        this.contextMenuStrip1.Show(axTOCControl1, e.x, e.y);//显示右击的窗口
                    }
                }
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// 【缩放】到当前图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomToLayer_Click(object sender, EventArgs e)
        {
            try
            {
                //缩放到当前图层
                axMapControl1.ActiveView.Extent = TOCRightLayer.AreaOfInterest;
                //刷新页面显示
                axMapControl1.ActiveView.Refresh();
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);  
            }
        }

        /// <summary>
        /// 【删除】当前图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteLayer_Click(object sender, EventArgs e)
        {
            try
            {
                //删除当前图层
                axMapControl1.Map.DeleteLayer(TOCRightLayer);
                //刷新当前页面
                axMapControl1.ActiveView.Refresh();
                //更新波段信息统计的图层和波段下拉框选项内容
                iniCmbItems();
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //2 图层和波段选择下拉框的初始化和联动变化
        
        //2.1 【初始化】函数

        //当加载图层时候，初始化Tab页面里的图层和波段下拉框的选项内容
        private void iniCmbItems()
        {
            try
            {
                //清除波段信息统计图层下拉框的选项内容
                cmb_StatisticsLayer.Items.Clear();
                cmb_StatisticsLayer.Text = "";
                //清除NDVI指数计算图层下拉框的选项内容
                cmb_NDVILayer.Items.Clear();
                cmb_NDVILayer.Text = "";
                //清除直方图绘制图层下拉框的选项内容
                cmb_DrawHisLayer.Items.Clear();
                cmb_DrawHisLayer.Text = "";
                //清除单波段灰度增强的图层下拉框的选项内容
                cmb_StretchLayer.Items.Clear();
                cmb_StretchLayer.Text = "";
                //清除单波段伪彩色渲染的图层下拉框的选项内容
                cmb_RenderLayer.Items.Clear();
                cmb_RenderLayer.Text = "";
                //清除多波段假彩色合成的图层下拉框的选项内容
                cmb_RGBLayer.Items.Clear();
                cmb_RGBLayer.Text = "";

                //第三节课自己加上去
                cmb_ClassifyLayer.Items.Clear();
                cmb_ClassifyLayer.Text = "";

                //2017/11/16增加,发现并解决一个小bug，当删除完所有图层后，combobox的text仍然有东西，，之后还要规定combobox的内容用户只能选择不能输入
                cmb_ClipLayer.Items.Clear();
                cmb_ClipLayer.Text = "";
                cmb_SingleBandLayer.Items.Clear();
                cmb_SingleBandLayer.Text = "";
                cmb_MultiBandLayer.Items.Clear();
                cmb_MultiBandLayer.Text = "";
                cmb_FilterLayer.Items.Clear();
                cmb_FilterLayer.Text = "";
                cmb_TransformLayer.Items.Clear();
                cmb_TransformLayer.Text = "";

                //2017/11/23 add
                cmb_AspectLayer.Items.Clear();
                cmb_AspectLayer.Text = "";
                cmb_SlopeLayer.Items.Clear();
                cmb_SlopeLayer.Text = "";
                cmb_HillshadeLayer.Items.Clear();
                cmb_HillshadeLayer.Text = "";
                cmb_ExtractionRasterLayer.Items.Clear();
                cmb_ExtractionRasterLayer.Text = "";
                cmb_NeighborhoodRasterLayer.Items.Clear();
                cmb_NeighborhoodRasterLayer.Text = "";
                //2017/11/24
                cmb_LineOfSightLayer.Items.Clear();
                cmb_LineOfSightLayer.Text = "";
                cmb_VisibilityLayer.Items.Clear();
                cmb_VisibilityLayer.Text = "";
                //2017/11/25
                cmb_CreateTinLayer.Items.Clear();
                cmb_CreateTinLayer.Text = "";
                cmb_ContourLayer.Items.Clear();
                cmb_ContourLayer.Text = "";
                cmb_TinContourLayer.Items.Clear();
                cmb_TinContourLayer.Text = "";
                cmb_TinVoronoiLayer.Items.Clear();
                cmb_TinVoronoiLayer.Text = "";
                //2017/12/15
                cmb_FlowDirectionLayer.Items.Clear();
                cmb_FlowDirectionLayer.Text = "";
                cmb_OutFlowDirectionRaster.Items.Clear();
                cmb_OutFlowDirectionRaster.Text = "";
                cmb_SinkLayer.Items.Clear();
                cmb_SinkLayer.Text = "";
                cmb_FillLayer.Items.Clear();
                cmb_FillLayer.Text = "";
                cmb_StreamNetLayer.Items.Clear();
                cmb_StreamNetLayer.Text = "";
                cmb_streamRasterLayer.Items.Clear();
                cmb_streamRasterLayer.Text = "";
                cmb_FlowDirectionToFeature.Items.Clear();
                cmb_FlowDirectionToFeature.Text = "";

                ILayer layer = null;
                IMap map = axMapControl1.Map;
                int count = map.LayerCount;
                if (count > 0)
                {
                    //遍历地图的所有图层，获取图层名字加入下拉框
                    for (int i = 0; i < count; i++)
                    {
                        layer = map.get_Layer(i);
                        string layerName = layer.Name;
                        //string layerName = layer.Name.Substring(layer.Name.LastIndexOf('.') + 1);//自己增加的，去除SDE前缀
                        //波段信息统计的图层下拉框
                        cmb_StatisticsLayer.Items.Add(layerName);
                        //NDVI指数计算的图层下拉框
                        cmb_NDVILayer.Items.Add(layerName);
                        //直方图绘制的图层下拉框
                        cmb_DrawHisLayer.Items.Add(layerName);
                        //单波段灰度增强的图层下拉框
                        cmb_StretchLayer.Items.Add(layerName);
                        //单波段伪彩色渲染的图层下拉框
                        cmb_RenderLayer.Items.Add(layerName);
                        //多波段假彩色合成的图层下拉框
                        cmb_RGBLayer.Items.Add(layerName);

                        //第三节课自己加上去的
                        cmb_ClassifyLayer.Items.Add(layerName);

                        //add 20171116
                        cmb_ClipLayer.Items.Add(layerName);
                        cmb_SingleBandLayer.Items.Add(layerName);
                        cmb_MultiBandLayer.Items.Add(layerName);
                        cmb_FilterLayer.Items.Add(layerName);
                        cmb_TransformLayer.Items.Add(layerName);

                        //2017/11/23 add
                        cmb_AspectLayer.Items.Add(layerName);
                        cmb_SlopeLayer.Items.Add(layerName);
                        cmb_HillshadeLayer.Items.Add(layerName);
                        cmb_ExtractionRasterLayer.Items.Add(layerName);
                        cmb_NeighborhoodRasterLayer.Items.Add(layerName);
                        //2017/11/24
                        cmb_LineOfSightLayer.Items.Add(layerName);
                        cmb_VisibilityLayer.Items.Add(layerName);
                        //2017/11/25
                        cmb_CreateTinLayer.Items.Add(layerName);
                        cmb_ContourLayer.Items.Add(layerName);
                        cmb_TinContourLayer.Items.Add(layerName);
                        cmb_TinVoronoiLayer.Items.Add(layerName);
                        //2017/12/15
                        cmb_FlowDirectionLayer.Items.Add(layerName);
                        cmb_OutFlowDirectionRaster.Items.Add(layerName);
                        cmb_SinkLayer.Items.Add(layerName);
                        cmb_FillLayer.Items.Add(layerName);
                        cmb_StreamNetLayer.Items.Add(layerName);
                        cmb_streamRasterLayer.Items.Add(layerName);
                        cmb_FlowDirectionToFeature.Items.Add(layerName);

                    }
                    //设置下拉框默认选项为第一个图层
                    if (cmb_StatisticsLayer.Items.Count > 0)
                    {
                        cmb_StatisticsLayer.SelectedIndex = 0;
                    }
                    if (cmb_NDVILayer.Items.Count > 0)
                    {
                        cmb_NDVILayer.SelectedIndex = 0;
                    }
                    if (cmb_DrawHisLayer.Items.Count > 0)
                    {
                        cmb_DrawHisLayer.SelectedIndex = 0;
                    }
                    if (cmb_StretchLayer.Items.Count > 0)
                    {
                        cmb_StretchLayer.SelectedIndex = 0;
                    }
                    if (cmb_RenderLayer.Items.Count > 0)
                    {
                        cmb_RenderLayer.SelectedIndex = 0;
                    }
                    if (cmb_RGBLayer.Items.Count > 0)
                    {
                        cmb_RGBLayer.SelectedIndex = 0;
                    }
                    //第三节课自己加上去的
                    if (cmb_ClassifyLayer.Items.Count > 0)
                    {
                        cmb_ClassifyLayer.SelectedIndex = 0;
                    }
                    //add 20171116
                    if (cmb_ClipLayer.Items.Count > 0)
                    {
                        cmb_ClipLayer.SelectedIndex = 0;
                    }
                    if (cmb_SingleBandLayer.Items.Count > 0)
                    {
                        cmb_SingleBandLayer.SelectedIndex = 0;
                    }
                    if (cmb_MultiBandLayer.Items.Count > 0)
                    {
                        cmb_MultiBandLayer.SelectedIndex = 0;
                    }
                    if (cmb_FilterLayer.Items.Count > 0)
                    {
                        cmb_FilterLayer.SelectedIndex = 0;
                    }
                    if (cmb_TransformLayer.Items.Count > 0)
                    {
                        cmb_TransformLayer.SelectedIndex = 0;
                    }
                    //add 2017/11/23
                    if (cmb_AspectLayer.Items.Count > 0)
                    {
                        cmb_AspectLayer.SelectedIndex = 0;
                    }
                    if (cmb_SlopeLayer.Items.Count > 0)
                    {
                        cmb_SlopeLayer.SelectedIndex = 0;
                    }
                    if (cmb_HillshadeLayer.Items.Count > 0)
                    {
                        cmb_HillshadeLayer.SelectedIndex = 0;
                    }
                    if (cmb_ExtractionRasterLayer.Items.Count > 0)
                    {
                        cmb_ExtractionRasterLayer.SelectedIndex = 0;
                    }
                    if (cmb_NeighborhoodRasterLayer.Items.Count > 0)
                    {
                        cmb_NeighborhoodRasterLayer.SelectedIndex = 0;
                    }
                    //2017/11/24
                    if (cmb_LineOfSightLayer.Items.Count > 0)
                    {
                        cmb_LineOfSightLayer.SelectedIndex = 0;
                    }
                    if (cmb_VisibilityLayer.Items.Count > 0)
                    {
                        cmb_VisibilityLayer.SelectedIndex = 0;
                    }
                    //2017/11/25
                    if (cmb_CreateTinLayer.Items.Count > 0)
                    {
                        cmb_CreateTinLayer.SelectedIndex = 0;
                    }
                    if (cmb_ContourLayer.Items.Count > 0)
                    {
                        cmb_ContourLayer.SelectedIndex = 0;
                    }
                    if (cmb_TinContourLayer.Items.Count > 0)
                    {
                        cmb_TinContourLayer.SelectedIndex = 0;
                    }
                    if (cmb_TinVoronoiLayer.Items.Count > 0)
                    {
                        cmb_TinVoronoiLayer.SelectedIndex = 0;
                    }
                    //2017/12/15
                    if (cmb_FlowDirectionLayer.Items.Count > 0)
                    {
                        cmb_FlowDirectionLayer.SelectedIndex = 0;
                    }
                    if (cmb_OutFlowDirectionRaster.Items.Count > 0)
                    {
                        cmb_OutFlowDirectionRaster.SelectedIndex = 0;
                    }
                    if (cmb_SinkLayer.Items.Count > 0)
                    {
                        cmb_SinkLayer.SelectedIndex = 0;
                    }
                    if (cmb_FillLayer.Items.Count > 0)
                    {
                        cmb_FillLayer.SelectedIndex = 0;
                    }
                    if (cmb_StreamNetLayer.Items.Count > 0)
                    {
                        cmb_StreamNetLayer.SelectedIndex = 0;
                    }
                    if (cmb_streamRasterLayer.Items.Count > 0)
                    {
                        cmb_streamRasterLayer.SelectedIndex = 0;
                    }
                    if (cmb_FlowDirectionToFeature.Items.Count > 0)
                    {
                        cmb_FlowDirectionToFeature.SelectedIndex = 0;
                    }


                    //清除波段信息统计波段下拉框的选项内容
                    cmb_StatisticsBand.Items.Clear();
                    cmb_StatisticsBand.Text = "";
                    //清除直方图绘制的波段下拉框的选项内容
                    cmb_DrawHisBand.Items.Clear();
                    cmb_DrawHisBand.Text = "";
                    //清除单波段灰度增强的波段下拉框的选项内容
                    cmb_StretchBand.Items.Clear();
                    cmb_StretchBand.Text = "";
                    //清除单波段伪彩色渲染的波段下拉框的选项内容
                    cmb_RenderBand.Items.Clear();
                    cmb_RenderBand.Text = "";
                    //清除多波段假彩色合成的波段下拉框的选项内容
                    cmb_RBand.Items.Clear();
                    cmb_RBand.Text = "";
                    cmb_GBand.Items.Clear();
                    cmb_GBand.Text = "";
                    cmb_BBand.Items.Clear();
                    cmb_BBand.Text = "";

                    //获取第一个图层的栅格波段 
                    //2017/11/25 debug 当layer不是 iRasterLayer时 出错
                    //因为前面确保了 加载第0个图层 所以这里选择第0图层的波段
                    layer = map.get_Layer(0);
                    if (layer is IRasterLayer)
                    {
                        IRasterLayer rstLayer = layer as IRasterLayer;
                        IRaster2 raster2 = rstLayer.Raster as IRaster2;
                        IRasterDataset rstDataset = raster2.RasterDataset;
                        IRasterBandCollection rstBandColl = rstDataset as IRasterBandCollection;

                        //波段总数
                        int bandCount = rstLayer.BandCount;
                        //添加 "所有波段" 的选项
                        cmb_StatisticsBand.Items.Add("全部波段");

                        //遍历图层的所有波段，获取波段名字加入下拉框
                        for (int i = 0; i < bandCount; i++)
                        {
                            int bandIdx = i + 1;//设置波段序号
                            //添加波段信息统计的波段下拉框的选项内容
                            cmb_StatisticsBand.Items.Add("波段" + bandIdx);
                            //添加直方图绘制的波段下拉框的选项内容
                            cmb_DrawHisBand.Items.Add("波段" + bandIdx);
                            //添加单波段灰度增强的下拉框的选项内容
                            cmb_StretchBand.Items.Add("波段" + bandIdx);
                            //添加单波段伪彩色渲染的的波段下拉框的选项内容
                            cmb_RenderBand.Items.Add("波段" + bandIdx);
                            //添加多波段假彩色合成的波段下拉框的选项内容
                            cmb_RBand.Items.Add("波段" + bandIdx);
                            cmb_GBand.Items.Add("波段" + bandIdx);
                            cmb_BBand.Items.Add("波段" + bandIdx);
                        }
                    }

                    //设置下拉框默认选项
                    if (cmb_StatisticsBand.Items.Count > 0)
                    {
                        cmb_StatisticsBand.SelectedIndex = 0;
                    }
                    if (cmb_DrawHisBand.Items.Count > 0)
                    {
                        cmb_DrawHisBand.SelectedIndex = 0;
                    }
                    if (cmb_StretchBand.Items.Count > 0)
                    {
                        cmb_StretchBand.SelectedIndex = 0;
                    }
                    if (cmb_RenderBand.Items.Count > 0)
                    {
                        cmb_RenderBand.SelectedIndex = 0;
                    }
                    if (cmb_RBand.Items.Count > 0)
                    {
                        cmb_RBand.SelectedIndex = 0;
                    }
                    if (cmb_GBand.Items.Count > 0)
                    {
                        cmb_GBand.SelectedIndex = 0;
                    }
                    if (cmb_BBand.Items.Count > 0)
                    {
                        cmb_BBand.SelectedIndex = 0;
                    }
                }
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //2.2 【联动】变化函数

        //当遥感图像处理分析的图层下拉框的选择项发生变化，则相应的波段下拉框的选项也会发生变化
        private void selectedIndexChangeFunction(ComboBox cmbLayer, ComboBox cmbBand, string type)
        {
            try
            {
                //此处代码填空，实现图层下拉框和波段下拉框的联动变化函数
                //type 要么是"statistics" 要么是null
                //如果有"statistics" 要加上"全部波段"  一项
                //！！！注意 目前只有一个layer，以后需要测试 这个代码，先假装是对的！！！

                //根据cmbLayer的index（因为是按顺序加的） 或者 name(此方法需要全称，不能用去掉SDE前缀的) 得到layer
                //方法1 index  要对statistics 那一项特殊考虑，那项第0项是 全部波段
                //int layerIndex = cmbLayer.SelectedIndex;
                //ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                //方法2 name（全称）
                string layerName = cmbLayer.SelectedItem.ToString();
                ILayer layer = getLayerByName(layerName);//自定义方法

                //获取每个图层的band，括号1中的方法是 上面助教的方法，个人觉得可以用括号2(好像会GG)
                //ILayer->IRasterLayer.Raster->IRaster->(IRaster2.RasterDataset->IRasterDataset->IRasterBandCollection)or(just->IRasterBandCollection)->IRasterBandCollection.item(bandindex)
                //2017/11/25 发现bugs 之前都是rasterLayer 所以可以顺利运行 
                //但是 等高线 是 featurelayer
                //所以要加上 layer is IRasterLayer 判断!
                if (layer is IRasterLayer)
                {
                    IRasterLayer rasterLayer = layer as IRasterLayer;
                    IRaster2 raster = rasterLayer.Raster as IRaster2;
                    IRasterDataset rasterDataset = raster.RasterDataset;
                    IRasterBandCollection rasterBandCollection = rasterDataset as IRasterBandCollection;
                    int bandcount = rasterBandCollection.Count;
                    //亲，先清空内容哦
                    cmbBand.Items.Clear();
                    //type 的作用在此体现
                    if (type == "statistics")
                    {
                        cmbBand.Items.Add("全部波段");
                    }
                    for (int i = 0; i < bandcount; i++)
                    {
                        int bandIdx = i + 1;//对用户来说，不会说波段0，而说波段1
                        cmbBand.Items.Add("波段" + bandIdx);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        //根据layerName(string) 找到 ILayer
        private ILayer getLayerByName(string layerName)
        { 
            //遍历所有的layer，如果name匹配则返回
            //bool findLayer = false;
            IMap map = axMapControl1.Map;
            int layerCount = map.LayerCount;
            ILayer layer = null;
            for (int i = 0; i < layerCount; i++)
            {
                layer = map.get_Layer(i);
                if (layer.Name == layerName) 
                {
                    //findLayer = true;
                    break;
                }
            }
            return layer;
            //if (findLayer)
            //{
            //    return layer;
            //}
            //else 
            //{
            //    return null;
            //}        
        }

        //2.3 图层下拉框的选择变化触发事件
        //当波段信息统计的图层下拉框的选择项发生变化，则相应的波段下拉框的选项也发生变化
        //其实就是调用上面写的函数

        //当波段信息统计的图层下拉框的选择项发生变化，则相应的波段下拉框也会发生变化
        private void cmb_StatisticsLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedIndexChangeFunction(cmb_StatisticsLayer, cmb_StatisticsBand, "statistics");
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //当直方图绘制的图层下拉框的选择项发生变化，则相应的波段下拉框也会发生变化
        private void cmb_DrawHisLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                selectedIndexChangeFunction(cmb_DrawHisLayer, cmb_DrawHisBand, null);
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //当单波段灰度增强的图层下拉框的选择项发生变化，则相应的波段下拉框也会发生变化
        private void cmb_StretchLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedIndexChangeFunction(cmb_StretchLayer, cmb_StretchBand, null);
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //当单波段伪彩色渲染的图层下拉框的选择项发生变化，则相应的波段下拉框也会发生变化
        private void cmb_RenderLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                selectedIndexChangeFunction(cmb_RenderLayer, cmb_RenderBand, null);
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //当多波段假彩色合成的图层下拉框的选择项发生变化，则相应的波段下拉框也会发生变化
        private void cmb_RGBLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                selectedIndexChangeFunction(cmb_RGBLayer, cmb_RBand, null);
                selectedIndexChangeFunction(cmb_RGBLayer, cmb_GBand, null);
                selectedIndexChangeFunction(cmb_RGBLayer, cmb_BBand, null);
            }
            catch (System.Exception ex)//异常处理，输出错误信息
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 3 波段信息【统计】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Statistics_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取当前选中的栅格图层的栅格波段
                string layerName = cmb_StatisticsLayer.SelectedItem.ToString();
                ILayer layer = getLayerByName(layerName);
                IRasterLayer rasterLayer = layer as IRasterLayer;
                IRaster2 raster = rasterLayer.Raster as IRaster2;
                IRasterDataset rasterDataset = raster.RasterDataset;
                IRasterBandCollection rstBandColl = rasterDataset as IRasterBandCollection;
                int bandCount = rstBandColl.Count;
                int indexBand = cmb_StatisticsBand.SelectedIndex;
     
                //代码填空，如果选择全部波段，则遍历该图层全部波段，并统计信息
                //！！！注意 多波段信息统计 仍需测试！！！ 受限于只导入了单一波段的影像
                if (indexBand == 0)//0表示选中 全部波段
                {
                    string StatRes = "";//弹窗信息
                    for (int i = 0; i < bandCount; i++)
                    {
                        IRasterBand rstBand = rstBandColl.Item(i);
                        //判断该波段是否已经存在统计数据
                        bool hasStat = false;
                        rstBand.HasStatistics(out hasStat);
                        //如果不存在统计数据，则进行波段信息统计
                        if (rstBand.Statistics == null || !hasStat)
                        {
                            IRasterBandEdit rasterBandEdit = rstBand as IRasterBandEdit;
                            rasterBandEdit.ComputeStatsHistogram(0);
                            //rstBand.ComputeStatsAndHist();
                        }
                        //获取统计结果
                        IRasterStatistics rstStat = rstBand.Statistics;
                        //代码填空，获取统计信息数据，拼接结果字符串
                        StatRes += "【波段" + (i + 1).ToString() + "】最大值：" + rstStat.Maximum + "，最小值：" + rstStat.Minimum + "，平均值：" + rstStat.Mean + "，标准差：" + rstStat.StandardDeviation + "。\n";
                    }
                    //提示框输出统计结果
                    MessageBox.Show(StatRes, "统计结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else//选择某一波段
                {
                    string bandStatRes = "";
                    IRasterBand rstBand = rstBandColl.Item(indexBand - 1);
                    //判断该波段是否已经存在统计数据
                    bool hasStat = false;
                    rstBand.HasStatistics(out hasStat);
                    //如果不存在统计数据，则进行波段信息统计
                    if (rstBand.Statistics == null || !hasStat)
                    {
                        IRasterBandEdit2 rasterBandEdit2 = rstBand as IRasterBandEdit2;
                        rasterBandEdit2.ComputeStatsHistogram(0);
                        //rstBand.ComputeStatsAndHist();
                    }
                    //获取统计结果
                    IRasterStatistics rstStat = rstBand.Statistics;
                    //代码填空，获取统计信息数据，拼接结果字符串
                    bandStatRes = "【波段" + indexBand + "】最大值：" + rstStat.Maximum + "，最小值：" + rstStat.Minimum+ "，平均值：" + rstStat.Mean + "，标准差：" + rstStat.StandardDeviation + "。";
                    //提示框输出统计结果
                    MessageBox.Show(bandStatRes, "统计结果", MessageBoxButtons.OK, MessageBoxIcon.Information);              
                }
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 4 【NDVI】计算 （近红外-红外）/（近红外+红外） IMathOp RasterMathOpsClass
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CalculateNDVI_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;//单击时修改鼠标光标形状
            try
            {
                //代码填空，选择的图层，转换接口
                string layerName = cmb_NDVILayer.SelectedItem.ToString();
                ILayer layer = getLayerByName(layerName);
                IRasterLayer rasterLayer = layer as IRasterLayer;
                IRaster2 raster = rasterLayer.Raster as IRaster2;
                IRasterDataset rasterDataset = raster.RasterDataset;
                IRasterBandCollection rasterBandCollection = rasterDataset as IRasterBandCollection;
                /////!!!注意 确保至少有4个波段？
                int bandCount = rasterBandCollection.Count;
                if (bandCount < 4)
                {
                    MessageBox.Show("所选图层波段数过少，无法计算", "提示");
                    return;
                }

                //代码填空，获取红外波段和近红外波段，转换IGeodataset接口
                IRasterBand rasterBand4 = rasterBandCollection.Item(3);//获取第四波段，即近红外波段
                IRasterBand rasterBand3 = rasterBandCollection.Item(2);//获取第三波段，即红外波段
                IGeoDataset geoDataset4 = rasterBand4 as IGeoDataset;
                IGeoDataset geoDataset3 = rasterBand3 as IGeoDataset;
                //代码填空，利用IGeodataset和math计算NDVI获得结果IGeodataset
                //创建一个用于栅格运算的类RasterMathOpClass
                IMathOp mathOp = new RasterMathOpsClass();
                //band4-band3
                IGeoDataset upDataset = mathOp.Minus(geoDataset4, geoDataset3);
                //band4+band3
                IGeoDataset downDataset = mathOp.Plus(geoDataset4, geoDataset3);
                //分子分母转为float类型
                IGeoDataset fltUpDataset = mathOp.Float(upDataset);
                IGeoDataset fltdownDataset = mathOp.Float(downDataset);
                //相除得到NDVI
                IGeoDataset resultDataset = mathOp.Divide(fltUpDataset, fltdownDataset);
                //将结果保存到一个RasterLayer中，命名为NDVI
                IRaster resRaster = resultDataset as IRaster;
                IRasterLayer resLayer = new RasterLayerClass();
                resLayer.CreateFromRaster(resRaster);
                resLayer.SpatialReference = geoDataset4.SpatialReference;
                resLayer.Name = "NDVI";
                //将此单波段图像用灰度显示，并按照最大最小值拉伸
                IRasterStretchColorRampRenderer grayStretch = null;
                if (resLayer.Renderer is IRasterStretchColorRampRenderer)
                {
                    grayStretch = resLayer.Renderer as IRasterStretchColorRampRenderer;
                }
                else
                {
                    grayStretch = new RasterStretchColorRampRendererClass();
                }
                IRasterStretch2 rstStr2 = grayStretch as IRasterStretch2;
                rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_MinimumMaximum;//设置拉伸模式为最大最小值
                resLayer.Renderer = grayStretch as IRasterRenderer;
                resLayer.Renderer.Update();
                //添加NDVI图层显示，并刷新视图
                axMapControl1.AddLayer(resLayer);
                axMapControl1.ActiveView.Extent = resLayer.AreaOfInterest;
                axMapControl1.Refresh();
                this.axTOCControl1.Update();

            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally//最后再将鼠标光标设置成默认形状
            {
                this.Cursor = Cursors.Default;
            }
            ////CalculateNDVI_2();
        }

        //5 单波段【直方图】绘制
        //5.1 绘制 鼠标点击事件
        private void btn_SingleBandHis_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;//单击时修改鼠标光标形状
            try
            {
                //代码填空，获取选择的图层和波段对象，接口转换
                string layerName = cmb_DrawHisLayer.SelectedItem.ToString();
                int bandIndex = cmb_DrawHisBand.SelectedIndex;
                ILayer layer = getLayerByName(layerName);
                IRasterLayer rasterLayer = layer as IRasterLayer;
                IRaster2 raster2 = rasterLayer.Raster as IRaster2;
                IRasterDataset rasterDataset = raster2.RasterDataset;
                IRasterBandCollection rasterBandCollection = rasterDataset as IRasterBandCollection;
                IRasterBand rasterBand = rasterBandCollection.Item(bandIndex);
                //代码填空，计算该波段的histogram（tips：类似于计算statistics）
                bool hasStat = false;
                rasterBand.HasStatistics(out hasStat);
                if (null == rasterBand.Statistics || !hasStat || rasterBand.Histogram == null)
                { 
                    //转换IRasterBandEdit2接口，调用ComputeStatsHistogram方法进行波段信息统计和直方图绘制
                    IRasterBandEdit2 rasterBandEdit = rasterBand as IRasterBandEdit2;
                    rasterBandEdit.ComputeStatsHistogram(0);
                }
                //获取每个象元值的统计个数
                double[] histo = rasterBand.Histogram.Counts as double[];
                //获取统计结果
                IRasterStatistics rasterStatistics = rasterBand.Statistics;
                //创建直方图窗体，并将象元统计、最小值、最大值作为参数传入
                HistogramForm histogramForm = new HistogramForm(histo, rasterStatistics.Minimum, rasterStatistics.Maximum);
                histogramForm.ShowDialog();
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally//最后再将鼠标光标设置成默认形状
            {
                this.Cursor = Cursors.Default;
            }
        }

        //6 多波段【直方图】对比绘制
        //6.1 "绘制"鼠标点击事件
        private void btn_MultiBandHis_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;//单击时修改鼠标光标形状
            try
            {
                //获取当前选中的图层index
                int indexLayer = cmb_DrawHisLayer.SelectedIndex;
                //读取MapControl中的map相应图层
                ILayer layer = this.axMapControl1.Map.get_Layer(indexLayer);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rasterLayer = layer as IRasterLayer;
                    SelectBandsForm SelectBands = new SelectBandsForm(rasterLayer);
                    SelectBands.ShowDialog();                  
                }

            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally//最后再将鼠标光标设置成默认形状
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 7 【单】波段灰度【增强】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Stretch_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取当前选择的图层和波段对象
                int layerIndex = cmb_StretchLayer.SelectedIndex;
                int bandIndex = cmb_StretchBand.SelectedIndex;
                ILayer layer = this.axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                { 
                    //代码填空，获取波段渲染信息，创建拉伸渲染类对象
                    IRasterLayer rasterLayer = layer as IRasterLayer;
                    //IRaster2 raster2 = rasterLayer.Raster as IRaster2;
                    //IRasterDataset rasterDataset = raster2.RasterDataset;
                    //IRasterBandCollection rasterBandCollection = rasterDataset as IRasterBandCollection;
                    //IRasterBand rasterBand = rasterBandCollection.Item(bandIndex);
                    IRasterRenderer rstRender = rasterLayer.Renderer;
                    IRasterStretchColorRampRenderer rstStretchColorRampRender = null;
                    if (rstRender is IRasterStretchColorRampRenderer)
                    {
                        rstStretchColorRampRender = rstRender as IRasterStretchColorRampRenderer;
                    }
                    else
                    {
                        rstStretchColorRampRender = new RasterStretchColorRampRendererClass();
                    }
                    ///注意！注意！注意！ 下面这句话指明了哪个 波段
                    rstStretchColorRampRender.BandIndex = bandIndex;
                    IRasterStretch2 rstStr2 = rstStretchColorRampRender as IRasterStretch2;
                    //判断拉伸方式
                    switch (cmb_StretchMethod.SelectedIndex)
                    { 
                        case 0://默认拉伸
                            rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_DefaultFromSource;
                            break;
                        case 1://标准差拉伸
                            rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_StandardDeviations;
                            break;
                        case 2://最大最小值拉伸
                            rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_MinimumMaximum;
                            break;
                        case 3://百分比最大最小值拉伸
                            rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_PercentMinimumMaximum;
                            break;
                        case 4://直方图均衡
                            rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_HistogramEqualize;
                            break;
                        case 5://直方图匹配
                            rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_HistogramSpecification;
                            break;
                        default:
                            break;
                    }
                    //设置不应用反色
                    rstStr2.Invert = false;
                    //应用拉伸渲染
                    rstRender = rstStretchColorRampRender as IRasterRenderer;
                    rstRender.Update();
                    rasterLayer.Renderer = rstRender;
                }
                //刷新空间
                this.axMapControl1.ActiveView.Refresh();
                this.axTOCControl1.Update();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        //8 单波段【伪彩色】渲染
        //8.1 产生AlgorithmicColorRamp
        /// <summary>
        /// 根据两端颜色和大小产生IAlgorithmicColorRamp   CreateRamp
        /// </summary>
        /// <param name="FromColor"></param>
        /// <param name="ToColor"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private IAlgorithmicColorRamp GetAlgorithmicColorRamp(Color FromColor, Color ToColor, int size)
        {
            try
            {
                //实例化接口
                IAlgorithmicColorRamp algorithmicColorRamp = new AlgorithmicColorRampClass();
                //代码填空，设置起始颜色，终止颜色，算法类型，尺寸大小
                //转到AE中的IColor
                IRgbColor rgbColor1 = new RgbColorClass();
                IRgbColor rgbColor2 = new RgbColorClass();
                rgbColor1.Red = (int)FromColor.R;
                rgbColor1.Green = (int)FromColor.G;
                rgbColor1.Blue = (int)FromColor.B;

                rgbColor2.Red = (int)ToColor.R;
                rgbColor2.Green = (int)ToColor.G;
                rgbColor2.Blue = (int)ToColor.B;

                IColor color1 = rgbColor1 as IColor;
                IColor color2 = rgbColor2 as IColor;
                algorithmicColorRamp.FromColor = color1;
                algorithmicColorRamp.ToColor = color2;
                //algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                //algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriLabLChAlgorithm;
                algorithmicColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;//七彩色
                algorithmicColorRamp.Size = size;
                //调用IAlgorithmicColorRamp接口的CreateRamp函数创建色带
                bool bResult = false;
                algorithmicColorRamp.CreateRamp(out bResult);
                if (bResult)
                {
                    return algorithmicColorRamp;
                }
                return null;

            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        //8.2创建色带bitmap图像
        /// <summary>
        /// 通过传入的起始和终止颜色创建包含色带的bitmap图像并返回
        /// </summary>
        /// <param name="FromColor">起始颜色</param>
        /// <param name="ToColor">终止颜色</param>
        /// <returns></returns>
        private Bitmap CreateColorRamp(Color FromColor, Color ToColor)
        {
            try
            {
                //获得色带
                IAlgorithmicColorRamp algorithmicColorRamp = GetAlgorithmicColorRamp(FromColor, ToColor, pb_ColorBar.Size.Width);
                //创建新的bitmap
                Bitmap bmpColorRamp = new Bitmap(pb_ColorBar.Size.Width, pb_ColorBar.Size.Height);
                //获取graphic对象
                Graphics graphic = Graphics.FromImage(bmpColorRamp);
                //用GDI+的方法逐一填充颜色到显示色带
                IColor color = null;
                for (int i = 0; i < pb_ColorBar.Size.Width; i++)
                { 
                    //获取当前颜色
                    color = algorithmicColorRamp.get_Color(i);
                    if (color == null)
                    {
                        continue;
                    }
                    IRgbColor rgbColor = new RgbColorClass();
                    rgbColor.RGB = color.RGB;
                    Color customColor = Color.FromArgb(rgbColor.Red,rgbColor.Green,rgbColor.Blue);
                    SolidBrush solidBrush = new SolidBrush(customColor);
                    //绘制
                    graphic.FillRectangle(solidBrush, i, 0, 1, pb_ColorBar.Size.Height); 
                }
                //删除Graphcis对象
                graphic.Dispose();
                return bmpColorRamp;
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
        }

        //8.3刷新颜色控件
        /// <summary>
        /// 刷新颜色控件
        /// </summary>
        /// <param name="FromColor"></param>
        /// <param name="Tocolor"></param>
        //重新绘制起始颜色、终止颜色，并根据这两种颜色绘制色带
        private void RefreshColors(Color FromColor, Color ToColor)
        {
            try
            {
                //初始化FromColor
                //创建bitmap
                Bitmap bmpFromColor = new Bitmap(pb_FromColor.Size.Width, pb_FromColor.Size.Height);
                //创建Graphics对象
                Graphics graphicFC = Graphics.FromImage(bmpFromColor);
                SolidBrush solidBrushFC = new SolidBrush(FromColor);
                //绘制起始颜色，左上到右下
                graphicFC.FillRectangle(solidBrushFC, 0, 0, pb_FromColor.Size.Width, pb_FromColor.Size.Height);
                //更新图像
                this.pb_FromColor.Image = bmpFromColor;
                //删除Graphics对象
                graphicFC.Dispose();

                //初始化ToColor
                //创建bitmap
                Bitmap bmpToColor = new Bitmap(pb_ToColor.Size.Width, pb_ToColor.Size.Height);
                //创建Graphics对象
                Graphics graphicTC = Graphics.FromImage(bmpToColor);
                SolidBrush solidBrushTC = new SolidBrush(ToColor);
                //绘制起始颜色，左上到右下
                graphicTC.FillRectangle(solidBrushTC, 0, 0, pb_ToColor.Size.Width, pb_ToColor.Size.Height);
                //更新图像
                this.pb_ToColor.Image = bmpToColor;
                //删除Graphics对象
                graphicTC.Dispose();

                //初始化色带
                Bitmap stretchRamp = CreateColorRamp(FromColor, ToColor);
                //更新图像
                this.pb_ColorBar.Image = stretchRamp;
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //8.4鼠标点击事件
        private void pb_FromColor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cd_FromColor.ShowDialog() == DialogResult.OK)//显示ColorDialog选项框，选择并获取色带起始颜色
                {
                    m_FromColor = this.cd_FromColor.Color;//设置起始颜色
                    RefreshColors(m_FromColor, m_ToColor);//刷新颜色控件
                }
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //添加终止颜色的鼠标点击事件
        private void pb_ToColor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cd_ToColor.ShowDialog() == DialogResult.OK)//显示ColorDialog选项框，选择并获取色带起始颜色
                {
                    m_ToColor = this.cd_ToColor.Color;      //设置终止颜色
                    RefreshColors(m_FromColor, m_ToColor);  //刷新颜色控件
                }
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 单波段伪彩色渲染
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Render_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取当前选择的图层和波段
                int layerIndex = cmb_RenderLayer.SelectedIndex;
                int bandIndex = cmb_RenderBand.SelectedIndex;
                ILayer layer = this.axMapControl1.Map.get_Layer(layerIndex);
                //代码填空，获取栅格对象，创建拉伸渲染类对象，设置其栅格和波段
                IRasterLayer rasterLayer = layer as IRasterLayer;
                IRasterRenderer rasterRender = rasterLayer.Renderer;
                IRasterStretchColorRampRenderer stretchRenderder = null;
                if (rasterRender is IRasterStretchColorRampRenderer)
                {
                    stretchRenderder = rasterRender as IRasterStretchColorRampRenderer;
                }
                else
                {
                    stretchRenderder = new RasterStretchColorRampRendererClass();
                }
                //！！！设置波段！！！
                stretchRenderder.BandIndex = bandIndex;
                //设置拉伸类型
                IRasterStretch2 rstStretch2 = stretchRenderder as IRasterStretch2;
                rstStretch2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_HistogramEqualize;//设置拉伸方式为直方图均衡化
                //获取色带，256层
                IAlgorithmicColorRamp algorithmicColorRamp = GetAlgorithmicColorRamp(m_FromColor, m_ToColor, 256);
                IColorRamp colorRamp = algorithmicColorRamp as IColorRamp;
                //！！！设置拉伸渲染的色带
                stretchRenderder.ColorRamp = colorRamp;

                //设置TOC中的图例
                ILegendInfo legendInfo = stretchRenderder as ILegendInfo;
                ILegendGroup legendGroup = legendInfo.get_LegendGroup(0);
                for (int i = 0; i < legendGroup.ClassCount; i++)
                {
                    ILegendClass legendClass = legendGroup.get_Class(i);
                    legendClass.Symbol = new ColorRampSymbolClass();
                    IColorRampSymbol colorRampSymbol = legendClass.Symbol as IColorRampSymbol;
                    colorRampSymbol.ColorRamp = colorRamp;
                    colorRampSymbol.ColorRampInLegendGroup = colorRamp;
                    colorRampSymbol.LegendClassIndex = i;
                    legendClass.Symbol = colorRampSymbol as ISymbol;
                }
                //应用渲染设置
                rasterRender = stretchRenderder as IRasterRenderer;
                rasterRender.Update();
                rasterLayer.Renderer = rasterRender;
                rasterLayer.Renderer.Update();
                //刷新控件
                this.axMapControl1.ActiveView.Refresh();
                this.axTOCControl1.Update();
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //9 多波段【假彩色】合成
        /// <summary>
        /// RGB多波段假彩色合成显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RGB_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取当前选择的图层和波段，创建RGB合成渲染类对象
                int layerIndex = cmb_RGBLayer.SelectedIndex;
                int indexRBand = cmb_RBand.SelectedIndex;
                int indexGBand = cmb_GBand.SelectedIndex;
                int indexBBand = cmb_BBand.SelectedIndex;
                ILayer layer = this.axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rasterLayer = layer as IRasterLayer;
                    IRasterRenderer rasterRender = rasterLayer.Renderer;
                    IRasterRGBRenderer rgbRenderer = null;
                    if (rasterRender is IRasterRGBRenderer)
                    {
                        rgbRenderer = rasterRender as IRasterRGBRenderer;
                    }
                    else
                    {
                        rgbRenderer = new RasterRGBRendererClass();
                    }
                    //获取并设置RGB对应波段
                    rgbRenderer.RedBandIndex = indexRBand;
                    rgbRenderer.GreenBandIndex = indexGBand;
                    rgbRenderer.BlueBandIndex = indexBBand;
                    //更新渲染类
                    rasterRender = rgbRenderer as IRasterRenderer;
                    rasterRender.Update();
                    //将RGB渲染参数赋值给图层渲染器
                    rasterLayer.Renderer = rasterRender;    
                }
                //更新控件
                this.axMapControl1.ActiveView.Refresh();
                this.axTOCControl1.Update();
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// 栅格图像唯一值渲染 IRenderer Isymbol.Color  IRandomColorRamp.get_Color(i);
        /// </summary>
        /// <param name="rasterDataset">栅格数据集</param>
        /// <returns></returns>
        public IRasterRenderer UniqueValueRenderer(IRasterDataset rasterDataset)
        {
            try
            {
                IRaster2 raster = rasterDataset.CreateDefaultRaster() as IRaster2;
                ITable rasterTable = raster.AttributeTable;
                if (rasterTable == null)
                {
                    return null;
                }
                int tableRows = rasterTable.RowCount(null);
                //为每一个属性的唯一值创建和设置一个唯一颜色
                IRandomColorRamp colorRamp = new RandomColorRampClass();
                //设置随机色带的属性参数
                colorRamp.Size = tableRows;
                colorRamp.Seed = 100;
                //调用createRamp方法来创建色带
                bool createColorRamp;
                colorRamp.CreateRamp(out createColorRamp);
                if (createColorRamp == false)
                {
                    return null;
                }
                //创建一个唯一值渲染器
                IRasterUniqueValueRenderer uvRenderer = new RasterUniqueValueRendererClass();
                IRasterRenderer rasterRender = uvRenderer as IRasterRenderer;
                //设置渲染器的栅格数据对象（属性）
                rasterRender.Raster = rasterDataset.CreateDefaultRaster();
                rasterRender.Update();
                //设置渲染器的属性值
                uvRenderer.HeadingCount = 1;
                uvRenderer.set_Heading(0, "All Data Value");
                uvRenderer.set_ClassCount(0, tableRows);
                uvRenderer.Field = "Value"; ;//或者表格中的其他字段
                //遍历属性表格，分别设置唯一值颜色
                IRow row;
                //创建简单填充符号接口的对象，用于每一个类别的像素的颜色填充
                ISimpleFillSymbol fillSymbol;
                for (int i = 0; i < tableRows; i++)
                {
                    row = rasterTable.GetRow(i);
                    //为某一个特定的类别添加值
                    uvRenderer.AddValue(0, i, Convert.ToByte(row.get_Value(1)));
                    //为某一个特定的类别设置标签
                    uvRenderer.set_Label(0, i, Convert.ToString(row.get_Value(1)));
                    //实例化创建一个简单填充符号类对象
                    fillSymbol = new SimpleFillSymbolClass();
                    fillSymbol.Color = colorRamp.get_Color(i);
                    //为某一个特定的类别设置渲染符号
                    uvRenderer.set_Symbol(0, i, (ISymbol)fillSymbol);
                }
                return rasterRender;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        //栅格图像【分类】操作
        private void btn_Classify_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;//单击时修改鼠标光标形状
            try
            {
                //代码填空，获取输入的分类数目，获取选中的图层栅格对象
                int layerIndex = cmb_ClassifyLayer.SelectedIndex;
                int NumClass = Convert.ToInt32(txb_ClassifyCount.Text.Trim());
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster2 raster2 = rstLayer.Raster as IRaster2;
                    IRasterDataset rasterDataset = raster2.RasterDataset;
                    if (rstLayer.BandCount > 1)
                    {
                        //代码填空，栅格对象转换IGeodataset接口
                        IGeoDataset geoDataset = rasterDataset as IGeoDataset;
                        //下面这行有点让我害怕，可能出错
                        //geoDataset = layer as IGeoDataset;
                        //创建多元操作组件类对象
                        IMultivariateOp mulop = new RasterMultivariateOpClass();
                        //代码填空，设定结果文件保存路径

                        string fullPath = "d:/hhx/raster/classify_signature.gsg";//---【.gsg】 extension must be specified.
                        string treePath = "d:/hhx/raster/dendrogram.txt";//---The output dendrogram ASCII file. The extension can be 【.txt or .asc.】
                        //Add by myself
                        string myPath = "d:/hhx/raster";
                        DeleteDir(myPath);//每次运行之前，清空这个文件夹下面的所有文件/文件夹

                        //---输出signatureFile---
                        mulop.IsoCluster(geoDataset,fullPath,NumClass,20,20,10);

                        //定义missing的类型（参数undefined）
                        object missing = Type.Missing;
                        //利用【最大似然法】进行遥感图像非监督分类,输入IGeodataset、signatureFile，
                        IGeoDataset outdataset = mulop.MLClassify(geoDataset, fullPath, false, esriGeoAnalysisAPrioriEnum.esriGeoAnalysisAPrioriEqual, missing, missing);//分类结果数据集
                        //定义输出结果栅格
                        IRaster2 outRaster1 = outdataset as IRaster2;

                        //代码填空，?保存?结果栅格数据，【加载】栅格图层显示，进行唯一值渲染
                        IRasterLayer rasterLayer1 = new RasterLayerClass();
                        rasterLayer1.CreateFromDataset(outRaster1.RasterDataset);
                        rasterLayer1.Name = "MaxLikehood";
                        rasterLayer1.Renderer = UniqueValueRenderer(outRaster1.RasterDataset);//唯一值渲染，可以去掉
                        ILayer layer1 = rasterLayer1 as ILayer;
                        axMapControl1.Map.AddLayer(layer1);
                        //axMapControl1.ActiveView.Refresh();
                        //axTOCControl1.Update();

                        //利用【class probility】
                        IGeoDataset outDataset2 = mulop.ClassProbability(geoDataset, fullPath, esriGeoAnalysisAPrioriEnum.esriGeoAnalysisAPrioriEqual, missing, missing);
                        IRaster2 outRaster2 = outDataset2 as IRaster2;
                        IRasterLayer rasterLayer2 = new RasterLayerClass();
                        rasterLayer2.CreateFromDataset(outRaster2.RasterDataset);
                        rasterLayer2.Name = "ClassProbility";
                        ILayer layer2 = rasterLayer2 as ILayer;
                        axMapControl1.Map.AddLayer(layer2);
                        axMapControl1.ActiveView.Refresh();
                        axTOCControl1.Update();
                        iniCmbItems();

                        //Dendrogram，输入signatureFile,输出dendrogramFile
                        mulop.Dendrogram(fullPath, treePath, true, Type.Missing);

                    }
                }
                
            }
            catch (System.Exception ex)//异常处理
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //分类【后处理】
        //注意！ 不能选择class probility这个图层会出错，，要选择MLClassify 这个图层
        private void btn_AfterClassify_Click(object sender, EventArgs e)
        {
           // System.Diagnostics.Process.Start("d:/hhx/raster/classify_signature.gsg");
            this.Cursor = Cursors.WaitCursor;//单击时修改鼠标光标形状
            try
            {
                //获取选中的图层
                int indexLayer = cmb_ClassifyLayer.SelectedIndex;
                ILayer layer = this.axMapControl1.get_Layer(indexLayer);
                if (layer is IRasterLayer)
                { 
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster2 raster2 = rstLayer.Raster as IRaster2;
                    IRasterDataset rstDataset = raster2.RasterDataset;
                    IGeoDataset geoDataset = rstDataset as IGeoDataset;
                    //RasterGeneralizeOp
                    IGeneralizeOp generalizeOp = new RasterGeneralizeOpClass();
                    //---Aggregate---
                    IGeoDataset outdataset1 = generalizeOp.Aggregate(geoDataset, 4, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMean, true, true);
                    IRaster2 outRaster1 = outdataset1 as IRaster2;
                    IRasterLayer rasterLayer1 = new RasterLayerClass();
                    rasterLayer1.CreateFromDataset(outRaster1.RasterDataset);
                    rasterLayer1.Name = "Aggregate";
                    ILayer layer1 = rasterLayer1 as ILayer;
                    axMapControl1.Map.AddLayer(layer1);

                    //---BoundaryClean---
                    IGeoDataset outdataset2 = generalizeOp.BoundaryClean(geoDataset, esriGeoAnalysisSortEnum.esriGeoAnalysisSortAscending, true);
                    IRaster2 outRaster2 = outdataset2 as IRaster2;
                    IRasterLayer rasterLayer2 = new RasterLayerClass();
                    rasterLayer2.CreateFromDataset(outRaster2.RasterDataset);
                    rasterLayer2.Name = "BoundaryClean";
                    ILayer layer2 = rasterLayer2 as ILayer;
                    axMapControl1.Map.AddLayer(layer2);

                    //---MajorityFilter---
                    IGeoDataset outdataset3 = generalizeOp.MajorityFilter(geoDataset, true, false);
                    IRaster2 outRaster3 = outdataset3 as IRaster2;
                    IRasterLayer rasterLayer3 = new RasterLayerClass();
                    rasterLayer3.CreateFromDataset(outRaster3.RasterDataset);
                    rasterLayer3.Name = "MajorityFilter";
                    ILayer layer3 = rasterLayer3 as ILayer;
                    axMapControl1.Map.AddLayer(layer3);

                    //Redresh
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        #region 文件(夹)相关操作
        //删除指定文件夹下所有文件/文件夹
        private static void DeleteDir(string srcPath)
        {
            try
            {
                if(Directory.Exists(srcPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(srcPath);
                    foreach (DirectoryInfo d in dir.GetDirectories())
                    {
                        d.Delete(true);
                    }
                    foreach (FileInfo f in dir.GetFiles())
                    {
                        f.Delete();
                    }   
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //确保指定的 文件夹 存在. 若不存在则创建 否则do nothing
        private static void EnsureDir(string srcPath)
        {
            if (!System.IO.Directory.Exists(srcPath))
            {
                System.IO.Directory.CreateDirectory(srcPath);
            }
        }
        #endregion

        //1.图像裁剪
        //弹出文件选择对话框，点击选择用于裁剪的矢量文件
        private void txb_ClipFeature_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //代码填空，弹出文件选择框，选择矢量文件，文件信息显示textbox
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Shapefile(*.shp)|*.shp";
                openFileDialog.Title = "选择矢量文件";
                openFileDialog.Multiselect = false;
                string fileName = "";
                //如果对话框已成功选择文件，将文件路径信息填写到输入框内
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                    txb_ClipFeature.Text = fileName;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //点击进行矢量文件对栅格图像的裁剪
        private void btn_Clip_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取选择的栅格图层、栅格对象 
                int indexLayer = cmb_ClipLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(indexLayer);
                IRasterLayer rasterLayer = layer as IRasterLayer;//已有的layer
                IRaster2 raster2 = rasterLayer.Raster as IRaster2;
                IRasterDataset rstDataset = raster2.RasterDataset;
                IRaster raster = rstDataset.CreateDefaultRaster();//不知道助教MM为什么要绕一个圈子

                //获取矢量文件的路径和文件名字
                string fileN = txb_ClipFeature.Text;
                FileInfo fileInfo = new FileInfo(fileN);
                string filePath = fileInfo.DirectoryName;
                string fileName = fileInfo.Name;

                //代码填空，根据选择的矢量文件的路径打开工作空间
                IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                IWorkspace shpWorkspace = shpWorkspaceFactory.OpenFromFile(filePath,0);
                IFeatureWorkspace featureWorkspace = shpWorkspace as IFeatureWorkspace;//这步都忘记了233
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);

                //构造一个裁剪过滤器
                IClipFilter2 clipFilter2 = new ClipFilterClass();
                clipFilter2.ClippingType = esriRasterClippingType.esriRasterClippingOutside;
                //代码填空，将矢量数据的几何属性加到过滤器中
                IGeometry clipGeometry = null;
                IFeature feature = null;
                int featureCount = featureClass.FeatureCount(null);
                for (int i = 0; i < featureCount; i++)
                {
                    feature = featureClass.GetFeature(i);
                    clipGeometry = feature.Shape;
                    clipFilter2.Add(clipGeometry);
                }

                //将这个过滤器作用于栅格图像
                IPixelOperation pixelOp = raster as IPixelOperation;
                pixelOp.PixelFilter = clipFilter2 as IPixelFilter;

                //如果输入的栅格中并不包含NoData 和 曾经使用过的最大像素深度，则 为输出文件的像素深度和NoData赋值
                IRasterProps rasterProps = raster as IRasterProps;
                rasterProps.NoDataValue = 0;
                rasterProps.PixelType = rstPixelType.PT_USHORT;

                //存储裁剪结果栅格图像
                IWorkspaceFactory wsf = new RasterWorkspaceFactoryClass();
                IWorkspace rstWS = wsf.OpenFromFile(@"D:\hhx\rasterClipOutput", 0);
                //保存输出
                ISaveAs saveas = raster as ISaveAs;
                saveas.SaveAs("clip_result.tif", rstWS, "TIFF");

                //代码填空，加载显示裁剪结果图像
                IRasterLayer newRasterLayer = new RasterLayerClass();
                newRasterLayer.CreateFromRaster(raster);
                ILayer newLayer = newRasterLayer as ILayer;
                axMapControl1.Map.AddLayer(newLayer);
                axMapControl1.ActiveView.Extent = newLayer.AreaOfInterest;
                axMapControl1.ActiveView.Refresh();
                axTOCControl1.Update();

                //更新combobox里面的选项（图层和波段的）
                iniCmbItems();
                MessageBox.Show("图像裁剪成功", "提示");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //2.图像融合
        //点击按钮实现高空间分辨率单波段图像 和 低空间分辨率多波段图像的 融合操作
        private void btn_PanSharpen_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取选择的全色和多波段的栅格图像的dataset
                int panLayerIndex = cmb_SingleBandLayer.SelectedIndex;
                int multiLayerIndex = cmb_MultiBandLayer.SelectedIndex;
                ILayer panLayer = axMapControl1.Map.get_Layer(panLayerIndex);
                ILayer multiLayer = axMapControl1.Map.get_Layer(multiLayerIndex);
                IRasterLayer panRasterLayer = panLayer as IRasterLayer;
                IRasterLayer multiRasterLayer = multiLayer as IRasterLayer;
                IRasterDataset panDataset = (panRasterLayer.Raster as IRaster2).RasterDataset;
                IRasterDataset multiDataset = (multiRasterLayer.Raster as IRaster2).RasterDataset;

                //假设波段顺序：RGB和近红外
                //创建全色和多光谱栅格数据集的full栅格对象
                IRaster panRaster = (panDataset as IRasterDataset2).CreateFullRaster();
                IRaster multiRaster = (multiDataset as IRasterDataset2).CreateFullRaster();
                //设置红外波段
                IRasterBandCollection rasterbandCol = multiRaster as IRasterBandCollection;
                IRasterBandCollection infredRaster = new RasterClass();
                infredRaster.AppendBand(rasterbandCol.Item(3));

                //设置全色波段的属性
                IRasterProps panSharpenRasterProps = multiRaster as IRasterProps;
                IRasterProps panRasterProps = panRaster as IRasterProps;
                panSharpenRasterProps.Width = panRasterProps.Width;
                panSharpenRasterProps.Height = panRasterProps.Height;
                panSharpenRasterProps.Extent = panRasterProps.Extent;
                multiRaster.ResampleMethod = rstResamplingTypes.RSP_BilinearInterpolation;

                //创建全色锐化过滤器和设置其参数
                IPansharpeningFilter pansharpenFilter = new PansharpeningFilterClass();
                pansharpenFilter.InfraredImage = infredRaster as IRaster;
                pansharpenFilter.PanImage = panRaster;
                pansharpenFilter.PansharpeningType = esriPansharpeningType.esriPansharpeningESRI;
                pansharpenFilter.PutWeights(0.166, 0.167, 0.167, 0.5);

                //将全色锐化过滤器设置于多光谱栅格对象上
                IPixelOperation pixelOperation = multiRaster as IPixelOperation;
                pixelOperation.PixelFilter = pansharpenFilter as IPixelFilter;

                //代码填空，保存结果数据集，并加载显示
                ISaveAs saveas = multiRaster as ISaveAs;
                saveas.SaveAs(@"D:\hhx\rasterOutput\panSharpen_result.tif", null, "TIFF");
                IRasterLayer newRasterLayer = new RasterLayerClass();
                newRasterLayer.CreateFromRaster(multiRaster);
                ILayer newLayer = newRasterLayer as ILayer;
                axMapControl1.Map.AddLayer(newLayer);
                axMapControl1.ActiveView.Extent = newLayer.AreaOfInterest;
                axMapControl1.ActiveView.Refresh();
                axTOCControl1.Update();

                //更新combobox里面的选项
                iniCmbItems();
                MessageBox.Show("图像融合成功", "提示");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        //3.图像镶嵌
        //点击镶嵌按钮，对选中的栅格目录的遥感影像进行镶嵌处理
        /// <summary>
        /// 方法1是 (只要点下按钮就行了，不用输入东西)从0开始创建 GDB，创建栅格目录，往栅格目录中加影像，之后 设置mosaicRaster的rasterCatalog属性，在saveas时执行。注意：按钮只能点一次，多点会报错，说文件被另一个进程访问。
        /// 方法2是 利用现有ArcSDE中的栅格目录！！！有【BUG】！！！！！！！！！！！！
        /// </1summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Mosaic_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int catalogIndex = cmb_MosaicRstCatalog.SelectedIndex;//加入没有选择 返回-1，选择了临时数据库，返回0。都用方法1

                if (catalogIndex < 1)//方法1，临时个人数据库
                {
                    //定义待用的字符串变量，表示原栅格数据文件夹路径，结果栅格数据保存路径
                    //特别地，先创建一个 个人地理数据库，再在其中创建一个栅格目录
                    //我怀疑助教 inputFolder是包含了2张被镶嵌的影像，outputFolder包含了临时数据库（数据库里面有catalog） 和 输出结果文件
                    string inputFolder = @"d:\hhx\mosaic";
                    string tempPGDB = "temp.mdb";
                    string outputFolder = @"d:\hhx\mosaicResult";//略有改动
                    string outputName = "mosaic.tif";
                    string tempPGDBPath = outputFolder + "\\" + tempPGDB;
                    string tempRasterCatalog = "temp_rc";
                    string tempRasterCatalogPath = tempPGDBPath + "\\" + tempRasterCatalog;

                    //自己新增，每次运行前都删除掉 旧的数据库和 旧的结果文件,否则已经存在数据库情况下，再建立一个会报错...
                    DeleteDir(outputFolder);

                    //使用Geoprocessor来创建地理数据库、栅格目录、以及加载目录到栅格目录中
                    //本宝宝真机智~
                    ESRI.ArcGIS.Geoprocessor.Geoprocessor geoProcessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();

                    //在临时文件夹中创建 个人地理数据库
                    CreatePersonalGDB createPersonalGDB = new CreatePersonalGDB();
                    createPersonalGDB.out_folder_path = outputFolder;
                    createPersonalGDB.out_name = tempPGDB;
                    //调用GeoProcessor 的 execute方法执行创建个人地理数据库
                    geoProcessor.Execute(createPersonalGDB, null);

                    //在新创建的个人地理数据库中创建一个非托管的栅格目录
                    CreateRasterCatalog createRasterCatalog = new CreateRasterCatalog();
                    //设置创建的栅格目录的输出路径、输出名字和栅格托管类型
                    createRasterCatalog.out_path = tempPGDBPath;
                    createRasterCatalog.out_name = tempRasterCatalog;
                    createRasterCatalog.raster_management_type = "unmanaged";
                    //调用GeoProcessor的execute方法执行 创建栅格目录
                    geoProcessor.Execute(createRasterCatalog, null);

                    //把用于镶嵌的原始栅格数据 加载到 新创建的非托管的 栅格目录中
                    WorkspaceToRasterCatalog wsToRasterCatalog = new WorkspaceToRasterCatalog();
                    //设置 加载栅格数据的 栅格目录路径、栅格数据路径、加载的类型（是否包含子文件夹）
                    wsToRasterCatalog.in_raster_catalog = tempRasterCatalogPath;
                    wsToRasterCatalog.in_workspace = inputFolder;
                    wsToRasterCatalog.include_subdirectories = "INCLUDE_SUBDIRECTORIES";
                    //调用GeoProcessor的execute方法 执行加载栅格数据 到 栅格目录中
                    geoProcessor.Execute(wsToRasterCatalog, null);

                    //代码填空，打开刚刚创建的personalGDB，打开和获取rasterCatalog
                    IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
                    IWorkspace ws = workspaceFactory.OpenFromFile(tempPGDBPath, 0);// .mdb
                    IRasterWorkspaceEx rstWorkspaceEx = ws as IRasterWorkspaceEx;
                    IRasterCatalog rasterCatalog = rstWorkspaceEx.OpenRasterCatalog(tempRasterCatalog);

                    //把栅格目录中所有栅格图像镶嵌成为 一个栅格图像/栅格数据集
                    IMosaicRaster mosaicRaster = new MosaicRasterClass();
                    mosaicRaster.RasterCatalog = rasterCatalog;

                    //代码填空 设置镶嵌的颜色映射表模式(colormapmode)和像素值运算类型(operatortype)
                    mosaicRaster.MosaicColormapMode = rstMosaicColormapMode.MM_MATCH;//我随便选了一个...
                    mosaicRaster.MosaicOperatorType = rstMosaicOperatorType.MT_BLEND;//随便选一个...

                    //代码填空，打开和获取 结果数据集保存的工作空间
                    IWorkspaceFactory wsf = new RasterWorkspaceFactoryClass();
                    IWorkspace wsOutput = wsf.OpenFromFile(outputFolder, 0);

                    //代码填空，保存结果数据集，并在图层中加载显示
                    ISaveAs saveas = mosaicRaster as ISaveAs;
                    saveas.SaveAs(outputName, wsOutput, "TIFF");

                    IRasterLayer rasterLayer = new RasterLayerClass();

                    //string str = outputFolder + "\\"+ outputName;//我都傻了，少了中间的斜杠...
                    rasterLayer.CreateFromFilePath(outputFolder + "\\" + outputName);//d:\hhx\mosaicResult\mosaic.tif
                    ILayer layer = rasterLayer as ILayer;

                    axMapControl1.Map.AddLayer(layer);
                    axMapControl1.ActiveView.Extent = layer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    //更新combobox里面的选项
                    iniCmbItems();
                    MessageBox.Show("图像镶嵌成功", "提示");



                }
                else//方法2，SDE数据库
                {
                    string catalogName = cmb_MosaicRstCatalog.SelectedItem.ToString();
                    if (catalogName != "mosaic")
                    {
                        MessageBox.Show("请选择mosaic栅格目录", "提示");
                        return;
                    }
                    IRasterWorkspaceEx rasterWorkspaceEx = workspace as IRasterWorkspaceEx;
                    IRasterCatalog rasterCatalog = rasterWorkspaceEx.OpenRasterCatalog(catalogName);

                    string outputFolder = @"d:\hhx\mosaicResult";//略有改动
                    string outputName = "mosaic.tif";

                    //把栅格目录中所有栅格图像镶嵌成为 一个栅格图像/栅格数据集
                    IMosaicRaster mosaicRaster = new MosaicRasterClass();
                    mosaicRaster.RasterCatalog = rasterCatalog;

                    //代码填空 设置镶嵌的颜色映射表模式(colormapmode)和像素值运算类型(operatortype)
                    mosaicRaster.MosaicColormapMode = rstMosaicColormapMode.MM_MATCH;//我随便选了一个...
                    mosaicRaster.MosaicOperatorType = rstMosaicOperatorType.MT_BLEND;//随便选一个...

                    //代码填空，打开和获取 结果数据集保存的工作空间
                    IWorkspaceFactory wsf = new RasterWorkspaceFactoryClass();
                    IWorkspace wsOutput = wsf.OpenFromFile(outputFolder, 0);

                    //自己新增，每次运行前都删除掉 旧的数据库和 旧的结果文件,否则已经存在数据库情况下，再建立一个会报错...
                    DeleteDir(outputFolder);

                    //代码填空，保存结果数据集，并在图层中加载显示
                    ISaveAs2 saveas = mosaicRaster as ISaveAs2;
                    //saveas.SaveAs(outputName, wsOutput, "TIFF");
                    //下面这句话会报错？
                    //完成该操作所需的数据还不可使用
                    saveas.SaveAs("d:/hhx/mosaicResult/mosaic.tif", null, "TIFF");
                    //saveas.SaveAsRasterDataset("d:/hhx/mosaicResult/mosaic.tif", null, "TIFF", null);

                    IRasterLayer rasterLayer = new RasterLayerClass();

                    //string str = outputFolder + "\\"+ outputName;//我都傻了，少了中间的斜杠...
                    rasterLayer.CreateFromFilePath(outputFolder + "\\" + outputName);//d:\hhx\mosaicResult\mosaic.tif
                    ILayer layer = rasterLayer as ILayer;

                    axMapControl1.Map.AddLayer(layer);
                    axMapControl1.ActiveView.Extent = layer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    //更新combobox里面的选项
                    iniCmbItems();
                    MessageBox.Show("图像镶嵌成功", "提示");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //4.卷积运算
        //点击滤波按钮，对选中图层的选中波段执行滤波操作
        //用栅格函数实现的
        private void btn_Filter_Click(object sender, EventArgs e)
        {
            try
            {
                //方案一  栅格函数  可运行
                this.Cursor = Cursors.WaitCursor;
                //代码填空，获取选中的图层
                int layerIndex = cmb_FilterLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                IRasterLayer rstLayer = layer as IRasterLayer;
                IRaster raster = rstLayer.Raster;
                int filterMethod = cmb_FilterMethod.SelectedIndex < 0 ? 0 : cmb_FilterMethod.SelectedIndex;
                string filterMethodName = cmb_FilterMethod.SelectedItem.ToString()=="" ? "NoMethod" : cmb_FilterMethod.SelectedItem.ToString();

                string afrFullPath = @"d:\hhx\rasterFilterOutput\Convolution.afr";
                string outputPath = @"d:\hhx\rasterFilterOutput\Convolution" + filterMethodName + ".tif";

                if (filterBtnFirstClick)
                {
                    filterBtnFirstClick = false;
                    DeleteDir(@"d:\hhx\rasterFilterOutput");//只在第一次清空，之后第二次清空会报错的...因为要删除的文件被进程锁住了
                }
                

                //代码填空，采用IFunctionRasterDataset,IRasterFunction,IRasterFunctionArguments,IFunctionRasterDatasetName 实现卷积运算，并且加载显示图层
                //函数参数
                IConvolutionFunctionArguments rasterFunctionArguments = new ConvolutionFunctionArgumentsClass();
                rasterFunctionArguments.Type = (esriRasterFilterTypeEnum)filterMethod;
                //input data can be of type IRasterDataset,IRasterBand,or IRaster
                rasterFunctionArguments.Raster = raster;
                //栅格函数
                IRasterFunction rasterFunction = new ConvolutionFunctionClass();
                //函数栅格数据集
                IFunctionRasterDataset functionRasterDataset = new FunctionRasterDatasetClass();
                //函数栅格数据集名称
                IFunctionRasterDatasetName functionRasterDatasetName = new FunctionRasterDatasetNameClass();
                functionRasterDatasetName.FullName = afrFullPath;//输出文件路径【.afr结尾】
                functionRasterDataset.FullName = functionRasterDatasetName as IName;
                //init
                functionRasterDataset.Init(rasterFunction, rasterFunctionArguments);

                IRasterDataset rasterDataset = functionRasterDataset as IRasterDataset;
                ISaveAs saveas = rasterDataset as ISaveAs;
                saveas.SaveAs(outputPath, null, "TIFF");//@"d:\hhx\rasterFilterOutput\Convolution.tif"

                IRasterLayer rasterLayer = new RasterLayerClass();
                rasterLayer.CreateFromFilePath(outputPath);
                //432 RGB 彩色合成
                IRasterRGBRenderer rasterRGBRender = null;
                if (rasterLayer.Renderer is IRasterRGBRenderer)
                {
                    rasterRGBRender = rasterLayer.Renderer as IRasterRGBRenderer;
                }
                else
                {
                    rasterRGBRender = new RasterRGBRendererClass();
                }
                rasterRGBRender.RedBandIndex = 3;
                rasterRGBRender.GreenBandIndex = 2;
                rasterRGBRender.BlueBandIndex = 1;
                rasterLayer.Renderer = rasterRGBRender as IRasterRenderer;

                ILayer newLayer = rasterLayer as ILayer;
                axMapControl1.Map.AddLayer(newLayer);
                axMapControl1.ActiveView.Extent = newLayer.AreaOfInterest;
                axMapControl1.ActiveView.Refresh();
                axTOCControl1.Update();

                iniCmbItems();
                MessageBox.Show("图像滤波成功！", "提示");
                

                /****
                //方案二 测试通过 可用
                int layerIndex = cmb_FilterLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                IRasterLayer rstLayer = layer as IRasterLayer;
                IRaster raster = rstLayer.Raster;
                int filterMethod = cmb_FilterMethod.SelectedIndex < 0 ? 0 : cmb_FilterMethod.SelectedIndex;
                string filterMethodName = cmb_FilterMethod.SelectedItem.ToString() == "" ? "NoMethod" : cmb_FilterMethod.SelectedItem.ToString();
                string outputPath = @"d:\hhx\rasterFilterOutput\Convolution" + filterMethodName + ".tif";

                if (filterBtnFirstClick)
                {
                    filterBtnFirstClick = false;
                    DeleteDir(@"d:\hhx\rasterFilterOutput");//只在第一次清空，之后第二次清空会报错的...因为要删除的文件被进程锁住了
                }

                IStockConvolutionFilter stockConvolutionFilter = new RasterConvolutionFilterClass();
                stockConvolutionFilter.Type = (esriRasterFilterTypeEnum)filterMethod;
                IPixelOperation pixelOperation = raster as IPixelOperation;
                pixelOperation.PixelFilter = stockConvolutionFilter as IPixelFilter;

                ISaveAs saveas = raster as ISaveAs;
                saveas.SaveAs(outputPath, null, "TIFF");

                IRasterLayer rasterLayer = new RasterLayerClass();
                rasterLayer.CreateFromFilePath(outputPath);
                //432 RGB 彩色合成
                IRasterRGBRenderer rasterRGBRender = null;
                if (rasterLayer.Renderer is IRasterRGBRenderer)
                {
                    rasterRGBRender = rasterLayer.Renderer as IRasterRGBRenderer;
                }
                else
                {
                    rasterRGBRender = new RasterRGBRendererClass();
                }
                rasterRGBRender.RedBandIndex = 3;
                rasterRGBRender.GreenBandIndex = 2;
                rasterRGBRender.BlueBandIndex = 1;
                rasterLayer.Renderer = rasterRGBRender as IRasterRenderer;

                ILayer newLayer = rasterLayer as ILayer;
                axMapControl1.Map.AddLayer(newLayer);
                axMapControl1.ActiveView.Extent = newLayer.AreaOfInterest;
                axMapControl1.ActiveView.Refresh();
                axTOCControl1.Update();

                iniCmbItems();
                MessageBox.Show("图像滤波成功！", "提示");
                ****/
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                this.Cursor = Cursors.Default;
            }

        }


        //5.图像变换
        //点击变换按钮，对选中的图层实施图像变换操作
        private void btn_Transform_Click(object sender, EventArgs e)
        {
            try
            {
                //代码填空，获取选中的图层，获取输入的角度，转换接口IGeodataset
                int layerIndex = cmb_TransformLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                IRasterLayer rstLayer = layer as IRasterLayer;
                IRaster raster = rstLayer.Raster;
                IGeoDataset geoDataset = raster as IGeoDataset;
                string transformMethodName = cmb_TransformMethod.SelectedItem.ToString();

                //创建栅格图像变换操作接口的对象
                ITransformationOp transop = new RasterTransformationOpClass();
                //定义输出地理数据集的对象
                IGeoDataset outdataset = null;
                switch (cmb_TransformMethod.SelectedIndex)
                { 
                    case 0://翻转  X轴对称
                        outdataset = transop.Flip(geoDataset);
                        break;
                    case 1://镜像  Y轴对称
                        outdataset = transop.Mirror(geoDataset);
                        break;
                    case 2://裁剪
                        fClip = true;
                        MessageBox.Show("请使用鼠标在图上绘制（鼠标按住不要放，拖动出一个矩形）","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);;
                        return;
                    case 3://旋转
                        object missing = Type.Missing;
                        if (txb_angle.Text.Trim() == "") 
                        {
                            MessageBox.Show("请输入旋转角度", "提示");
                            return;
                        }
                        double angle = Convert.ToDouble(txb_angle.Text.Trim());
                        outdataset = transop.Rotate(geoDataset, esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleNearest, angle, ref missing);
                        break;
                    default:
                        return;
                }
                //通过图像变换结果 获取栅格数据集，进而创建栅格图层 加以显示
                //获取结果数据集  GeoDataset->RasterDataSet
                IRasterDataset rasterDataset = outdataset as IRasterDataset;
                IRaster outRaster = rasterDataset.CreateDefaultRaster();
                //将结果保存到一个RasterLayer
                IRasterLayer resRasterLayer = new RasterLayerClass();
                resRasterLayer.CreateFromRaster(outRaster);
                ILayer resLayer = resRasterLayer as ILayer;
                resLayer.Name = transformMethodName + txb_angle.Text.Trim();
                
                axMapControl1.Map.AddLayer(resLayer);
                //axMapControl1.ActiveView.Extent = resLayer.AreaOfInterest;
                axMapControl1.ActiveView.Refresh();
                axTOCControl1.Update();

                iniCmbItems();
                MessageBox.Show("图像变换成功！", "提示");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cmb_TransformMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            int methodIndex = cmb_TransformMethod.SelectedIndex;
            if (methodIndex == 3) //旋转需要输入角度
            {
                txb_angle.ReadOnly = false;
            }
        }

        //2017/11/23
        //1.坡度分析函数
        //点击坡度函数按钮，对选中的DEM数据进行坡度计算
        //???使用functionRasterDataset的图例是0-90???偏暗
        //???使用ISurfaceOp2 的图例是0-实际最大值44左右???
        //两者显示效果不同 但数值是相同的
        //!有时间 去试一下 改变render 使得显示效果相同!
        private void btn_Slope_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region method 1 use functionRasterDataset
                ///****use functionRasterDataset****/
                ////1.获取选中的Dem图层
                //int layerIndex = cmb_SlopeLayer.SelectedIndex;
                //ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                //if (layer is IRasterLayer)
                //{
                //    IRasterLayer rstLayer = layer as IRasterLayer;
                //    IRaster raster = rstLayer.Raster;

                //    string outputPath = @"d:\hhx\slope\slope.afr";
                //    DeleteDir(@"d:\hhx\slope");
                //    //2.利用IFunctionRasterDataset一套接口实现坡度函数
                //    IRasterFunction slopeFunction = new SlopeFunctionClass();
                //    ISlopeFunctionArguments slopeFunctionArguments = new SlopeFunctionArgumentsClass();
                //    slopeFunctionArguments.DEM = raster;//IRasterDataset IRasterBand IRaster
                //    slopeFunctionArguments.ZFactor = 1 / 111111.0;
                //    IFunctionRasterDataset functionRasterDataset = new FunctionRasterDatasetClass();
                //    IFunctionRasterDatasetName functionRasterDatasetName = new FunctionRasterDatasetNameClass();
                //    functionRasterDatasetName.FullName = outputPath;
                //    functionRasterDataset.FullName = functionRasterDatasetName as IName;
                //    functionRasterDataset.Init(slopeFunction, slopeFunctionArguments);

                //    //3.可选择直接加载图层显示.或者保存图像到文件系统.或者SDE数据库
                //    //此处不保存 只是显示
                //    IRasterLayer rasterLayer = new RasterLayerClass();
                //    rasterLayer.CreateFromDataset(functionRasterDataset as IRasterDataset);

                //    //4.刷新显示视图
                //    axMapControl1.Map.AddLayer(rasterLayer as ILayer);
                //    axMapControl1.ActiveView.Refresh();
                //    axTOCControl1.Update();

                //    //更新波段信息统计的图层和波段下拉框选项内容
                //    iniCmbItems();
                //}
                ///*******end*****/
                #endregion

                #region method 2 use ISurfaceOp2
                /****use RasterSurfaceOpClass  ISurfaceOp2 ****/
                int layerIndex = cmb_SlopeLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;

                    ////may be use for saveas
                    ////string outputPath = @"d:\hhx\slope\slope.afr";
                    ////DeleteDir(@"d:\hhx\slope");

                    ////开始我的表演
                    ISurfaceOp2 surfaceOp2 = new RasterSurfaceOpClass();
                    IGeoDataset resDataset = surfaceOp2.Slope(geoDataset, esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopeDegrees, 1 / 111111.0);
                    IRasterLayer resLayer = new RasterLayerClass();
                    resLayer.CreateFromRaster(resDataset as IRaster);
                    axMapControl1.Map.AddLayer(resLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                }
                /******end *****/
                #endregion

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                this.Cursor = Cursors.Default;
            }
        }

        //2.坡向分析函数
        private void btn_Aspect_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region method 1 use IFunctionRasterDataset
                ///****use IFunctionRasterDataset****/
                ////1.getlayer
                //int layerIndex = cmb_AspectLayer.SelectedIndex;
                //ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                //if (layer is IRasterLayer)
                //{
                //    IRasterLayer rstLayer = layer as IRasterLayer;
                //    IRaster raster = rstLayer.Raster;
                //    string outpath = @"d:\hhx\aspect\aspect.afr";
                //    DeleteDir(@"d:\hhx\aspect");
                //    //2.functionRasterDataset
                //    IRasterFunction aspectFunction = new AspectFunctionClass();
                //    //震惊 这家伙没有参数 或者说函数参数就是 输入的栅格数据
                //    IFunctionRasterDataset functionRasterDataset = new FunctionRasterDatasetClass();
                //    IFunctionRasterDatasetName functionRasterDatasetName = new FunctionRasterDatasetNameClass();
                //    functionRasterDatasetName.FullName = outpath;
                //    functionRasterDataset.FullName = functionRasterDatasetName as IName;
                //    functionRasterDataset.Init(aspectFunction, raster);

                //    //3.add it and refresh map
                //    IRasterLayer rasterLayer = new RasterLayerClass();
                //    rasterLayer.CreateFromDataset(functionRasterDataset as IRasterDataset);
                //    axMapControl1.Map.AddLayer(rasterLayer as ILayer);
                //    axMapControl1.ActiveView.Refresh();
                //    axTOCControl1.Update();

                //    //更新波段信息统计的图层和波段下拉框选项内容
                //    iniCmbItems();
                //}
                ///****end****/
                #endregion

                #region method 2 use ISurfaceOp2
                /****use RasterSurfaceOpClass  ISurfaceOp2 ****/
                int layerIndex = cmb_AspectLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    //may be use for saveas
                    //string outpath = @"d:\hhx\aspect\aspect.afr";
                    //DeleteDir(@"d:\hhx\aspect");
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    //开始了
                    ISurfaceOp2 surfaceOp2 = new RasterSurfaceOpClass();
                    IGeoDataset resDataset = surfaceOp2.Aspect(geoDataset);
                    IRasterLayer resLayer = new RasterLayerClass();
                    resLayer.CreateFromRaster(resDataset as IRaster);
                    axMapControl1.Map.AddLayer(resLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                }

                /****end****/
                #endregion

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //3.山体阴影函数
        private void btn_HillShade_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                #region method 1 use IFunctionRasterDataset
                ///****use IFunctionRasterDataset****/
                ////get layer
                //int layerIndex = cmb_HillshadeLayer.SelectedIndex;
                //ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                //if (layer is IRasterLayer)
                //{
                //    IRasterLayer rstLayer = layer as IRasterLayer;
                //    IRaster raster = rstLayer.Raster;
                //    string outputPath = @"d:\hhx\hillshade\hillshade.afr";
                //    DeleteDir(@"d:\hhx\hillshade");
                //    //use IfunctionRasterDataset
                //    IRasterFunction hillshadeFunction = new HillshadeFunctionClass();
                //    IHillshadeFunctionArguments hillshadeFunctionArguments = new HillshadeFunctionArgumentsClass();
                //    hillshadeFunctionArguments.DEM = raster;
                //    hillshadeFunctionArguments.Altitude = 45;//太阳高度角
                //    hillshadeFunctionArguments.Azimuth = 315;//方位角
                //    hillshadeFunctionArguments.ZFactor = 1 / 111111.0;
                //    IFunctionRasterDataset functionRasterDataset = new FunctionRasterDatasetClass();
                //    IFunctionRasterDatasetName functionRasterDatasetName = new FunctionRasterDatasetNameClass();
                //    functionRasterDatasetName.FullName = outputPath;
                //    functionRasterDataset.FullName = functionRasterDatasetName as IName;
                //    functionRasterDataset.Init(hillshadeFunction, hillshadeFunctionArguments);
                //    //add layer and refresh
                //    IRasterLayer rasterLayer = new RasterLayerClass();
                //    rasterLayer.CreateFromDataset(functionRasterDataset as IRasterDataset);
                //    axMapControl1.Map.AddLayer(rasterLayer as ILayer);
                //    axMapControl1.ActiveView.Refresh();
                //    axTOCControl1.Update();

                //    //更新波段信息统计的图层和波段下拉框选项内容
                //    iniCmbItems();
                //}
                ///****end ****/
                #endregion

                #region method 2 use ISurfaceOp2
                /****use RasterSurfaceOpClass  ISurfaceOp2 ****/
                int layerIndex = cmb_AspectLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    //may be use for saveas
                    //string outpath = @"d:\hhx\aspect\aspect.afr";
                    //DeleteDir(@"d:\hhx\aspect");
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    //开始了
                    ISurfaceOp2 surfaceOp2 = new RasterSurfaceOpClass();
                    IGeoDataset resDataset = surfaceOp2.HillShade(geoDataset, 315, 45, true, 1 / 111111.0);//look here!!!
                    IRasterLayer resLayer = new RasterLayerClass();
                    resLayer.CreateFromRaster(resDataset as IRaster);
                    axMapControl1.Map.AddLayer(resLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                }

                /****end****/
                #endregion

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        //4.邻域分析  INeighborhoodOp
        private void btn_Neighborhood_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //获取选中的DEM图层及其数据
                int layerIndex = cmb_NeighborhoodRasterLayer.SelectedIndex;
                int indexMethod = cmb_NeighborhoodMethod.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                { 
                    //从Rasterlayer中获取raster 转换IGeodataset接口
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;

                    //创建栅格数据 邻域分析 操作相关的类对象
                    INeighborhoodOp pNeighborhoodOP = new RasterNeighborhoodOpClass();
                    //创建栅格数据 邻域分析 参数对象
                    IRasterNeighborhood pRasterNeighborhood = new RasterNeighborhoodClass();
                    //设置矩形邻域分析范围
                    pRasterNeighborhood.SetRectangle(3, 3, esriGeoAnalysisUnitsEnum.esriUnitsCells);
                    IGeoDataset pGeoOutput = null;
                    //执行邻域分析操作得到结果数据集
                    #region switch
                    switch (indexMethod)
                    {
                        case 0://esriGeoAnalysisFilter3x3HighPass
                            pGeoOutput = pNeighborhoodOP.Filter(geoDataset,esriGeoAnalysisFilterEnum.esriGeoAnalysisFilter3x3HighPass, true);
                            break;
                        case 1://esriGeoAnalysisFilter3x3LowPass
                            pGeoOutput = pNeighborhoodOP.Filter(geoDataset,esriGeoAnalysisFilterEnum.esriGeoAnalysisFilter3x3LowPass, true);
                            break;
                            //完成剩余的case 还可以加不同类型的BlockStatistics FocalStatistics略掉 代码太长了不好玩 
                        case 2://esriGeoAnalysisStatsLength
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsLength, pRasterNeighborhood, true);
                            break;
                        case 3://esriGeoAnalysisStatsMajority
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMajority, pRasterNeighborhood, true);
                            break;
                        case 4://esriGeoAnalysisStatsMaximum
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMaximum, pRasterNeighborhood, true);
                            break;
                        case 5://esriGeoAnalysisStatsMean
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMean, pRasterNeighborhood, true);
                            break;
                        case 6://esriGeoAnalysisStatsMedian
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMedian, pRasterNeighborhood, true);
                            break;
                        case 7://esriGeoAnalysisStatsMinimum
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMinimum, pRasterNeighborhood, true);
                            break;
                        case 8://esriGeoAnalysisStatsMinority
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMinority, pRasterNeighborhood, true);
                            break;
                        case 9://esriGeoAnalysisStatsRange
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsRange, pRasterNeighborhood, true);
                            break;
                        case 10://esriGeoAnalysisStatsStd
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsStd, pRasterNeighborhood, true);
                            break;
                        case 11://esriGeoAnalysisStatsSum
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsSum, pRasterNeighborhood, true);
                            break;
                        case 12://esriGeoAnalysisStatsVariety
                            pGeoOutput = pNeighborhoodOP.BlockStatistics(geoDataset, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsVariety, pRasterNeighborhood, true);
                            break;
                        default:
                            break;

                    }
                    #endregion
                    //加载显示邻域分析 处理后的栅格图像
                    IRasterLayer resultRstLayer = new RasterLayerClass();
                    //【注意】下面这行很关键~~~pGeoOutput是内存中执行后产生的 而IRaster是对栅格数据在内存中的表示
                    //要是把pGeoOutput 转成 IRasterDataset 就会是null
                    resultRstLayer.CreateFromRaster(pGeoOutput as IRaster);
                    ILayer resultLayer = resultRstLayer as ILayer;
                    axMapControl1.Map.AddLayer(resultLayer);
                    axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    //更新 图层 和 波段 下拉框
                    iniCmbItems();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        //5.裁剪分析函数
        //点击裁剪分析的按钮 进行裁剪分析的函数  //大约在2588行 map mousedown事件
        private void btn_Extraction_Click(object sender, EventArgs e)
        {
            fExtraction = true;
            MessageBox.Show("请使用鼠标在图上绘制", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        //2017/11/24
        //1.通视分析
        //点击通视分析按钮 进行通视分析的函数
        //ISurface GetLineOfSight 注意不带Op
        //RasterSurfaceClass IRasterSurface ISurface
        //map trackline() 得到一条线 进而得到fromPoint ToPoint
        //GetLineOfSight得到 可见的线 以及 不可见的线 绘制出来
        private void btn_LineOfSight_Click(object sender, EventArgs e)
        {
            fLineOfSight = true;
            MessageBox.Show("请使用鼠标在图上绘制", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        //2.视域分析 约2701行 map mouse down
        //关键在于如何得到用户在屏幕上点击的一个点
        //助教的方法如下:
        //1.featureWorkspace 创建 featureclass,这一步需要fields信息(包括OID、几何字段、名称字段),其中的几何字段 还需要spatialReference..
        //2.用featureclass 创建 feature,并将屏幕上点击的点 构建一个IPoint 赋值给feature.Shape
        //3.IsurfaceOp2 的 visibility方法
        private void btn_Visibility_Click(object sender, EventArgs e)
        {
            fVisibility = true;
            MessageBox.Show("请使用鼠标在图上绘制", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        //3.构建TIN
        private void btn_CreateTIN_Click(object sender, EventArgs e)
        {
            fTIN = true;
            int layerIndex = cmb_CreateTinLayer.SelectedIndex;
            ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
            if (layer is IRasterLayer)
            {
                IRasterLayer pRasterLayer = layer as IRasterLayer;
                TinEdit = new TinClass();
                //用一个envelope初始化tin.envelope的空间参考也成为tin的空间参考
                IEnvelope Env = pRasterLayer.AreaOfInterest;
                //根据envelope初始化tinedit
                TinEdit.InitNew(Env);
                //获取map, 清除所有的element的标记marker
                IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
                pGra.DeleteAllElements();
            }
            MessageBox.Show("请使用鼠标在图上绘制", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        //4 为DEM生成等高线 RasterSurfaceOpClass ISurfaceOp2
        private void btn_Contour_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int layerIndex = cmb_ContourLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    //创建表面操作处理对象
                    ISurfaceOp2 sfOp = new RasterSurfaceOpClass();
                    //根据设置的等高线起点高度、高度间距等来对DEM数据构建等高线
                    IGeoDataset outputGeo = sfOp.Contour(geoDataset, 100, Type.Missing, Type.Missing);
                    //加载显示获得的等高线featureclass(outputGeo对象)
                    IFeatureLayer featureLayer = new FeatureLayerClass();
                    featureLayer.FeatureClass = outputGeo as IFeatureClass;
                    featureLayer.Name = "DemContour";
                    axMapControl1.Map.AddLayer(featureLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }


        //5.为TIN生成等高线 ITinSurface
        private void btn_TinContour_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int layerIndex = cmb_TinContourLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is ITinLayer)
                {
                    ITinLayer tinLayer = layer as ITinLayer;
                    ITin tin = tinLayer.Dataset;
                    ITinSurface tinSurface = tin as ITinSurface;

                    //实例化要素描述对象来获取 构建要素类必须的字段集合
                    IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass();
                    IObjectClassDescription ocDescription = fcDescription as IObjectClassDescription;
                    IFields fields = ocDescription.RequiredFields;
                    //找到shape形状字段， 设置几何类型 投影坐标系统
                    int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);
                    IField field = fields.get_Field(shapeFieldIndex);
                    IGeometryDef geometryDef = field.GeometryDef;
                    IGeometryDefEdit geometryDefEdit = geometryDef as IGeometryDefEdit;
                    //设置几何类型
                    geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline;
                    ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                    ISpatialReference spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRProjCS_NAD1983UTM_20N);
                    ISpatialReferenceResolution spatialReferenceResolution = spatialReference as ISpatialReferenceResolution;
                    spatialReferenceResolution.ConstructFromHorizon();
                    spatialReferenceResolution.SetDefaultXYResolution();
                    ISpatialReferenceTolerance spatialReferenceTolerance = spatialReference as ISpatialReferenceTolerance;
                    spatialReferenceTolerance.SetDefaultXYTolerance();
                    //设置坐标系统
                    geometryDefEdit.SpatialReference_2 = spatialReference;

                    //转换工作空间接口 watch out!connect DB first
                    //IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;

                    IFeatureWorkspace featureWorkspace = null;
                    if (workspace is IFeatureWorkspace)//true when connect DB before,workspace is create by SDEWorkspaceFactory
                    {
                        featureWorkspace = workspace as IFeatureWorkspace;//所以这句成立的前提是  SDE连接了Oracle数据库
                    }
                    else//load image from file directly,NOT connect DB,workspace = null
                    {
                        string outputFolder = @"d:\hhx\tinContour";
                        string tempGDB = "temp.gdb";
                        //DeleteDir(outputFolder);
                        //直接用C# File 判断
                        if (!Directory.Exists(outputFolder + "\\" + tempGDB))
                        {
                            //使用Geoprocessor来创建地理数据库 参考mdb的创建
                            ESRI.ArcGIS.Geoprocessor.Geoprocessor geoProcessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();

                            CreateFileGDB createFileGDB = new CreateFileGDB();
                            createFileGDB.out_folder_path = outputFolder;
                            createFileGDB.out_name = tempGDB;
                            geoProcessor.Execute(createFileGDB, null);

                        }
                        IWorkspaceFactory tmpworkspaceFactory = new FileGDBWorkspaceFactoryClass();
                        //临时工作空间 会在此文件夹下面产生一个 临时文件 
                        IWorkspace tmpworkspace = tmpworkspaceFactory.OpenFromFile(outputFolder + "\\" + tempGDB, 0);//有了才可以打开

                        featureWorkspace = tmpworkspace as IFeatureWorkspace;//all work for here
                    }

                    //hhx 假如featureclass已经存在 再次创建会报错
                    //方案一 先删除后创建 。用 iWorkspace2 nameExist判断存在
                    string featureClassName = "tinContour";
                    IWorkspace2 workspace2 = featureWorkspace as IWorkspace2;
                    if (workspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, featureClassName))
                    {
                        IFeatureClass myfeatureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                        if (myfeatureClass != null)
                        {
                            IDataset dataset = myfeatureClass as IDataset;
                            dataset.Delete();
                        }
                    }
                    ////方案二 取名后缀+时间,在createfeatureclass的时候 选用uniqueFeatureClassTime
                    //string nowTime = System.DateTime.Now.ToLocalTime().ToString();
                    //string nowtimestr = System.Text.RegularExpressions.Regex.Replace(nowTime, @"[^0-9]+", "");
                    //string uniqueFeatureClassTime = featureClassName + nowtimestr;

                    IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(featureClassName, fields, ocDescription.InstanceCLSID, ocDescription.ClassExtensionCLSID, esriFeatureType.esriFTSimple, fcDescription.ShapeFieldName, "");

                    //不要启动编辑 因为这个接口会在要素类中 添加字段
                    //调用ITinSurface的contour方法生成等高线
                    tinSurface.Contour(0, 100, featureClass, "Height", 0);
                    //用featurelayer加载显示 生成的等高线featureclass
                    IFeatureLayer featureLayer = new FeatureLayerClass();
                    featureLayer.FeatureClass = featureClass;
                    featureLayer.Name = "TinContour";
                    axMapControl1.Map.AddLayer(featureLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();
                }
                else
                {
                    MessageBox.Show("请输入正确的TIN图层!");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        //6.为TIN生成 泰森多边形
        private void btn_TinVoronoi_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int layerIndex = cmb_TinContourLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is ITinLayer)
                {
                    ITinLayer tinLayer = layer as ITinLayer;
                    ITin tin = tinLayer.Dataset;
                    ITinNodeCollection tinNodeCollection = tin as ITinNodeCollection;

                    //实例化要素描述对象来获取 构建要素类必须的字段集合
                    IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass();
                    IObjectClassDescription ocDescription = fcDescription as IObjectClassDescription;
                    IFields fields = ocDescription.RequiredFields;
                    //找到shape形状字段， 设置几何类型 投影坐标系统
                    int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);
                    IField field = fields.get_Field(shapeFieldIndex);
                    //只改了shape字段的信息是否足够?
                    //field.GeometryDef.GeometryType
                    //field.GeometryDef.SpatialReference
                    IGeometryDef geometryDef = field.GeometryDef;
                    IGeometryDefEdit geometryDefEdit = geometryDef as IGeometryDefEdit;
                    //设置几何类型
                    geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                    ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                    ISpatialReference spatialReference = spatialReferenceFactory.CreateProjectedCoordinateSystem((int)esriSRProjCSType.esriSRProjCS_NAD1983UTM_20N);
                    ISpatialReferenceResolution spatialReferenceResolution = spatialReference as ISpatialReferenceResolution;
                    spatialReferenceResolution.ConstructFromHorizon();
                    spatialReferenceResolution.SetDefaultXYResolution();
                    ISpatialReferenceTolerance spatialReferenceTolerance = spatialReference as ISpatialReferenceTolerance;
                    spatialReferenceTolerance.SetDefaultXYTolerance();
                    //设置坐标系统
                    geometryDefEdit.SpatialReference_2 = spatialReference;

                    //转换工作空间接口 watch out!connect DB first
                    //IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;

                    IFeatureWorkspace featureWorkspace = null;
                    if (workspace is IFeatureWorkspace)//true when connect DB before,workspace is create by SDEWorkspaceFactory
                    {
                        featureWorkspace = workspace as IFeatureWorkspace;//所以这句成立的前提是  SDE连接了Oracle数据库
                    }
                    else//load image from file directly,NOT connect DB,workspace = null
                    {
                        string outputFolder = @"d:\hhx\tinVoronoi";
                        string tempGDB = "temp.gdb";
                        //DeleteDir(outputFolder);
                        //直接用C# File 判断
                        if (!Directory.Exists(outputFolder + "\\" + tempGDB))
                        {
                            //使用Geoprocessor来创建地理数据库 参考mdb的创建
                            ESRI.ArcGIS.Geoprocessor.Geoprocessor geoProcessor = new ESRI.ArcGIS.Geoprocessor.Geoprocessor();

                            CreateFileGDB createFileGDB = new CreateFileGDB();
                            createFileGDB.out_folder_path = outputFolder;
                            createFileGDB.out_name = tempGDB;
                            geoProcessor.Execute(createFileGDB, null);

                        }
                        IWorkspaceFactory tmpworkspaceFactory = new FileGDBWorkspaceFactoryClass();
                        //临时工作空间 会在此文件夹下面产生一个 临时文件 
                        IWorkspace tmpworkspace = tmpworkspaceFactory.OpenFromFile(outputFolder + "\\" + tempGDB, 0);//有了才可以打开

                        featureWorkspace = tmpworkspace as IFeatureWorkspace;//all work for here
                    }

                    //hhx 假如featureclass已经存在 再次创建会报错
                    //方案一 先删除后创建 。用 iWorkspace2 nameExist判断存在
                    string featureClassName = "tinVoronoi";
                    IWorkspace2 workspace2 = featureWorkspace as IWorkspace2;
                    if (workspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, featureClassName))
                    {
                        IFeatureClass myfeatureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                        if (myfeatureClass != null)
                        {
                            IDataset dataset = myfeatureClass as IDataset;
                            dataset.Delete();
                        }
                    }
                    ////方案二 取名后缀+时间,在createfeatureclass的时候 选用uniqueFeatureClassTime
                    //string nowTime = System.DateTime.Now.ToLocalTime().ToString();
                    //string nowtimestr = System.Text.RegularExpressions.Regex.Replace(nowTime, @"[^0-9]+", "");
                    //string uniqueFeatureClassTime = featureClassName + nowtimestr;


                    IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(featureClassName, fields, ocDescription.InstanceCLSID, ocDescription.ClassExtensionCLSID, esriFeatureType.esriFTSimple, fcDescription.ShapeFieldName, "");

                    //不要启动编辑 因为这个接口会在要素类中添加字段
                    //调用ITinNodeCollection ConvertToVoronoiRegions
                    tinNodeCollection.ConvertToVoronoiRegions(featureClass, null, null, "", "");

                    IFeatureLayer featureLayer = new FeatureLayerClass();
                    featureLayer.FeatureClass = featureClass;
                    featureLayer.Name = "Voronoi";
                    axMapControl1.Map.AddLayer(featureLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();

                    iniCmbItems();

                }//end if ITinLayer
                else
                {
                    MessageBox.Show("请输入正确的TIN图层!");
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //pixblock操作,从像素层面实现之前的NDVI操作
        private void CalculateNDVI_2()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //打开存储NDVI图像的文件夹路径
                IWorkspaceFactory workspaceFact = new RasterWorkspaceFactoryClass();
                IRasterWorkspace2 rasterWs = workspaceFact.OpenFromFile(@"d:\hhx\NDVI", 0) as IRasterWorkspace2;
                //获取当前选中的图层index
                int indexLayer = cmb_NDVILayer.SelectedIndex;
                ILayer layer = this.axMapControl1.Map.get_Layer(indexLayer);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    if (rstLayer.BandCount == 6)
                    {
                        IRaster2 raster = rstLayer.Raster as IRaster2;
                        IRasterProps rasterProps = raster as IRasterProps;
                        //定义raster dataset的空间参考
                        ISpatialReference sr = rasterProps.SpatialReference;
                        //define the origin for the raster dataset, lower left corner
                        IPoint origin = new PointClass();
                        origin.PutCoords(rasterProps.Extent.XMin,rasterProps.Extent.YMin);
                        //define the dimensions of the raster dataset
                        int width = rasterProps.Width;
                        int height = rasterProps.Height;
                        double xCell = rasterProps.MeanCellSize().X;
                        double yCell = rasterProps.MeanCellSize().Y;
                        int NumBand = 1;//this is the number of bands the raster dataset contains
                        //create a raster dataset in TIFF format
                        IRasterDataset2 rasterDsNdvi = rasterWs.CreateRasterDataset("ndvi_pixelblock.tif", "TIFF", origin, width, height, xCell, yCell, NumBand, rstPixelType.PT_DOUBLE, sr, true) as IRasterDataset2;
                        //以上的意思就是模仿输入图层 创造一个空的栅格数据集
                        IRaster2 raster2ndvi = rasterDsNdvi.CreateFullRaster() as IRaster2;
                        //create a raster cursor with a system-optimized pixel block size by passing a null 128*128
                        IRasterCursor rasterCursorndvi = raster2ndvi.CreateCursorEx(null);
                        //loop through each band and pixel block
                        IPixelBlock3 pixelblock3ndvi = null;
                        IRasterEdit rasterEditndvi = raster2ndvi as IRasterEdit;
                        //获取输入图层的rasterDataset
                        IRasterDataset2 rasterDs = raster.RasterDataset as IRasterDataset2;
                        IRaster2 raster2 = rasterDs.CreateFullRaster() as IRaster2;//play snake???
                        IRasterCursor rasterCursor = raster2.CreateCursorEx(null);//128*128
                        //loop through each band and pixel block
                        IPixelBlock3 pixelblock3 = null;
                        long blockwidth = 0;
                        long blockheight = 0;
                        System.Array pixels2;
                        System.Array pixels3;
                        System.Array pixelsndvi;
                        IPnt tlcndvi = null;
                        //object v;
                        do
                        {
                            pixelblock3 = rasterCursor.PixelBlock as IPixelBlock3;
                            pixelblock3ndvi = rasterCursorndvi.PixelBlock as IPixelBlock3;
                            blockwidth = pixelblock3.Width;
                            blockheight = pixelblock3.Height;
                            pixels2 = pixelblock3.get_PixelData(2) as System.Array;//第2波段
                            pixels3 = pixelblock3.get_PixelData(3) as System.Array;//第3波段
                            pixelsndvi = pixelblock3ndvi.get_PixelData(0) as System.Array;
                            for (long i = 0; i < blockwidth; i++)
                            {
                                for (long j = 0; j < blockheight; j++)
                                {
                                    double f2 = 0;
                                    double f3 = 0;
                                    double.TryParse(pixels2.GetValue(i, j).ToString(), out f2);
                                    double.TryParse(pixels3.GetValue(i, j).ToString(), out f3);
                                    double ndvi = 0;
                                    if ((f3 + f2) != 0)
                                    {
                                        ndvi = (f3 - f2) / (f3 + f2 + 0.0001);
                                    }
                                    pixelsndvi.SetValue(ndvi, i, j);//set 1*1pixel value in block
                                }
                            }
                            pixelblock3ndvi.set_PixelData(0, pixelsndvi);//set block value ,the 0 band
                            //write back to the raster
                            tlcndvi = rasterCursorndvi.TopLeft;
                            rasterEditndvi.Write(tlcndvi, pixelblock3ndvi as IPixelBlock);
                        } while (rasterCursor.Next() && rasterCursorndvi.Next());
                        //raster edit ndvi refresh
                        rasterEditndvi.Refresh();
                        //save result to a RasterLayer named NDVI
                        IRasterLayer resLayer = new RasterLayerClass();
                        resLayer.CreateFromRaster(raster2ndvi as IRaster);
                        resLayer.SpatialReference = sr;
                        resLayer.Name = "NDVI";
                        //将此单波段用灰度显示 并按照最大最小值拉伸
                        IRasterStretchColorRampRenderer grayStretch = null;
                        if (resLayer.Renderer is IRasterStretchColorRampRenderer)
                        {
                            grayStretch = resLayer.Renderer as IRasterStretchColorRampRenderer;
                        }
                        else
                        {
                            grayStretch = new RasterStretchColorRampRendererClass();
                        }
                        IRasterStretch2 rstStr2 = grayStretch as IRasterStretch2;
                        rstStr2.StretchType = esriRasterStretchTypesEnum.esriRasterStretch_MinimumMaximum;//最大最小值拉伸
                        resLayer.Renderer = grayStretch as IRasterRenderer;
                        resLayer.Renderer.Update();
                        this.axMapControl1.AddLayer(resLayer, 0);
                        this.axMapControl1.ActiveView.Refresh();
                        this.axTOCControl1.Update();
                        iniCmbItems();
                    }//end if 6 bands
                }//end if rasterLayer
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //利用pixelblock来遍历图像从底层实现 波段信息统计
        private void Statistics2()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int indexLayer = cmb_StatisticsLayer.SelectedIndex;
                int indexBand = cmb_StatisticsBand.SelectedIndex;
                ILayer layer = this.axMapControl1.Map.get_Layer(indexLayer);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster2 raster2 = rstLayer.Raster as IRaster2;
                    IRasterDataset rstDataset = raster2.RasterDataset;
                    if (indexBand == 0)//如果是全部波段 此处的值是combobox的值 实际的波段还是从0开始计数的
                    {
                        //存放输出信息
                        ArrayList outputList = new ArrayList();
                        //波段总数
                        int bandCount = rstLayer.BandCount;
                        //逐个波段统计
                        for (int i = 0; i < bandCount; i++)
                        {
                            int bandIdx = i + 1;//设置波段序号
                            IRasterDataset2 rstDataset2 = rstDataset as IRasterDataset2;
                            Dictionary<string, double> resultList = get_raster_statistic(rstDataset2, i);
                            string bandstatRes = "波段" + bandIdx + " 最大值: " + resultList["max"] + " 最小值: " + resultList["min"] + " 平均值: " + resultList["mean"] + " 标准差: " + resultList["sd"];
                            //将统计结果 添加到ArrayList
                            outputList.Add(bandstatRes);
                        }
                        //将统计结果拼接成一个string
                        string statRes = "";
                        foreach (object obj in outputList)
                        {
                            statRes += obj.ToString() + "\r\n";
                        }
                        //提示框输出统计结果
                        MessageBox.Show(statRes, "统计结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else//single band statistic
                    {
                        IRasterDataset2 rstDataset2 = rstDataset as IRasterDataset2;
                        Dictionary<string, double> resultList = get_raster_statistic(rstDataset2, indexBand - 1);
                        string bandstatRes = "波段" + indexBand + " 最大值: " + resultList["max"] + " 最小值: " + resultList["min"] + " 平均值: " + resultList["mean"] + " 标准差: " + resultList["sd"];
                        MessageBox.Show(bandstatRes, "统计结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }//end if rasterLayer
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //从底层实现 遥感图像波段的 直方图绘制
        //被上面那个函数 调用
        private Dictionary<string, double> get_raster_statistic(IRasterDataset2 rasterDs, int bandindex)
        {
            double[] pixelArray = new double[256];//用来记录每个灰度灰度值出现的个数
            for (int i = 0; i < 256; i++)
            {
                pixelArray[i] = 0.0;
            }
            Dictionary<string, double> resultList = new Dictionary<string, double>();
            //create a raster
            IRaster2 raster2 = rasterDs.CreateFullRaster() as IRaster2;
            //create a raster cursor with a system-optimized pixel block size by pass a null 128*128
            IRasterCursor rasterCursor = raster2.CreateCursorEx(null);
            //loop through each band and pixel block
            IPixelBlock3 pixelBlock3 = null;
            long blockwidth = 0;
            long blockheight = 0;
            System.Array pixels;
            object v;
            int value;
            do
            {
                pixelBlock3 = rasterCursor.PixelBlock as IPixelBlock3;
                blockwidth = pixelBlock3.Width;
                blockheight = pixelBlock3.Height;
                pixels = pixelBlock3.get_PixelData(bandindex) as System.Array;
                for (long i = 0; i < blockwidth; i++)
                {
                    for (long j = 0; j < blockheight; j++)
                    {
                        v = pixels.GetValue(i, j);
                        value = int.Parse(v.ToString());
                        pixelArray[value] += 1;
                    }
                }
            }while(rasterCursor.Next());
            double sum = 0;
            double num = 0;
            int max = 0;
            int min = 255;
            //计算平均值 最大值 最小值 标准差
            for (int i = 0; i < 256; i++)
            {
                num += pixelArray[i];
                sum += i * pixelArray[i];
                if (pixelArray[i] != 0 && i > max)
                {
                    max = i;
                }
                if (pixelArray[i] != 0 && i < min)
                {
                    min = i;
                }
            }
            double mean = sum / num;
            double sdsum = 0;
            for (int i = 0; i < 256; i++)
            {
                sdsum += pixelArray[i] * Math.Pow((i - mean), 2);             
            }
            double sd = Math.Sqrt(sdsum / (num - 1));
            //add the results to resultList
            resultList.Add("min", min);
            resultList.Add("max", max);
            resultList.Add("mean", mean);
            resultList.Add("sd", sd);

            return resultList;
        }

        //自动采样生成TIN
        //给出地理坐标下的原点 以及 每个像素对应的长宽//用多少经纬度表示
        //给出上述范围的 图像的像素值数组 //用多少个像素表示
        private void btn_CreateTinAuto_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int indexLayer = cmb_CreateTinLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(indexLayer);
                if (layer is IRasterLayer)
                {
                    //获取map,清除所有的element标记marker
                    IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
                    pGra.DeleteAllElements();

                    IRasterLayer pRasterLayer = layer as IRasterLayer;
                    IRaster2 iRaster = pRasterLayer.Raster as IRaster2;
                    IGeoDataset pGeoData = iRaster as IGeoDataset;
                    IEnvelope pExtent = pGeoData.Extent;//经纬度坐标 
                    IRasterBandCollection pRasBC = iRaster as IRasterBandCollection;
                    IRasterBand pRasBand = pRasBC.Item(0);
                    IRawPixels pRawPixels = pRasBand as IRawPixels;
                    IRasterProps pProps = pRawPixels as IRasterProps;//play snake...
                    int iWid = pProps.Width;//图像像素宽度 762
                    int iHei = pProps.Height;//630
                    double w = iWid / 1000.0f;
                    double h = iHei / 1000.0f;
                    double cellsizeX = pProps.MeanCellSize().X;//IEnvelope.Width/IRasterProps.Width 一个像素代表实际距离(经纬度)的宽度
                    double cellsizeY = pProps.MeanCellSize().Y;

                    IPnt pBlockSize = new PntClass();//设置像素块的大小
                    bool IterationFlag;
                    if (w < 1 && h < 1)
                    {
                        pBlockSize.X = iWid;
                        pBlockSize.Y = iHei;
                        IterationFlag = true;
                    }
                    else
                    {
                        pBlockSize.X = 1000.0f;
                        pBlockSize.Y = 1000.0f;
                        IterationFlag = false;
                    }

                    IPixelBlock pPixelBlock = pRawPixels.CreatePixelBlock(pBlockSize);//根据传入的Size创建空的pixelBlock 存放之后的读取结果
                    IPnt pOrigin = new PntClass();//地理坐标
                    IPnt pPixelBlockOrigin = new PntClass();//top left Corner 像素坐标
                    object nodata = pProps.NoDataValue;

                    ISpatialReference pSpatial = pGeoData.SpatialReference;
                    pExtent.SpatialReference = pSpatial;
                    ITinEdit pTinEdit = new TinClass();
                    pTinEdit.InitNew(pExtent);//给TIN指定地理范围
                    ITinAdvanced2 pTinNodeCount = pTinEdit as ITinAdvanced2;
                    int nodeCount = pTinNodeCount.NodeCount;//4
                    object vtMissing = Type.Missing;
                    object vPixels = null;//数组信息
                    if (IterationFlag)//当为一个处理单元格时
                    {
                        pPixelBlockOrigin.SetCoords(0.0f, 0.0f);
                        pRawPixels.Read(pPixelBlockOrigin, pPixelBlock);//往pixelBlock赋值
                        vPixels = pPixelBlock.get_SafeArray(0);

                        pOrigin.X = pExtent.XMin + cellsizeX / 2;
                        pOrigin.Y = pExtent.YMax - cellsizeY / 2;
                        //采集和添加结点
                        pTinEdit.AddFromPixelBlock(pOrigin.X, pOrigin.Y, cellsizeX, cellsizeY, nodata, vPixels, 20.0f, ref vtMissing, out vtMissing);
                    }
                    else//多个处理单元格 这里还没有测试数据进去过  待验证
                    {
                        int i = 0, j = 0, count = 0;
                        int FirstGoNodeCount = 0;
                        while (nodeCount != FirstGoNodeCount)
                        {
                            count++;
                            nodeCount = pTinNodeCount.NodeCount;
                            for (i = 0; i < h + 1; i++)
                            {
                                for (j = 0; j < w + 1; j++)
                                {
                                    double bX1, bY1;
                                    //像素块高度宽度
                                    bX1 = pBlockSize.X;
                                    bY1 = pBlockSize.Y;
                                    pPixelBlockOrigin.SetCoords(j * bX1, i * bY1);//top left corner changed 像素坐标
                                    pRawPixels.Read(pPixelBlockOrigin, pPixelBlock);
                                    vPixels = pPixelBlock.get_SafeArray(0);

                                    pOrigin.X = pExtent.XMin + j * bX1 * cellsizeX + cellsizeX / 2.0f;//多少个像素 * 每个像素代表的经纬度
                                    pOrigin.Y = pExtent.YMax - i * bY1 * cellsizeY - cellsizeY / 2.0f;
                                    pTinEdit.AddFromPixelBlock(pOrigin.X, pOrigin.Y, cellsizeX, cellsizeY, nodata, vPixels, 20.0f, ref vtMissing, out vtMissing);
                                    FirstGoNodeCount = pTinNodeCount.NodeCount;
                                }
                            }
                        }
                    }

                    ITinLayer pTinLayer = new TinLayerClass();
                    pTinLayer.Name = "Tin";
                    pTinLayer.Dataset = pTinEdit as ITin;
                    axMapControl1.Map.AddLayer(pTinLayer as ILayer);
                    axMapControl1.ActiveView.Refresh();
                    axMapControl1.Update();

                    iniCmbItems();
                }
                else
                {
                    MessageBox.Show("请输入正确的Raster图层");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        //2017/12/15
        //ESRI.ArcGIS.SpatialAnalyst IHydrologyOp Interface
        //计算流向
        private void btn_FlowDirection_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex = cmb_FlowDirectionLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    IHydrologyOp2 hydrologyOp = new RasterHydrologyOpClass();
                    IGeoDataset pGeoOutput = hydrologyOp.FlowDirection(geoDataset, false, false);//default
                    //save
                    //DeleteDir(@"d:\hhx\hydrology");
                    if (File.Exists(@"d:\hhx\hydrology\flowDirection.tif"))
                    {
                        File.Delete(@"d:\hhx\hydrology\flowDirection.tif");
                    }
                    ISaveAs saveAs = pGeoOutput as ISaveAs;
                    saveAs.SaveAs(@"d:\hhx\hydrology\flowDirection.tif", null, "TIFF");

                    //view
                    IRasterLayer resultRstLayer = new RasterLayerClass();
                    resultRstLayer.CreateFromRaster(pGeoOutput as IRaster);
                    ILayer resultLayer = resultRstLayer as ILayer;
                    resultLayer.Name = "FlowDirection";
                    axMapControl1.Map.AddLayer(resultLayer);
                    //axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //计算流量
        //!!!跟ARCMAP的结果不太一样  难道是显示的问题??
        //此外  用fill之后的DEM进行 流量计算 会卡很久??
        private void btn_FlowAccumulation_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex = cmb_OutFlowDirectionRaster.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    IHydrologyOp2 hydrologyOp = new RasterHydrologyOpClass();
                    object missing = Type.Missing;
                    IGeoDataset pGeoOutput = hydrologyOp.FlowAccumulation(geoDataset, missing);
                    //save
                    //DeleteDir(@"d:\hhx\hydrology");
                    if (File.Exists(@"d:\hhx\hydrology\flowAccumulation.tif"))
                    {
                        File.Delete(@"d:\hhx\hydrology\flowAccumulation.tif");
                    }

                    ISaveAs saveAs = pGeoOutput as ISaveAs;
                    saveAs.SaveAs(@"d:\hhx\hydrology\flowAccumulation.tif", null, "TIFF");

                    //view
                    IRasterLayer resultRstLayer = new RasterLayerClass();
                    resultRstLayer.CreateFromRaster(pGeoOutput as IRaster);
                    ILayer resultLayer = resultRstLayer as ILayer;
                    resultLayer.Name = "FlowAccumulation";
                    axMapControl1.Map.AddLayer(resultLayer);
                    //axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }


            //FlowAccumulation2();

        }

        private void btn_Sink_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex = cmb_SinkLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    IHydrologyOp2 hydrologyOp = new RasterHydrologyOpClass();
                    //object missing = Type.Missing;
                    IGeoDataset pGeoOutput = hydrologyOp.Sink(geoDataset);
                    //save
                    //DeleteDir(@"d:\hhx\hydrology");
                    if (File.Exists(@"d:\hhx\hydrology\sinkFlowDirection.tif"))
                    {
                        File.Delete(@"d:\hhx\hydrology\sinkFlowDirection.tif");
                    }

                    ISaveAs saveAs = pGeoOutput as ISaveAs;
                    saveAs.SaveAs(@"d:\hhx\hydrology\sinkFlowDirection.tif", null, "TIFF");

                    //view
                    IRasterLayer resultRstLayer = new RasterLayerClass();
                    resultRstLayer.CreateFromRaster(pGeoOutput as IRaster);
                    ILayer resultLayer = resultRstLayer as ILayer;
                    resultLayer.Name = "sinkFlowDirection";
                    axMapControl1.Map.AddLayer(resultLayer);
                    //axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void btn_Fill_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex = cmb_FillLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                string zLimitString = txb_FillZ.Text.ToString().Trim();
                double zLimit = 0;
                if(zLimitString!="" && zLimitString!=null)
                {
                    zLimit = double.Parse(zLimitString);
                    if(zLimit <= 0)
                    {
                        MessageBox.Show("Z Limit must greater than 0!");
                        return;
                    }
                }
                
                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    IHydrologyOp2 hydrologyOp = new RasterHydrologyOpClass();
                    object missing = Type.Missing;
                    IGeoDataset pGeoOutput = default(IGeoDataset);
                    if(zLimit==0)//没有输入zlimit
                    {
                        pGeoOutput = hydrologyOp.Fill(geoDataset,missing);             
                    }
                    else
                    {
                        pGeoOutput = hydrologyOp.Fill(geoDataset, zLimit);   
                    }
                 
                    //save
                    //DeleteDir(@"d:\hhx\hydrology");
                    if (File.Exists(@"d:\hhx\hydrology\fillFlowDirection.tif"))
                    {
                        File.Delete(@"d:\hhx\hydrology\fillFlowDirection.tif");
                    }

                    ISaveAs saveAs = pGeoOutput as ISaveAs;
                    saveAs.SaveAs(@"d:\hhx\hydrology\fillFlowDirection.tif", null, "TIFF");

                    //view
                    IRasterLayer resultRstLayer = new RasterLayerClass();
                    resultRstLayer.CreateFromRaster(pGeoOutput as IRaster);
                    ILayer resultLayer = resultRstLayer as ILayer;
                    resultLayer.Name = "fillFlowDirection";
                    axMapControl1.Map.AddLayer(resultLayer);
                    //axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void btn_StreamNet_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex = cmb_StreamNetLayer.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                string streamLimitString = txb_StreamLimit.Text.Trim();
                double streamLimit = 0;
                if (streamLimitString != "" && streamLimitString != null)
                {
                    streamLimit = double.Parse(streamLimitString);
                    if (streamLimit <= 0)
                    {
                        MessageBox.Show("Limit value must greater than 0!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please input limit value!");
                    return;
                }

                if (layer is IRasterLayer)
                {
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IGeoDataset geoDataset = raster as IGeoDataset;
                    IConditionalOp conditionalOp = new RasterConditionalOpClass();
                    object missing = Type.Missing;
                    IGeoDataset pGeoOutput = default(IGeoDataset);

                    //重分类
                    INumberRemap numberRemap = new NumberRemapClass();
                    numberRemap.MapRange(0, streamLimit, 0);
                    numberRemap.MapRange(streamLimit,Double.MaxValue,1);
                    IReclassOp reclassOp = new RasterReclassOpClass();
                    IGeoDataset reclassGeodataset = reclassOp.ReclassByRemap(geoDataset, numberRemap as IRemap, true);
                    INumberRemap numberRemap2 = new NumberRemapClass();
                    numberRemap2.MapRange(0, Double.MaxValue, 1);
                    IGeoDataset reclassGeodataset2 = reclassOp.ReclassByRemap(geoDataset, numberRemap2 as IRemap, true);


                    //条件函数
                    //The values should be comprised of “1”’s and “0”’s with a “1” representing True condition and a “0” a False condition
                    pGeoOutput = conditionalOp.Con(reclassGeodataset, reclassGeodataset2, ref missing);
                    //IRaster2 resultRaster = pGeoOutput as IRaster2;
                    //IRasterDataset resultRasterDataset = resultRaster.RasterDataset;
                    //IRasterBandCollection rasterBandColl = resultRasterDataset as IRasterBandCollection;
                    //IRasterBand rasterBand = rasterBandColl.Item(0);
                    //bool hasStat = false;
                    //rasterBand.HasStatistics(out hasStat);
                    //if (!hasStat || rasterBand.Statistics == null)
                    //{
                    //    rasterBand.ComputeStatsAndHist();
                    //}

                    //save
                    //DeleteDir(@"d:\hhx\hydrology");
                    if (File.Exists(@"d:\hhx\hydrology\StreamNet.tif"))
                    {
                        File.Delete(@"d:\hhx\hydrology\StreamNet.tif");
                    }

                    ISaveAs saveAs = pGeoOutput as ISaveAs;
                    saveAs.SaveAs(@"d:\hhx\hydrology\StreamNet.tif", null, "TIFF");

                    //view
                    IRasterLayer resultRstLayer = new RasterLayerClass();
                    resultRstLayer.CreateFromRaster(pGeoOutput as IRaster);
                    ILayer resultLayer = resultRstLayer as ILayer;
                    resultLayer.Name = "StreamNet";
                    axMapControl1.Map.AddLayer(resultLayer);
                    //axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        //栅格河网矢量化
        private void btn_StreamToFeature_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex1 = cmb_streamRasterLayer.SelectedIndex;
                ILayer layer1 = axMapControl1.Map.get_Layer(layerIndex1);
                int layerIndex2 = cmb_FlowDirectionToFeature.SelectedIndex;
                ILayer layer2 = axMapControl1.Map.get_Layer(layerIndex2);

                if (layer1 is IRasterLayer && layer2 is IRasterLayer)
                {
                    IRasterLayer rstLayer1 = layer1 as IRasterLayer;
                    IRaster raster1 = rstLayer1.Raster;
                    IGeoDataset geoDataset1 = raster1 as IGeoDataset;

                    IRasterLayer rstLayer2 = layer2 as IRasterLayer;
                    IRaster raster2 = rstLayer2.Raster;
                    IGeoDataset geoDataset2 = raster2 as IGeoDataset;
                    IHydrologyOp2 hydrologyOp = new RasterHydrologyOpClass();
                    object missing = Type.Missing;
                    IGeoDataset pGeoOutput = default(IGeoDataset);

                    pGeoOutput = hydrologyOp.StreamToFeature(geoDataset1, geoDataset2, true);//true简化线

                    //得到的是个featureclass 怎么保存啊?
                    //NOT use使用IFeatureDataConverter.ConvertFeatureClass方法
                    //Converts a featuredataset to a Personal Geodatabase/Geodatabase featuredataset.
                    SaveFeatureClassToShapefile(pGeoOutput as IFeatureClass, @"d:\hhx\hydrology");

                    //view
                    IFeatureLayer featureLayer = new FeatureLayerClass();
                    featureLayer.FeatureClass = pGeoOutput as IFeatureClass;
                    ILayer resultLayer = featureLayer as ILayer;
                    resultLayer.Name = "StreamFeature";
                    axMapControl1.Map.AddLayer(resultLayer);
                    //axMapControl1.ActiveView.Extent = resultLayer.AreaOfInterest;
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();

                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //方法二：使用GP服务调用ESRI.ArcGIS.ConversionTools.FeatureClassToShapefile工具
        private void SaveFeatureClassToShapefile(IFeatureClass featureClass,string shpPath)
        {
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            ESRI.ArcGIS.ConversionTools.FeatureClassToShapefile covertToshp = new ESRI.ArcGIS.ConversionTools.FeatureClassToShapefile();
            covertToshp.Input_Features = featureClass;
            covertToshp.Output_Folder = shpPath;
            try
            {
                gp.Execute(covertToshp, null);
            }
            catch (Exception ex)
            {
                string str = "";
                for (int i = 0; i < gp.MessageCount; i++)
                {
                    str += gp.GetMessage(i);
                    str += "\n";
                }
                MessageBox.Show(str);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            //MessageBox.Show("转换成功");
        }

        //不可用
        private void FlowAccumulation2()
        { 
            
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int layerIndex = cmb_OutFlowDirectionRaster.SelectedIndex;
                ILayer layer = axMapControl1.Map.get_Layer(layerIndex);
                if (layer is IRasterLayer)
                {
                    //1.get 【full】 pixels array 
                    IRasterLayer rstLayer = layer as IRasterLayer;
                    IRaster raster = rstLayer.Raster;
                    IRasterProps rasterProps = raster as IRasterProps;
                    IPnt blockSize = new PntClass();
                    blockSize.SetCoords(rasterProps.Width, rasterProps.Height);
                    IPixelBlock pixelBlock = raster.CreatePixelBlock(blockSize);
                    IPnt topLeftCorner = new PntClass();
                    topLeftCorner.SetCoords(0, 0);
                    raster.Read(topLeftCorner, pixelBlock);
                    System.Array pixelsArray = pixelBlock.get_SafeArray(0) as System.Array;

                    //2.calculate
                    System.Array resultArray;
                    using (FlowAccumulation flowAccumulation = new FlowAccumulation(pixelsArray, pixelBlock.Height, pixelBlock.Width))
                    {
                        resultArray = flowAccumulation.CalculateFlowAccumulation();
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    //3.get result array and save it
                    IWorkspaceFactory workspaceFact = new RasterWorkspaceFactoryClass();
                    IRasterWorkspace2 rasterWs = workspaceFact.OpenFromFile(@"d:\hhx\hydrology", 0) as IRasterWorkspace2;
                    ISpatialReference sr = rasterProps.SpatialReference;
                    IPoint origin = new PointClass();
                    origin.PutCoords(rasterProps.Extent.XMin, rasterProps.Extent.YMin);
                    int width = rasterProps.Width;
                    int height = rasterProps.Height;
                    double xCell = rasterProps.MeanCellSize().X;
                    double yCell = rasterProps.MeanCellSize().Y;
                    int NumBand = 1;
                    if (File.Exists(@"d:\hhx\hydrology\flowAccumulation2.tif"))
                    {
                        File.Delete(@"d:\hhx\hydrology\flowAccumulation2.tif");
                    }
                    IRasterDataset2 rasterDs = rasterWs.CreateRasterDataset("flowAccumulation2.tif", "TIFF", origin, width, height, xCell, yCell, NumBand, rstPixelType.PT_LONG, sr, true) as IRasterDataset2;
                    IRaster resultRaster = rasterDs.CreateFullRaster();
                    IPixelBlock resultPixelBlock = resultRaster.CreatePixelBlock(blockSize);

                    //System.Array pixelsArray2 = resultPixelBlock.get_SafeArray(0) as System.Array;
                    resultPixelBlock.set_SafeArray(0, resultArray);
                    IRasterEdit rasterEdit = resultRaster as IRasterEdit;
                    rasterEdit.Write(topLeftCorner, resultPixelBlock);
                    rasterEdit.Refresh();

                    IRasterLayer rasterLayer = new RasterLayerClass();
                    rasterLayer.CreateFromRaster(resultRaster);
                    ILayer resultLayer = rasterLayer as ILayer;
                    resultLayer.Name = "FlowAccumulation2";
                    axMapControl1.Map.AddLayer(resultLayer);
                    axMapControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                    iniCmbItems();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pixelBlock);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(resultPixelBlock);
                }

            }
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }







































    }
}