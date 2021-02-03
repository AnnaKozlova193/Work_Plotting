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

namespace mini_Kvantor
{
    public partial class Schedule_REFGPS : Form
    {
        public int dataPRN;

        public string nameTTS;

        List<int> dataREFGPS;

        List<DateTime> dateTime;

        DateTime startDate;

        DateTime endDate;
   
        public Schedule_REFGPS(string nameTTS, int dataPRN,List<int> dataREFGPS, List<DateTime> dateTime)
        {
            InitializeComponent();

            this.dataPRN = dataPRN;

            this.dataREFGPS = dataREFGPS;

            this.dateTime = dateTime;

            this.nameTTS = nameTTS;
         
            DrawGraph(dataPRN, dataREFGPS,dateTime);
     
        }

        public void DrawGraph(int dataPRN, List<int> dataREFGPS, List<DateTime> dateTime)
        {
            for (int i = 0; i < dateTime.Count; i++)
            {
                if (i == 0)
                {
                    startDate = dateTime[i];
                }
                if (i == dateTime.Count-1)
                {
                    endDate = dateTime[i];
                }
            }

            var model = new PlotModel
            {
                Title = $" Данные по спутнику № {dataPRN},  TTS-{nameTTS}\n" +
                $" { startDate} - {endDate}"

            };
    
            var myLine = new LineSeries();

            myLine.Color = OxyColors.Teal;// цвет линии графика 
       
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                TitleFontSize = 20,
                TitleFontWeight = 20,
                TitleColor = OxyColors.Maroon,
                Title = "REFGPS",
                IntervalLength = 50,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Solid,

            });
        
            for (int i = 0; i < dateTime.Count; i++)
            {
                string strDateTime = dateTime[i].ToString();
                // вырезаем символы из строки 
                char[] arr = new char[3] { ' ', '.', ':' };
                string[] TextArray = strDateTime.Split(arr, StringSplitOptions.RemoveEmptyEntries);
                string one = string.Join(null, TextArray);
                // строим график
                myLine.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime[i]),dataREFGPS[i]));
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
         
            model.Series.Add(myLine);

            PlotView myPlot = new PlotView();

            myPlot.Model = model;

            myPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            myPlot.Location = new System.Drawing.Point(0, 0);
            myPlot.Size = new System.Drawing.Size(500, 500);
            myPlot.TabIndex = 0;

            Controls.Add(myPlot);
        }


        private void Schedule_REFGPS_Load(object sender, EventArgs e)
        {

        }
    }
}
