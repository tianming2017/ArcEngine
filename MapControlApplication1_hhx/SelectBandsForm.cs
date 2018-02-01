using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
//using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.SpatialAnalyst;
//using ESRI.ArcGIS.GeoAnalyst;

//6.2 波段选择窗体
namespace MapControlApplication1_hhx
{
    public partial class SelectBandsForm : Form
    {
        #region class private member
        private IRasterLayer m_rstlayer;//存储要绘制对比直方图的栅格图层对象
        //private int m_bandnum;          //存储栅格图层的波段总数
        private int[] m_selband;        //存储栅格图层的选择的波段
        #endregion

        public SelectBandsForm()
        {
            InitializeComponent();
        }
        public SelectBandsForm(IRasterLayer rstlayer)
        { 
            //代码填空，波段选择窗体的实现
            InitializeComponent();
            //基本操作
            this.m_rstlayer = rstlayer;
            //遍历当前图层并把波段添加到CheckedListBox中，供用户选择需要进行对比的波段。
            IRaster2 raster2 = rstlayer.Raster as IRaster2;
            IRasterDataset rasterDataset = raster2.RasterDataset;
            IRasterBandCollection rasterBandCollection = rasterDataset as IRasterBandCollection;
            int bandCount = rasterBandCollection.Count;
            IRasterBand rasterBand = null;
            cklb_CompareHistogram.Items.Clear();
            for (int i = 0; i < bandCount; i++)
            { 
                rasterBand = rasterBandCollection.Item(i);
                string bandName = rasterBand.Bandname;
                cklb_CompareHistogram.Items.Add(bandName);
            }
        }

        //点击 绘制对比直方图，进行多直方图
        private void btn_DrawCompareHistogram_Click(object sender, EventArgs e)
        {
            //检测有多少选择的波段
            int k = 0;
            for (int i = 0; i < cklb_CompareHistogram.Items.Count; i++)
            {
                if (cklb_CompareHistogram.GetItemChecked(i)) 
                {
                    k++;
                }
            }

            m_selband = new int[k];

            //把选择的波段索引存储在整数数组里
            int j = 0;
            for (int i = 0; i < cklb_CompareHistogram.Items.Count; i++)
            {
                if (cklb_CompareHistogram.GetItemChecked(i))
                {
                    m_selband[j] = i;
                    j++;
                }
            }
            //创建对比直方图绘制窗体对象，并展示出来
            HistogramCompareForm histogramCompare = new HistogramCompareForm(m_rstlayer, m_selband);
            histogramCompare.ShowDialog();
        }
    }
}
