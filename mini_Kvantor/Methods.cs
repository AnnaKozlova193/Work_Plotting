using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mini_Kvantor
{
    public class Methods
    {
        // получаем среднее значение из трех выбранных показателей
        public static void AverageOfThree(List<DataName> threeVal, List<string> threeStr,
            out DataName val, out string str)
        {
            val = new DataName();
            str = "";
            DataName temp;
            string tempStr;

            for (int k = 0; k < threeVal.Count - 1; k++)
            {
                for (int m = 0; m < threeStr.Count - 1; m++)
                {
                    if (m == k)
                    {
                        if (threeVal[0].REFGPS <= threeVal[1].REFGPS
                                     && threeVal[1].REFGPS <= threeVal[2].REFGPS)
                        {
                            val = threeVal[1];
                            str = threeStr[1];
                        }
                        else
                        {
                            for (int l = k + 1; l < threeVal.Count; l++)
                            {
                                for (int n = m + 1; n < threeStr.Count; n++)
                                {
                                    if (l == n)
                                    {
                                        if (threeVal[k].REFGPS > threeVal[l].REFGPS)
                                        {
                                            temp = threeVal[k];
                                            tempStr = threeStr[m];

                                            threeVal[k] = threeVal[l];
                                            threeStr[m] = threeStr[n];

                                            threeVal[l] = temp;
                                            threeStr[n] = tempStr;

                                            val = threeVal[1];
                                            str = threeStr[1];

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // сглаживание данных - "Медианы по три"
        public static void MovingMedianOverTriplets(List<DataName> values,
            List<string> noTitle, out List<string> smoothedValuesStr) 
        {
            Filter(values, noTitle, out List<DataName> filterValues, out List<string> filterStr);
            // для хранения сглаженных показателей 
            List<DataName> smoothedValues = new List<DataName>() { };
            // для записи в текстовый документ
            smoothedValuesStr = new List<string>() { };
            // хранение 3 выбранных елементов 
            List<DataName> threeVal = new List<DataName>() { };
            List<string> threeStr = new List<string>() { };

            for (int i = 0; i < filterValues.Count; i++)
            {
                for (int j = 0; j < filterStr.Count; j++)
                {
                    if (i == j && i <= filterValues.Count - 3)
                    {
                        threeVal = filterValues.GetRange(i, 3).ToList();
                        threeStr = filterStr.GetRange(j, 3).ToList();

                        AverageOfThree(threeVal, threeStr, out DataName val, out string str);

                        smoothedValues.Add(val);
                        smoothedValuesStr.Add(str);

                    }
                }
            }
        }
        // Среднеквадратичное отклонение далее СКО
        public static void RMSDeviation(List<DataName> val, out int rms)
        {
            int u = val.Sum(i => i.REFGPS) / 3;// среднее от суммы

            rms = (int)Math.Sqrt((Math.Pow(val[0].REFGPS - u, 2) +
                  Math.Pow(val[1].REFGPS - u, 2) + Math.Pow(val[2].REFGPS - u, 2)) / 2);

        }
        // фильтруем данные используя СКО 
        public static void FilterRMS(List<DataName> filterValues, List<string> filterStr,
            out List<string> RmsFilterStr)
        {
            List<DataName> RmsFilterVal = new List<DataName>() { };// выбранные значения в диапозоне скользящего СКО

            RmsFilterStr = new List<string>() { };// текстовый список нужных значений

            List<DataName> myNums = new List<DataName>() { }; // для СКО

            int RMS = 0;
            int origNum = filterValues[2].REFGPS;
            int firstNumTest = 3;
            int firstStrTest = firstNumTest;

            myNums = filterValues.GetRange(0, 3).ToList();

            RmsFilterVal = filterValues.GetRange(0, 3).ToList();

            RmsFilterStr = filterStr.GetRange(0, 3).ToList();

            while (true)
            {
                bool finish = true;

                RMSDeviation(myNums, out RMS);

                for (int j = firstStrTest; j < filterStr.Count; j++)
                {
                    for (int i = firstNumTest; i < filterValues.Count; i++)
                    {
                        if (j == i)
                        {   //    ловим числа в пределах подходящего допуска
                            if (origNum - RMS <= filterValues[i].REFGPS && filterValues[i].REFGPS <= origNum + RMS)
                            {
                                myNums.Add(filterValues[i]);

                                RmsFilterVal.Add(filterValues[i]);

                                RmsFilterStr.Add(filterStr[j]);

                                myNums.RemoveAt(0);

                                origNum = filterValues[i].REFGPS;

                                firstNumTest = i + 1;

                                firstStrTest = j + 1;

                                finish = false;

                                break;
                            }
                        }
                    }
                }
                if (finish)
                {
                    break;
                }
            }
        }
        //фильтр вырезает строки где У REFGPS значетие более 5 цыфр
        public static void Filter(List<DataName> values, List<string> noTitle,
            out List<DataName> filterValues, out List<string> filterStr)
        {
            filterValues = new List<DataName>() { };

            filterStr = new List<string>() { };

            for (int i = 0; i < noTitle.Count; i++)
            {
                for (int j = 0; j < values.Count; j++)
                {
                    if (i == j)
                    {
                        // исключаем числа более 5 знаков
                        long digitCount = (long)Math.Log10(Math.Abs(values[j].REFGPS)) + 1;
                        if (digitCount < 5)
                        {
                            filterValues.Add(values[j]);

                            filterStr.Add(noTitle[i]);

                        }
                    }
                }
            }
        }

        //       ИЗ ЮЛИАНСКОЙ В ГРИГОРИАНСКУЮ
        public static DateTime FromJulian(double julianDate)
        {
            return new DateTime(
            (long)((julianDate - 1721425.5) * TimeSpan.TicksPerDay),
            DateTimeKind.Utc);
        }

        // метод создания суточных данных кнопками
        public static void UploadWeekFileByButton(string fileName,string pathCreatingDailyFilesTTS,
            string pathCreatFilterDailyFilesTTS, string nameFileTTS, out int nameLastDay)
        {
            nameLastDay = 0;
            //путь каталога  откуда считываем данные
            string path = fileName;
            ConvertToNumber(path,out List<DataName> values,out List<string> noTitle,
                             out List<string> title, out int firstDay);

            nameLastDay = firstDay + 6;

            CreateDayFile(path,pathCreatingDailyFilesTTS,pathCreatFilterDailyFilesTTS,
                nameFileTTS,title,firstDay, values,noTitle);
                
        }

        //   метод для открытия диалогового окна 
        public static void FileDialogOpen(string pathNameGeneratedFileTTS, out string fileName,
                           out string labNameFileTTS)
        {
            fileName = ""; //  название файла = полный путь
            labNameFileTTS = ""; // имя выбранного файла  DATA.064
            List<string> fileContentList1 = new List<string>() { };
          
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var nameFile = string.Empty;
            var fullNameFile = string.Empty;

            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {  
                   // недельные файлы по конкретному TTS
                    openFileDialog.InitialDirectory = @"D:\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Получаем путь к указанному файлу
                        filePath = openFileDialog.FileName;

                        // Считываем содержимое файла в поток
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            fileContent = reader.ReadToEnd();

                            fileContentList1.Add(fileContent);
                        }
                    }
                }
                string last = filePath.Substring(filePath.Length - 8); // = DATA.064

                string date = last.Remove(0, 4); // = .064
                // путь для записи исправленного недельного файла
                string newPath = $@"{pathNameGeneratedFileTTS}{date}";
             
                Console.WriteLine($"путь для записи исправленного недельного файла:{newPath}");

                CutTitle(filePath, newPath);

                labNameFileTTS += last;

                fileName += newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // определяем нужный файл для записи в свою папку 
        public static void CopyLastRecordedFile(string pathRead,string pathWrite, out string fullPathRecordFile)
        {
            // ДЛЯ ТЕСТИРОВАНИЯ МЕТОДА , ФАЙЛ В D:\Test_Anna\Main ПЕРЕИМЕНОВАТЬ В ДАДУ СЕГОДНЯ,И ВНЕСТИ В НЕГО ИЗМЕНЕНИЯ
            //string[] dirs = Directory.GetFiles($"{mainPath}"); // string mainPath ==  @"D:\Test_Anna\Main"; //  @"\\192.168.0.117\moto";
            fullPathRecordFile = "";

            string[] dirs = Directory.GetFiles($@"{pathRead}");

            string pathWork = $@"{pathWrite}"; 

            List<string> route = new List<string>() { };

            List<string> data = new List<string>() { };

            DateTime date = DateTime.Now;

            var JD = date.ToOADate() + 2415018.5; // ПЕРЕВОД В ЮЛИАНСКУЮ ДАТУ

            int MJD = (int)(JD - 2400000.5);

            string strDay = MJD.ToString();

            string today = strDay.Substring(strDay.Length - 3);

            int tydayInt = Convert.ToInt32(today);

            string testStr = $"DATA.{today}";

            for (int i = 0; i < dirs.Length; i++)
            {
                route.Add(dirs[i]);
            }
            route.Reverse(); // читаем директорию с конца
            foreach (var item in route)
            {
                if (item.Contains(testStr) &&
                     File.GetLastWriteTime(item).ToShortDateString() == date.ToShortDateString())
                {
                    // считываем файл
                    using (StreamReader sr = new StreamReader(item, System.Text.Encoding.Default))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            data.Add(line);
                        }
                    }
                    //  записываем данные в папку с копиями
                    string path = $"{pathWork}{testStr}";

                    fullPathRecordFile = path;

                    using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                    {
                        data.ForEach(i => sw.WriteLine(i));
      
                    }
                }
                else
                {
                    string str = item.Substring(item.Length - 3);
                    int num;
                    bool isNum = int.TryParse(str, out num);
                    if (isNum)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            if (num == tydayInt - i &&
                            File.GetLastWriteTime(item).ToShortDateString() == date.ToShortDateString())
                            {
                                using (StreamReader sr = new StreamReader(item, System.Text.Encoding.Default))
                                {
                                    string line;
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        data.Add(line);
                                    }
                                }
                                string path = $"{pathWork}DATA.{num}";

                                fullPathRecordFile = path;

                                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                                {
                                    data.ForEach(j => sw.WriteLine(j));
                                }
                            }
                        }
                    }
                }
            }
        }

        // метод вырезания лишних заголовков из недельного файла
        // формирует в папке TTS исправленный  недельный файл
        // для методов связанных с кнопками
        public static void CutTitle(string path, string newPath)
        {
            //путь каталога  откуда считываем данные = path
            //путь к папке для заиси данных  = newPath
            // список считанных данных
            List<string> dataWeek = new List<string>() { };
            // список из строк заголовка
            List<string> title = new List<string>() { };
            // список данных без заголовка
            List<string> noTitle = new List<string>() { };
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    dataWeek.Add(line);
                }
            }
            for (int i = 0; i < dataWeek.Count; i++)
            {
                if (i < 19)
                {
                    title.Add(dataWeek[i]); // список со строками заголовка
                }
                else if (!title.Contains(dataWeek[i]))  // сравниваем список из строк заголовка с остальными данными
                {
                    noTitle.Add(dataWeek[i]);
                }
            }
            // список который записываем в недельный файл 
            using (StreamWriter sw = new StreamWriter(newPath, false, System.Text.Encoding.Default))
            {
                title.ForEach(i => sw.WriteLine(i));
                noTitle.ForEach(i => sw.WriteLine(i));
            }
        }
        // метод чтения файла всех недельных данных  
        public static void ReadAllWeeksFile(string namePath, out List<DataName> values)
        {
            string path = $"{namePath}";

            values = new List<DataName>() { };

            List<string> allWeeks = new List<string>() { };

            try
            {   // вычитываем данные из файла и заполняем ими список
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        allWeeks.Add(line);
                    }
                }
                for (int i = 0; i < allWeeks.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = allWeeks[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        DataName val = new DataName();
                        // преобразовываем строки к интовому значению 
                        number = Convert.ToInt32(TextArray[0], 10);
                        val.PRN = number;

                        number = Convert.ToInt32(TextArray[1], 16);
                        val.CL = number;

                        number = Convert.ToInt32(TextArray[2], 10);
                        val.MJD = number;
                        //преобразовываем строку к интовому значению параметр время STTIME
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
                        // преобразовываем для отображения в секундах
                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[4], 10);
                        val.TRKL = number;

                        number = Convert.ToInt32(TextArray[5], 10);
                        val.ELV = number;

                        number = Convert.ToInt32(TextArray[6], 10);
                        val.AZTH = number;

                        number = Convert.ToInt32(TextArray[7], 10);
                        val.REFSV = number;

                        number = Convert.ToInt32(TextArray[8], 10);
                        val.SRSV = number;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFGPS = number;

                        number = Convert.ToInt32(TextArray[10], 10);
                        val.SRGPS = number;

                        number = Convert.ToInt32(TextArray[11], 10);
                        val.DSG = number;

                        number = Convert.ToInt32(TextArray[12], 10);
                        val.IOE = number;

                        number = Convert.ToInt32(TextArray[13], 10);
                        val.MDTR = number;

                        number = Convert.ToInt32(TextArray[14], 10);
                        val.SMDT = number;

                        number = Convert.ToInt32(TextArray[15], 10);
                        val.MDIO = number;

                        number = Convert.ToInt32(TextArray[16], 10);
                        val.SMDI = number;

                        number = Convert.ToInt32(TextArray[17], 16);
                        val.CK = number;

                        // Добавляем Элементы в общий список  "values"
                        values.Add(val);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // дозаписывает все недельные данные в непрерывный текстовый файл
        // для методов самостоятельной загрузки данных
        public static void CutTitleWriteAllWeeks(string path,string pathWriteAllWeeks,string newPath)
        {   
            List<string> dataWeek = new List<string>() { };
            // список из строк заголовка
            List<string> title = new List<string>() { };
            // список данных без заголовка
            List<string> noTitle = new List<string>() { };

            List<string> dataTest = new List<string>() { };
            // определить последний записанный файл в каталоге читать этот файл

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    dataWeek.Add(line);
                }
            }

            for (int i = 0; i < dataWeek.Count; i++)
            {
                if (i < 19)
                {
                    title.Add(dataWeek[i]); // список со строками заголовка
                }
                else if (!title.Contains(dataWeek[i]))  // сравниваем список из строк заголовка с остальными данными
                {
                    noTitle.Add(dataWeek[i]);
                }
            }
            //дозапись в список всех недельных данных
            using (StreamWriter sw = File.AppendText(pathWriteAllWeeks))
            {
                noTitle.ForEach(i => sw.WriteLine(i));
            }
            //список который записываем в недельный файл 
            using (StreamWriter sw = new StreamWriter(newPath, false, System.Text.Encoding.Default))
            {
                ConvertToNumber(path, out List<DataName> values,
                out List<string> noTitleStr, out List<string> titleStr, out int firstDayNumber);

                Filter(values, noTitleStr,
                out List<DataName> filterValues, out List<string> filterStr);

                titleStr.ForEach(i => sw.WriteLine(i));
                filterStr.ForEach(i => sw.WriteLine(i));

            }
        }
        // метод конвертации строкового списка в список int значений
        public static void ConvertToNumber(string path,out List<DataName> values,
            out List<string> noTitle, out List<string> title, out int firstDayNumber)
        {
            // path - путь каталога  откуда считываем данные
           // недельный файл без лишних заголовков в каталоге Weeks
            values = new List<DataName>() { }; // выходящий список конвертированных данных
            //список данных без заголовка
            noTitle = new List<string>() { };
            // строки загловка
            title = new List<string>() { };
            // считанные  данные
            List<string> data = new List<string>() { };
            
            // вычитываем данные из файла и заполняем ими список
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }
     
            for (int i = 0; i < data.Count; i++)
            {
                if (i < 19)
                {
                    title.Add(data[i]); 
                }
                else if (!title.Contains(data[i]))
                {
                    noTitle.Add(data[i]);
                }
            }

            for (int i = 0; i < noTitle.Count; i++)
            {
                int number;
                char[] arr = new char[1] { ' ' };
                string[] TextArray = noTitle[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                {
                    DataName val = new DataName();
                    // преобразовываем строки к интовому значению 
                    number = Convert.ToInt32(TextArray[0], 10);
                    val.PRN = number;

                    number = Convert.ToInt32(TextArray[1], 16);
                    val.CL = number;

                    number = Convert.ToInt32(TextArray[2], 10);
                    val.MJD = number;

                    //преобразовываем строку к интовому значению параметр время STTIME
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
                    // преобразовываем для отображения в секундах
                    double min = TimeSpan.Parse(strTime).TotalSeconds;
                    val.STTIME = (int)min;

                    number = Convert.ToInt32(TextArray[4], 10);
                    val.TRKL = number;

                    number = Convert.ToInt32(TextArray[5], 10);
                    val.ELV = number;

                    number = Convert.ToInt32(TextArray[6], 10);
                    val.AZTH = number;

                    number = Convert.ToInt32(TextArray[7], 10);
                    val.REFSV = number;

                    number = Convert.ToInt32(TextArray[8], 10);
                    val.SRSV = number;

                    number = Convert.ToInt32(TextArray[9], 10);
                    val.REFGPS = number;

                    number = Convert.ToInt32(TextArray[10], 10);
                    val.SRGPS = number;

                    number = Convert.ToInt32(TextArray[11], 10);
                    val.DSG = number;

                    number = Convert.ToInt32(TextArray[12], 10);
                    val.IOE = number;

                    number = Convert.ToInt32(TextArray[13], 10);
                    val.MDTR = number;

                    number = Convert.ToInt32(TextArray[14], 10);
                    val.SMDT = number;

                    number = Convert.ToInt32(TextArray[15], 10);
                    val.MDIO = number;

                    number = Convert.ToInt32(TextArray[16], 10);
                    val.SMDI = number;

                    number = Convert.ToInt32(TextArray[17], 16);
                    val.CK = number;

                    // Добавляем Элементы в общий список  "values"
                    values.Add(val);
                }
            }
            firstDayNumber = 0;

            for (int i = 0; i < values.Count; i++)
            {
                if (i == 0)
                {
                    firstDayNumber += values[i].MJD; // номер первых суток
                }
            }

        }
        // формирование суточных файлов
        public static void CreateDayFile(string path, string pathCreatingDailyFilesTTS,string pathCreatFilterDailyFilesTTS,
            string nameFileTTS, List<string> title, int firstDayNumber, List<DataName> values,List<string> noTitle)
        {
            // path - путь каталога  откуда считываем данные
            // pathCreatingDailyFilesTTS - путь где будем формировать суточный файл
            // nameFileTTS - имя для суточного файла (у каждого TTS суточное имя файла разное)
            #region
  
            List<DataName> valDay1 = new List<DataName>() { };
            List<DataName> valDay2 = new List<DataName>() { };
            List<DataName> valDay3 = new List<DataName>() { };
            List<DataName> valDay4 = new List<DataName>() { };
            List<DataName> valDay5 = new List<DataName>() { };
            List<DataName> valDay6 = new List<DataName>() { };
            List<DataName> valDay7 = new List<DataName>() { };

            List<string> DataDay_1 = new List<string>() { };
            List<string> DataDay_2 = new List<string>() { };
            List<string> DataDay_3 = new List<string>() { };
            List<string> DataDay_4 = new List<string>() { };
            List<string> DataDay_5 = new List<string>() { };
            List<string> DataDay_6 = new List<string>() { };
            List<string> DataDay_7 = new List<string>() { };
        
            List<string> FilterDataDay_1 = new List<string>() { };
            List<string> FilterDataDay_2 = new List<string>() { };
            List<string> FilterDataDay_3 = new List<string>() { };
            List<string> FilterDataDay_4 = new List<string>() { };
            List<string> FilterDataDay_5 = new List<string>() { };
            List<string> FilterDataDay_6 = new List<string>() { };
            List<string> FilterDataDay_7 = new List<string>() { };

            string writePathFilterDay_1 = "";
            string writePathFilterDay_2 = "";
            string writePathFilterDay_3 = "";
            string writePathFilterDay_4 = "";
            string writePathFilterDay_5 = "";
            string writePathFilterDay_6 = "";
            string writePathFilterDay_7 = "";
          
            // дописываем в суточные файлы заголовок документа
            DataDay_1.AddRange(title);
            DataDay_2.AddRange(title);
            DataDay_3.AddRange(title);
            DataDay_4.AddRange(title);
            DataDay_5.AddRange(title);
            DataDay_6.AddRange(title); // InsertRange(0, title);
            DataDay_7.AddRange(title);

            for (int d = 0; d < 7; d++)
            {
                for (int s = 0; s < noTitle.Count; s++)
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (d == 0 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay1.Add(values[i]);

                            DataDay_1.Add(noTitle[s]);

                            FilterDataDay_1.Add(noTitle[s]);

                            int numDay1 = firstDayNumber + d;

                            string strName = numDay1.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);
                            // путь к суточным файлам 
                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_1 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_1.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 1 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay2.Add(values[i]);

                            DataDay_2.Add(noTitle[s]);

                            FilterDataDay_2.Add(noTitle[s]);

                            int numDay2 = firstDayNumber + d;

                            string strName = numDay2.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_2 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_2.ForEach(j => sw.WriteLine(j));
                            }
                        
                        }
                        else if (d == 2 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay3.Add(values[i]);

                            DataDay_3.Add(noTitle[s]);

                            FilterDataDay_3.Add(noTitle[s]);

                            int numDay3 = firstDayNumber + d;

                            string strName = numDay3.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_3 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_3.ForEach(j => sw.WriteLine(j));
                            }
                       
                        }
                        else if (d == 3 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay4.Add(values[i]);

                            DataDay_4.Add(noTitle[s]);

                            FilterDataDay_4.Add(noTitle[s]);

                            int numDay4 = firstDayNumber + d;
                            
                            string strName = numDay4.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_4 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_4.ForEach(j => sw.WriteLine(j));
                            }
                         
                        }
                        else if (d == 4 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay5.Add(values[i]);

                            DataDay_5.Add(noTitle[s]);

                            FilterDataDay_5.Add(noTitle[s]);

                            int numDay5 = firstDayNumber + d;

                            string strName = numDay5.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_5 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_5.ForEach(j => sw.WriteLine(j));
                            }
                         
                        }
                        else if (d == 5 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay6.Add(values[i]);

                            DataDay_6.Add(noTitle[s]);
                            
                            FilterDataDay_6.Add(noTitle[s]);
                           
                            int numDay6 = firstDayNumber + d;

                            string strName = numDay6.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_6 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_6.ForEach(j => sw.WriteLine(j));
                            }
                    
                        }
                        else if (d == 6 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay7.Add(values[i]);

                            DataDay_7.Add(noTitle[s]);
                            
                            FilterDataDay_7.Add(noTitle[s]);
                           
                            int numDay7 = firstDayNumber + d;

                            string strName = numDay7.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_7 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_7.ForEach(j => sw.WriteLine(j));
                            }
                     
                        }
                    }
                }
            }
            // создаем файлы с усредненными показателями
            MovingMedianOverTriplets(valDay1, FilterDataDay_1, out List<string> smoothed_1);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_1, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_1.ForEach(j => sw.WriteLine(j));
            }

            MovingMedianOverTriplets(valDay2, FilterDataDay_2, out List<string> smoothed_2);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_2, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_2.ForEach(j => sw.WriteLine(j));
            }

            MovingMedianOverTriplets(valDay3, FilterDataDay_3, out List<string> smoothed_3);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_3, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_3.ForEach(j => sw.WriteLine(j));
            }

            MovingMedianOverTriplets(valDay4, FilterDataDay_4, out List<string> smoothed_4);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_4, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_4.ForEach(j => sw.WriteLine(j));
            }

            MovingMedianOverTriplets(valDay5, FilterDataDay_5, out List<string> smoothed_5);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_5, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_5.ForEach(j => sw.WriteLine(j));
            }

            MovingMedianOverTriplets(valDay6, FilterDataDay_6, out List<string> smoothed_6);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_6, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_6.ForEach(j => sw.WriteLine(j));
            }

            MovingMedianOverTriplets(valDay7, FilterDataDay_7, out List<string> smoothed_7);
            using (StreamWriter sw = new StreamWriter(writePathFilterDay_7, false, System.Text.Encoding.Default))
            {
                title.ForEach(t => sw.WriteLine(t));
                smoothed_7.ForEach(j => sw.WriteLine(j));
            }

        }

        #endregion
 //*******************************************************************************
     
        // считываем файл формируем, список всех значений 
        public static void RinexReadAllWeeksFile(string namePath, out List<RinexData> values)
        {
            string path = $"{namePath}";

            values = new List<RinexData>() { };

            List<string> allWeeks = new List<string>() { };

            try
            {   // вычитываем данные из файла и заполняем ими список
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        allWeeks.Add(line);
                    }
                }
                for (int i = 0; i < allWeeks.Count; i++)
                {
                    int number;
                    char[] arr = new char[1] { ' ' };
                    string[] TextArray = allWeeks[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                    if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                    {
                        RinexData val = new RinexData();
                        // преобразовываем строки к интовому значению 
                        number = Convert.ToInt32(TextArray[0], 10);
                        val.SAT = number;

                        number = Convert.ToInt32(TextArray[1], 16);
                        val.CL = number;

                        number = Convert.ToInt32(TextArray[2], 10);
                        val.MJD = number;
                        //преобразовываем строку к интовому значению параметр время STTIME
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
                        // преобразовываем для отображения в секундах
                        double min = TimeSpan.Parse(strTime).TotalSeconds;
                        val.STTIME = (int)min;

                        number = Convert.ToInt32(TextArray[4], 10);
                        val.TRKL = number;

                        number = Convert.ToInt32(TextArray[5], 10);
                        val.ELV = number;

                        number = Convert.ToInt32(TextArray[6], 10);
                        val.AZTH = number;

                        number = Convert.ToInt32(TextArray[7], 10);
                        val.REFSV = number;

                        number = Convert.ToInt32(TextArray[8], 10);
                        val.SRSV = number;

                        number = Convert.ToInt32(TextArray[9], 10);
                        val.REFSYS = number;

                        number = Convert.ToInt32(TextArray[10], 10);
                        val.SRSYS= number;

                        number = Convert.ToInt32(TextArray[11], 10);
                        val.DSG = number;

                        number = Convert.ToInt32(TextArray[12], 10);
                        val.IOE = number;

                        number = Convert.ToInt32(TextArray[13], 10);
                        val.MDTR = number;

                        number = Convert.ToInt32(TextArray[14], 10);
                        val.SMDT = number;

                        number = Convert.ToInt32(TextArray[15], 10);
                        val.MDIO = number;

                        number = Convert.ToInt32(TextArray[16], 10);
                        val.SMDI = number;

                        number = Convert.ToInt32(TextArray[17],10 );
                        val.MSIO = number;

                        number = Convert.ToInt32(TextArray[18], 10);
                        val.SMSI = number;

                        number = Convert.ToInt32(TextArray[19], 10);
                        val.ISG = number;

                        number = Convert.ToInt32(TextArray[20],10 );
                        val.FR = number;

                        number = Convert.ToInt32(TextArray[21],10 );
                        val.HC = number;
                        //// L3P  == название канала передавемого сигнала
                        //1 спутник в одно время передает данные по нескольким каналам
                    
                        val.FRC = TextArray[22];

                        number = Convert.ToInt32(TextArray[23], 16);
                        val.CK = number;

                        // Добавляем Элементы в общий список  "values"
                        values.Add(val);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RinexReadAllWeeksFile : {ex.Message}");
            }
        }
        // метод конвертации строкового списка в список int значений
        public static void RinexConvertToNumber(string path, out List<RinexData> values,
            out List<string> noTitle, out List<string> title, out int firstDayNumber)
        {
            // path - путь каталога  откуда считываем данные
            // недельный файл без лишних заголовков в каталоге Weeks
            values = new List<RinexData>() { }; // выходящий список конвертированных данных
            //список данных без заголовка
            noTitle = new List<string>() { };
            // строки загловка
            title = new List<string>() { };
            // считанные  данные
            List<string> data = new List<string>() { };

            // вычитываем данные из файла и заполняем ими список
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    data.Add(line);
                }
            }

            for (int i = 0; i < data.Count; i++)
            {
                if (i < 19)
                {
                    title.Add(data[i]);
                }
                else if (!title.Contains(data[i]))
                {
                    noTitle.Add(data[i]);
                }
            }
            for (int i = 0; i < noTitle.Count; i++)
            {
                int number;
                char[] arr = new char[1] { ' ' };
                string[] TextArray = noTitle[i].Split(arr, StringSplitOptions.RemoveEmptyEntries);

                if (TextArray.Length > 0 && Int32.TryParse(TextArray[0], out number))
                {
                    RinexData val = new RinexData();
                    // преобразовываем строки к интовому значению 
                    number = Convert.ToInt32(TextArray[0], 10);
                    val.SAT = number;

                    number = Convert.ToInt32(TextArray[1], 16);
                    val.CL = number;

                    number = Convert.ToInt32(TextArray[2], 10);
                    val.MJD = number;
                    //преобразовываем строку к интовому значению параметр время STTIME
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
                    // преобразовываем для отображения в секундах
                    double min = TimeSpan.Parse(strTime).TotalSeconds;
                    val.STTIME = (int)min;

                    number = Convert.ToInt32(TextArray[4], 10);
                    val.TRKL = number;

                    number = Convert.ToInt32(TextArray[5], 10);
                    val.ELV = number;

                    number = Convert.ToInt32(TextArray[6], 10);
                    val.AZTH = number;

                    number = Convert.ToInt32(TextArray[7], 10);
                    val.REFSV = number;

                    number = Convert.ToInt32(TextArray[8], 10);
                    val.SRSV = number;

                    number = Convert.ToInt32(TextArray[9], 10);
                    val.REFSYS = number;

                    number = Convert.ToInt32(TextArray[10], 10);
                    val.SRSYS = number;

                    number = Convert.ToInt32(TextArray[11], 10);
                    val.DSG = number;

                    number = Convert.ToInt32(TextArray[12], 10);
                    val.IOE = number;

                    number = Convert.ToInt32(TextArray[13], 10);
                    val.MDTR = number;

                    number = Convert.ToInt32(TextArray[14], 10);
                    val.SMDT = number;

                    number = Convert.ToInt32(TextArray[15], 10);
                    val.MDIO = number;

                    number = Convert.ToInt32(TextArray[16], 10);
                    val.SMDI = number;

                    number = Convert.ToInt32(TextArray[17], 10);
                    val.MSIO = number;

                    number = Convert.ToInt32(TextArray[18], 10);
                    val.SMSI = number;

                    number = Convert.ToInt32(TextArray[19], 10);
                    val.ISG = number;

                    number = Convert.ToInt32(TextArray[20], 10);
                    val.FR = number;

                    number = Convert.ToInt32(TextArray[21], 10);
                    val.HC = number;
                  
                    val.FRC = TextArray[22];// оставили строкой

                    number = Convert.ToInt32(TextArray[23], 16);
                    val.CK = number;

                    // Добавляем Элементы в общий список  "values"
                    values.Add(val);
                }

            }

            firstDayNumber = 0;

            for (int i = 0; i < values.Count; i++)
            {
                if (i == 0)
                {
                    firstDayNumber += values[i].MJD; // номер первых суток
                }
            }

        }

        // формирование суточных файлов
        public static void RinexCreateDayFile(string path, string pathCreatingDailyFilesTTS, string pathCreatFilterDailyFilesTTS,
            string nameFileTTS, List<string> title, int firstDayNumber, List<RinexData> values, List<string> noTitle)
        {
            // path - путь каталога  откуда считываем данные
            // pathCreatingDailyFilesTTS - путь где будем формировать суточный файл
            // nameFileTTS - имя для суточного файла (у каждого TTS суточное имя файла разное)
            #region

            List<RinexData> valDay1 = new List<RinexData> () { };
            List<RinexData> valDay2 = new List<RinexData>() { };
            List<RinexData> valDay3 = new List<RinexData>() { };
            List<RinexData> valDay4 = new List<RinexData>() { };
            List<RinexData> valDay5 = new List<RinexData>() { };
            List<RinexData> valDay6 = new List<RinexData>() { };
            List<RinexData> valDay7 = new List<RinexData>() { };

            List<string> DataDay_1 = new List<string>() { };
            List<string> DataDay_2 = new List<string>() { };
            List<string> DataDay_3 = new List<string>() { };
            List<string> DataDay_4 = new List<string>() { };
            List<string> DataDay_5 = new List<string>() { };
            List<string> DataDay_6 = new List<string>() { };
            List<string> DataDay_7 = new List<string>() { };

            List<string> FilterDataDay_1 = new List<string>() { };
            List<string> FilterDataDay_2 = new List<string>() { };
            List<string> FilterDataDay_3 = new List<string>() { };
            List<string> FilterDataDay_4 = new List<string>() { };
            List<string> FilterDataDay_5 = new List<string>() { };
            List<string> FilterDataDay_6 = new List<string>() { };
            List<string> FilterDataDay_7 = new List<string>() { };

            string writePathFilterDay_1 = "";
            string writePathFilterDay_2 = "";
            string writePathFilterDay_3 = "";
            string writePathFilterDay_4 = "";
            string writePathFilterDay_5 = "";
            string writePathFilterDay_6 = "";
            string writePathFilterDay_7 = "";

            // дописываем в суточные файлы заголовок документа
            DataDay_1.AddRange(title);
            DataDay_2.AddRange(title);
            DataDay_3.AddRange(title);
            DataDay_4.AddRange(title);
            DataDay_5.AddRange(title);
            DataDay_6.AddRange(title); // InsertRange(0, title);
            DataDay_7.AddRange(title);

            for (int d = 0; d < 7; d++)
            {
                for (int s = 0; s < noTitle.Count; s++)
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (d == 0 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay1.Add(values[i]);

                            DataDay_1.Add(noTitle[s]);

                            FilterDataDay_1.Add(noTitle[s]);

                            int numDay1 = firstDayNumber + d;

                            string strName = numDay1.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);
                            // путь к суточным файлам 
                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_1 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_1.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 1 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay2.Add(values[i]);

                            DataDay_2.Add(noTitle[s]);

                            FilterDataDay_2.Add(noTitle[s]);

                            int numDay2 = firstDayNumber + d;

                            string strName = numDay2.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_2 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_2.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 2 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay3.Add(values[i]);

                            DataDay_3.Add(noTitle[s]);

                            FilterDataDay_3.Add(noTitle[s]);

                            int numDay3 = firstDayNumber + d;

                            string strName = numDay3.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_3 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_3.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 3 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay4.Add(values[i]);

                            DataDay_4.Add(noTitle[s]);

                            FilterDataDay_4.Add(noTitle[s]);

                            int numDay4 = firstDayNumber + d;

                            string strName = numDay4.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_4 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_4.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 4 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay5.Add(values[i]);

                            DataDay_5.Add(noTitle[s]);

                            FilterDataDay_5.Add(noTitle[s]);

                            int numDay5 = firstDayNumber + d;

                            string strName = numDay5.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_5 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_5.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 5 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay6.Add(values[i]);

                            DataDay_6.Add(noTitle[s]);

                            FilterDataDay_6.Add(noTitle[s]);

                            int numDay6 = firstDayNumber + d;

                            string strName = numDay6.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_6 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_6.ForEach(j => sw.WriteLine(j));
                            }

                        }
                        else if (d == 6 && values[i].MJD == firstDayNumber + d && i == s)
                        {
                            valDay7.Add(values[i]);

                            DataDay_7.Add(noTitle[s]);

                            FilterDataDay_7.Add(noTitle[s]);

                            int numDay7 = firstDayNumber + d;

                            string strName = numDay7.ToString();

                            string myName1 = strName.Substring(0, 2);

                            string myName2 = strName.Remove(0, 2);

                            string writePath = $"{pathCreatingDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            writePathFilterDay_7 = $"{pathCreatFilterDailyFilesTTS}{nameFileTTS}{myName1}.{myName2}";

                            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                            {
                                DataDay_7.ForEach(j => sw.WriteLine(j));
                            }

                        }
                    }
                }
            }
      

        }

        #endregion
        // метод создания суточных данных кнопками
        public static void RinexUploadWeekFileByButton(string fileName, string pathCreatingDailyFilesTTS,
            string pathCreatFilterDailyFilesTTS, string nameFileTTS, out int nameLastDay)
        {
            nameLastDay = 0;
            //путь каталога  откуда считываем данные
            string path = fileName;
            RinexConvertToNumber(path, out List<RinexData> values, out List<string> noTitle,
                             out List<string> title, out int firstDay);

            nameLastDay = firstDay + 6;

            RinexCreateDayFile(path, pathCreatingDailyFilesTTS, pathCreatFilterDailyFilesTTS,
                nameFileTTS, title, firstDay, values, noTitle);

        }

        // читаем файл выбираем данные по конкретному спутнику(satellite)
        // по конкретному каналу связи
        // считываем данные из файла,составляем списки с данными по конкретному спутнику 
        // но с разными каналами получения сигнала
     
        public static void RinexCanalDataSatellite(int dataPRN,out List<List<RinexData>> listCanal,
            out DateTime startDate, out DateTime endDate)
        {
            startDate = new DateTime();
            endDate = new DateTime();

            List<RinexData> canal_1 = new List<RinexData>() { };
            List<RinexData> canal_2 = new List<RinexData>() { };
            List<RinexData> canal_3 = new List<RinexData>() { };
            List<RinexData> canal_4 = new List<RinexData>() { };
            List<RinexData> canal_5 = new List<RinexData>() { };
            List<RinexData> canal_6 = new List<RinexData>() { };
            List<RinexData> canal_7 = new List<RinexData>() { };
            List<RinexData> canal_8 = new List<RinexData>() { };
            List<RinexData> canal_9 = new List<RinexData>() { };
            List<RinexData> canal_10 = new List<RinexData>() { };

            listCanal = new List<List<RinexData>>
            {
                canal_1,canal_2,canal_3,canal_4,canal_5,
                canal_6,canal_7,canal_8,canal_9,canal_10
            };

            try
            {
                // номер спутника   dataPRN -  получаем  от пользователя
                // путь считывания данных
                string path = @"D:\KvantWork\Work_TTS-4\AllWeeks\AllWeeks_tts-4.txt";

                RinexReadAllWeeksFile(path, out List<RinexData> values);

                List<DateTime> dateTime = new List<DateTime>();

                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].SAT == dataPRN)
                    {
                        double JD = values[i].MJD + 2400000.5; // приводим к григорианскому календарю

                        DateTime date = Methods.FromJulian(JD) + TimeSpan.FromSeconds(values[i].STTIME);

                        dateTime.Add(date);
                    }
                }
             
                for (int i = 0; i < dateTime.Count; i++)
                {
                    if (i == 0)
                    {
                        startDate = dateTime[i];
                    }
                    if (i == dateTime.Count - 1)
                    {
                        endDate = dateTime[i];
                    }
                }

                var canalL1P = values.Where(v => v.SAT == dataPRN && v.FRC == "L1P");
                foreach (var item in canalL1P)
                {
                    canal_1.Add(item);
                }

                var canalL2P = values.Where(v => v.SAT == dataPRN && v.FRC == "L2P");
                foreach (var item in canalL2P)
                {
                    canal_2.Add(item);
                }

                var canalL3P = values.Where(v => v.SAT == dataPRN && v.FRC == "L3P");
                foreach (var item in canalL3P)
                {
                    canal_3.Add(item);
                }

                var canalL4P = values.Where(v => v.SAT == dataPRN && v.FRC == "L4P");
                foreach (var item in canalL4P)
                {
                    canal_4.Add(item);
                }

                var canalL5P = values.Where(v => v.SAT == dataPRN && v.FRC == "L5P");
                foreach (var item in canalL5P)
                {
                    canal_5.Add(item);
                }

                var canalL1C = values.Where(v => v.SAT == dataPRN && v.FRC == "L1C");
                foreach (var item in canalL1C)
                {
                    canal_6.Add(item);
                }

                var canalL2C = values.Where(v => v.SAT == dataPRN && v.FRC == "L2C");
                foreach (var item in canalL2C)
                {
                    canal_7.Add(item);
                }

                var canalL3C = values.Where(v => v.SAT == dataPRN && v.FRC == "L3C");
                foreach (var item in canalL3C)
                {
                    canal_8.Add(item);
                }

                var canalL4C = values.Where(v => v.SAT == dataPRN && v.FRC == "L4C");
                foreach (var item in canalL4C)
                {
                    canal_9.Add(item);
                }

                var canalL5C = values.Where(v => v.SAT == dataPRN && v.FRC == "L5C");
                foreach (var item in canalL5C)
                {
                    canal_10.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" RinexCanalDataSatellite : {ex.Message}");
            }
        }
    }
}


