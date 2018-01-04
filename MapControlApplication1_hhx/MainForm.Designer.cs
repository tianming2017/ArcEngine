namespace MapControlApplication1_hhx
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            //Ensures that any ESRI libraries that have been used are unloaded in the correct order. 
            //Failure to do this may result in random crashes on exit due to the operating system unloading 
            //the libraries in the incorrect order. 
            ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNewDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRaster = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAccessFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConnectDB = new System.Windows.Forms.ToolStripMenuItem();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBarXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grp_import = new System.Windows.Forms.GroupBox();
            this.txb_importRstDataset = new System.Windows.Forms.TextBox();
            this.btn_import = new System.Windows.Forms.Button();
            this.cmb_ImportRstCatalog = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grp_load = new System.Windows.Forms.GroupBox();
            this.btn_load = new System.Windows.Forms.Button();
            this.cmb_LoadRstDataset = new System.Windows.Forms.ComboBox();
            this.cmb_LoadRstCatalog = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grp_create = new System.Windows.Forms.GroupBox();
            this.btn_create = new System.Windows.Forms.Button();
            this.txb_create = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cmb_BBand = new System.Windows.Forms.ComboBox();
            this.cmb_GBand = new System.Windows.Forms.ComboBox();
            this.cmb_RBand = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btn_RGB = new System.Windows.Forms.Button();
            this.cmb_RGBLayer = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pb_ToColor = new System.Windows.Forms.PictureBox();
            this.pb_ColorBar = new System.Windows.Forms.PictureBox();
            this.pb_FromColor = new System.Windows.Forms.PictureBox();
            this.btn_Render = new System.Windows.Forms.Button();
            this.cmb_RenderBand = new System.Windows.Forms.ComboBox();
            this.cmb_RenderLayer = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_Stretch = new System.Windows.Forms.Button();
            this.cmb_StretchBand = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmb_StretchMethod = new System.Windows.Forms.ComboBox();
            this.cmb_StretchLayer = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_MultiBandHis = new System.Windows.Forms.Button();
            this.btn_SingleBandHis = new System.Windows.Forms.Button();
            this.cmb_DrawHisBand = new System.Windows.Forms.ComboBox();
            this.cmb_DrawHisLayer = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_CalculateNDVI = new System.Windows.Forms.Button();
            this.cmb_NDVILayer = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Statistics = new System.Windows.Forms.Button();
            this.cmb_StatisticsBand = new System.Windows.Forms.ComboBox();
            this.cmb_StatisticsLayer = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btn_PanSharpen = new System.Windows.Forms.Button();
            this.cmb_MultiBandLayer = new System.Windows.Forms.ComboBox();
            this.cmb_SingleBandLayer = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.cmb_MosaicRstCatalog = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.btn_Mosaic = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.btn_Filter = new System.Windows.Forms.Button();
            this.cmb_FilterMethod = new System.Windows.Forms.ComboBox();
            this.cmb_FilterLayer = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.btn_Transform = new System.Windows.Forms.Button();
            this.txb_angle = new System.Windows.Forms.TextBox();
            this.cmb_TransformMethod = new System.Windows.Forms.ComboBox();
            this.cmb_TransformLayer = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btn_Clip = new System.Windows.Forms.Button();
            this.txb_ClipFeature = new System.Windows.Forms.TextBox();
            this.cmb_ClipLayer = new System.Windows.Forms.ComboBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.cmb_ClassifyLayer = new System.Windows.Forms.ComboBox();
            this.btn_AfterClassify = new System.Windows.Forms.Button();
            this.txb_ClassifyCount = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.btn_Classify = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.btn_CreateTinAuto = new System.Windows.Forms.Button();
            this.cmb_CreateTinLayer = new System.Windows.Forms.ComboBox();
            this.btn_CreateTIN = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.btn_Extraction = new System.Windows.Forms.Button();
            this.cmb_ExtractionRasterLayer = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.btn_Neighborhood = new System.Windows.Forms.Button();
            this.cmb_NeighborhoodMethod = new System.Windows.Forms.ComboBox();
            this.cmb_NeighborhoodRasterLayer = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.btn_TinVoronoi = new System.Windows.Forms.Button();
            this.btn_TinContour = new System.Windows.Forms.Button();
            this.btn_Contour = new System.Windows.Forms.Button();
            this.cmb_TinVoronoiLayer = new System.Windows.Forms.ComboBox();
            this.cmb_TinContourLayer = new System.Windows.Forms.ComboBox();
            this.cmb_ContourLayer = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.btn_Visibility = new System.Windows.Forms.Button();
            this.btn_LineOfSight = new System.Windows.Forms.Button();
            this.cmb_VisibilityLayer = new System.Windows.Forms.ComboBox();
            this.cmb_LineOfSightLayer = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.btn_Aspect = new System.Windows.Forms.Button();
            this.btn_Slope = new System.Windows.Forms.Button();
            this.btn_HillShade = new System.Windows.Forms.Button();
            this.cmb_AspectLayer = new System.Windows.Forms.ComboBox();
            this.cmb_SlopeLayer = new System.Windows.Forms.ComboBox();
            this.cmb_HillshadeLayer = new System.Windows.Forms.ComboBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.btn_StreamToFeature = new System.Windows.Forms.Button();
            this.label51 = new System.Windows.Forms.Label();
            this.cmb_FlowDirectionToFeature = new System.Windows.Forms.ComboBox();
            this.cmb_streamRasterLayer = new System.Windows.Forms.ComboBox();
            this.label50 = new System.Windows.Forms.Label();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.txb_FillZ = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.btn_Fill = new System.Windows.Forms.Button();
            this.cmb_FillLayer = new System.Windows.Forms.ComboBox();
            this.label46 = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.btn_Sink = new System.Windows.Forms.Button();
            this.cmb_SinkLayer = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.txb_StreamLimit = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.btn_StreamNet = new System.Windows.Forms.Button();
            this.cmb_StreamNetLayer = new System.Windows.Forms.ComboBox();
            this.label48 = new System.Windows.Forms.Label();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.btn_FlowAccumulation = new System.Windows.Forms.Button();
            this.label44 = new System.Windows.Forms.Label();
            this.cmb_OutFlowDirectionRaster = new System.Windows.Forms.ComboBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.btn_FlowDirection = new System.Windows.Forms.Button();
            this.cmb_FlowDirectionLayer = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.zoomToLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLayer = new System.Windows.Forms.ToolStripMenuItem();
            this.cd_FromColor = new System.Windows.Forms.ColorDialog();
            this.cd_ToColor = new System.Windows.Forms.ColorDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grp_import.SuspendLayout();
            this.grp_load.SuspendLayout();
            this.grp_create.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ToColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ColorBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_FromColor)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox24.SuspendLayout();
            this.groupBox23.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox19.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuRaster});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1132, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewDoc,
            this.menuOpenDoc,
            this.menuSaveDoc,
            this.menuSaveAs,
            this.menuSeparator,
            this.menuExitApp});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(39, 21);
            this.menuFile.Text = "File";
            // 
            // menuNewDoc
            // 
            this.menuNewDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuNewDoc.Image")));
            this.menuNewDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuNewDoc.Name = "menuNewDoc";
            this.menuNewDoc.Size = new System.Drawing.Size(180, 22);
            this.menuNewDoc.Text = "New Document";
            this.menuNewDoc.Click += new System.EventHandler(this.menuNewDoc_Click);
            // 
            // menuOpenDoc
            // 
            this.menuOpenDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuOpenDoc.Image")));
            this.menuOpenDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuOpenDoc.Name = "menuOpenDoc";
            this.menuOpenDoc.Size = new System.Drawing.Size(180, 22);
            this.menuOpenDoc.Text = "Open Document...";
            this.menuOpenDoc.Click += new System.EventHandler(this.menuOpenDoc_Click);
            // 
            // menuSaveDoc
            // 
            this.menuSaveDoc.Image = ((System.Drawing.Image)(resources.GetObject("menuSaveDoc.Image")));
            this.menuSaveDoc.ImageTransparentColor = System.Drawing.Color.White;
            this.menuSaveDoc.Name = "menuSaveDoc";
            this.menuSaveDoc.Size = new System.Drawing.Size(180, 22);
            this.menuSaveDoc.Text = "SaveDocument";
            this.menuSaveDoc.Click += new System.EventHandler(this.menuSaveDoc_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(180, 22);
            this.menuSaveAs.Text = "Save As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuSeparator
            // 
            this.menuSeparator.Name = "menuSeparator";
            this.menuSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // menuExitApp
            // 
            this.menuExitApp.Name = "menuExitApp";
            this.menuExitApp.Size = new System.Drawing.Size(180, 22);
            this.menuExitApp.Text = "Exit";
            this.menuExitApp.Click += new System.EventHandler(this.menuExitApp_Click);
            // 
            // menuRaster
            // 
            this.menuRaster.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAccessFile,
            this.menuConnectDB});
            this.menuRaster.Name = "menuRaster";
            this.menuRaster.Size = new System.Drawing.Size(92, 21);
            this.menuRaster.Text = "加载栅格图像";
            // 
            // menuAccessFile
            // 
            this.menuAccessFile.Name = "menuAccessFile";
            this.menuAccessFile.Size = new System.Drawing.Size(136, 22);
            this.menuAccessFile.Text = "从文件打开";
            this.menuAccessFile.Click += new System.EventHandler(this.menuAccessFile_Click);
            // 
            // menuConnectDB
            // 
            this.menuConnectDB.Name = "menuConnectDB";
            this.menuConnectDB.Size = new System.Drawing.Size(136, 22);
            this.menuConnectDB.Text = "连接数据库";
            this.menuConnectDB.Click += new System.EventHandler(this.menuConnectDB_Click);
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(234, 53);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(585, 492);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnDoubleClick += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnDoubleClickEventHandler(this.axMapControl1_OnDoubleClick);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 25);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(819, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 53);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(231, 492);
            this.axTOCControl1.TabIndex = 4;
            this.axTOCControl1.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl1_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(466, 278);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Control;
            this.splitter1.Location = new System.Drawing.Point(0, 53);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 514);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarXY});
            this.statusStrip1.Location = new System.Drawing.Point(3, 545);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(816, 22);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusBar1";
            // 
            // statusBarXY
            // 
            this.statusBarXY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusBarXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarXY.Name = "statusBarXY";
            this.statusBarXY.Size = new System.Drawing.Size(57, 17);
            this.statusBarXY.Text = "Test 123";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(819, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(313, 542);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grp_import);
            this.tabPage1.Controls.Add(this.grp_load);
            this.tabPage1.Controls.Add(this.grp_create);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(305, 516);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据管理";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grp_import
            // 
            this.grp_import.Controls.Add(this.txb_importRstDataset);
            this.grp_import.Controls.Add(this.btn_import);
            this.grp_import.Controls.Add(this.cmb_ImportRstCatalog);
            this.grp_import.Controls.Add(this.label4);
            this.grp_import.Controls.Add(this.label3);
            this.grp_import.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grp_import.Location = new System.Drawing.Point(17, 295);
            this.grp_import.Name = "grp_import";
            this.grp_import.Size = new System.Drawing.Size(242, 119);
            this.grp_import.TabIndex = 2;
            this.grp_import.TabStop = false;
            this.grp_import.Text = "导入栅格图像";
            // 
            // txb_importRstDataset
            // 
            this.txb_importRstDataset.Location = new System.Drawing.Point(93, 60);
            this.txb_importRstDataset.Name = "txb_importRstDataset";
            this.txb_importRstDataset.Size = new System.Drawing.Size(121, 21);
            this.txb_importRstDataset.TabIndex = 5;
            this.txb_importRstDataset.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txb_importRstDataset_MouseDown);
            // 
            // btn_import
            // 
            this.btn_import.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btn_import.Location = new System.Drawing.Point(93, 90);
            this.btn_import.Name = "btn_import";
            this.btn_import.Size = new System.Drawing.Size(75, 23);
            this.btn_import.TabIndex = 4;
            this.btn_import.Text = "导入";
            this.btn_import.UseVisualStyleBackColor = true;
            this.btn_import.Click += new System.EventHandler(this.btn_import_Click);
            // 
            // cmb_ImportRstCatalog
            // 
            this.cmb_ImportRstCatalog.FormattingEnabled = true;
            this.cmb_ImportRstCatalog.Location = new System.Drawing.Point(93, 28);
            this.cmb_ImportRstCatalog.Name = "cmb_ImportRstCatalog";
            this.cmb_ImportRstCatalog.Size = new System.Drawing.Size(121, 20);
            this.cmb_ImportRstCatalog.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(16, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "栅格图像";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(16, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "栅格目录";
            // 
            // grp_load
            // 
            this.grp_load.Controls.Add(this.btn_load);
            this.grp_load.Controls.Add(this.cmb_LoadRstDataset);
            this.grp_load.Controls.Add(this.cmb_LoadRstCatalog);
            this.grp_load.Controls.Add(this.label2);
            this.grp_load.Controls.Add(this.label1);
            this.grp_load.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grp_load.Location = new System.Drawing.Point(17, 144);
            this.grp_load.Name = "grp_load";
            this.grp_load.Size = new System.Drawing.Size(242, 119);
            this.grp_load.TabIndex = 1;
            this.grp_load.TabStop = false;
            this.grp_load.Text = "加载栅格图像";
            // 
            // btn_load
            // 
            this.btn_load.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btn_load.Location = new System.Drawing.Point(93, 90);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(75, 23);
            this.btn_load.TabIndex = 4;
            this.btn_load.Text = "加载";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // cmb_LoadRstDataset
            // 
            this.cmb_LoadRstDataset.FormattingEnabled = true;
            this.cmb_LoadRstDataset.Location = new System.Drawing.Point(93, 60);
            this.cmb_LoadRstDataset.Name = "cmb_LoadRstDataset";
            this.cmb_LoadRstDataset.Size = new System.Drawing.Size(121, 20);
            this.cmb_LoadRstDataset.TabIndex = 3;
            // 
            // cmb_LoadRstCatalog
            // 
            this.cmb_LoadRstCatalog.FormattingEnabled = true;
            this.cmb_LoadRstCatalog.Location = new System.Drawing.Point(93, 28);
            this.cmb_LoadRstCatalog.Name = "cmb_LoadRstCatalog";
            this.cmb_LoadRstCatalog.Size = new System.Drawing.Size(121, 20);
            this.cmb_LoadRstCatalog.TabIndex = 2;
            this.cmb_LoadRstCatalog.SelectedIndexChanged += new System.EventHandler(this.cmb_LoadRstCatalog_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label2.Location = new System.Drawing.Point(16, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "栅格图像";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "栅格目录";
            // 
            // grp_create
            // 
            this.grp_create.Controls.Add(this.btn_create);
            this.grp_create.Controls.Add(this.txb_create);
            this.grp_create.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.grp_create.Location = new System.Drawing.Point(17, 26);
            this.grp_create.Name = "grp_create";
            this.grp_create.Size = new System.Drawing.Size(242, 93);
            this.grp_create.TabIndex = 0;
            this.grp_create.TabStop = false;
            this.grp_create.Text = "创建栅格目录";
            // 
            // btn_create
            // 
            this.btn_create.ForeColor = System.Drawing.SystemColors.MenuText;
            this.btn_create.Location = new System.Drawing.Point(141, 39);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(77, 22);
            this.btn_create.TabIndex = 1;
            this.btn_create.Text = "创建";
            this.btn_create.UseVisualStyleBackColor = true;
            this.btn_create.Click += new System.EventHandler(this.btn_create_Click);
            // 
            // txb_create
            // 
            this.txb_create.Location = new System.Drawing.Point(17, 41);
            this.txb_create.Name = "txb_create";
            this.txb_create.Size = new System.Drawing.Size(111, 21);
            this.txb_create.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(305, 516);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "图像处理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cmb_BBand);
            this.groupBox6.Controls.Add(this.cmb_GBand);
            this.groupBox6.Controls.Add(this.cmb_RBand);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Controls.Add(this.label17);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Controls.Add(this.btn_RGB);
            this.groupBox6.Controls.Add(this.cmb_RGBLayer);
            this.groupBox6.Controls.Add(this.label15);
            this.groupBox6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox6.Location = new System.Drawing.Point(6, 426);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(291, 82);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "多波段假彩色合成";
            // 
            // cmb_BBand
            // 
            this.cmb_BBand.FormattingEnabled = true;
            this.cmb_BBand.Location = new System.Drawing.Point(214, 47);
            this.cmb_BBand.Name = "cmb_BBand";
            this.cmb_BBand.Size = new System.Drawing.Size(70, 20);
            this.cmb_BBand.TabIndex = 12;
            // 
            // cmb_GBand
            // 
            this.cmb_GBand.FormattingEnabled = true;
            this.cmb_GBand.Location = new System.Drawing.Point(123, 47);
            this.cmb_GBand.Name = "cmb_GBand";
            this.cmb_GBand.Size = new System.Drawing.Size(70, 20);
            this.cmb_GBand.TabIndex = 11;
            // 
            // cmb_RBand
            // 
            this.cmb_RBand.FormattingEnabled = true;
            this.cmb_RBand.Location = new System.Drawing.Point(29, 47);
            this.cmb_RBand.Name = "cmb_RBand";
            this.cmb_RBand.Size = new System.Drawing.Size(70, 20);
            this.cmb_RBand.TabIndex = 10;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.Blue;
            this.label18.Location = new System.Drawing.Point(203, 51);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(11, 12);
            this.label18.TabIndex = 9;
            this.label18.Text = "B";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Green;
            this.label17.Location = new System.Drawing.Point(109, 52);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(11, 12);
            this.label17.TabIndex = 8;
            this.label17.Text = "G";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(15, 51);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(11, 12);
            this.label16.TabIndex = 7;
            this.label16.Text = "R";
            // 
            // btn_RGB
            // 
            this.btn_RGB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_RGB.Location = new System.Drawing.Point(214, 17);
            this.btn_RGB.Name = "btn_RGB";
            this.btn_RGB.Size = new System.Drawing.Size(75, 23);
            this.btn_RGB.TabIndex = 6;
            this.btn_RGB.Text = "合成";
            this.btn_RGB.UseVisualStyleBackColor = true;
            this.btn_RGB.Click += new System.EventHandler(this.btn_RGB_Click);
            // 
            // cmb_RGBLayer
            // 
            this.cmb_RGBLayer.FormattingEnabled = true;
            this.cmb_RGBLayer.Location = new System.Drawing.Point(46, 17);
            this.cmb_RGBLayer.Name = "cmb_RGBLayer";
            this.cmb_RGBLayer.Size = new System.Drawing.Size(160, 20);
            this.cmb_RGBLayer.TabIndex = 5;
            this.cmb_RGBLayer.SelectedIndexChanged += new System.EventHandler(this.cmb_RGBLayer_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(10, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(29, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "图层";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.pb_ToColor);
            this.groupBox5.Controls.Add(this.pb_ColorBar);
            this.groupBox5.Controls.Add(this.pb_FromColor);
            this.groupBox5.Controls.Add(this.btn_Render);
            this.groupBox5.Controls.Add(this.cmb_RenderBand);
            this.groupBox5.Controls.Add(this.cmb_RenderLayer);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox5.Location = new System.Drawing.Point(6, 338);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(291, 82);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "单波段伪彩色渲染";
            // 
            // pb_ToColor
            // 
            this.pb_ToColor.Location = new System.Drawing.Point(177, 53);
            this.pb_ToColor.Name = "pb_ToColor";
            this.pb_ToColor.Size = new System.Drawing.Size(35, 20);
            this.pb_ToColor.TabIndex = 7;
            this.pb_ToColor.TabStop = false;
            this.pb_ToColor.Click += new System.EventHandler(this.pb_ToColor_Click);
            // 
            // pb_ColorBar
            // 
            this.pb_ColorBar.Location = new System.Drawing.Point(46, 53);
            this.pb_ColorBar.Name = "pb_ColorBar";
            this.pb_ColorBar.Size = new System.Drawing.Size(128, 20);
            this.pb_ColorBar.TabIndex = 6;
            this.pb_ColorBar.TabStop = false;
            // 
            // pb_FromColor
            // 
            this.pb_FromColor.Location = new System.Drawing.Point(8, 53);
            this.pb_FromColor.Name = "pb_FromColor";
            this.pb_FromColor.Size = new System.Drawing.Size(35, 20);
            this.pb_FromColor.TabIndex = 5;
            this.pb_FromColor.TabStop = false;
            this.pb_FromColor.Click += new System.EventHandler(this.pb_FromColor_Click);
            // 
            // btn_Render
            // 
            this.btn_Render.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Render.Location = new System.Drawing.Point(214, 53);
            this.btn_Render.Name = "btn_Render";
            this.btn_Render.Size = new System.Drawing.Size(75, 23);
            this.btn_Render.TabIndex = 4;
            this.btn_Render.Text = "渲染";
            this.btn_Render.UseVisualStyleBackColor = true;
            this.btn_Render.Click += new System.EventHandler(this.btn_Render_Click);
            // 
            // cmb_RenderBand
            // 
            this.cmb_RenderBand.FormattingEnabled = true;
            this.cmb_RenderBand.Location = new System.Drawing.Point(214, 26);
            this.cmb_RenderBand.Name = "cmb_RenderBand";
            this.cmb_RenderBand.Size = new System.Drawing.Size(75, 20);
            this.cmb_RenderBand.TabIndex = 3;
            // 
            // cmb_RenderLayer
            // 
            this.cmb_RenderLayer.FormattingEnabled = true;
            this.cmb_RenderLayer.Location = new System.Drawing.Point(46, 25);
            this.cmb_RenderLayer.Name = "cmb_RenderLayer";
            this.cmb_RenderLayer.Size = new System.Drawing.Size(128, 20);
            this.cmb_RenderLayer.TabIndex = 2;
            this.cmb_RenderLayer.SelectedIndexChanged += new System.EventHandler(this.cmb_RenderLayer_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(178, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 1;
            this.label14.Text = "波段";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(10, 28);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "图层";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_Stretch);
            this.groupBox4.Controls.Add(this.cmb_StretchBand);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.cmb_StretchMethod);
            this.groupBox4.Controls.Add(this.cmb_StretchLayer);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox4.Location = new System.Drawing.Point(6, 248);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(291, 84);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "单波段灰度增强";
            // 
            // btn_Stretch
            // 
            this.btn_Stretch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Stretch.Location = new System.Drawing.Point(214, 51);
            this.btn_Stretch.Name = "btn_Stretch";
            this.btn_Stretch.Size = new System.Drawing.Size(75, 23);
            this.btn_Stretch.TabIndex = 5;
            this.btn_Stretch.Text = "增强";
            this.btn_Stretch.UseVisualStyleBackColor = true;
            this.btn_Stretch.Click += new System.EventHandler(this.btn_Stretch_Click);
            // 
            // cmb_StretchBand
            // 
            this.cmb_StretchBand.FormattingEnabled = true;
            this.cmb_StretchBand.Location = new System.Drawing.Point(214, 26);
            this.cmb_StretchBand.Name = "cmb_StretchBand";
            this.cmb_StretchBand.Size = new System.Drawing.Size(75, 20);
            this.cmb_StretchBand.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(178, 30);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 4;
            this.label12.Text = "波段";
            // 
            // cmb_StretchMethod
            // 
            this.cmb_StretchMethod.FormattingEnabled = true;
            this.cmb_StretchMethod.Items.AddRange(new object[] {
            "默认拉伸",
            "标准差拉伸",
            "最大最小值拉伸",
            "百分比最大最小值拉伸",
            "直方图均衡",
            "直方图匹配"});
            this.cmb_StretchMethod.Location = new System.Drawing.Point(46, 54);
            this.cmb_StretchMethod.Name = "cmb_StretchMethod";
            this.cmb_StretchMethod.Size = new System.Drawing.Size(160, 20);
            this.cmb_StretchMethod.TabIndex = 3;
            // 
            // cmb_StretchLayer
            // 
            this.cmb_StretchLayer.FormattingEnabled = true;
            this.cmb_StretchLayer.Location = new System.Drawing.Point(46, 25);
            this.cmb_StretchLayer.Name = "cmb_StretchLayer";
            this.cmb_StretchLayer.Size = new System.Drawing.Size(128, 20);
            this.cmb_StretchLayer.TabIndex = 2;
            this.cmb_StretchLayer.SelectedIndexChanged += new System.EventHandler(this.cmb_StretchLayer_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(10, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 1;
            this.label11.Text = "方法";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(10, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "图层";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_MultiBandHis);
            this.groupBox3.Controls.Add(this.btn_SingleBandHis);
            this.groupBox3.Controls.Add(this.cmb_DrawHisBand);
            this.groupBox3.Controls.Add(this.cmb_DrawHisLayer);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox3.Location = new System.Drawing.Point(6, 163);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(291, 79);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "直方图绘制";
            // 
            // btn_MultiBandHis
            // 
            this.btn_MultiBandHis.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_MultiBandHis.Location = new System.Drawing.Point(214, 44);
            this.btn_MultiBandHis.Name = "btn_MultiBandHis";
            this.btn_MultiBandHis.Size = new System.Drawing.Size(75, 23);
            this.btn_MultiBandHis.TabIndex = 7;
            this.btn_MultiBandHis.Text = "多波段";
            this.btn_MultiBandHis.UseVisualStyleBackColor = true;
            this.btn_MultiBandHis.Click += new System.EventHandler(this.btn_MultiBandHis_Click);
            // 
            // btn_SingleBandHis
            // 
            this.btn_SingleBandHis.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_SingleBandHis.Location = new System.Drawing.Point(214, 15);
            this.btn_SingleBandHis.Name = "btn_SingleBandHis";
            this.btn_SingleBandHis.Size = new System.Drawing.Size(75, 23);
            this.btn_SingleBandHis.TabIndex = 4;
            this.btn_SingleBandHis.Text = "单波段";
            this.btn_SingleBandHis.UseVisualStyleBackColor = true;
            this.btn_SingleBandHis.Click += new System.EventHandler(this.btn_SingleBandHis_Click);
            // 
            // cmb_DrawHisBand
            // 
            this.cmb_DrawHisBand.FormattingEnabled = true;
            this.cmb_DrawHisBand.Location = new System.Drawing.Point(46, 46);
            this.cmb_DrawHisBand.Name = "cmb_DrawHisBand";
            this.cmb_DrawHisBand.Size = new System.Drawing.Size(160, 20);
            this.cmb_DrawHisBand.TabIndex = 6;
            // 
            // cmb_DrawHisLayer
            // 
            this.cmb_DrawHisLayer.FormattingEnabled = true;
            this.cmb_DrawHisLayer.Location = new System.Drawing.Point(46, 18);
            this.cmb_DrawHisLayer.Name = "cmb_DrawHisLayer";
            this.cmb_DrawHisLayer.Size = new System.Drawing.Size(160, 20);
            this.cmb_DrawHisLayer.TabIndex = 4;
            this.cmb_DrawHisLayer.SelectedIndexChanged += new System.EventHandler(this.cmb_DrawHisLayer_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(10, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "波段";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(10, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "图层";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_CalculateNDVI);
            this.groupBox2.Controls.Add(this.cmb_NDVILayer);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox2.Location = new System.Drawing.Point(6, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 59);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NDVI指数计算";
            // 
            // btn_CalculateNDVI
            // 
            this.btn_CalculateNDVI.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_CalculateNDVI.Location = new System.Drawing.Point(214, 20);
            this.btn_CalculateNDVI.Name = "btn_CalculateNDVI";
            this.btn_CalculateNDVI.Size = new System.Drawing.Size(75, 23);
            this.btn_CalculateNDVI.TabIndex = 2;
            this.btn_CalculateNDVI.Text = "计算";
            this.btn_CalculateNDVI.UseVisualStyleBackColor = true;
            this.btn_CalculateNDVI.Click += new System.EventHandler(this.btn_CalculateNDVI_Click);
            // 
            // cmb_NDVILayer
            // 
            this.cmb_NDVILayer.FormattingEnabled = true;
            this.cmb_NDVILayer.Location = new System.Drawing.Point(46, 22);
            this.cmb_NDVILayer.Name = "cmb_NDVILayer";
            this.cmb_NDVILayer.Size = new System.Drawing.Size(160, 20);
            this.cmb_NDVILayer.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(10, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "图层";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Statistics);
            this.groupBox1.Controls.Add(this.cmb_StatisticsBand);
            this.groupBox1.Controls.Add(this.cmb_StatisticsLayer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "波段信息统计";
            // 
            // btn_Statistics
            // 
            this.btn_Statistics.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Statistics.Location = new System.Drawing.Point(214, 46);
            this.btn_Statistics.Name = "btn_Statistics";
            this.btn_Statistics.Size = new System.Drawing.Size(75, 23);
            this.btn_Statistics.TabIndex = 4;
            this.btn_Statistics.Text = "统计";
            this.btn_Statistics.UseVisualStyleBackColor = true;
            this.btn_Statistics.Click += new System.EventHandler(this.btn_Statistics_Click);
            // 
            // cmb_StatisticsBand
            // 
            this.cmb_StatisticsBand.FormattingEnabled = true;
            this.cmb_StatisticsBand.Location = new System.Drawing.Point(46, 48);
            this.cmb_StatisticsBand.Name = "cmb_StatisticsBand";
            this.cmb_StatisticsBand.Size = new System.Drawing.Size(160, 20);
            this.cmb_StatisticsBand.TabIndex = 3;
            // 
            // cmb_StatisticsLayer
            // 
            this.cmb_StatisticsLayer.FormattingEnabled = true;
            this.cmb_StatisticsLayer.Location = new System.Drawing.Point(46, 20);
            this.cmb_StatisticsLayer.Name = "cmb_StatisticsLayer";
            this.cmb_StatisticsLayer.Size = new System.Drawing.Size(160, 20);
            this.cmb_StatisticsLayer.TabIndex = 2;
            this.cmb_StatisticsLayer.SelectedIndexChanged += new System.EventHandler(this.cmb_StatisticsLayer_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(10, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "波段";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(10, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "图层";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox9);
            this.tabPage3.Controls.Add(this.groupBox10);
            this.tabPage3.Controls.Add(this.groupBox11);
            this.tabPage3.Controls.Add(this.groupBox12);
            this.tabPage3.Controls.Add(this.groupBox8);
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(305, 516);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "图像分析";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btn_PanSharpen);
            this.groupBox9.Controls.Add(this.cmb_MultiBandLayer);
            this.groupBox9.Controls.Add(this.cmb_SingleBandLayer);
            this.groupBox9.Controls.Add(this.label28);
            this.groupBox9.Controls.Add(this.label27);
            this.groupBox9.ForeColor = System.Drawing.Color.Blue;
            this.groupBox9.Location = new System.Drawing.Point(17, 104);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(280, 79);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "全色锐化（融合）";
            // 
            // btn_PanSharpen
            // 
            this.btn_PanSharpen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_PanSharpen.Location = new System.Drawing.Point(191, 42);
            this.btn_PanSharpen.Name = "btn_PanSharpen";
            this.btn_PanSharpen.Size = new System.Drawing.Size(75, 23);
            this.btn_PanSharpen.TabIndex = 4;
            this.btn_PanSharpen.Text = "融合";
            this.btn_PanSharpen.UseVisualStyleBackColor = true;
            this.btn_PanSharpen.Click += new System.EventHandler(this.btn_PanSharpen_Click);
            // 
            // cmb_MultiBandLayer
            // 
            this.cmb_MultiBandLayer.FormattingEnabled = true;
            this.cmb_MultiBandLayer.Location = new System.Drawing.Point(55, 46);
            this.cmb_MultiBandLayer.Name = "cmb_MultiBandLayer";
            this.cmb_MultiBandLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_MultiBandLayer.TabIndex = 3;
            // 
            // cmb_SingleBandLayer
            // 
            this.cmb_SingleBandLayer.FormattingEnabled = true;
            this.cmb_SingleBandLayer.Location = new System.Drawing.Point(55, 19);
            this.cmb_SingleBandLayer.Name = "cmb_SingleBandLayer";
            this.cmb_SingleBandLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_SingleBandLayer.TabIndex = 2;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label28.Location = new System.Drawing.Point(6, 48);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(41, 12);
            this.label28.TabIndex = 1;
            this.label28.Text = "多波段";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Location = new System.Drawing.Point(6, 22);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(41, 12);
            this.label27.TabIndex = 0;
            this.label27.Text = "单波段";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.cmb_MosaicRstCatalog);
            this.groupBox10.Controls.Add(this.label26);
            this.groupBox10.Controls.Add(this.btn_Mosaic);
            this.groupBox10.ForeColor = System.Drawing.Color.Blue;
            this.groupBox10.Location = new System.Drawing.Point(17, 189);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(280, 53);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "图像镶嵌";
            // 
            // cmb_MosaicRstCatalog
            // 
            this.cmb_MosaicRstCatalog.FormattingEnabled = true;
            this.cmb_MosaicRstCatalog.Location = new System.Drawing.Point(65, 22);
            this.cmb_MosaicRstCatalog.Name = "cmb_MosaicRstCatalog";
            this.cmb_MosaicRstCatalog.Size = new System.Drawing.Size(121, 20);
            this.cmb_MosaicRstCatalog.TabIndex = 11;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label26.Location = new System.Drawing.Point(6, 25);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(53, 12);
            this.label26.TabIndex = 10;
            this.label26.Text = "栅格目录";
            // 
            // btn_Mosaic
            // 
            this.btn_Mosaic.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Mosaic.Location = new System.Drawing.Point(191, 20);
            this.btn_Mosaic.Name = "btn_Mosaic";
            this.btn_Mosaic.Size = new System.Drawing.Size(75, 23);
            this.btn_Mosaic.TabIndex = 9;
            this.btn_Mosaic.Text = "镶嵌";
            this.btn_Mosaic.UseVisualStyleBackColor = true;
            this.btn_Mosaic.Click += new System.EventHandler(this.btn_Mosaic_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.btn_Filter);
            this.groupBox11.Controls.Add(this.cmb_FilterMethod);
            this.groupBox11.Controls.Add(this.cmb_FilterLayer);
            this.groupBox11.Controls.Add(this.label25);
            this.groupBox11.Controls.Add(this.label24);
            this.groupBox11.ForeColor = System.Drawing.Color.Blue;
            this.groupBox11.Location = new System.Drawing.Point(17, 252);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(280, 73);
            this.groupBox11.TabIndex = 0;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "卷积运算";
            // 
            // btn_Filter
            // 
            this.btn_Filter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Filter.Location = new System.Drawing.Point(191, 42);
            this.btn_Filter.Name = "btn_Filter";
            this.btn_Filter.Size = new System.Drawing.Size(75, 23);
            this.btn_Filter.TabIndex = 4;
            this.btn_Filter.Text = "滤波";
            this.btn_Filter.UseVisualStyleBackColor = true;
            this.btn_Filter.Click += new System.EventHandler(this.btn_Filter_Click);
            // 
            // cmb_FilterMethod
            // 
            this.cmb_FilterMethod.FormattingEnabled = true;
            this.cmb_FilterMethod.Items.AddRange(new object[] {
            "LineDetectionHorizontal",
            "LineDetectionVertical",
            "LineDetectionLeftDiagonal",
            "LineDetectionRightDiagonal",
            "GradientNorth",
            "GradientWest",
            "GradientEast",
            "GradientSouth",
            "GradientNorthEast",
            "GradientNorthWest",
            "SmoothArithmeticMean",
            "Smoothing3x3",
            "Smoothing5x5",
            "Sharpening3x3",
            "Sharpening5x5",
            "Laplacian3x3",
            "Laplacian5x5",
            "SobelHorizontal",
            "SobelVertical",
            "Sharpen",
            "Sharpen2",
            "PointSpread"});
            this.cmb_FilterMethod.Location = new System.Drawing.Point(55, 45);
            this.cmb_FilterMethod.Name = "cmb_FilterMethod";
            this.cmb_FilterMethod.Size = new System.Drawing.Size(121, 20);
            this.cmb_FilterMethod.TabIndex = 3;
            // 
            // cmb_FilterLayer
            // 
            this.cmb_FilterLayer.FormattingEnabled = true;
            this.cmb_FilterLayer.Location = new System.Drawing.Point(55, 18);
            this.cmb_FilterLayer.Name = "cmb_FilterLayer";
            this.cmb_FilterLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_FilterLayer.TabIndex = 2;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label25.Location = new System.Drawing.Point(13, 47);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(29, 12);
            this.label25.TabIndex = 1;
            this.label25.Text = "方法";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label24.Location = new System.Drawing.Point(13, 23);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(29, 12);
            this.label24.TabIndex = 0;
            this.label24.Text = "图层";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.btn_Transform);
            this.groupBox12.Controls.Add(this.txb_angle);
            this.groupBox12.Controls.Add(this.cmb_TransformMethod);
            this.groupBox12.Controls.Add(this.cmb_TransformLayer);
            this.groupBox12.Controls.Add(this.label23);
            this.groupBox12.Controls.Add(this.label22);
            this.groupBox12.Controls.Add(this.label21);
            this.groupBox12.ForeColor = System.Drawing.Color.Blue;
            this.groupBox12.Location = new System.Drawing.Point(17, 333);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(280, 82);
            this.groupBox12.TabIndex = 0;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "图像变换";
            // 
            // btn_Transform
            // 
            this.btn_Transform.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Transform.Location = new System.Drawing.Point(191, 50);
            this.btn_Transform.Name = "btn_Transform";
            this.btn_Transform.Size = new System.Drawing.Size(75, 23);
            this.btn_Transform.TabIndex = 6;
            this.btn_Transform.Text = "变换";
            this.btn_Transform.UseVisualStyleBackColor = true;
            this.btn_Transform.Click += new System.EventHandler(this.btn_Transform_Click);
            // 
            // txb_angle
            // 
            this.txb_angle.Location = new System.Drawing.Point(220, 21);
            this.txb_angle.Name = "txb_angle";
            this.txb_angle.ReadOnly = true;
            this.txb_angle.Size = new System.Drawing.Size(46, 21);
            this.txb_angle.TabIndex = 5;
            // 
            // cmb_TransformMethod
            // 
            this.cmb_TransformMethod.FormattingEnabled = true;
            this.cmb_TransformMethod.Items.AddRange(new object[] {
            "翻转Flip",
            "镜像Mirror",
            "裁剪Clip",
            "旋转Rotate"});
            this.cmb_TransformMethod.Location = new System.Drawing.Point(55, 52);
            this.cmb_TransformMethod.Name = "cmb_TransformMethod";
            this.cmb_TransformMethod.Size = new System.Drawing.Size(121, 20);
            this.cmb_TransformMethod.TabIndex = 4;
            this.cmb_TransformMethod.SelectedIndexChanged += new System.EventHandler(this.cmb_TransformMethod_SelectedIndexChanged);
            // 
            // cmb_TransformLayer
            // 
            this.cmb_TransformLayer.FormattingEnabled = true;
            this.cmb_TransformLayer.Location = new System.Drawing.Point(55, 23);
            this.cmb_TransformLayer.Name = "cmb_TransformLayer";
            this.cmb_TransformLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_TransformLayer.TabIndex = 3;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label23.Location = new System.Drawing.Point(189, 26);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(29, 12);
            this.label23.TabIndex = 2;
            this.label23.Text = "角度";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(13, 54);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 12);
            this.label22.TabIndex = 1;
            this.label22.Text = "方法";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Location = new System.Drawing.Point(13, 26);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(29, 12);
            this.label21.TabIndex = 0;
            this.label21.Text = "图层";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btn_Clip);
            this.groupBox8.Controls.Add(this.txb_ClipFeature);
            this.groupBox8.Controls.Add(this.cmb_ClipLayer);
            this.groupBox8.Controls.Add(this.label30);
            this.groupBox8.Controls.Add(this.label29);
            this.groupBox8.ForeColor = System.Drawing.Color.Blue;
            this.groupBox8.Location = new System.Drawing.Point(17, 15);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(280, 83);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "图层裁剪";
            // 
            // btn_Clip
            // 
            this.btn_Clip.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Clip.Location = new System.Drawing.Point(191, 42);
            this.btn_Clip.Name = "btn_Clip";
            this.btn_Clip.Size = new System.Drawing.Size(75, 23);
            this.btn_Clip.TabIndex = 4;
            this.btn_Clip.Text = "裁剪";
            this.btn_Clip.UseVisualStyleBackColor = true;
            this.btn_Clip.Click += new System.EventHandler(this.btn_Clip_Click);
            // 
            // txb_ClipFeature
            // 
            this.txb_ClipFeature.Location = new System.Drawing.Point(55, 46);
            this.txb_ClipFeature.Name = "txb_ClipFeature";
            this.txb_ClipFeature.Size = new System.Drawing.Size(121, 21);
            this.txb_ClipFeature.TabIndex = 3;
            this.txb_ClipFeature.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txb_ClipFeature_MouseDown);
            // 
            // cmb_ClipLayer
            // 
            this.cmb_ClipLayer.FormattingEnabled = true;
            this.cmb_ClipLayer.Location = new System.Drawing.Point(55, 20);
            this.cmb_ClipLayer.Name = "cmb_ClipLayer";
            this.cmb_ClipLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_ClipLayer.TabIndex = 2;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label30.Location = new System.Drawing.Point(13, 53);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(29, 12);
            this.label30.TabIndex = 1;
            this.label30.Text = "矢量";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label29.Location = new System.Drawing.Point(13, 28);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(29, 12);
            this.label29.TabIndex = 0;
            this.label29.Text = "图层";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.cmb_ClassifyLayer);
            this.groupBox7.Controls.Add(this.btn_AfterClassify);
            this.groupBox7.Controls.Add(this.txb_ClassifyCount);
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.label19);
            this.groupBox7.Controls.Add(this.btn_Classify);
            this.groupBox7.ForeColor = System.Drawing.Color.Blue;
            this.groupBox7.Location = new System.Drawing.Point(17, 423);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(280, 80);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "图像分类";
            // 
            // cmb_ClassifyLayer
            // 
            this.cmb_ClassifyLayer.FormattingEnabled = true;
            this.cmb_ClassifyLayer.Location = new System.Drawing.Point(55, 21);
            this.cmb_ClassifyLayer.Name = "cmb_ClassifyLayer";
            this.cmb_ClassifyLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_ClassifyLayer.TabIndex = 5;
            // 
            // btn_AfterClassify
            // 
            this.btn_AfterClassify.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_AfterClassify.Location = new System.Drawing.Point(191, 48);
            this.btn_AfterClassify.Name = "btn_AfterClassify";
            this.btn_AfterClassify.Size = new System.Drawing.Size(75, 23);
            this.btn_AfterClassify.TabIndex = 4;
            this.btn_AfterClassify.Text = "分类后处理";
            this.btn_AfterClassify.UseVisualStyleBackColor = true;
            this.btn_AfterClassify.Click += new System.EventHandler(this.btn_AfterClassify_Click);
            // 
            // txb_ClassifyCount
            // 
            this.txb_ClassifyCount.Location = new System.Drawing.Point(55, 50);
            this.txb_ClassifyCount.Name = "txb_ClassifyCount";
            this.txb_ClassifyCount.Size = new System.Drawing.Size(121, 21);
            this.txb_ClassifyCount.TabIndex = 3;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label20.Location = new System.Drawing.Point(13, 54);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(29, 12);
            this.label20.TabIndex = 2;
            this.label20.Text = "数量";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Location = new System.Drawing.Point(13, 25);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(29, 12);
            this.label19.TabIndex = 1;
            this.label19.Text = "图层";
            // 
            // btn_Classify
            // 
            this.btn_Classify.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Classify.Location = new System.Drawing.Point(191, 21);
            this.btn_Classify.Name = "btn_Classify";
            this.btn_Classify.Size = new System.Drawing.Size(75, 23);
            this.btn_Classify.TabIndex = 0;
            this.btn_Classify.Text = "分类";
            this.btn_Classify.UseVisualStyleBackColor = true;
            this.btn_Classify.Click += new System.EventHandler(this.btn_Classify_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox17);
            this.tabPage4.Controls.Add(this.groupBox16);
            this.tabPage4.Controls.Add(this.groupBox15);
            this.tabPage4.Controls.Add(this.groupBox18);
            this.tabPage4.Controls.Add(this.groupBox14);
            this.tabPage4.Controls.Add(this.groupBox13);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(305, 516);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "空间分析";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.btn_CreateTinAuto);
            this.groupBox17.Controls.Add(this.cmb_CreateTinLayer);
            this.groupBox17.Controls.Add(this.btn_CreateTIN);
            this.groupBox17.Controls.Add(this.label39);
            this.groupBox17.ForeColor = System.Drawing.Color.Blue;
            this.groupBox17.Location = new System.Drawing.Point(15, 342);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(282, 52);
            this.groupBox17.TabIndex = 2;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "构建TIN";
            // 
            // btn_CreateTinAuto
            // 
            this.btn_CreateTinAuto.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_CreateTinAuto.Location = new System.Drawing.Point(236, 18);
            this.btn_CreateTinAuto.Name = "btn_CreateTinAuto";
            this.btn_CreateTinAuto.Size = new System.Drawing.Size(40, 23);
            this.btn_CreateTinAuto.TabIndex = 7;
            this.btn_CreateTinAuto.Text = "采样";
            this.btn_CreateTinAuto.UseVisualStyleBackColor = true;
            this.btn_CreateTinAuto.Click += new System.EventHandler(this.btn_CreateTinAuto_Click);
            // 
            // cmb_CreateTinLayer
            // 
            this.cmb_CreateTinLayer.FormattingEnabled = true;
            this.cmb_CreateTinLayer.Location = new System.Drawing.Point(53, 20);
            this.cmb_CreateTinLayer.Name = "cmb_CreateTinLayer";
            this.cmb_CreateTinLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_CreateTinLayer.TabIndex = 6;
            // 
            // btn_CreateTIN
            // 
            this.btn_CreateTIN.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_CreateTIN.Location = new System.Drawing.Point(190, 18);
            this.btn_CreateTIN.Name = "btn_CreateTIN";
            this.btn_CreateTIN.Size = new System.Drawing.Size(40, 23);
            this.btn_CreateTIN.TabIndex = 5;
            this.btn_CreateTIN.Text = "手绘";
            this.btn_CreateTIN.UseVisualStyleBackColor = true;
            this.btn_CreateTIN.Click += new System.EventHandler(this.btn_CreateTIN_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label39.Location = new System.Drawing.Point(19, 26);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(23, 12);
            this.label39.TabIndex = 4;
            this.label39.Text = "DEM";
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.btn_Extraction);
            this.groupBox16.Controls.Add(this.cmb_ExtractionRasterLayer);
            this.groupBox16.Controls.Add(this.label38);
            this.groupBox16.ForeColor = System.Drawing.Color.Blue;
            this.groupBox16.Location = new System.Drawing.Point(15, 278);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(282, 58);
            this.groupBox16.TabIndex = 0;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "裁剪分析";
            // 
            // btn_Extraction
            // 
            this.btn_Extraction.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Extraction.Location = new System.Drawing.Point(190, 22);
            this.btn_Extraction.Name = "btn_Extraction";
            this.btn_Extraction.Size = new System.Drawing.Size(75, 23);
            this.btn_Extraction.TabIndex = 2;
            this.btn_Extraction.Text = "裁剪分析";
            this.btn_Extraction.UseVisualStyleBackColor = true;
            this.btn_Extraction.Click += new System.EventHandler(this.btn_Extraction_Click);
            // 
            // cmb_ExtractionRasterLayer
            // 
            this.cmb_ExtractionRasterLayer.FormattingEnabled = true;
            this.cmb_ExtractionRasterLayer.Location = new System.Drawing.Point(53, 24);
            this.cmb_ExtractionRasterLayer.Name = "cmb_ExtractionRasterLayer";
            this.cmb_ExtractionRasterLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_ExtractionRasterLayer.TabIndex = 1;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label38.Location = new System.Drawing.Point(19, 27);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(29, 12);
            this.label38.TabIndex = 0;
            this.label38.Text = "栅格";
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.btn_Neighborhood);
            this.groupBox15.Controls.Add(this.cmb_NeighborhoodMethod);
            this.groupBox15.Controls.Add(this.cmb_NeighborhoodRasterLayer);
            this.groupBox15.Controls.Add(this.label37);
            this.groupBox15.Controls.Add(this.label36);
            this.groupBox15.ForeColor = System.Drawing.Color.Blue;
            this.groupBox15.Location = new System.Drawing.Point(15, 197);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(282, 75);
            this.groupBox15.TabIndex = 0;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "领域分析";
            // 
            // btn_Neighborhood
            // 
            this.btn_Neighborhood.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Neighborhood.Location = new System.Drawing.Point(190, 44);
            this.btn_Neighborhood.Name = "btn_Neighborhood";
            this.btn_Neighborhood.Size = new System.Drawing.Size(75, 23);
            this.btn_Neighborhood.TabIndex = 7;
            this.btn_Neighborhood.Text = "领域分析";
            this.btn_Neighborhood.UseVisualStyleBackColor = true;
            this.btn_Neighborhood.Click += new System.EventHandler(this.btn_Neighborhood_Click);
            // 
            // cmb_NeighborhoodMethod
            // 
            this.cmb_NeighborhoodMethod.FormattingEnabled = true;
            this.cmb_NeighborhoodMethod.Items.AddRange(new object[] {
            "esriGeoAnalysisFilter3x3HighPass",
            "esriGeoAnalysisFilter3x3LowPass",
            "esriGeoAnalysisStatsLength",
            "esriGeoAnalysisStatsMajority",
            "esriGeoAnalysisStatsMaximum",
            "esriGeoAnalysisStatsMean",
            "esriGeoAnalysisStatsMedian",
            "esriGeoAnalysisStatsMinimum",
            "esriGeoAnalysisStatsMinority",
            "esriGeoAnalysisStatsRange",
            "esriGeoAnalysisStatsStd",
            "esriGeoAnalysisStatsSum",
            "esriGeoAnalysisStatsVariety"});
            this.cmb_NeighborhoodMethod.Location = new System.Drawing.Point(53, 44);
            this.cmb_NeighborhoodMethod.Name = "cmb_NeighborhoodMethod";
            this.cmb_NeighborhoodMethod.Size = new System.Drawing.Size(121, 20);
            this.cmb_NeighborhoodMethod.TabIndex = 6;
            // 
            // cmb_NeighborhoodRasterLayer
            // 
            this.cmb_NeighborhoodRasterLayer.FormattingEnabled = true;
            this.cmb_NeighborhoodRasterLayer.Location = new System.Drawing.Point(53, 17);
            this.cmb_NeighborhoodRasterLayer.Name = "cmb_NeighborhoodRasterLayer";
            this.cmb_NeighborhoodRasterLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_NeighborhoodRasterLayer.TabIndex = 5;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label37.Location = new System.Drawing.Point(19, 48);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(29, 12);
            this.label37.TabIndex = 4;
            this.label37.Text = "方法";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label36.Location = new System.Drawing.Point(19, 22);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(29, 12);
            this.label36.TabIndex = 3;
            this.label36.Text = "栅格";
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.btn_TinVoronoi);
            this.groupBox18.Controls.Add(this.btn_TinContour);
            this.groupBox18.Controls.Add(this.btn_Contour);
            this.groupBox18.Controls.Add(this.cmb_TinVoronoiLayer);
            this.groupBox18.Controls.Add(this.cmb_TinContourLayer);
            this.groupBox18.Controls.Add(this.cmb_ContourLayer);
            this.groupBox18.Controls.Add(this.label42);
            this.groupBox18.Controls.Add(this.label41);
            this.groupBox18.Controls.Add(this.label40);
            this.groupBox18.ForeColor = System.Drawing.Color.Blue;
            this.groupBox18.Location = new System.Drawing.Point(15, 408);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(282, 100);
            this.groupBox18.TabIndex = 3;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "等高线/泰森多边形";
            // 
            // btn_TinVoronoi
            // 
            this.btn_TinVoronoi.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_TinVoronoi.Location = new System.Drawing.Point(190, 74);
            this.btn_TinVoronoi.Name = "btn_TinVoronoi";
            this.btn_TinVoronoi.Size = new System.Drawing.Size(75, 23);
            this.btn_TinVoronoi.TabIndex = 8;
            this.btn_TinVoronoi.Text = "泰森多边形";
            this.btn_TinVoronoi.UseVisualStyleBackColor = true;
            this.btn_TinVoronoi.Click += new System.EventHandler(this.btn_TinVoronoi_Click);
            // 
            // btn_TinContour
            // 
            this.btn_TinContour.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_TinContour.Location = new System.Drawing.Point(190, 45);
            this.btn_TinContour.Name = "btn_TinContour";
            this.btn_TinContour.Size = new System.Drawing.Size(75, 23);
            this.btn_TinContour.TabIndex = 7;
            this.btn_TinContour.Text = "生成等高线";
            this.btn_TinContour.UseVisualStyleBackColor = true;
            this.btn_TinContour.Click += new System.EventHandler(this.btn_TinContour_Click);
            // 
            // btn_Contour
            // 
            this.btn_Contour.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Contour.Location = new System.Drawing.Point(190, 15);
            this.btn_Contour.Name = "btn_Contour";
            this.btn_Contour.Size = new System.Drawing.Size(75, 23);
            this.btn_Contour.TabIndex = 6;
            this.btn_Contour.Text = "生成等高线";
            this.btn_Contour.UseVisualStyleBackColor = true;
            this.btn_Contour.Click += new System.EventHandler(this.btn_Contour_Click);
            // 
            // cmb_TinVoronoiLayer
            // 
            this.cmb_TinVoronoiLayer.FormattingEnabled = true;
            this.cmb_TinVoronoiLayer.Location = new System.Drawing.Point(53, 74);
            this.cmb_TinVoronoiLayer.Name = "cmb_TinVoronoiLayer";
            this.cmb_TinVoronoiLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_TinVoronoiLayer.TabIndex = 5;
            // 
            // cmb_TinContourLayer
            // 
            this.cmb_TinContourLayer.FormattingEnabled = true;
            this.cmb_TinContourLayer.Location = new System.Drawing.Point(53, 46);
            this.cmb_TinContourLayer.Name = "cmb_TinContourLayer";
            this.cmb_TinContourLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_TinContourLayer.TabIndex = 4;
            // 
            // cmb_ContourLayer
            // 
            this.cmb_ContourLayer.FormattingEnabled = true;
            this.cmb_ContourLayer.Location = new System.Drawing.Point(53, 18);
            this.cmb_ContourLayer.Name = "cmb_ContourLayer";
            this.cmb_ContourLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_ContourLayer.TabIndex = 3;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label42.Location = new System.Drawing.Point(19, 77);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(23, 12);
            this.label42.TabIndex = 2;
            this.label42.Text = "TIN";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label41.Location = new System.Drawing.Point(19, 49);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(23, 12);
            this.label41.TabIndex = 1;
            this.label41.Text = "TIN";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label40.Location = new System.Drawing.Point(19, 23);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(23, 12);
            this.label40.TabIndex = 0;
            this.label40.Text = "DEM";
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.btn_Visibility);
            this.groupBox14.Controls.Add(this.btn_LineOfSight);
            this.groupBox14.Controls.Add(this.cmb_VisibilityLayer);
            this.groupBox14.Controls.Add(this.cmb_LineOfSightLayer);
            this.groupBox14.Controls.Add(this.label35);
            this.groupBox14.Controls.Add(this.label34);
            this.groupBox14.ForeColor = System.Drawing.Color.Blue;
            this.groupBox14.Location = new System.Drawing.Point(15, 117);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(282, 72);
            this.groupBox14.TabIndex = 1;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "通视/视域";
            // 
            // btn_Visibility
            // 
            this.btn_Visibility.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Visibility.Location = new System.Drawing.Point(190, 42);
            this.btn_Visibility.Name = "btn_Visibility";
            this.btn_Visibility.Size = new System.Drawing.Size(75, 23);
            this.btn_Visibility.TabIndex = 8;
            this.btn_Visibility.Text = "视域分析";
            this.btn_Visibility.UseVisualStyleBackColor = true;
            this.btn_Visibility.Click += new System.EventHandler(this.btn_Visibility_Click);
            // 
            // btn_LineOfSight
            // 
            this.btn_LineOfSight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_LineOfSight.Location = new System.Drawing.Point(190, 13);
            this.btn_LineOfSight.Name = "btn_LineOfSight";
            this.btn_LineOfSight.Size = new System.Drawing.Size(75, 23);
            this.btn_LineOfSight.TabIndex = 7;
            this.btn_LineOfSight.Text = "通视分析";
            this.btn_LineOfSight.UseVisualStyleBackColor = true;
            this.btn_LineOfSight.Click += new System.EventHandler(this.btn_LineOfSight_Click);
            // 
            // cmb_VisibilityLayer
            // 
            this.cmb_VisibilityLayer.FormattingEnabled = true;
            this.cmb_VisibilityLayer.Location = new System.Drawing.Point(53, 45);
            this.cmb_VisibilityLayer.Name = "cmb_VisibilityLayer";
            this.cmb_VisibilityLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_VisibilityLayer.TabIndex = 6;
            // 
            // cmb_LineOfSightLayer
            // 
            this.cmb_LineOfSightLayer.FormattingEnabled = true;
            this.cmb_LineOfSightLayer.Location = new System.Drawing.Point(53, 18);
            this.cmb_LineOfSightLayer.Name = "cmb_LineOfSightLayer";
            this.cmb_LineOfSightLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_LineOfSightLayer.TabIndex = 5;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label35.Location = new System.Drawing.Point(18, 49);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(23, 12);
            this.label35.TabIndex = 4;
            this.label35.Text = "DEM";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label34.Location = new System.Drawing.Point(19, 23);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(23, 12);
            this.label34.TabIndex = 3;
            this.label34.Text = "DEM";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.btn_Aspect);
            this.groupBox13.Controls.Add(this.btn_Slope);
            this.groupBox13.Controls.Add(this.btn_HillShade);
            this.groupBox13.Controls.Add(this.cmb_AspectLayer);
            this.groupBox13.Controls.Add(this.cmb_SlopeLayer);
            this.groupBox13.Controls.Add(this.cmb_HillshadeLayer);
            this.groupBox13.Controls.Add(this.label33);
            this.groupBox13.Controls.Add(this.label32);
            this.groupBox13.Controls.Add(this.label31);
            this.groupBox13.ForeColor = System.Drawing.Color.Blue;
            this.groupBox13.Location = new System.Drawing.Point(15, 10);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(282, 98);
            this.groupBox13.TabIndex = 0;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "山体阴影/坡度/坡向";
            // 
            // btn_Aspect
            // 
            this.btn_Aspect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Aspect.Location = new System.Drawing.Point(190, 69);
            this.btn_Aspect.Name = "btn_Aspect";
            this.btn_Aspect.Size = new System.Drawing.Size(75, 23);
            this.btn_Aspect.TabIndex = 8;
            this.btn_Aspect.Text = "坡向函数";
            this.btn_Aspect.UseVisualStyleBackColor = true;
            this.btn_Aspect.Click += new System.EventHandler(this.btn_Aspect_Click);
            // 
            // btn_Slope
            // 
            this.btn_Slope.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Slope.Location = new System.Drawing.Point(190, 43);
            this.btn_Slope.Name = "btn_Slope";
            this.btn_Slope.Size = new System.Drawing.Size(75, 23);
            this.btn_Slope.TabIndex = 7;
            this.btn_Slope.Text = "坡度函数";
            this.btn_Slope.UseVisualStyleBackColor = true;
            this.btn_Slope.Click += new System.EventHandler(this.btn_Slope_Click);
            // 
            // btn_HillShade
            // 
            this.btn_HillShade.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_HillShade.Location = new System.Drawing.Point(190, 17);
            this.btn_HillShade.Name = "btn_HillShade";
            this.btn_HillShade.Size = new System.Drawing.Size(75, 23);
            this.btn_HillShade.TabIndex = 6;
            this.btn_HillShade.Text = "山体阴影";
            this.btn_HillShade.UseVisualStyleBackColor = true;
            this.btn_HillShade.Click += new System.EventHandler(this.btn_HillShade_Click);
            // 
            // cmb_AspectLayer
            // 
            this.cmb_AspectLayer.FormattingEnabled = true;
            this.cmb_AspectLayer.Location = new System.Drawing.Point(53, 69);
            this.cmb_AspectLayer.Name = "cmb_AspectLayer";
            this.cmb_AspectLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_AspectLayer.TabIndex = 5;
            // 
            // cmb_SlopeLayer
            // 
            this.cmb_SlopeLayer.FormattingEnabled = true;
            this.cmb_SlopeLayer.Location = new System.Drawing.Point(53, 45);
            this.cmb_SlopeLayer.Name = "cmb_SlopeLayer";
            this.cmb_SlopeLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_SlopeLayer.TabIndex = 4;
            // 
            // cmb_HillshadeLayer
            // 
            this.cmb_HillshadeLayer.FormattingEnabled = true;
            this.cmb_HillshadeLayer.Location = new System.Drawing.Point(53, 17);
            this.cmb_HillshadeLayer.Name = "cmb_HillshadeLayer";
            this.cmb_HillshadeLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_HillshadeLayer.TabIndex = 3;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label33.Location = new System.Drawing.Point(19, 72);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(23, 12);
            this.label33.TabIndex = 2;
            this.label33.Text = "DEM";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label32.Location = new System.Drawing.Point(19, 46);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(23, 12);
            this.label32.TabIndex = 1;
            this.label32.Text = "DEM";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label31.Location = new System.Drawing.Point(19, 21);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(23, 12);
            this.label31.TabIndex = 0;
            this.label31.Text = "DEM";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox24);
            this.tabPage5.Controls.Add(this.groupBox23);
            this.tabPage5.Controls.Add(this.groupBox22);
            this.tabPage5.Controls.Add(this.groupBox21);
            this.tabPage5.Controls.Add(this.groupBox20);
            this.tabPage5.Controls.Add(this.groupBox19);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(305, 516);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "水文分析";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox24
            // 
            this.groupBox24.Controls.Add(this.btn_StreamToFeature);
            this.groupBox24.Controls.Add(this.label51);
            this.groupBox24.Controls.Add(this.cmb_FlowDirectionToFeature);
            this.groupBox24.Controls.Add(this.cmb_streamRasterLayer);
            this.groupBox24.Controls.Add(this.label50);
            this.groupBox24.ForeColor = System.Drawing.Color.Blue;
            this.groupBox24.Location = new System.Drawing.Point(7, 402);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(289, 87);
            this.groupBox24.TabIndex = 5;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "栅格河网矢量化";
            // 
            // btn_StreamToFeature
            // 
            this.btn_StreamToFeature.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_StreamToFeature.Location = new System.Drawing.Point(200, 23);
            this.btn_StreamToFeature.Name = "btn_StreamToFeature";
            this.btn_StreamToFeature.Size = new System.Drawing.Size(75, 23);
            this.btn_StreamToFeature.TabIndex = 12;
            this.btn_StreamToFeature.Text = "矢量化";
            this.btn_StreamToFeature.UseVisualStyleBackColor = true;
            this.btn_StreamToFeature.Click += new System.EventHandler(this.btn_StreamToFeature_Click);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label51.Location = new System.Drawing.Point(17, 57);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(29, 12);
            this.label51.TabIndex = 11;
            this.label51.Text = "流向";
            // 
            // cmb_FlowDirectionToFeature
            // 
            this.cmb_FlowDirectionToFeature.FormattingEnabled = true;
            this.cmb_FlowDirectionToFeature.Location = new System.Drawing.Point(65, 54);
            this.cmb_FlowDirectionToFeature.Name = "cmb_FlowDirectionToFeature";
            this.cmb_FlowDirectionToFeature.Size = new System.Drawing.Size(121, 20);
            this.cmb_FlowDirectionToFeature.TabIndex = 10;
            // 
            // cmb_streamRasterLayer
            // 
            this.cmb_streamRasterLayer.FormattingEnabled = true;
            this.cmb_streamRasterLayer.Location = new System.Drawing.Point(65, 26);
            this.cmb_streamRasterLayer.Name = "cmb_streamRasterLayer";
            this.cmb_streamRasterLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_streamRasterLayer.TabIndex = 9;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label50.Location = new System.Drawing.Point(8, 29);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(53, 12);
            this.label50.TabIndex = 0;
            this.label50.Text = "栅格河网";
            // 
            // groupBox23
            // 
            this.groupBox23.Controls.Add(this.txb_FillZ);
            this.groupBox23.Controls.Add(this.label47);
            this.groupBox23.Controls.Add(this.btn_Fill);
            this.groupBox23.Controls.Add(this.cmb_FillLayer);
            this.groupBox23.Controls.Add(this.label46);
            this.groupBox23.ForeColor = System.Drawing.Color.Blue;
            this.groupBox23.Location = new System.Drawing.Point(7, 136);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(289, 90);
            this.groupBox23.TabIndex = 4;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "填洼(Fill)";
            // 
            // txb_FillZ
            // 
            this.txb_FillZ.Location = new System.Drawing.Point(66, 61);
            this.txb_FillZ.Name = "txb_FillZ";
            this.txb_FillZ.Size = new System.Drawing.Size(120, 21);
            this.txb_FillZ.TabIndex = 10;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label47.Location = new System.Drawing.Point(16, 64);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(35, 12);
            this.label47.TabIndex = 9;
            this.label47.Text = "Z阈值";
            // 
            // btn_Fill
            // 
            this.btn_Fill.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Fill.Location = new System.Drawing.Point(200, 27);
            this.btn_Fill.Name = "btn_Fill";
            this.btn_Fill.Size = new System.Drawing.Size(75, 23);
            this.btn_Fill.TabIndex = 2;
            this.btn_Fill.Text = "填洼";
            this.btn_Fill.UseVisualStyleBackColor = true;
            this.btn_Fill.Click += new System.EventHandler(this.btn_Fill_Click);
            // 
            // cmb_FillLayer
            // 
            this.cmb_FillLayer.FormattingEnabled = true;
            this.cmb_FillLayer.Location = new System.Drawing.Point(65, 29);
            this.cmb_FillLayer.Name = "cmb_FillLayer";
            this.cmb_FillLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_FillLayer.TabIndex = 1;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label46.Location = new System.Drawing.Point(17, 32);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(29, 12);
            this.label46.TabIndex = 0;
            this.label46.Text = "流向";
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.btn_Sink);
            this.groupBox22.Controls.Add(this.cmb_SinkLayer);
            this.groupBox22.Controls.Add(this.label45);
            this.groupBox22.ForeColor = System.Drawing.Color.Blue;
            this.groupBox22.Location = new System.Drawing.Point(6, 76);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(290, 56);
            this.groupBox22.TabIndex = 3;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "汇(Sink)";
            // 
            // btn_Sink
            // 
            this.btn_Sink.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Sink.Location = new System.Drawing.Point(201, 21);
            this.btn_Sink.Name = "btn_Sink";
            this.btn_Sink.Size = new System.Drawing.Size(75, 23);
            this.btn_Sink.TabIndex = 2;
            this.btn_Sink.Text = "汇";
            this.btn_Sink.UseVisualStyleBackColor = true;
            this.btn_Sink.Click += new System.EventHandler(this.btn_Sink_Click);
            // 
            // cmb_SinkLayer
            // 
            this.cmb_SinkLayer.FormattingEnabled = true;
            this.cmb_SinkLayer.Location = new System.Drawing.Point(67, 24);
            this.cmb_SinkLayer.Name = "cmb_SinkLayer";
            this.cmb_SinkLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_SinkLayer.TabIndex = 1;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label45.Location = new System.Drawing.Point(18, 27);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(29, 12);
            this.label45.TabIndex = 0;
            this.label45.Text = "流向";
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.txb_StreamLimit);
            this.groupBox21.Controls.Add(this.label49);
            this.groupBox21.Controls.Add(this.btn_StreamNet);
            this.groupBox21.Controls.Add(this.cmb_StreamNetLayer);
            this.groupBox21.Controls.Add(this.label48);
            this.groupBox21.ForeColor = System.Drawing.Color.Blue;
            this.groupBox21.Location = new System.Drawing.Point(7, 296);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(291, 100);
            this.groupBox21.TabIndex = 2;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "识别河流网络";
            // 
            // txb_StreamLimit
            // 
            this.txb_StreamLimit.Location = new System.Drawing.Point(65, 60);
            this.txb_StreamLimit.Name = "txb_StreamLimit";
            this.txb_StreamLimit.Size = new System.Drawing.Size(121, 21);
            this.txb_StreamLimit.TabIndex = 4;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label49.Location = new System.Drawing.Point(6, 63);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(53, 12);
            this.label49.TabIndex = 3;
            this.label49.Text = "流量阈值";
            // 
            // btn_StreamNet
            // 
            this.btn_StreamNet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_StreamNet.Location = new System.Drawing.Point(200, 28);
            this.btn_StreamNet.Name = "btn_StreamNet";
            this.btn_StreamNet.Size = new System.Drawing.Size(75, 23);
            this.btn_StreamNet.TabIndex = 2;
            this.btn_StreamNet.Text = "河流网络";
            this.btn_StreamNet.UseVisualStyleBackColor = true;
            this.btn_StreamNet.Click += new System.EventHandler(this.btn_StreamNet_Click);
            // 
            // cmb_StreamNetLayer
            // 
            this.cmb_StreamNetLayer.FormattingEnabled = true;
            this.cmb_StreamNetLayer.Location = new System.Drawing.Point(65, 25);
            this.cmb_StreamNetLayer.Name = "cmb_StreamNetLayer";
            this.cmb_StreamNetLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_StreamNetLayer.TabIndex = 1;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label48.Location = new System.Drawing.Point(16, 28);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(29, 12);
            this.label48.TabIndex = 0;
            this.label48.Text = "流量";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.btn_FlowAccumulation);
            this.groupBox20.Controls.Add(this.label44);
            this.groupBox20.Controls.Add(this.cmb_OutFlowDirectionRaster);
            this.groupBox20.ForeColor = System.Drawing.Color.Blue;
            this.groupBox20.Location = new System.Drawing.Point(5, 233);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(291, 60);
            this.groupBox20.TabIndex = 1;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "计算流量";
            // 
            // btn_FlowAccumulation
            // 
            this.btn_FlowAccumulation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_FlowAccumulation.Location = new System.Drawing.Point(201, 26);
            this.btn_FlowAccumulation.Name = "btn_FlowAccumulation";
            this.btn_FlowAccumulation.Size = new System.Drawing.Size(75, 23);
            this.btn_FlowAccumulation.TabIndex = 2;
            this.btn_FlowAccumulation.Text = "计算流量";
            this.btn_FlowAccumulation.UseVisualStyleBackColor = true;
            this.btn_FlowAccumulation.Click += new System.EventHandler(this.btn_FlowAccumulation_Click);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label44.Location = new System.Drawing.Point(18, 33);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(29, 12);
            this.label44.TabIndex = 0;
            this.label44.Text = "流向";
            // 
            // cmb_OutFlowDirectionRaster
            // 
            this.cmb_OutFlowDirectionRaster.FormattingEnabled = true;
            this.cmb_OutFlowDirectionRaster.Location = new System.Drawing.Point(67, 28);
            this.cmb_OutFlowDirectionRaster.Name = "cmb_OutFlowDirectionRaster";
            this.cmb_OutFlowDirectionRaster.Size = new System.Drawing.Size(121, 20);
            this.cmb_OutFlowDirectionRaster.TabIndex = 1;
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.btn_FlowDirection);
            this.groupBox19.Controls.Add(this.cmb_FlowDirectionLayer);
            this.groupBox19.Controls.Add(this.label43);
            this.groupBox19.ForeColor = System.Drawing.Color.Blue;
            this.groupBox19.Location = new System.Drawing.Point(6, 10);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(293, 63);
            this.groupBox19.TabIndex = 0;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "计算流向";
            // 
            // btn_FlowDirection
            // 
            this.btn_FlowDirection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_FlowDirection.Location = new System.Drawing.Point(201, 27);
            this.btn_FlowDirection.Name = "btn_FlowDirection";
            this.btn_FlowDirection.Size = new System.Drawing.Size(75, 23);
            this.btn_FlowDirection.TabIndex = 5;
            this.btn_FlowDirection.Text = "计算流向";
            this.btn_FlowDirection.UseVisualStyleBackColor = true;
            this.btn_FlowDirection.Click += new System.EventHandler(this.btn_FlowDirection_Click);
            // 
            // cmb_FlowDirectionLayer
            // 
            this.cmb_FlowDirectionLayer.FormattingEnabled = true;
            this.cmb_FlowDirectionLayer.Location = new System.Drawing.Point(67, 27);
            this.cmb_FlowDirectionLayer.Name = "cmb_FlowDirectionLayer";
            this.cmb_FlowDirectionLayer.Size = new System.Drawing.Size(121, 20);
            this.cmb_FlowDirectionLayer.TabIndex = 4;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label43.Location = new System.Drawing.Point(18, 27);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(23, 12);
            this.label43.TabIndex = 3;
            this.label43.Text = "DEM";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomToLayer,
            this.deleteLayer});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 48);
            // 
            // zoomToLayer
            // 
            this.zoomToLayer.Name = "zoomToLayer";
            this.zoomToLayer.Size = new System.Drawing.Size(160, 22);
            this.zoomToLayer.Text = "缩放至当前图层";
            this.zoomToLayer.Click += new System.EventHandler(this.zoomToLayer_Click);
            // 
            // deleteLayer
            // 
            this.deleteLayer.Name = "deleteLayer";
            this.deleteLayer.Size = new System.Drawing.Size(160, 22);
            this.deleteLayer.Text = "删除图层";
            this.deleteLayer.Click += new System.EventHandler(this.deleteLayer_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 567);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "ArcEngine Controls Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.grp_import.ResumeLayout(false);
            this.grp_import.PerformLayout();
            this.grp_load.ResumeLayout(false);
            this.grp_load.PerformLayout();
            this.grp_create.ResumeLayout(false);
            this.grp_create.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ToColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ColorBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_FromColor)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox24.ResumeLayout(false);
            this.groupBox24.PerformLayout();
            this.groupBox23.ResumeLayout(false);
            this.groupBox23.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.groupBox21.ResumeLayout(false);
            this.groupBox21.PerformLayout();
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox19.ResumeLayout(false);
            this.groupBox19.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNewDoc;
        private System.Windows.Forms.ToolStripMenuItem menuOpenDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveDoc;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuExitApp;
        private System.Windows.Forms.ToolStripSeparator menuSeparator;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBarXY;
        private System.Windows.Forms.ToolStripMenuItem menuRaster;
        private System.Windows.Forms.ToolStripMenuItem menuAccessFile;
        private System.Windows.Forms.ToolStripMenuItem menuConnectDB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox grp_import;
        private System.Windows.Forms.GroupBox grp_load;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grp_create;
        private System.Windows.Forms.Button btn_create;
        private System.Windows.Forms.TextBox txb_create;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_import;
        private System.Windows.Forms.ComboBox cmb_ImportRstCatalog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.ComboBox cmb_LoadRstDataset;
        private System.Windows.Forms.ComboBox cmb_LoadRstCatalog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txb_importRstDataset;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmb_StatisticsBand;
        private System.Windows.Forms.ComboBox cmb_StatisticsLayer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmb_StretchMethod;
        private System.Windows.Forms.ComboBox cmb_StretchLayer;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btn_MultiBandHis;
        private System.Windows.Forms.Button btn_SingleBandHis;
        private System.Windows.Forms.ComboBox cmb_DrawHisBand;
        private System.Windows.Forms.ComboBox cmb_DrawHisLayer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_CalculateNDVI;
        private System.Windows.Forms.ComboBox cmb_NDVILayer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_Statistics;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_Render;
        private System.Windows.Forms.ComboBox cmb_RenderBand;
        private System.Windows.Forms.ComboBox cmb_RenderLayer;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_Stretch;
        private System.Windows.Forms.ComboBox cmb_StretchBand;
        private System.Windows.Forms.ComboBox cmb_BBand;
        private System.Windows.Forms.ComboBox cmb_GBand;
        private System.Windows.Forms.ComboBox cmb_RBand;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btn_RGB;
        private System.Windows.Forms.ComboBox cmb_RGBLayer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem zoomToLayer;
        private System.Windows.Forms.ToolStripMenuItem deleteLayer;
        private System.Windows.Forms.PictureBox pb_ToColor;
        private System.Windows.Forms.PictureBox pb_ColorBar;
        private System.Windows.Forms.PictureBox pb_FromColor;
        private System.Windows.Forms.ColorDialog cd_FromColor;
        private System.Windows.Forms.ColorDialog cd_ToColor;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btn_Classify;
        private System.Windows.Forms.Button btn_AfterClassify;
        private System.Windows.Forms.TextBox txb_ClassifyCount;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cmb_ClassifyLayer;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button btn_PanSharpen;
        private System.Windows.Forms.ComboBox cmb_MultiBandLayer;
        private System.Windows.Forms.ComboBox cmb_SingleBandLayer;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox cmb_MosaicRstCatalog;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btn_Mosaic;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button btn_Filter;
        private System.Windows.Forms.ComboBox cmb_FilterMethod;
        private System.Windows.Forms.ComboBox cmb_FilterLayer;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Button btn_Transform;
        private System.Windows.Forms.TextBox txb_angle;
        private System.Windows.Forms.ComboBox cmb_TransformMethod;
        private System.Windows.Forms.ComboBox cmb_TransformLayer;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btn_Clip;
        private System.Windows.Forms.TextBox txb_ClipFeature;
        private System.Windows.Forms.ComboBox cmb_ClipLayer;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.Button btn_CreateTinAuto;
        private System.Windows.Forms.ComboBox cmb_CreateTinLayer;
        private System.Windows.Forms.Button btn_CreateTIN;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.Button btn_Extraction;
        private System.Windows.Forms.ComboBox cmb_ExtractionRasterLayer;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Button btn_Neighborhood;
        private System.Windows.Forms.ComboBox cmb_NeighborhoodMethod;
        private System.Windows.Forms.ComboBox cmb_NeighborhoodRasterLayer;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Button btn_TinVoronoi;
        private System.Windows.Forms.Button btn_TinContour;
        private System.Windows.Forms.Button btn_Contour;
        private System.Windows.Forms.ComboBox cmb_TinVoronoiLayer;
        private System.Windows.Forms.ComboBox cmb_TinContourLayer;
        private System.Windows.Forms.ComboBox cmb_ContourLayer;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Button btn_Visibility;
        private System.Windows.Forms.Button btn_LineOfSight;
        private System.Windows.Forms.ComboBox cmb_VisibilityLayer;
        private System.Windows.Forms.ComboBox cmb_LineOfSightLayer;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Button btn_Aspect;
        private System.Windows.Forms.Button btn_Slope;
        private System.Windows.Forms.Button btn_HillShade;
        private System.Windows.Forms.ComboBox cmb_AspectLayer;
        private System.Windows.Forms.ComboBox cmb_SlopeLayer;
        private System.Windows.Forms.ComboBox cmb_HillshadeLayer;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.Button btn_FlowAccumulation;
        private System.Windows.Forms.ComboBox cmb_OutFlowDirectionRaster;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.Button btn_FlowDirection;
        private System.Windows.Forms.ComboBox cmb_FlowDirectionLayer;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.Button btn_Sink;
        private System.Windows.Forms.ComboBox cmb_SinkLayer;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.TextBox txb_FillZ;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Button btn_Fill;
        private System.Windows.Forms.ComboBox cmb_FillLayer;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox txb_StreamLimit;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Button btn_StreamNet;
        private System.Windows.Forms.ComboBox cmb_StreamNetLayer;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.Button btn_StreamToFeature;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.ComboBox cmb_FlowDirectionToFeature;
        private System.Windows.Forms.ComboBox cmb_streamRasterLayer;
        private System.Windows.Forms.Label label50;
    }
}

