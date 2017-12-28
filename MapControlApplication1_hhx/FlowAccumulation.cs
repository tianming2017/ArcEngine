using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geoprocessor;

namespace MapControlApplication1_hhx
{
    public class FlowAccumulation : IDisposable
    {
        private static int height = 6;
        private static int width = 6;
        private ItemInfo[,] array1;//这里的array的大小一定要在构造函数中写 根据具体大小申请
        private int[,] array2;
        private byte[,] inputArray = {{2,2,2,4,4,8},
                                    {2,2,2,4,4,8},
                                    {1,1,2,4,8,4},
                                    {128,128,1,2,4,8},
                                    {2,2,1,4,4,4},
                                    {1,1,1,1,4,16}};

        public FlowAccumulation()
        {
            array1 = new ItemInfo[height, width];
            array2 = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array1[i, j] = new ItemInfo();
                }
            }
        }

        public FlowAccumulation(System.Array array, int h, int w)
        {
            height = h;
            width = w;
            inputArray = (byte[,])array;
            array1 = new ItemInfo[height, width];
            array2 = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array1[i, j] = new ItemInfo();
                }
            }

        }

        ~FlowAccumulation()
        {
            array1 = null;
            array2 = null;
            inputArray = null;
            
        }

        //step1
        private void Init()
        {
            //Console.WriteLine("-----show Flow Direction-----");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //Console.Write(inputArray[i, j] + " ");
                    //array1[i, j] = 0;//我被多少个人指向,默认0个
                    array2[i, j] = -1;//结果,-1表示尚未计算
                }
               // Console.Write("\r\n");
            }
        }

        //step2
        private void CalculatePointArray()
        {
            //计算array1, 保存了指向我的箭头来源
            for (ushort i = 0; i < height; i++)
            {
                for (ushort j = 0; j < width; j++)
                {
                    switch (inputArray[i, j])//注意边界
                    {
                        case 1://右边
                            if (j + 1 < width)
                            {
                                array1[i, j + 1].AddPoint(i, j);
                            }
                            break;
                        case 2://右下
                            if (i + 1 < height && j + 1 < width)
                            {
                                array1[i + 1, j + 1].AddPoint(i, j);
                            }
                            break;
                        case 4://下面
                            if (i + 1 < height)
                            {
                                array1[i + 1, j].AddPoint(i, j);
                            }
                            break;
                        case 8://左下
                            if (i + 1 < height && j - 1 >= 0)
                            {
                                array1[i + 1, j - 1].AddPoint(i, j);
                            }
                            break;
                        case 16://左边
                            if (j - 1 >= 0)
                            {
                                array1[i, j - 1].AddPoint(i, j);
                            }
                            break;
                        case 32://左上
                            if (i - 1 >= 0 && j - 1 >= 0)
                            {
                                array1[i - 1, j - 1].AddPoint(i, j);
                            }
                            break;
                        case 64://上面
                            if (i - 1 >= 0)
                            {
                                array1[i - 1, j].AddPoint(i, j);
                            }

                            break;
                        case 128://右上
                            if (i - 1 >= 0 && j + 1 < width)
                            {
                                array1[i - 1, j + 1].AddPoint(i, j);
                            }
                            break;
                        default:
                            break;//未定向
                    }
                }
            }

        }

        //step3
        private void CalculateFlowAccumulationFromPointArray()
        {
            //计算array2
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    CalculateFlowAccumulationFromPoint(i, j);
                }
            }
        }

        //内嵌于step3 需要递归
        //注意array2初始值为-1 表示未计算
        private int CalculateFlowAccumulationFromPoint(int i, int j)
        {
            int count = array1[i, j].GetPointCounts();
            if (count == 0)
            {
                array2[i, j] = 0;//没有人指向我 流量为0
            }
            else//有指向的人
            {
                array2[i, j] = count;
                //遍历每一个指向我的人
                for (int k = 0; k < count; k++)
                {
                    Point point = array1[i, j].GetOnePoint(k);
                    if (array2[point.GetI(), point.GetJ()] == -1)//指向我的人 尚未计算
                    {
                        //递归计算该值
                        int temp = CalculateFlowAccumulationFromPoint(point.GetI(), point.GetJ());//计算指向我的人
                        array2[i, j] += temp;
                    }
                    else
                    {
                        array2[i, j] += array2[point.GetI(), point.GetJ()];//指向我的人已经计算
                    }
                }
            }
            return array2[i, j];
        }

        //step4
        //view
        public void ShowResult()
        {
            Console.WriteLine("-----show Flow Accumulation-----");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(array2[i, j] + " ");
                }
                Console.Write("\r\n");
            }
        }

        //step2.5  view
        public void ShowPointArray()
        {
            Console.WriteLine("-----show Point Array-----");
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.Write(array1[i, j].GetPointCounts() + " ");
                }
                Console.Write("\r\n");
            }
        }

        private void ReleaseMemory()
        {
            inputArray = null;
            array1 = null;
        }

        public System.Array CalculateFlowAccumulation()
        {
            Init();
            CalculatePointArray();
            inputArray = null;
            //ShowPointArray();
            CalculateFlowAccumulationFromPointArray();
            //ShowResult();
            ReleaseMemory();

            return array2;
        }


        public void Dispose()
        {
            inputArray = null;
            array1 = null;
            array2 = null;
        }
    }


    public class ItemInfo
    {
        private List<Point> points;
        public ItemInfo()
        {
            points = new List<Point>();
        }

        public int GetPointCounts()
        {
            return points.Count;
        }
        public void AddPoint(ushort i, ushort j)
        {
            points.Add(new Point(i, j));
        }
        public Point GetOnePoint(int pointNum)
        {
            return points[pointNum];
        }
    }

    public class Point
    {
        private ushort i;
        private ushort j;
        public Point(ushort i, ushort j)
        {
            this.i = i;
            this.j = j;
        }
        public int GetI()
        {
            return i;
        }
        public int GetJ()
        {
            return j;
        }
    }
}
