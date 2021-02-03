using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mini_Kvantor.UI
{
    public partial class Schedule_REFSYS : Form
    {
        public int dataPRN;

        public string nameTTS;

        List<List<RinexData>> listCanal;
  
        List<DateTime> dateTime1 = new List<DateTime>();
        List<DateTime> dateTime2 = new List<DateTime>();
        List<DateTime> dateTime3 = new List<DateTime>();
        List<DateTime> dateTime4 = new List<DateTime>();
        List<DateTime> dateTime5 = new List<DateTime>();
        List<DateTime> dateTime6 = new List<DateTime>();
        List<DateTime> dateTime7 = new List<DateTime>();
        List<DateTime> dateTime8 = new List<DateTime>();
        List<DateTime> dateTime9 = new List<DateTime>();
        List<DateTime> dateTime10 = new List<DateTime>();

        DateTime startDate;
        DateTime endDate;

        PlotModel model;

        LineSeries myLine_1 = new LineSeries();
        LineSeries myLine_2 = new LineSeries();
        LineSeries myLine_3 = new LineSeries();
        LineSeries myLine_4 = new LineSeries();
        LineSeries myLine_5 = new LineSeries();
        LineSeries myLine_6 = new LineSeries();
        LineSeries myLine_7 = new LineSeries();
        LineSeries myLine_8 = new LineSeries();
        LineSeries myLine_9 = new LineSeries();
        LineSeries myLine_10 = new LineSeries();

        public Schedule_REFSYS(string nameTTS, int dataPRN, List<List<RinexData>> listCanal,
             DateTime startDate,  DateTime endDate)
        {
            InitializeComponent();

            this.dataPRN = dataPRN;

            this.listCanal = listCanal;

            this.nameTTS = nameTTS;

            this.startDate = startDate;

            this.endDate = endDate;

            DrawGraph(dataPRN, listCanal);

            chB_L1P.Checked = true;
            chB_L2P.Checked = true;
            chB_L3P.Checked = true;
            chB_L5P.Checked = true;
            chB_L1C.Checked = true;
            chB_L2C.Checked = true;
            chB_L3C.Checked = true;

        }

        public void DrawGraph(int dataPRN,List<List<RinexData>> listCanal)
        {
            List<int> can_1 = new List<int>() { };
            List<int> can_2 = new List<int>() { };
            List<int> can_3 = new List<int>() { };
            List<int> can_4 = new List<int>() { };
            List<int> can_5 = new List<int>() { };
            List<int> can_6 = new List<int>() { };
            List<int> can_7 = new List<int>() { };
            List<int> can_8 = new List<int>() { };
            List<int> can_9 = new List<int>() { };
            List<int> can_10 = new List<int>() { };
            //   получаем все каналы
            foreach (var canal in listCanal)
            {
                foreach (var item in canal)// item - конкретный канал 
                {
                    if (item.SAT == dataPRN)
                    {
                        // конкретный канал со своим значением REFSYS
                        switch (item.FRC)
                        {
                            case "L1P":
                                can_1.Add(item.REFSYS);
                                double JD1 = item.MJD + 2400000.5;
                                DateTime date1 = Methods.FromJulian(JD1) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime1.Add(date1);
                                break;

                            case "L2P":
                                can_2.Add(item.REFSYS);
                                double JD2 = item.MJD + 2400000.5;
                                DateTime date2 = Methods.FromJulian(JD2) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime2.Add(date2);
                                break;

                            case "L3P":
                                can_3.Add(item.REFSYS);
                                double JD3 = item.MJD + 2400000.5;
                                DateTime date3 = Methods.FromJulian(JD3) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime3.Add(date3);
                                break;

                            case "L4P":
                                can_4.Add(item.REFSYS);
                                double JD4 = item.MJD + 2400000.5;
                                DateTime date4 = Methods.FromJulian(JD4) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime4.Add(date4);
                                break;

                            case "L5P":
                                can_5.Add(item.REFSYS);
                                double JD5 = item.MJD + 2400000.5;
                                DateTime date5 = Methods.FromJulian(JD5) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime5.Add(date5);
                                break;

                            case "L1C":
                                can_6.Add(item.REFSYS);
                                double JD6 = item.MJD + 2400000.5;
                                DateTime date6 = Methods.FromJulian(JD6) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime6.Add(date6);
                                break;

                            case "L2C":
                                can_7.Add(item.REFSYS);
                                double JD7 = item.MJD + 2400000.5;
                                DateTime date7 = Methods.FromJulian(JD7) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime7.Add(date7);
                                break;

                            case "L3C":
                                can_8.Add(item.REFSYS);
                                double JD8 = item.MJD + 2400000.5;
                                DateTime date8 = Methods.FromJulian(JD8) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime8.Add(date8);
                                break;

                            case "L4C":
                                can_9.Add(item.REFSYS);
                                double JD9 = item.MJD + 2400000.5;
                                DateTime date9 = Methods.FromJulian(JD9) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime9.Add(date9);
                                break;

                            case "L5C":
                                can_10.Add(item.REFSYS);
                                double JD = item.MJD + 2400000.5; 
                                DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(item.STTIME);
                                dateTime10.Add(date);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            model = new PlotModel
            {
                Title = $" Данные по спутнику № {dataPRN},  TTS-{nameTTS}\n" +
                $" { startDate} - {endDate}"

            };

            myLine_1.Color = OxyColors.Aqua;// цвет линии графика 
            myLine_2.Color = OxyColors.Blue;
            myLine_3.Color = OxyColors.DarkOrange;
            myLine_4.Color = OxyColors.DarkCyan;
            myLine_5.Color = OxyColors.Red;
            myLine_6.Color = OxyColors.Green;
            myLine_7.Color = OxyColors.Violet;
            myLine_8.Color = OxyColors.DarkSlateGray;
            myLine_9.Color = OxyColors.Brown;
            myLine_10.Color = OxyColors.Lime;

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                TitleFontSize = 20,
                TitleFontWeight = 20,
                TitleColor = OxyColors.Maroon,
                Title = "REFSYS",
                IntervalLength = 50,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Solid,

            });

            for (int i = 0; i < dateTime1.Count; i++)
            {
                string strDateTime = dateTime1[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);
              
                myLine_1.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime1[i]), can_1[i]));
            
            }
            for (int i = 0; i < dateTime2.Count; i++)
            {
                string strDateTime = dateTime2[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_2.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime2[i]), can_2[i]));
            }
            for (int i = 0; i < dateTime3.Count; i++)
            {
                string strDateTime = dateTime3[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_3.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime3[i]), can_3[i]));
            }
            for (int i = 0; i < dateTime4.Count; i++)
            {
                string strDateTime = dateTime4[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_4.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime4[i]), can_4[i]));
            }
            for (int i = 0; i < dateTime5.Count; i++)
            {
                string strDateTime = dateTime5[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_5.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime5[i]), can_5[i]));
            }
            for (int i = 0; i < dateTime6.Count; i++)
            {
                string strDateTime = dateTime6[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_6.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime6[i]), can_6[i]));
            }
            for (int i = 0; i < dateTime7.Count; i++)
            {
                string strDateTime = dateTime7[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_7.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime7[i]), can_7[i]));
            }
            for (int i = 0; i < dateTime8.Count; i++)
            {
                string strDateTime = dateTime8[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_8.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime8[i]), can_8[i]));
            }
            for (int i = 0; i < dateTime9.Count; i++)
            {
                string strDateTime = dateTime9[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_9.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime9[i]), can_9[i]));
            }
            for (int i = 0; i < dateTime10.Count; i++)
            {
                string strDateTime = dateTime10[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);

                myLine_10.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime10[i]), can_10[i]));
            }
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                TitleFontSize = 20,
                TitleFontWeight = 15,
                TitleColor = OxyColors.Maroon,
                Title = "DateTime",// "STTIME"
                StringFormat = "dd/MM/yyyy ",
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Solid,
          
            });

            model.Series.Add(myLine_1);
            model.Series.Add(myLine_2);
            model.Series.Add(myLine_3);
            model.Series.Add(myLine_4);
            model.Series.Add(myLine_5);
            model.Series.Add(myLine_6);
            model.Series.Add(myLine_7);
            model.Series.Add(myLine_8);
            model.Series.Add(myLine_9);
            model.Series.Add(myLine_10);
            
            plotView1.Model = model;

        }

        private void Schedule_REFSYS_Load(object sender, EventArgs e)
        {
            this.Text = "Диаграмма REFSYS";
        }

        private void chB_L1P_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L1P.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_1);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_1))
                {
                    plotView1.Model.Series.Add(myLine_1);
                    plotView1.Refresh();
                }
            }
        }

        private void chB_L2P_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L2P.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_2);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_2))
                {
                    plotView1.Model.Series.Add(myLine_2);
                    plotView1.Refresh();
                }
            }
        }
        private void chB_L3P_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L3P.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_3);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_3))
                {
                    plotView1.Model.Series.Add(myLine_3);
                    plotView1.Refresh();
                }
            }
        }

        private void chB_L5P_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L5P.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_5);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_5))
                {
                    plotView1.Model.Series.Add(myLine_5);
                    plotView1.Refresh();
                }
            }
        }

        private void chB_L1C_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L1C.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_6);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_6))
                {
                    plotView1.Model.Series.Add(myLine_6);
                    plotView1.Refresh();
                }
            }
        }

        private void chB_L2C_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L2C.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_7);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_7))
                {
                    plotView1.Model.Series.Add(myLine_7);
                    plotView1.Refresh();
                }
            }
        }

        private void chB_L3C_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_L3C.Checked == false)
            {
                plotView1.Model.Series.Remove(myLine_8);
                plotView1.Refresh();
            }
            else
            {
                if (!plotView1.Model.Series.Contains(myLine_8))
                {
                    plotView1.Model.Series.Add(myLine_8);
                    plotView1.Refresh();
                }
            }
        }
        private void btnShowAllTracks_Click(object sender, EventArgs e)
        {
            chB_L1P.Checked = true;
            chB_L2P.Checked = true;
            chB_L3P.Checked = true;
            chB_L5P.Checked = true;
            chB_L1C.Checked = true;
            chB_L2C.Checked = true;
            chB_L3C.Checked = true;
    
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            // вставляем метод открытия директории
            // для отображения графика по : суткам,неделе и накопительный 
        }
    }
}
