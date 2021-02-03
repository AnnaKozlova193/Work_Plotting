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
    public partial class OtherParametersForm : Form
    {
        public int dataPRN;// получаем номе PRN

        List<DataName> values; // получаем список с данными 

        public string nameTTS; 

        public OtherParametersForm(string nameTTS,int dataPRN, List<DataName> values)
        {
            InitializeComponent();

            this.dataPRN = dataPRN;

            this.values = values;

            this.nameTTS = nameTTS;

        }

        private void OtherParametersForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_TRKL_Click(object sender, EventArgs e)
        {
            string name = "TRKL";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].TRKL);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS,name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_ELV_Click(object sender, EventArgs e)
        {
            string name = "ELV";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].ELV);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS,name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_AZTH_Click(object sender, EventArgs e)
        {
            string name = "AZTH";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].AZTH);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_REFSV_Click(object sender, EventArgs e)
        {
            string name = "REFSV";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].REFSV);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_SRSV_Click(object sender, EventArgs e)
        {
            string name = "SRSV";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].SRSV);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_SRGPS_Click(object sender, EventArgs e)
        {
            string name = "SRGPS";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].SRGPS);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_SMDI_Click(object sender, EventArgs e)
        {
            string name = "SMDI";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].SMDI);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_DSG_Click(object sender, EventArgs e)
        {
            string name = "DSG";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].DSG);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_IOE_Click(object sender, EventArgs e)
        {
            string name = "IOE";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].IOE);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();

        }

        private void btn_MDTR_Click(object sender, EventArgs e)
        {
            string name = "MDTR";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].MDTR);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();
        }

        private void btn_SMDT_Click(object sender, EventArgs e)
        {
            string name = "SMDT";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].SMDT);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();

        }

        private void btn_MDIO_Click(object sender, EventArgs e)
        {
            string name = "MDIO";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    data.Add(values[i].MDIO);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name, dataPRN, data, dateTime);
            form.Show();

        }

        private void btn_CK_Click(object sender, EventArgs e)
        {
            string name = "CK";

            List<DateTime> dateTime = new List<DateTime>();

            List<int> data = new List<int>() { };

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);
                
                    data.Add(values[i].CK);
                }
            }
            UI.windowMainForm form = new windowMainForm(nameTTS, name,dataPRN, data, dateTime);
            form.Show();
        }
   
    }

}

