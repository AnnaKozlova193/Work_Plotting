using mini_Kvantor.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mini_Kvantor
{
    public partial class Form1 : Form
    {   // путь открытия директории для поиска глобальной папки
        const string mainPathTTS_1 = @"D:\";          // "\\192.168.0.117";   //
        const string mainPathTTS_2 = @"D:\";            // "\\192.168.0.117";
        const string mainPathTTS_3 = @"D:\";          // "d:\\Test_Anna";
        const string mainPathTTS_4 = "192.168.0.84"; //  http://192.168.0.84/ 
        // путь к папке откуда переписываем данные
        const string mainDataPathTTS_1 = @"D:\";  //  @"\\192.168.0.117\moto"
        const string mainDataPathTTS_2 = "D:/Test_Anna/Main"; // @"\\192.168.0.117\moto";
        const string mainDataPathTTS_3 = "d:/Test_Anna/Main";
        const string mainDataPathTTS_4 = " http://192.168.0.84/index.php?action=view&filename=GMUA__59.036&type=gps&year=2020";
        // путь для хранения оригинальных данных
        const string mainCopiedFileTTS_1 = @"D:\KvantWork\HomeCopyTTS-1\";
        const string mainCopiedFileTTS_2 = @"D:\KvantWork\HomeCopyTTS-2\";
        const string mainCopiedFileTTS_3 = @"D:\KvantWork\HomeCopyTTS-3\";
        const string mainCopiedFileTTS_4 = @"D:\KvantWork\HomeCopyTTS-4\";
        // полный путь и имя создаваемого недельного файла в папке //если в первой папке длинна имени !=9 знакам  меняем кол-во символов в Remove - значение зависит от длины пути
        const string pathNameGeneratedFileTTS_1 = @"D:\KvantWork\Work_TTS-1\Weeks\DATA";
        const string pathNameGeneratedFileTTS_2 = @"D:\KvantWork\Work_TTS-2\Weeks\DATA";
        const string pathNameGeneratedFileTTS_3 = @"D:\KvantWork\Work_TTS-3\Weeks\DATA";
        const string pathNameGeneratedFileTTS_4 = @"D:\KvantWork\Work_TTS-4\Weeks\DATA";
        // полный путь и имя создаваемого непрерывного файла
        const string pathAllWeeksFileTTS_1 = @"D:\KvantWork\Work_TTS-1\AllWeeks\AllWeeks_tts-1.txt";
        const string pathAllWeeksFileTTS_2 = @"D:\KvantWork\Work_TTS-2\AllWeeks\AllWeeks_tts-2.txt";
        const string pathAllWeeksFileTTS_3 = @"D:\KvantWork\Work_TTS-3\AllWeeks\AllWeeks_tts-3.txt";
        const string pathAllWeeksFileTTS_4 = @"D:\KvantWork\Work_TTS-4\AllWeeks\AllWeeks_tts-4.txt";
        // путь формирования суточных файлов в папке  @"D:\Test_Anna\Work_TTS-2\Days";
        const string pathCreatingDailyFilesTTS_1 = @"D:\KvantWork\Work_TTS-1\Days\";
        const string pathCreatingDailyFilesTTS_2 = @"D:\KvantWork\Work_TTS-2\Days\";
        const string pathCreatingDailyFilesTTS_3 = @"D:\KvantWork\Work_TTS-3\Days\";
        const string pathCreatingDailyFilesTTS_4 = @"D:\KvantWork\Work_TTS-4\Days\";
        // путь формирования Фильтрованных суточных файлов в папке D:\KvantWork\Work_TTS-1\FilteredDays
        const string pathCreatFilterDailyFilesTTS_1 = @"D:\KvantWork\Work_TTS-1\FilteredDays\";
        const string pathCreatFilterDailyFilesTTS_2 = @"D:\KvantWork\Work_TTS-2\FilteredDays\";
        const string pathCreatFilterDailyFilesTTS_3 = @"D:\KvantWork\Work_TTS-3\FilteredDays\";
        const string pathCreatFilterDailyFilesTTS_4 = @"D:\KvantWork\Work_TTS-4\FilteredDays\";
        // имена суточных файлов TTS
        const string nameFileTTS_1 = "GMUA07";
        const string nameFileTTS_2 = "GMUA04";
        const string nameFileTTS_3 = "GMUA06";
        const string nameFileTTS_4 = "RMUA_(GMUA05)";   // "GMUA05";
        // номер последнего загруженного суточного файла
        int valueLastDay;
        // номер выбранного дня для обработки файла
        int valueDay;
        List<int> numPRN = new List<int>()
        { 1,2,3,4,5,6,7,8,
          9,10,11,12,13,14,15,16,
         17,18,19,20,21,22,23,24,
         25,26,27,28,29,30,31,32
        };
     
        public static bool tts1click;
        public static bool tts2click;
        public static bool tts3click;
        public static bool tts4click;

        static public bool timerStart = false;

        DateTime date = DateTime.Now;
      
        public Form1()
        {
            InitializeComponent();
         
            lab_date_nowG.Text = date.ToShortDateString();

            var JD = date.ToOADate() + 2415018.5; // ПЕРЕВОД В ЮЛИАНСКУЮ ДАТУ

            int MJD =(int)(JD - 2400000.5);

            lab_date_nowMJD.Text = $"{MJD}"; // сегодня деннь MJD

            lab_date.Text = DateTime.Now.ToShortDateString();

        //            ПРОВЕРИТЬ !!!!
            //    //Task task1 = new Task(UploadWeeklyFilesHomeTTS_1);
            //    //task1.Start();
            //    //Thread.Sleep(1000 * 60);
            //    Task task2 = new Task(UploadWeeklyFilesHomeTTS_2);
            //    task2.Start();
            //    //Thread.Sleep(1000 * 60);
            //    //Task task3 = new Task(UploadWeeklyFilesHomeTTS_3);
            //    //task3.Start();
            //    //Thread.Sleep(1000 * 60);
            //    //Task task4 = new Task(UploadWeeklyFilesHomeTTS_4);
            //    //task4.Start();

        }
        //                   ТАЙМЕР    
        #region
      
        int timeH = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            int h = DateTime.Now.Hour + timeH;
            int m = DateTime.Now.Minute;
            int s = DateTime.Now.Second;

            string time = "";

            if (h < 10)
            {
                time += "0" + h;
            }
            else
            {
                time += h;
            }

            time += ":";

            if (m < 10)
            {
                time += "0" + m;
            }
            else
            {
                time += m;
            }

            time += ":";

            if (s < 10)
            {
                time += "0" + s;
            }
            else
            {
                time += s;
            }

            lab_time.Text = time;

        }
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Start();
     
        }
        // методы самостоятельно загружающие недельные файлы в папку  конкретного TTS
        // они же формируют суточые файлы в папке конкретного TTS   UploadWeeklyFilesHomeTTS_1()
        #region
        public void UploadWeeklyFilesHomeTTS_1()
        {
            var path = string.Empty;
            string newPath;
            var nameStr = string.Empty;
            try
            {   // скопировали данные из глобальной папки в папку для работы данного приложения
                Methods.CopyLastRecordedFile(mainDataPathTTS_1,mainCopiedFileTTS_1, out string fullPathRecordFile);
              
                newPath = $"{pathNameGeneratedFileTTS_1}{ fullPathRecordFile.Substring(fullPathRecordFile.Length - 4)}";
         
                //дозаписывает все недельные данные в непрерывный текстовый файл
                 //для методов самостоятельной загрузки данных
                Methods.CutTitleWriteAllWeeks(fullPathRecordFile, pathAllWeeksFileTTS_1, newPath);
                // начинаем работу с недельным файлом в каталоге Weeks 

                Methods.ConvertToNumber(newPath, out List<DataName> values, out List<string> noTitle,
                    out List<string> title, out int firstDayNumber);

                // создаем суточные файлы + создаем фильтрованные суточнае файлы
                Methods.CreateDayFile(fullPathRecordFile, pathCreatingDailyFilesTTS_1, pathCreatFilterDailyFilesTTS_1,
                    nameFileTTS_1, title, firstDayNumber, values, noTitle);

            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.ToString()}");
            }

            Thread.Sleep(1000 * 86400 * 7); // через 1 сутки считываем файл

        } 
    
        public void UploadWeeklyFilesHomeTTS_2()
        {
            var path = string.Empty;
            string newPath;
            var nameStr = string.Empty;
            try
            {
                // скопировали данные из глобальной папки в папку для работы данного приложения
                Methods.CopyLastRecordedFile(mainDataPathTTS_2, mainCopiedFileTTS_2, out string fullPathRecordFile);

                newPath = $"{pathNameGeneratedFileTTS_2}{ fullPathRecordFile.Substring(fullPathRecordFile.Length - 4)}";

                //дозаписывает все недельные данные в непрерывный текстовый файл
                //для методов самостоятельной загрузки данных
                Methods.CutTitleWriteAllWeeks(fullPathRecordFile, pathAllWeeksFileTTS_2, newPath);
                // начинаем работу с недельным файлом в каталоге Weeks 

                Methods.ConvertToNumber(newPath, out List<DataName> values, out List<string> noTitle,
                    out List<string> title, out int firstDayNumber);

                // создаем суточные файлы + сглаженные суточные файлы с потерей 21 строки !!!
                Methods.CreateDayFile(fullPathRecordFile, pathCreatingDailyFilesTTS_2, pathCreatFilterDailyFilesTTS_2,
                    nameFileTTS_2, title, firstDayNumber, values, noTitle);
              

            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.ToString()}" );
            }

            Thread.Sleep(1000 * 86400 * 7); // через 1 сутки считываем файл
         
        }

        public void UploadWeeklyFilesHomeTTS_3()
        {
            var path = string.Empty;
            string newPath;
            var nameStr = string.Empty;
            try
            {
                // скопировали данные из глобальной папки в папку для работы данного приложения
                Methods.CopyLastRecordedFile(mainDataPathTTS_3, mainCopiedFileTTS_3, out string fullPathRecordFile);

                newPath = $"{pathNameGeneratedFileTTS_3}{ fullPathRecordFile.Substring(fullPathRecordFile.Length - 4)}";

                //дозаписывает все недельные данные в непрерывный текстовый файл
                //для методов самостоятельной загрузки данных
                Methods.CutTitleWriteAllWeeks(fullPathRecordFile, pathAllWeeksFileTTS_3, newPath);
                // начинаем работу с недельным файлом в каталоге Weeks 

                Methods.ConvertToNumber(newPath, out List<DataName> values, out List<string> noTitle,
                    out List<string> title, out int firstDayNumber);

                // создаем суточные файлы + создаем фильтрованные суточнае файлы
                Methods.CreateDayFile(fullPathRecordFile, pathCreatingDailyFilesTTS_3, pathCreatFilterDailyFilesTTS_3,
                    nameFileTTS_3, title, firstDayNumber, values, noTitle);

            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.ToString()}");
            }

            Thread.Sleep(1000 * 86400 * 7); // через 1 сутки считываем файл
          
        }

        public void UploadWeeklyFilesHomeTTS_4()
        {
            var path = string.Empty;
            string newPath;
            var nameStr = string.Empty;
            try
            {   // скопировали данные из глобальной папки в папку для работы данного приложения
                Methods.CopyLastRecordedFile(mainDataPathTTS_4, mainCopiedFileTTS_4, out string fullPathRecordFile);

                newPath = $"{pathNameGeneratedFileTTS_4}{ fullPathRecordFile.Substring(fullPathRecordFile.Length - 4)}";

                //дозаписывает все недельные данные в непрерывный текстовый файл
                //для методов самостоятельной загрузки данных
                Methods.CutTitleWriteAllWeeks(fullPathRecordFile, pathAllWeeksFileTTS_4, newPath);
                // начинаем работу с недельным файлом в каталоге Weeks 

                Methods.RinexConvertToNumber(newPath, out List<RinexData> values, out List<string> noTitle,
                    out List<string> title, out int firstDayNumber);

                // создаем суточные файлы + создаем фильтрованные суточнае файлы
                Methods.RinexCreateDayFile(fullPathRecordFile, pathCreatingDailyFilesTTS_1, pathCreatFilterDailyFilesTTS_1,
                    nameFileTTS_1, title, firstDayNumber, values, noTitle);

            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ошибка: {ex.ToString()}");
            }

            Thread.Sleep(1000 * 86400 * 7); // через 1 сутки считываем файл
         
        }

        #endregion
     
        // найти суточный  файл для для нахождения средних показателей  DailyDataProcessingTTS_1()
        #region
        public void DailyDataProcessingTTS_1(int numDay,out double utc,out double t,out double rms,out int countTracks)
        {
            utc = 0;

            t = 0;

            rms = 0; // Среднеквадрати́ческое отклоне́ние  

            countTracks = 0;

            double  utcLast = 0;

            int numday = numDay;

            string strNumDay = numday.ToString();

            string day = strNumDay.Remove(0,2);

            string path;

            int lastDay = numday - 1;

            string strLastDay = lastDay.ToString();

            string dayLast = strLastDay.Remove(0,2);

            string path2;
            // список считанных строк из суточного файла
            List<string> valStrDay = new List<string>() { };
            // список конвертированных данных 
            List<DataName> valuesDay = new List<DataName>() { };
            // списоки данных предидущего дня
            List<string> yesterdayStr = new List<string>() { };

            List<DataName> yesterday = new List<DataName>() { };

            try
            {
                string[] dirs = Directory.GetFiles($@"{ pathCreatingDailyFilesTTS_1}");

                foreach (var item in dirs)
                {
                    if (item.Substring(item.Length - 3) == day)
                    {
                        path = item; // путь к суточному файлу
                        Console.WriteLine($"Requested Day:{path}\n");
                        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                valStrDay.Add(line);

                            }
                        }
                    }
                    if (item.Substring(item.Length - 3) == dayLast)
                    {
                        path2 = item;
                        Console.WriteLine($"Last Day:{path2}\n");
                        using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                yesterdayStr.Add(line);

                            }
                        }
                    }
                }
       
                for (int i = 0; i < valStrDay.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = valStrDay[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();
                  
                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        valuesDay.Add(val);
                    }

                }

                countTracks = valuesDay.Count;

                double sumREFGPS = 0;

                double middleREFGPS = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Сумма всех REFGPS
                    sumREFGPS += valuesDay[i].REFGPS; 

                }
                Console.WriteLine($"Сумма всех REFGPS:{sumREFGPS}\n");
                // среднее от суммы REFGPS 
                middleREFGPS = sumREFGPS / valuesDay.Count -1;

                double dispersion = 0; //дисперсия

                double k = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    k += Math.Pow((valuesDay[i].REFGPS - middleREFGPS), 2);

                    k++;

                }

                dispersion = k / valuesDay.Count - 1;

                rms = Math.Round(Math.Sqrt(dispersion), 2);

                utc = middleREFGPS * Math.Pow(10, -1);
                // данные за вчера
                for (int i = 0; i < yesterdayStr.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = yesterdayStr[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();
                     
                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        yesterday.Add(val);
                    }
                }
                double lastSumREFGPS = 0;

                double lastMiddleREFGPS = 0;

                double lastRms = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    lastSumREFGPS += yesterday[i].REFGPS;
                }
                // среднее от суммы REFGPS 
                lastMiddleREFGPS = lastSumREFGPS / yesterday.Count;

                double dispersionLast = 0; //дисперсия

                double kLast = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    kLast += Math.Pow((yesterday[i].REFGPS - lastMiddleREFGPS), 2);

                    kLast++;

                }

                dispersionLast = kLast / yesterday.Count - 1;

                lastRms = Math.Round(Math.Sqrt(dispersionLast), 2);

                utcLast = lastMiddleREFGPS * Math.Pow(10, -1);

                t = utc - utcLast;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public void DailyDataProcessingTTS_2(int numDay, out double utc, out double t, out double rms, out int countTracks)
        {
            utc = 0;

            t = 0;

            rms = 0; // Среднеквадрати́ческое отклоне́ние

            countTracks = 0;

            double utcLast = 0;

            int numday = numDay;

            string strNumDay = numday.ToString();

            string day = strNumDay.Remove(0, 2);

            string path;

            int lastDay = numday - 1;

            string strLastDay = lastDay.ToString();

            string dayLast = strLastDay.Remove(0, 2);

            string path2;
            // список считанных строк из суточного файла
            List<string> valStrDay = new List<string>() { };
            // список конвертированных данных 
            List<DataName> valuesDay = new List<DataName>() { };
            // списоки данных предидущего дня
            List<string> yesterdayStr = new List<string>() { };

            List<DataName> yesterday = new List<DataName>() { };

            try
            {
                string[] dirs = Directory.GetFiles($@"{ pathCreatingDailyFilesTTS_2}");

                foreach (var item in dirs)
                {
                    if (item.Substring(item.Length - 3) == day)
                    {
                        path = item; // путь к суточному файлу
                       
                        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                valStrDay.Add(line);

                            }
                        }
                    }
                    if (item.Substring(item.Length - 3) == dayLast)
                    {
                        path2 = item;
                        Console.WriteLine($"Last Day:{path2}\n");
                        using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                yesterdayStr.Add(line);

                            }
                        }
                    }
                }

                for (int i = 0; i < valStrDay.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = valStrDay[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();

                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        valuesDay.Add(val);
                    }

                }

                countTracks = valuesDay.Count;

                double sumREFGPS = 0;

                double middleREFGPS = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Сумма всех REFGPS
                    sumREFGPS += valuesDay[i].REFGPS;

                }
                // среднее от суммы REFGPS 
                middleREFGPS = sumREFGPS / valuesDay.Count;

                double dispersion = 0; //дисперсия

                double k = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    k += Math.Pow((valuesDay[i].REFGPS - middleREFGPS), 2);

                    k++;
                }

                dispersion = k / valuesDay.Count - 1;

                rms = Math.Round(Math.Sqrt(dispersion), 2);

                utc = middleREFGPS * Math.Pow(10, -1);
                // данные за вчера
                for (int i = 0; i < yesterdayStr.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = yesterdayStr[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();

                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        yesterday.Add(val);
                    }
                }
                double lastSumREFGPS = 0;

                double lastMiddleREFGPS = 0;

                double lastRms = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    lastSumREFGPS += yesterday[i].REFGPS;
                }
                // среднее от суммы REFGPS 
                lastMiddleREFGPS = lastSumREFGPS / yesterday.Count;

                double dispersionLast = 0; //дисперсия

                double kLast = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    kLast += Math.Pow((yesterday[i].REFGPS - lastMiddleREFGPS), 2);

                    kLast++;

                }

                dispersionLast = kLast / yesterday.Count - 1;

                lastRms = Math.Round(Math.Sqrt(dispersionLast), 2);

                utcLast = lastMiddleREFGPS * Math.Pow(10, -1);

                t = utc - utcLast;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public void DailyDataProcessingTTS_3(int numDay, out double utc, out double t, out double rms, out int countTracks)
        {
            utc = 0;

            t = 0;

            rms = 0; // Среднеквадрати́ческое отклоне́ние

            countTracks = 0;

            double utcLast = 0;

            int numday = numDay;

            string strNumDay = numday.ToString();

            string day = strNumDay.Remove(0, 2);

            string path;

            int lastDay = numday - 1;

            string strLastDay = lastDay.ToString();

            string dayLast = strLastDay.Remove(0, 2);

            string path2;
            // список считанных строк из суточного файла
            List<string> valStrDay = new List<string>() { };
            // список конвертированных данных 
            List<DataName> valuesDay = new List<DataName>() { };
            // списоки данных предидущего дня
            List<string> yesterdayStr = new List<string>() { };

            List<DataName> yesterday = new List<DataName>() { };

            try
            {
                string[] dirs = Directory.GetFiles($@"{ pathCreatingDailyFilesTTS_3}");

                foreach (var item in dirs)
                {
                    if (item.Substring(item.Length - 3) == day)
                    {
                        path = item; // путь к суточному файлу
                        Console.WriteLine($"Requested Day:{path}\n");
                        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                valStrDay.Add(line);

                            }
                        }
                    }
                    if (item.Substring(item.Length - 3) == dayLast)
                    {
                        path2 = item;
                        Console.WriteLine($"Last Day:{path2}\n");
                        using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                yesterdayStr.Add(line);

                            }
                        }
                    }
                }

                for (int i = 0; i < valStrDay.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = valStrDay[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();

                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        valuesDay.Add(val);
                    }

                }
                countTracks = valuesDay.Count;

                double sumREFGPS = 0;

                double middleREFGPS = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Сумма всех REFGPS
                    sumREFGPS += valuesDay[i].REFGPS;

                }
                // среднее от суммы REFGPS 
                middleREFGPS = sumREFGPS / valuesDay.Count;

                double dispersion = 0; //дисперсия

                double k = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    k += Math.Pow((valuesDay[i].REFGPS - middleREFGPS), 2);

                    k++;
                }

                dispersion = k / valuesDay.Count - 1;

                rms = Math.Round(Math.Sqrt(dispersion), 2);

                utc = middleREFGPS * Math.Pow(10, -1);
                // данные за вчера
                for (int i = 0; i < yesterdayStr.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = yesterdayStr[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();

                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";
                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        yesterday.Add(val);
                    }
                }
                double lastSumREFGPS = 0;

                double lastMiddleREFGPS = 0;

                double lastRms = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    lastSumREFGPS += yesterday[i].REFGPS;
                }
                // среднее от суммы REFGPS 
                lastMiddleREFGPS = lastSumREFGPS / yesterday.Count;

                double dispersionLast = 0; //дисперсия

                double kLast = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    kLast += Math.Pow((yesterday[i].REFGPS - lastMiddleREFGPS), 2);

                    kLast++;
                
                }

                dispersionLast = kLast / yesterday.Count - 1;

                lastRms = Math.Round(Math.Sqrt(dispersionLast), 2);

                utcLast = lastMiddleREFGPS * Math.Pow(10, -1);

                t = utc - utcLast;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public void DailyDataProcessingTTS_4(int numDay, out double utc, out double t, out double rms, out int countTracks)
        {
            utc = 0;

            t = 0;

            rms = 0; // Среднеквадрати́ческое отклоне́ние

            countTracks = 0;

            double utcLast = 0;

            int numday = numDay;

            string strNumDay = numday.ToString();

            string day = strNumDay.Remove(0, 2);

            string path;

            int lastDay = numday - 1;

            string strLastDay = lastDay.ToString();

            string dayLast = strLastDay.Remove(0, 2);

            string path2;
            // список считанных строк из суточного файла
            List<string> valStrDay = new List<string>() { };
            // список конвертированных данных 
            List<RinexData> valuesDay = new List<RinexData>() { };
            // списоки данных предидущего дня
            List<string> yesterdayStr = new List<string>() { };

            List<RinexData> yesterday = new List<RinexData>() { };

            try
            {
                string[] dirs = Directory.GetFiles($@"{ pathCreatingDailyFilesTTS_4}");

                foreach (var item in dirs)
                {
                    if (item.Substring(item.Length - 3) == day)
                    {
                        path = item; // путь к суточному файлу
                        Console.WriteLine($"Requested Day:{path}\n");
                        using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                valStrDay.Add(line);

                            }
                        }
                    }
                    if (item.Substring(item.Length - 3) == dayLast)
                    {
                        path2 = item;
                        Console.WriteLine($"Last Day:{path2}\n");
                        using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                yesterdayStr.Add(line);

                            }
                        }
                    }
                }

                for (int i = 0; i < valStrDay.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = valStrDay[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        RinexData val = new RinexData();

                        number = Convert.ToInt32(TextArray[0], 10);
                        val.SAT = number;

                        string h = TextArray[3].Substring(0, 2);
                        string m = TextArray[3].Substring(2, 2);
                        string s = TextArray[3].Substring(4, 2);

                        if (int.Parse($"{h}") >= 24)
                        {
                            h = "23";
                        }
                        if (int.Parse($"{m}") >= 60)
                        {
                            m = "59";
                        }
                        if (int.Parse($"{s}") >= 60)
                        {
                            s = "59";
                        }

                        string strTime = $"{h}:{m}:{s}";

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFSYS = number;

                        valuesDay.Add(val);
                    }

                }
                countTracks = valuesDay.Count;

                double sumREFGPS = 0;

                double middleREFGPS = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Сумма всех REFGPS
                    sumREFGPS += valuesDay[i].REFSYS;

                }
                // среднее от суммы REFGPS 
                middleREFGPS = sumREFGPS / valuesDay.Count;

                double dispersion = 0; //дисперсия

                double k = 0;

                for (int i = 0; i < valuesDay.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    k += Math.Pow((valuesDay[i].REFSYS - middleREFGPS), 2);

                    k++;
                }

                dispersion = k / valuesDay.Count - 1;

                rms = Math.Round(Math.Sqrt(dispersion), 2);

                utc = middleREFGPS * Math.Pow(10, -1);
                // данные за вчера
                for (int i = 0; i < yesterdayStr.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = yesterdayStr[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        RinexData val = new RinexData();

                        number = Convert.ToInt32(TextArray[0], 10);
                        val.SAT = number;

                        string str = ":";
                        string str1 = TextArray[3].Insert(2, str);
                        string strTime = str1.Insert(5, str);

                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFSYS = number;

                        yesterday.Add(val);
                    }
                }
                double lastSumREFGPS = 0;

                double lastMiddleREFGPS = 0;

                double lastRms = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    lastSumREFGPS += yesterday[i].REFSYS;
                }
                // среднее от суммы REFGPS 
                lastMiddleREFGPS = lastSumREFGPS / yesterday.Count - 1;

                double dispersionLast = 0; //дисперсия

                double kLast = 0;

                for (int i = 0; i < yesterday.Count; i++)
                {
                    // Вычислим квадраты отклонений 
                    kLast += Math.Pow((yesterday[i].REFSYS - lastMiddleREFGPS), 2);

                    kLast++;
                }

                dispersionLast = kLast / yesterday.Count - 1;

                lastRms = Math.Round(Math.Sqrt(dispersionLast), 2);

                utcLast = lastMiddleREFGPS * Math.Pow(10, -1);

                t = utc - utcLast;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"DailyDataProcessingTTS_4{ex.ToString()}");
            }

        }
        #endregion

        //  кнопки TTS
        #region
        private void btnTTS1_Click(object sender, EventArgs e)
        {
            if (true)
            {
                labNameFileTTS1.ForeColor = Color.Maroon;

                tts1click = true;
                tts2click = false;
                tts3click = false;
                tts4click = false;

            }
            // метод самостоятельной загрузки
            //UploadWeeklyFilesHomeTTS_1();
            MessageBox.Show("Данный TTS  ОТСУТСТВУЕТ");
            //// выбираем файл и записываем исправленный недельный файл
            //Methods.FileDialogOpen(pathNameGeneratedFileTTS_1,out string fileName,
            //    out string labNameFileTTS);

            //labNameFileTTS1.Text = labNameFileTTS;
            //// формируем суточные файлы
            //Methods.UploadWeekFileByButton(fileName, pathCreatingDailyFilesTTS_1,
            //    pathCreatFilterDailyFilesTTS_1, nameFileTTS_1, out int nameLastDay);

            //valueLastDay = nameLastDay; // номер последнего суточного файла MJD
            ////   номер последнего суточного файла переводим в григорианскую дату
            //var JD = valueLastDay + 2400000.5; // получаем юлианский день

            //var dayG = Methods.FromJulian(JD); // получили григорианскую дату файла

            //textBox_Gday.Text = dayG.ToShortDateString();

            //Console.WriteLine($"Григорианский день файла{ dayG.ToShortDateString()}");
            //// конвертируем число в строку
            //string str = valueLastDay.ToString();

            //textBox_MJDday.Text = str;

        }

        private void btnTTS2_Click(object sender, EventArgs e)
        {
            if (true)
            {
                labNameFileTTS2.ForeColor = Color.Maroon;
                tts1click = false;
                tts2click = true;
                tts3click = false;
                tts4click = false;
            }
            // метод самостоятельной загрузки
            //UploadWeeklyFilesHomeTTS_2();
            // выбираем файл и записываем исправленный недельный файл
            Methods.FileDialogOpen(pathNameGeneratedFileTTS_2, out string fileName,
                out string labNameFileTTS);

            labNameFileTTS2.Text = labNameFileTTS;
            // формируем суточные файлы
            Methods.UploadWeekFileByButton(fileName, pathCreatingDailyFilesTTS_2,
                pathCreatFilterDailyFilesTTS_2, nameFileTTS_2, out int nameLastDay);

            valueLastDay = nameLastDay; // номер последнего суточного файла MJD
            //   номер последнего суточного файла переводим в григорианскую дату
            var JD = valueLastDay + 2400000.5; // получаем юлианский день

            var dayG = Methods.FromJulian(JD); // получили григорианскую дату файла

            textBox_Gday.Text = dayG.ToShortDateString();

            Console.WriteLine($"Григорианский день файла{ dayG.ToShortDateString()}");
            // конвертируем число в строку
            string str = valueLastDay.ToString();

            textBox_MJDday.Text = str;

        }

        private void btnTTS3_Click(object sender, EventArgs e)
        {
            if (true)
            {
                labNameFileTTS3.ForeColor = Color.Maroon;
                tts1click = false;
                tts2click = false;
                tts3click = true;
                tts4click = false;
            }
             MessageBox.Show("Данный TTS  ОТСУТСТВУЕТ");

            //// выбираем файл и записываем исправленный недельный файл
            //Methods.FileDialogOpen(pathNameGeneratedFileTTS_3, out string fileName,
            //    out string labNameFileTTS);

            //labNameFileTTS3.Text = labNameFileTTS;
            //// формируем суточные файлы
            //Methods.UploadWeekFileByButton(fileName, pathCreatingDailyFilesTTS_3,
            //    pathCreatFilterDailyFilesTTS_3, nameFileTTS_3, out int nameLastDay);

            //valueLastDay = nameLastDay; // номер последнего суточного файла MJD
            ////   номер последнего суточного файла переводим в григорианскую дату
            //var JD = valueLastDay + 2400000.5; // получаем юлианский день

            //var dayG = Methods.FromJulian(JD); // получили григорианскую дату файла

            //textBox_Gday.Text = dayG.ToShortDateString();

            //Console.WriteLine($"Григорианский день файла{ dayG.ToShortDateString()}");
            //// конвертируем число в строку
            //string str = valueLastDay.ToString();

            //textBox_MJDday.Text = str;
        }

        private void btnTTS4_Click(object sender, EventArgs e)
        {
            if (true)
            {
                labNameFileTTS4.ForeColor = Color.Maroon;
                tts1click = false;
                tts2click = false;
                tts3click = false;
                tts4click = true;
            }

            //MessageBox.Show("Данный TTS  ОТСУТСТВУЕТ");
           
            // выбираем файл и записываем исправленный недельный файл
            Methods.FileDialogOpen(pathNameGeneratedFileTTS_4, out string fileName,
                out string labNameFileTTS);

            labNameFileTTS4.Text = labNameFileTTS;
            // формируем суточные файлы
            Methods.RinexUploadWeekFileByButton(fileName, pathCreatingDailyFilesTTS_4,
                pathCreatFilterDailyFilesTTS_4, nameFileTTS_4, out int nameLastDay);

            valueLastDay = nameLastDay; // номер последнего суточного файла MJD
            //   номер последнего суточного файла переводим в григорианскую дату
            var JD = valueLastDay + 2400000.5; // получаем юлианский день

            var dayG = Methods.FromJulian(JD); // получили григорианскую дату файла

            textBox_Gday.Text = dayG.ToShortDateString();

            Console.WriteLine($"Григорианский день файла{ dayG.ToShortDateString()}");
            // конвертируем число в строку
            string str = valueLastDay.ToString();

            textBox_MJDday.Text = str;

        }
        #endregion

        // /кнопка обработки суточных данных по выбранному TTS +  textBox_numPRN,textBox_tts

        #region
        public int dataPRN; //введенный номер PRN

        private void textBox_numPRN_TextChanged(object sender, EventArgs e)
        {
            int number = 0;

            if (Int32.TryParse(textBox_numPRN.Text, out number))
            {
                if (number > 0 && number < 33)
                {
                    dataPRN = number;  // присвоить введенное значение переменной
                }
                else
                {
                    MessageBox.Show("Введите число 1 - 32 ");

                    textBox_numPRN.Clear(); // очистить текст бокс
                }
            }
        }
        public int numTTS; 

        private void textBox_numPRN_KeyPress(object sender, KeyPressEventArgs e)
        {
            // блокируем ввод символов кроме цыфр и клавиши удаления Backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox_tts_TextChanged(object sender, EventArgs e)
        {
            int number = 0;

            if (Int32.TryParse(textBox_tts.Text, out number))
            {
                if (number > 0 && number < 5)
                {
                    numTTS = number; 
                }
                else
                {
                    MessageBox.Show("Введите число от 1 - 4 ");

                    textBox_tts.Clear();
                }
            }
        }
        private void textBox_tts_KeyPress(object sender, KeyPressEventArgs e)
        {
            // блокируем ввод символов кроме цыфр и клавиши удаления Backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        //кнопка обработки суточных данных по вфыбранному TTS
        private void btn_getDayFile_Click(object sender, EventArgs e)
        {
            // номер выбранного дня для обработки файла
            valueDay =  Convert.ToInt32(textBox_MJDday.Text);

            if (checkBox_tts1.Checked)
            {
                // передаем методу отображаемую дату MJD
                DailyDataProcessingTTS_1(valueDay, out double utc1,  out double t1, out double rms1, out int countTracks1);

                lab_rms_tts1.Text = rms1.ToString();

                lab_tr_tts1.Text = countTracks1.ToString();

                lab_utc_tts1.Text = $"({Math.Round(utc1,2)}).({Math.Round(t1,2)})";

                Console.WriteLine($"Выходные параметры: utc1 =  {utc1}\n" +
                    $"t1 =  {t1}");
            }
          
            if (checkBox_tts2.Checked)
            {
                DailyDataProcessingTTS_2(valueDay, out double utc2,out double t2,out double rms2,out int countTracks2);

                lab_rms_tts2.Text = rms2.ToString();

                lab_tr_tts2.Text = countTracks2.ToString();

                lab_utc_tts2.Text = $"({Math.Round(utc2, 2)}).({Math.Round(t2, 2)})";

                Console.WriteLine($"Выходные параметры: utc1 =  {utc2}\n" +
                                 $"t1 =  {t2}");
            }

            if (checkBox_tts3.Checked)
            {
                DailyDataProcessingTTS_3(valueDay, out double utc3, out double t3, out double rms3, out int countTracks3);

                lab_rms_tts3.Text = rms3.ToString();

                lab_tr_tts3.Text = countTracks3.ToString();

                lab_utc_tts3.Text = $"({Math.Round(utc3, 2)}).({Math.Round(t3, 2)})";

                Console.WriteLine($"Выходные параметры: utc1 =  {utc3}\n" +
                               $"t1 =  {t3}");
            }

            if (checkBox_tts4.Checked)
            {
                DailyDataProcessingTTS_4(valueDay, out double utc4, out double t4, out double rms4, out int countTracks4);

                lab_rms_tts4.Text = rms4.ToString();

                lab_tr_tts4.Text = countTracks4.ToString();

                lab_utc_tts4.Text = $"({Math.Round(utc4, 2)}).({Math.Round(t4, 2)})";

                Console.WriteLine($"Выходные параметры: utc1 =  {utc4}\n" +
                             $"t1 =  {t4}");

            }

        }

#endregion
        // чекбоксы
        #region 
        private void checkBox_tts1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_tts1.Checked)
            {
                lab_utc_tts1.Text = "";

                lab_rms_tts1.Text = "";

                lab_tr_tts1.Text = "";
            }
            
        }

        private void checkBox_tts2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_tts2.Checked)
            {
                lab_utc_tts2.Text = "";

                lab_rms_tts2.Text = "";

                lab_tr_tts2.Text = "";
            }
        }

        private void checkBox_tts3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_tts3.Checked)
            {
                lab_utc_tts3.Text = "";

                lab_rms_tts3.Text = "";

                lab_tr_tts3.Text = "";
            }
        }

        private void checkBox_tts4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_tts4.Checked)
            {
                lab_utc_tts4.Text = "";

                lab_rms_tts4.Text = "";

                lab_tr_tts4.Text = "";
            }
        }
        #endregion

        //   кнопка отображения графика ,обработка ввода PRN и номера  TTS
        #region
        private void textBox_Gday_TextChanged(object sender, EventArgs e)
        {
            DateTime valDayG = Convert.ToDateTime(textBox_Gday.Text);

            var JD = valDayG.ToOADate() + 2415018.5;

            int MJD = (int)(JD - 2400000.5);

            textBox_MJDday.Text = $"{MJD}";    
             
        }

        private void textBox_MJDday_TextChanged(object sender, EventArgs e)
        {
            double JD = Convert.ToInt32(textBox_MJDday.Text) + 2400000.5;

            DateTime dayG = Methods.FromJulian(JD);

            textBox_Gday.Text = dayG.ToShortDateString();
                             
        }

        private void textBox_Gday_KeyPress(object sender, KeyPressEventArgs e)
        {
            // блокируем ввод символов кроме цыфр и клавиши удаления Backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox_MJDday_KeyPress(object sender, KeyPressEventArgs e)
        {
            // блокируем ввод символов кроме цыфр и клавиши удаления Backspace
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        string namePath;
        string nameTTS;

        private void btn_showGraph_Click(object sender, EventArgs e)
        {
            Methods.RinexCanalDataSatellite(dataPRN, out List<List<RinexData>> listCanal,
                out DateTime startDate, out DateTime endDate);
            switch (numTTS)
            {
                case 1:

                    namePath = pathAllWeeksFileTTS_1;
                    nameTTS = "1";
                    break;

                case 2:

                    namePath = pathAllWeeksFileTTS_2;
                    nameTTS = "2";
                    break;

                case 3:

                    namePath = pathAllWeeksFileTTS_3;
                    nameTTS = "3";
                    break;
                case 4:

                    namePath = pathAllWeeksFileTTS_4;
                    nameTTS = "4";
                    Schedule_REFSYS form = new Schedule_REFSYS(nameTTS, dataPRN,listCanal,
                         startDate,endDate);
                    form.Show();
                    break;

                default:
                    break;
            }

            //       dataPRN  введенный номер PRN

           Methods.ReadAllWeeksFile(namePath, out List<DataName> values);

            List<DateTime> dateTime = new List<DateTime>();

            List<int> dataREFGPS = new List<int>() { };
     
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].PRN == dataPRN)
                {
                    double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                    DateTime date =Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                    dateTime.Add(date);

                    dataREFGPS.Add(values[i].REFGPS);
                }

            }

            if (nameTTS != "4")
            {
                Schedule_REFGPS newForm = new Schedule_REFGPS(nameTTS, dataPRN, dataREFGPS, dateTime);
                newForm.Show();
            }
          
        }

        private void btn_collectData_Click(object sender, EventArgs e)
        {
            btn_other.Visible = true;
        }
     
        private void btn_other_Click(object sender, EventArgs e)
        {
            switch (numTTS)
            {
                case 1:

                    namePath = pathAllWeeksFileTTS_1;
                    nameTTS = "1";
                    break;

                case 2:

                    namePath = pathAllWeeksFileTTS_2;
                    nameTTS = "2";
                    break;

                case 3:

                    namePath = pathAllWeeksFileTTS_3;
                    nameTTS = "3";
                    break;
                case 4:

                    namePath = pathAllWeeksFileTTS_4;
                    nameTTS = "4";
                    break;
                default:
                    break;
            }
            //  dataPRN введенный номер PRN, values список данных за все время
           Methods.ReadAllWeeksFile(namePath, out List<DataName> values);
            
            UI.OtherParametersForm form = new UI.OtherParametersForm(nameTTS, dataPRN,values);
            form.Show();
        }



        #endregion

    
    }

}
 