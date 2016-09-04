using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using NHibernate.Cfg;
using System.Security.Cryptography;
using LY.EMIS5.Const;
using LY.EMIS5.Common.Extensions;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace LY.EMIS5.Common
{
    public static class Util
    {
        public static DateTime MinDateTime = new DateTime(1900, 1, 1);

        public static DateTime MaxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static byte[] HexStringToByteArray(this string hexString)
        {
            if (string.IsNullOrWhiteSpace(hexString) || hexString.Length % 2 != 0)
                throw new ArgumentException("hexString必须是16进制字符串", "hexString");
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hexString.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
            }
            return bytes;
        }

        public static string ByteArrayToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            for (var i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string Truncate(this string source, int maxLength)
        {
            if (maxLength <= 0) throw new ArgumentException("maxLength必须大于0", "maxLength");
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length <= maxLength)
                return source;
            return source.Substring(0, maxLength);
        }

        public static int IndexOf<T>(this IEnumerable<T> source, IEnumerable<T> value, int start = 0)
        {
            for (var i = start; i < source.Count() - value.Count(); i++)
            {
                var equals = true;
                for (var j = 0; j < value.Count(); j++)
                {
                    if (!source.ElementAt(i + j).Equals(value.ElementAt(j)))
                    {
                        equals = false;
                        break;
                    }
                }
                if (equals)
                    return i;
            }
            return -1;
        }

        public static IEnumerable<T> SubOf<T>(this IEnumerable<T> source, int startIndex, int length)
        {
            return source.Skip(startIndex).Take(length);
        }

        public static IEnumerable<T> SubOf<T>(this IEnumerable<T> source, int startIndex)
        {
            return source.Skip(startIndex).Take(source.Count() - startIndex);
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, IEnumerable<T> splitter)
        {
            var ret = new List<IEnumerable<T>>();
            var splitter_length = splitter.Count();
            var p = 0;
            var i = 0;
            while ((i = source.IndexOf(splitter, p)) > -1)
            {
                if (i > 0)
                    ret.Add(source.SubOf(p, i - p));
                p = i + splitter_length;
            }
            if (p < source.Count())
                ret.Add(source.SubOf(p));

            return ret;
        }

        public static string ToChineseDateString(this DateTime datetime)
        {
            if (datetime.Year < 1990)
            {
                return "";
            }
            if (datetime.Year > 2050)
            {
                return "";
            }
            return datetime.ToString("yyyy-MM-dd HH:mm");
        }
        public static string ToYearMonthDayString(this DateTime datetime)
        {
            if (datetime.Year<1990)
            {
                return "";
            }
            if (datetime.Year > 2050)
            {
                return "";
            }
            return datetime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取配置文件参数
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetSettings(string key)
        {
            return ConfigurationManager.AppSettings[key] == null ? "" : ConfigurationManager.AppSettings[key].ToString();
        }
        /// <summary>
        /// 返回JSON提示信息
        /// </summary>
        /// <param name="icon">类型</param>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <param name="location">跳转页</param>
        /// <returns></returns>
        public static JsonResult Echo(IconFlags icon, string title, string message, string location = "")
        {
            return new JsonResult()
            {
                Data = new
                {
                    icon = icon.GetName(),
                    title = title,
                    message = message,
                    location = location
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 返回JSON提示信息
        /// </summary>
        /// <param name="icon">类型</param>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <param name="location">跳转页</param>
        /// <returns></returns>
        public static JsonResult Echo(int icon, string title, string message, string location = "")
        {
            IconFlags iconFlag = (IconFlags)icon;
            var echo = JsonConvert.SerializeObject(new
                {
                    icon = iconFlag.GetName(),
                    title = title,
                    message = message,
                    location = location
                });
            return new JsonResult()
            {
                Data = new
                {
                    icon = iconFlag.GetName(),
                    title = title,
                    message = message,
                    location = location
                },
                ContentType = "text/html",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            //var response = HttpContext.Current.Response;
            //response.Clear();
            //response.Write(echo);
            //response.ContentType = "text/html";
            //response.ContentEncoding = Encoding.UTF8;
            //response.Flush();
            //response.End();
            //return null;

        }

        public static string toSubtring(this string str, int startIndex, int length)
        {
            if (str.Length < length)
                return str;
            else
                return str.Substring(startIndex, length);
        }
        public static ILog GetLogger()
        {
            //System.Reflection.MethodBase.GetCurrentMethod().DeclaringType获取的是当前对象，我们现在需要的是调用本方法的对象
            return LogManager.GetLogger(new StackFrame(1).GetMethod().DeclaringType);
        }

        #region 汉字转拼音
        private static int[] pyValue = new int[] {
            -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, 
            -20230, -20051, -20036, -20032, -20026, -20002, -19990, -19986, -19982,
            -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, 
            -19741, -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515, 
            -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, 
            -19263, -19261, -19249, -19243, -19242, -19238, -19235, -19227, -19224, 
            -19218, -19212, -19038, -19023, -19018, -19006, -19003, -18996, -18977,
            -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, 
            -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490,
            -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220,
            -18211, -18201, -18184, -18183, -18181, -18012, -17997, -17988, -17970, 
            -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752, 
            -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676,
            -17496, -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, 
            -17185, -16983, -16970, -16942, -16915, -16733, -16708, -16706, -16689, 
            -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448, 
            -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, 
            -16393, -16220, -16216, -16212, -16205, -16202, -16187, -16180, -16171,
            -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, 
            -15903, -15889, -15878, -15707, -15701, -15681, -15667, -15661, -15659, 
            -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419,
            -15416, -15408, -15394, -15385, -15377, -15375, -15369, -15363, -15362, 
            -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, 
            -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, 
            -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, -14921,
            -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, 
            -14678, -14674, -14670, -14668, -14663, -14654, -14645, -14630, -14594,
            -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345,
            -14170, -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, 
            -14123, -14122, -14112, -14109, -14099, -14097, -14094, -14092, -14090, 
            -14087, -14083, -13917, -13914, -13910, -13907, -13906, -13905, -13896, 
            -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601,
            -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, 
            -13359, -13356, -13343, -13340, -13329, -13326, -13318, -13147, -13138, 
            -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060, 
            -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831,
            -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359,
            -12346, -12320, -12300, -12120, -12099, -12089, -12074, -12067, -12058,
            -12039, -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, 
            -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067,
            -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018,
            -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587,
            -10544, -10533, -10519, -10331, -10329, -10328, -10322, -10315, -10309, 
            -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254 
        };
        private static string[] pyName = new string[] {
            "A", "Ai", "An", "Ang", "Ao", "Ba", "Bai", "Ban", "Bang", "Bao", "Bei", 
            "Ben", "Beng", "Bi", "Bian", "Biao", "Bie", "Bin", "Bing", "Bo", "Bu",
            "Ba", "Cai", "Can", "Cang", "Cao", "Ce", "Ceng", "Cha", "Chai", "Chan",
            "Chang", "Chao", "Che", "Chen", "Cheng", "Chi", "Chong", "Chou", "Chu",
            "Chuai", "Chuan", "Chuang", "Chui", "Chun", "Chuo", "Ci", "Cong", "Cou",
            "Cu", "Cuan", "Cui", "Cun", "Cuo", "Da", "Dai", "Dan", "Dang", "Dao", "De", 
            "Deng", "Di", "Dian", "Diao", "Die", "Ding", "Diu", "Dong", "Dou", "Du", 
            "Duan", "Dui", "Dun", "Duo", "E", "En", "Er", "Fa", "Fan", "Fang", "Fei", 
            "Fen", "Feng", "Fo", "Fou", "Fu", "Ga", "Gai", "Gan", "Gang", "Gao", "Ge", 
            "Gei", "Gen", "Geng", "Gong", "Gou", "Gu", "Gua", "Guai", "Guan", "Guang", 
            "Gui", "Gun", "Guo", "Ha", "Hai", "Han", "Hang", "Hao", "He", "Hei", "Hen", 
            "Heng", "Hong", "Hou", "Hu", "Hua", "Huai", "Huan", "Huang", "Hui", "Hun",
            "Huo", "Ji", "Jia", "Jian", "Jiang", "Jiao", "Jie", "Jin", "Jing", "Jiong", 
            "Jiu", "Ju", "Juan", "Jue", "Jun", "Ka", "Kai", "Kan", "Kang", "Kao", "Ke",
            "Ken", "Keng", "Kong", "Kou", "Ku", "Kua", "Kuai", "Kuan", "Kuang", "Kui", 
            "Kun", "Kuo", "La", "Lai", "Lan", "Lang", "Lao", "Le", "Lei", "Leng", "Li",
            "Lia", "Lian", "Liang", "Liao", "Lie", "Lin", "Ling", "Liu", "Long", "Lou", 
            "Lu", "Lv", "Luan", "Lue", "Lun", "Luo", "Ma", "Mai", "Man", "Mang", "Mao",
            "Me", "Mei", "Men", "Meng", "Mi", "Mian", "Miao", "Mie", "Min", "Ming", "Miu",
            "Mo", "Mou", "Mu", "Na", "Nai", "Nan", "Nang", "Nao", "Ne", "Nei", "Nen", 
            "Neng", "Ni", "Nian", "Niang", "Niao", "Nie", "Nin", "Ning", "Niu", "Nong", 
            "Nu", "Nv", "Nuan", "Nue", "Nuo", "O", "Ou", "Pa", "Pai", "Pan", "Pang",
            "Pao", "Pei", "Pen", "Peng", "Pi", "Pian", "Piao", "Pie", "Pin", "Ping", 
            "Po", "Pu", "Qi", "Qia", "Qian", "Qiang", "Qiao", "Qie", "Qin", "Qing",
            "Qiong", "Qiu", "Qu", "Quan", "Que", "Qun", "Ran", "Rang", "Rao", "Re",
            "Ren", "Reng", "Ri", "Rong", "Rou", "Ru", "Ruan", "Rui", "Run", "Ruo", 
            "Sa", "Sai", "San", "Sang", "Sao", "Se", "Sen", "Seng", "Sha", "Shai", 
            "Shan", "Shang", "Shao", "She", "Shen", "Sheng", "Shi", "Shou", "Shu", 
            "Shua", "Shuai", "Shuan", "Shuang", "Shui", "Shun", "Shuo", "Si", "Song", 
            "Sou", "Su", "Suan", "Sui", "Sun", "Suo", "Ta", "Tai", "Tan", "Tang", 
            "Tao", "Te", "Teng", "Ti", "Tian", "Tiao", "Tie", "Ting", "Tong", "Tou",
            "Tu", "Tuan", "Tui", "Tun", "Tuo", "Wa", "Wai", "Wan", "Wang", "Wei",
            "Wen", "Weng", "Wo", "Wu", "Xi", "Xia", "Xian", "Xiang", "Xiao", "Xie",
            "Xin", "Xing", "Xiong", "Xiu", "Xu", "Xuan", "Xue", "Xun", "Ya", "Yan",
            "Yang", "Yao", "Ye", "Yi", "Yin", "Ying", "Yo", "Yong", "You", "Yu", 
            "Yuan", "Yue", "Yun", "Za", "Zai", "Zan", "Zang", "Zao", "Ze", "Zei",
            "Zen", "Zeng", "Zha", "Zhai", "Zhan", "Zhang", "Zhao", "Zhe", "Zhen", 
            "Zheng", "Zhi", "Zhong", "Zhou", "Zhu", "Zhua", "Zhuai", "Zhuan", 
            "Zhuang", "Zhui", "Zhun", "Zhuo", "Zi", "Zong", "Zou", "Zu", "Zuan",
            "Zui", "Zun", "Zuo" 
        };
        public static string ChineseToPinyin(string hzString, int maxLength = 0)
        {
            try
            {
                if (hzString.IsEnglish())
                    return hzString;
                if (string.IsNullOrEmpty(hzString))//输入为空
                    return null;
                if (maxLength <= 0) maxLength = hzString.Length;
                char str = '"';//英文状态双引号处理
                //字符处理
                hzString = hzString.Trim().Replace(" ", "").Replace("?", "_").Replace("\\", "_").Replace("/", "_").Replace(":", "").Replace("*", "").Replace(">", "").Replace("<", "").Replace("?", "").Replace("|", "").Replace("\"", "'").Replace("(", "_").Replace(")", "_").Replace(";", "_");
                hzString = hzString.Replace("，", ",").Replace(str.ToString(), "").Replace(str.ToString(), "").Replace("；", "_").Replace("。", "_").Replace("[", "").Replace("]", "").Replace("【", "").Replace("】", "");
                hzString = hzString.Replace("{", "").Replace("}", "").Replace("^", "").Replace("&", "_").Replace("=", "").Replace("~", "_").Replace("@", "_").Replace("￥", "");
                if (hzString.Length > maxLength)
                {
                    hzString = hzString.Substring(0, maxLength);
                }
                Regex regex = new Regex(@"([a-zA-Z0-9\._]+)", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
                if (regex.IsMatch(hzString))
                {
                    if (hzString.Equals(regex.Match(hzString).Groups[1].Value, StringComparison.OrdinalIgnoreCase))
                    {
                        return hzString;
                    }
                }
                // 匹配中文字符          
                regex = new Regex("^[\u4e00-\u9fa5]$");
                byte[] array = new byte[2];
                string pyString = "";
                int chrAsc = 0;
                int i1 = 0;
                int i2 = 0;
                char[] noWChar = hzString.ToCharArray();
                for (int j = 0; j < noWChar.Length; j++)
                {// 中文字符          
                    if (regex.IsMatch(noWChar[j].ToString()))
                    {
                        array = System.Text.Encoding.Default.GetBytes(noWChar[j].ToString());
                        i1 = (short)(array[0]);
                        i2 = (short)(array[1]);
                        chrAsc = i1 * 256 + i2 - 65536;
                        if (chrAsc > 0 && chrAsc < 160)
                        {
                            pyString += noWChar[j];
                        }
                        else
                        {
                            // 修正部分文字          
                            if (chrAsc == -9254)  // 修正"圳"字      
                                pyString += "Zhen";
                            else
                            {
                                for (int i = (pyValue.Length - 1); i >= 0; i--)
                                {
                                    if (pyValue[i] <= chrAsc)
                                    {
                                        pyString += pyName[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    // 非中文字符      
                    else
                    {
                        pyString += noWChar[j].ToString();
                    }
                }
                return pyString;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 汉字转首字母
        /// </summary>
        /// <param name="OneIndexTxt">汉字</param>
        /// <returns>首拼</returns>
        public static String GetOneIndex(String OneIndexTxt)
        {
            if (OneIndexTxt.IsEnglish())
                return OneIndexTxt;

            return string.Concat(OneIndexTxt.Select(c => ChineseToPinyin(c.ToString()).Length > 0 ? ChineseToPinyin(c.ToString()).Substring(0, 1) : ""));
        }

        #endregion

        private static string GetExcelCellValue(ICell cell, CellType cellType)
        {
            var cellValue = "";
            if (cell != null)
            {
                switch (cellType)
                {
                    case CellType.BLANK:
                        cellValue = "";
                        break;
                    case CellType.STRING:
                        cellValue = cell.StringCellValue.Trim();
                        break;
                    case CellType.BOOLEAN:
                        cellValue = cell.BooleanCellValue.ToString();
                        break;
                    case CellType.FORMULA:
                        cellValue = GetExcelCellValue(cell, cell.CachedFormulaResultType);
                        break;
                    case CellType.NUMERIC:
                        cellValue = cell.ToString().Trim();
                        var numbericValue = 0.0;
                        if (!double.TryParse(cellValue, out numbericValue))
                            cellValue = cell.DateCellValue.ToString();
                        break;
                    default:
                        cellValue = cell.ToString().Trim();
                        break;
                }
            }
            return cellValue;
        }

        private static string GetExcelCellValue(ICell cell)
        {
            return GetExcelCellValue(cell, cell == null ? CellType.BLANK : cell.CellType);
        }

        //获取当天星期几
        public static string Week(int year, int month, int day)
        {
            if (month == 1 || month == 2)
            {
                month += 12;
                year -= 1;
            }
            int week = (day + 2 * month + 3 * (month + 1) / 5 + year + year / 4 - year / 100 + year / 400) % 7;
            string weekstr = "";
            switch (week + 1)
            {
                case 1: weekstr = "星期一"; break;
                case 2: weekstr = "星期二"; break;
                case 3: weekstr = "星期三"; break;
                case 4: weekstr = "星期四"; break;
                case 5: weekstr = "星期五"; break;
                case 6: weekstr = "星期六"; break;
                case 7: weekstr = "星期日"; break;
            }

            return weekstr;
        }
        /// <summary>
        /// 验证上传文件是否正确
        /// </summary>
        /// <returns></returns>
        public static string CheckXls()
        {
            if (HttpContext.Current.Request.Files == null || HttpContext.Current.Request.Files.Count == 0)
                return "上传文件出错，请联系系统维护人员！";
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!(extension == ".xls" || extension == ".xlsx"))
                return "文件格式不对，请输入合法的Excel文件，支持Excel97--Excel2007！";
            return "";
        }
        /// <summary>
        /// 组合EXCEL错误行
        /// </summary>
        /// <returns></returns>
        public static DataRow CombinedError(this DataTable Table, string name, string phone, string error)
        {
            var Row = Table.NewRow();
            Row["姓名"] = name;
            Row["手机号码"] = phone;
            Row["错误描述"] = error;
            return Row;
        }
        public static DataTable ImportFromXls(this HttpPostedFileBase file)
        {
            var workbook = WorkbookFactory.Create(file.InputStream);
            var rows = workbook.GetSheetAt(0).GetRowEnumerator();
            //获取表头
            DataTable dt = new DataTable(workbook.GetSheetAt(0).SheetName);
            if (rows.MoveNext())
            {
                var row = rows.Current as IRow;
                for (int i = 0; i <= row.LastCellNum; i++)
                {
                    var cellValue = GetExcelCellValue(row.GetCell(i));
                    if (string.IsNullOrWhiteSpace(cellValue))
                        break;
                    dt.Columns.Add(cellValue, typeof(string));
                }
            }
            while (rows.MoveNext())
            {
                var row = rows.Current as IRow;
                DataRow dr = dt.NewRow();
                bool emptyRow = true;
                foreach (var cell in row.Cells)
                {
                    var cellValue = GetExcelCellValue(cell);
                    if (cell.ColumnIndex >= dr.ItemArray.Length)
                        break;
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        emptyRow = false;
                        dr[cell.ColumnIndex] = cellValue;
                    }
                }
                if (!emptyRow)
                    dt.Rows.Add(dr);
            }
            return dt;
        }

        public static FileResult ExportToXls(this DataTable input, string filename)
        {
            var hssfworkbook = new HSSFWorkbook();
            var sheet1 = hssfworkbook.CreateSheet(input.TableName);
            //sheet1.ProtectSheet("123");
            var row = sheet1.CreateRow(0);
            var cellStyle = hssfworkbook.CreateCellStyle();
            cellStyle.IsLocked = false;
            for (var i = 0; i < input.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(input.Columns[i].ColumnName);
                if (input.Columns[i].Unique)
                {
                    sheet1.SetColumnHidden(i, true);
                }
            }
            for (var i = 0; i < input.Rows.Count; i++)
            {
                row = sheet1.CreateRow(i + 1);
                for (var j = 0; j < input.Columns.Count; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.SetCellValue(input.Rows[i][j].ToString());
                    if (!input.Columns[j].ReadOnly)
                    {
                        cell.CellStyle = cellStyle;
                    }
                }
            }
            MemoryStream fileStream = new MemoryStream();
            hssfworkbook.Write(fileStream);
            fileStream.Position = 0;
            return new FileStreamResult(fileStream, "application/vnd.ms-excel")
            {
                FileDownloadName = HttpContext.Current.Request.Browser.Browser == "IE" ? HttpUtility.UrlEncode(filename, Encoding.UTF8) : filename
            };
        }
        public static void ExportToFile(this DataTable input, string filename)
        {
            var hssfworkbook = new HSSFWorkbook();
            var sheet1 = hssfworkbook.CreateSheet(input.TableName);
            //sheet1.ProtectSheet("123");
            var row = sheet1.CreateRow(0);
            var cellStyle = hssfworkbook.CreateCellStyle();
            cellStyle.IsLocked = false;
            for (var i = 0; i < input.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(input.Columns[i].ColumnName);
                if (input.Columns[i].Unique)
                {
                    sheet1.SetColumnHidden(i, true);
                }
            }
            for (var i = 0; i < input.Rows.Count; i++)
            {
                row = sheet1.CreateRow(i + 1);
                for (var j = 0; j < input.Columns.Count; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.SetCellValue(input.Rows[i][j].ToString());
                    if (!input.Columns[j].ReadOnly)
                    {
                        cell.CellStyle = cellStyle;
                    }
                }
            }
            FileStream fileStream = new FileStream(filename, FileMode.Create);
            hssfworkbook.Write(fileStream);
            fileStream.Flush();
            fileStream.Close();

        }
        public static string ReplaceEmpty(this string inputStr)
        {
            return inputStr.Replace("\u3000", string.Empty).Replace("\u0020", string.Empty).Replace("，", ",").Replace("；", ";");
        }
        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string EncryptString, string EncryptKey)
        {
            string m_strEncrypt = "";
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            Rijndael m_AESProvider = Rijndael.Create();
            try
            {
                byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);
                m_csstream.Write(m_btEncryptString, 0, m_btEncryptString.Length); m_csstream.FlushFinalBlock();
                m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
                m_stream.Close(); m_stream.Dispose();
                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }
            return m_strEncrypt;
        }
        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(string DecryptString, string DecryptKey)
        {
            string m_strDecrypt = "";
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            Rijndael m_AESProvider = Rijndael.Create();
            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);
                m_csstream.Write(m_btDecryptString, 0, m_btDecryptString.Length); m_csstream.FlushFinalBlock();
                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                m_stream.Close(); m_stream.Dispose();
                m_csstream.Close(); m_csstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }
            return m_strDecrypt;
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        public static Stream ProcessRequest()
        {
            using (Bitmap b = new Bitmap(200, 60))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.FillRectangle(new SolidBrush(Color.YellowGreen), 0, 0, 200, 60);
                    Font font = new Font("微软雅黑", 48, FontStyle.Bold, GraphicsUnit.Pixel);
                    Random r = new Random(DateTime.Now.Millisecond);

                    //合法随机显示字符列表
                    string strLetters = "abcdefghijkmnopqrstuvwxyzABCDEFGHIJKMNPQRSTUVWXYZ23456789";
                    StringBuilder s = new StringBuilder();

                    //将随机生成的字符串绘制到图片上
                    for (int i = 0; i < 5; i++)
                    {
                        s.Append(strLetters.Substring(r.Next(0, strLetters.Length - 1), 1));
                        g.DrawString(s[s.Length - 1].ToString(), font, new SolidBrush(Color.Blue), i * 38, r.Next(0, 15));
                    }

                    //生成干扰线条
                    Pen pen = new Pen(new SolidBrush(Color.Blue), 2);
                    for (int i = 0; i < 10; i++)
                    {
                        g.DrawLine(pen, new Point(r.Next(0, 199), r.Next(0, 59)), new Point(r.Next(0, 199), r.Next(0, 59)));
                    }

                    var stream = new MemoryStream();
                    b.Save(stream, ImageFormat.Png);
                    stream.Position = 0;

                    var cookie = new HttpCookie("YZCode", s.ToString().ToLower());
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return stream;
                }
            }
        }

        /// <summary>
        /// 时间转化为长整型
        /// </summary>
        /// <returns></returns>
        public static long TimeToId()
        {
            return DateTime.Now.Ticks;
        }

        public static string ToUnicodeString(string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }

        public static string FromUnicodeString(this string str)
        {
            //最直接的方法Regex.Unescape(str);

            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException ex)
                {
                    return Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }

    }
}
