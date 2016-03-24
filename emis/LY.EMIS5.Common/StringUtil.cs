using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NPOI.Util;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringUtil
    {
        /// <summary>
        /// 卡号中间转换为*，显示前后4位
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string CardToString(this string card)
        {
            if (card.Length > 8)
            {
                var temp = "";
                for (int i = 0; i < card.Length - 8; i++)
                {
                    temp += "*";
                }
                card = card.Substring(0, 4) + temp + card.Substring(card.Length - 4, 4);
            }
            return card;
        }

        public static string Left(this string str, int length)
        {
            if (str.Length <= length)
                return str;
            return str.Substring(0, length);
        }

        public static string Right(this string str, int length)
        {
            if (str.Length <= length)
                return str;
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// 手机号码中间4位转化为*
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string PhoneToString(this string phone)
        {
            phone = phone.Trim();
            if (phone.Length > 7)
                phone = phone.Left(3) + phone.Right(4).PadLeft(phone.Length - 3, '*');
            return phone;
        }
        /// <summary>
        /// 姓名前转化为*
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string NameToString(this string name)
        {
            name = name.Trim();
            if (name.Length >= 2)
                name = name.Last().ToString().PadLeft(name.Length, '*');
            return name;
        }
        public static byte[] PathToByte(this string path)
        {
            FileStream stream = new FileInfo(path).OpenRead();
            Byte[] buffer = new Byte[stream.Length];
            //从流中读取字节块并将该数据写入给定缓冲区buffer中
            stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
            return buffer;
        }
        //返回时间戳
        public static int ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
            return (int)(time - startTime).TotalSeconds;
        }
        //根据文件的扩展名来获取对应的“输出流的HTTP MIME“类型
        public static string TypeToMIME(this string type)
        {
            string ContentType;
            switch (type.ToLower())
            {
                case "asf":
                    ContentType = "video/x-ms-asf";
                    break;
                case "avi":
                    ContentType = "video/avi";
                    break;
                case "doc":
                    ContentType = "application/msword"; break;
                case "zip":
                    ContentType = "application/zip"; break;
                case "xls":
                    ContentType = "application/vnd.ms-excel"; break;
                case "gif":
                    ContentType = "image/gif"; break;
                case "jpg":
                    ContentType = "image/jpeg"; break;
                case "jpeg":
                    ContentType = "image/jpeg"; break;
                case "wav":
                    ContentType = "audio/wav"; break;
                case "mp3":
                    ContentType = "audio/mpeg3"; break;
                case "mpg":
                    ContentType = "video/mpeg"; break;
                case "mepg":
                    ContentType = "video/mpeg"; break;
                case "rtf":
                    ContentType = "application/rtf"; break;
                case "html":
                    ContentType = "text/html"; break;
                case "htm":
                    ContentType = "text/html"; break;
                case "txt":
                    ContentType = "text/plain"; break;
                default:
                    ContentType = "application/octet-stream";
                    break;
            }
            return ContentType;
        }

        //根据文件的扩展名来获取对应的“输出流的HTTP MIME“类型
        public static int TypeToText(this string type)
        {
            int ContentType;
            switch (type.ToLower())
            {
                case "gif":
                    ContentType = 1; break;
                case "jpg":
                    ContentType = 1; break;
                case "jpeg":
                    ContentType = 1; break;
                case "mp3":
                    ContentType = 2; break;
                default:
                    ContentType = 3;
                    break;
            }
            return ContentType;
        }

        public static byte[] Thumbnail(int tWidth, int tHeight, byte[] BigImg, string mode = "")
        {
            MemoryStream ms = new MemoryStream(BigImg);
            System.Drawing.Image oImage = System.Drawing.Image.FromStream(ms);
            int x = 0;
            int y = 0;
            int oWidth = oImage.Width;
            int oHeight = oImage.Height;
            if (oWidth <= tWidth)
            {
                return BigImg;
            }
            if (mode == "Cut")
            {
                if ((double)oWidth / (double)oHeight > (double)tWidth / (double)tHeight)
                {
                    oWidth = oHeight * tWidth / tHeight;
                    y = 0;
                    x = (oImage.Width - oWidth) / 2;
                }
                else
                {
                    oHeight = oImage.Width * tHeight / tWidth;
                    x = 0;
                    y = (oImage.Height - oHeight) / 2;
                }
            }
            else
            {
                if (tHeight > 0)
                    tWidth = (int)Math.Floor(Convert.ToDouble(oWidth) * (Convert.ToDouble(tHeight) / Convert.ToDouble(oHeight)));
                else if (tWidth > 0)
                    tHeight = (int)Math.Floor(Convert.ToDouble(oHeight) * (Convert.ToDouble(tWidth) / Convert.ToDouble(oWidth)));
            }
            Bitmap tImage = new Bitmap(tWidth, tHeight);
            Graphics g = Graphics.FromImage(tImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.DrawImage(oImage, new System.Drawing.Rectangle(0, 0, tWidth, tHeight),
            new System.Drawing.Rectangle(x, y, oWidth, oHeight),
            System.Drawing.GraphicsUnit.Pixel);
            MemoryStream ms1 = new MemoryStream();
            try
            {
                tImage.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oImage.Dispose();
                g.Dispose();
                tImage.Dispose();
            }
            return ms1.ToArray();
        }

        public static byte[] Thumbnail(int tWidth, int tHeight, string BigImg, string mode = "")
        {
            System.Drawing.Image oImage = System.Drawing.Image.FromFile(BigImg);
            int x = 0;
            int y = 0;
            int oWidth = oImage.Width;
            int oHeight = oImage.Height;
            if (mode == "Cut")
            {
                if ((double)oWidth / (double)oHeight > (double)tWidth / (double)tHeight)
                {
                    oWidth = oHeight * tWidth / tHeight;
                    y = 0;
                    x = (oImage.Width - oWidth) / 2;
                }
                else
                {
                    oHeight = oImage.Width * tHeight / tWidth;
                    x = 0;
                    y = (oImage.Height - oHeight) / 2;
                }
            }
            else
            {
                //if (oWidth >= oHeight)
                //{
                tHeight = (int)Math.Floor(Convert.ToDouble(oHeight) * (Convert.ToDouble(tWidth) / Convert.ToDouble(oWidth)));
                //}
                //else { 
                //    tWidth = (int)Math.Floor(Convert.ToDouble(oWidth) * (Convert.ToDouble(tHeight) / Convert.ToDouble(oHeight))); 
                //}
            }
            Bitmap tImage = new Bitmap(tWidth, tHeight);
            Graphics g = Graphics.FromImage(tImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);
            g.DrawImage(oImage, new System.Drawing.Rectangle(0, 0, tWidth, tHeight),
            new System.Drawing.Rectangle(x, y, oWidth, oHeight),
            System.Drawing.GraphicsUnit.Pixel);
            MemoryStream ms1 = new MemoryStream();
            try
            {
                tImage.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oImage.Dispose();
                g.Dispose();
                tImage.Dispose();
            }
            return ms1.ToArray();
        }

        public static double ToDouble(this string s, double d = 0)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch (Exception)
            {
                return d;
            }
        }

        public static int ToInt(this string s, int d = 0)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch (Exception)
            {
                return d;
            }
        }

        /// <summary>
        /// 去html
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NoHtml(this string s)
        {
            return s;
        }

        /// <summary>
        /// 转html
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHtml(this string s)
        {
            return s;
        }
        /// <summary>
        /// 验证是否为手机号码
        /// </summary>
        /// <param name="s"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsPhone(this string s, string pattern)
        {
            return Regex.IsMatch(s, pattern);
        }

        public static List<string> StringToPhone(this string s)
        {
            var list = new List<string>();
            Regex r = new Regex(@"^1[3,5,8]{1}\d{9}$");
            Match m = r.Match(s);
            while (m.Success)
            {
                list.Add(m.Value);
                m = m.NextMatch();
            }
            return list;
        }

        public static string NullToEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s) ? "" : s;
        }

        public static int GetSemester(this DateTime time, bool isSemester)
        {
            var beginyear = time.Month < 9 ? time.Year - 1 : time.Year;
            var beginmonth = time.Month < 9 && time.Month > 1 ? 1 : 0;
            var nowyear = DateTime.Now.Month < 9 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
            if (beginyear == nowyear)
                return beginmonth;
            else if (beginyear - 1 == nowyear)
                return beginmonth + 2;
            return -1;
        }

        public static int GetSemester(this DateTime time)
        {
            return (time.Month < 9 ? time.Year - 1 : time.Year) * 10 + (time.Month < 9 && time.Month > 1 ? 1 : 0);
        }

        public static string GetSemester(this int time)
        {
            return time / 10 + "年" + (time % 10 == 1 ? "上半年" : "下半年");
        }

    }
}
