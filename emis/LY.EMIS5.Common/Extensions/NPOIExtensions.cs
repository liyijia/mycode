using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace LY.EMIS5.Common.Extensions
{
    /// <summary>
    /// NPOI的扩展方法
    /// </summary>
    public static class NPOIExtensions
    {
        #region IWorkbook的扩展方法

        #region 创建单元格样式

        /// <summary>
        /// 在Workbook中创建一个单元格样式并为其设置字体
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="font">字体</param>
        /// <returns>单元格样式</returns>
        public static ICellStyle CreateCellStyle(this IWorkbook hssfWorkbook, NPOI.SS.UserModel.IFont font)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(font != null);

            ICellStyle cellStyle = hssfWorkbook.CreateCellStyle();
            cellStyle.SetFont(font);

            return cellStyle;
        }

        /// <summary>
        /// 在Workbook中创建一个单元格样式并为其设置字体及设置水平和垂直对齐方式
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="font">字体</param>
        /// <param name="horizontalAlignment">水平对齐方式</param>
        /// <param name="verticalAlignment">垂直对齐方式</param>
        /// <returns>单元格样式</returns>
        public static ICellStyle CreateCellStyle(this IWorkbook hssfWorkbook, NPOI.SS.UserModel.IFont font, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(font != null);

            ICellStyle cellStyle = hssfWorkbook.CreateCellStyle(font);
            cellStyle.Alignment = horizontalAlignment;
            cellStyle.VerticalAlignment = verticalAlignment;

            return cellStyle;
        }

        /// <summary>
        /// 在Workbook中创建一个单元格样式并为其四周边框样式
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="borderTop">上边框样式</param>
        /// <param name="borderBottom">下边框样式</param>
        /// <param name="borderLeft">左边框样式</param>
        /// <param name="borderRight">右边框样式</param>
        /// <returns>单元格样式</returns>
        public static ICellStyle CreateCellStyle(this IWorkbook hssfWorkbook, BorderStyle borderTop, BorderStyle borderBottom, BorderStyle borderLeft, BorderStyle borderRight)
        {
            Contract.Requires(hssfWorkbook != null);

            ICellStyle cellStyle = hssfWorkbook.CreateCellStyle();
            cellStyle.BorderTop = borderTop;
            cellStyle.BorderBottom = borderBottom;
            cellStyle.BorderLeft = borderLeft;
            cellStyle.BorderRight = borderRight;

            return cellStyle;
        }

        /// <summary>
        /// 在Workbook中创建一个单元格样式为其设置字体和边框样式
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="borderTop">上边框样式</param>
        /// <param name="borderBottom">下边框样式</param>
        /// <param name="borderLeft">左边框样式</param>
        /// <param name="borderRight">右边框样式</param>
        /// <param name="font">字体</param>
        /// <returns>单元格样式</returns>
        public static ICellStyle CreateCellStyle(this IWorkbook hssfWorkbook, BorderStyle borderTop, BorderStyle borderBottom, BorderStyle borderLeft, BorderStyle borderRight, NPOI.SS.UserModel.IFont font)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(font != null);

            ICellStyle cellStyle = hssfWorkbook.CreateCellStyle(borderTop, borderBottom, borderLeft, borderRight);
            cellStyle.SetFont(font);

            return cellStyle;
        }

        /// <summary>
        /// 在Workbook中创建一个单元格样式为其设置字体和边框样式以及设置水平和垂直对齐方式
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="borderTop">上边框样式</param>
        /// <param name="borderBottom">下边框样式</param>
        /// <param name="borderLeft">左边框样式</param>
        /// <param name="borderRight">右边框样式</param>
        /// <param name="font">字体</param>
        /// <param name="horizontalAlignment">水平对齐方式</param>
        /// <param name="verticalAlignment">垂直对齐方式</param>
        /// <returns>单元格样式</returns>
        public static ICellStyle CreateCellStyle(this IWorkbook hssfWorkbook, BorderStyle borderTop, BorderStyle borderBottom, BorderStyle borderLeft, BorderStyle borderRight, NPOI.SS.UserModel.IFont font, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(font != null);

            ICellStyle cellStyle = hssfWorkbook.CreateCellStyle(borderTop, borderBottom, borderLeft, borderRight, font);
            cellStyle.Alignment = horizontalAlignment;
            cellStyle.VerticalAlignment = verticalAlignment;

            return cellStyle;
        }

        #endregion

        #region 创建字体

        /// <summary>
        /// 在Workbook中创建指定字体并设置其大小
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="fontName">字体</param>
        /// <param name="fontHeightInPoints">字体大小 示例:如要设置字号为12,则代码为:font.FontHeightInPoints = 12;</param>
        /// <returns>字体</returns>
        public static IFont CreateFont(this IWorkbook hssfWorkbook, string fontName, short fontHeightInPoints)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(fontName));
            Contract.Requires(fontHeightInPoints > 0);

            IFont font = hssfWorkbook.CreateFont();
            font.FontName = fontName;
            font.FontHeightInPoints = fontHeightInPoints;

            return font;
        }

        /// <summary>
        /// 在Workbook中创建指定字体并设置其大小和粗细
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="fontName">字体</param>
        /// <param name="fontHeightInPoints">字体大小 示例:如要设置字号为12,则代码为:font.FontHeightInPoints = 12;</param>
        /// <param name="boldweight">字体粗细</param>
        /// <returns>字体</returns>
        public static IFont CreateFont(this IWorkbook hssfWorkbook, string fontName, short fontHeightInPoints, short boldweight)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(fontName));
            Contract.Requires(fontHeightInPoints > 0);
            Contract.Requires(boldweight > 0);

            IFont font = hssfWorkbook.CreateFont(fontName, fontHeightInPoints);
            font.Boldweight = boldweight;

            return font;
        }

        #endregion

        #region 保存文件

        /// <summary>
        /// 保存工作簿
        /// 备注:先获取工作簿的第一个工作表名,并以此为文件名保存文件至程序的根目录下
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        public static void SaveFile(this IWorkbook hssfWorkbook)
        {
            Contract.Requires(hssfWorkbook != null);

            string fileName = string.Empty;
            var sheet = hssfWorkbook.GetSheetAt(0);
            if (sheet != null && !string.IsNullOrWhiteSpace(sheet.SheetName))
            {
                fileName = string.Format("{0}.xls", sheet.SheetName);
            }
            else
            {
                fileName = "New Excel Workbook.xls";
            }

            hssfWorkbook.SaveFile(fileName);
        }

        /// <summary>
        /// 保存工作簿至指定路径
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="fileName">文件路径和文件名 示例:C:\Users\Administrator\Desktop\Example.xls</param>
        public static void SaveFile(this IWorkbook hssfWorkbook, string fileName)
        {
            Contract.Requires(hssfWorkbook != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(fileName));

            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    hssfWorkbook.Write(file);
                    file.Close();
                }
            }
            catch (IOException)
            {
                try
                {
                    string tmpFileName = string.Empty;
                    if (fileName.ToLower().EndsWith(".xls"))
                    {
                        tmpFileName = fileName.Substring(0, fileName.Length - 4);
                        if (string.IsNullOrWhiteSpace(tmpFileName))
                        {
                            tmpFileName = "New Excel Workbook";
                        }

                        tmpFileName = string.Format("{0}_{1}{2}.xls", tmpFileName, DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒fff毫秒"), Guid.NewGuid().ToString("N").Substring(0, 3));
                    }
                    else
                    {
                        if (fileName.ToLower().EndsWith(".xlsx"))
                        {
                            tmpFileName = fileName.Substring(0, fileName.Length - 5);
                            if (string.IsNullOrWhiteSpace(tmpFileName))
                            {
                                tmpFileName = "New Excel Workbook";
                            }

                            tmpFileName = string.Format("{0}_{1}{2}.xlsx", tmpFileName, DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒fff毫秒"), Guid.NewGuid().ToString("N").Substring(0, 3));
                        }
                        else
                        {
                            tmpFileName = string.Format("New Excel Workbook_{0}{1}.xls", DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒fff毫秒"), Guid.NewGuid().ToString("N").Substring(0, 3));
                        }
                    }

                    using (FileStream file = new FileStream(tmpFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        hssfWorkbook.Write(file);
                        file.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new IOException(ex.Message);
                }
            }
        }

        #endregion

        #region 将工作簿数据转化为字节数组

        /// <summary>
        /// 把工作簿写入进内存流中,获取在内存流中的字节数组
        /// </summary>
        /// <param name="hssfWorkbook">工作簿</param>
        /// <param name="isBuffer">是否获取内存流缓冲区的数据
        /// <para>true:调用MemoryStream中的GetBuffer()方法,但是可能包含缓冲区未使用的字节,特点是速度快</para> 
        /// <para>false:调用MemoryStream中的ToArray()方法,获取的内容的副本,会省略 MemoryStream 中未使用的字节,特点是速度稍微慢点 消耗内存多一些,但返回的字节数组要短一些</para>
        /// </param>
        /// <returns></returns>
        public static byte[] ToBytes(this IWorkbook hssfWorkbook, bool isBuffer)
        {
            Contract.Requires(hssfWorkbook != null);

            using (MemoryStream ms = new MemoryStream())    //内存文件流(应该可以写成普通的文件流)
            {
                hssfWorkbook.Write(ms);                     //把文件写入到内存流里面

                if (isBuffer)
                {
                    return ms.GetBuffer();                  //返回缓冲区的数据
                }
                else
                {
                    return ms.ToArray();                    //返回内容字节数组
                }
            }
        }

        #endregion

        #endregion

        #region ISheet的扩展方法

        /// <summary>
        /// 获取IRow,没有则创建
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="rownum"></param>
        /// <returns></returns>
        public static IRow GetOrCreateRow(this ISheet sheet, int rownum)
        {
            Contract.Requires(sheet != null);
            Contract.Requires(rownum >= 0);

            if (sheet.GetRow(rownum) == null)
            {
                return sheet.CreateRow(rownum);
            }
            else
            {
                return sheet.GetRow(rownum);
            }
        }

        /// <summary>
        /// 合并单元格并设置单元格样式
        /// </summary>
        /// <param name="sheet">工作表</param>
        /// <param name="region">单元格地址范围</param>
        /// <param name="cellStyle">单元格样式</param>
        /// <returns></returns>
        public static int AddMergedRegion(this ISheet sheet, CellRangeAddress region, ICellStyle cellStyle)
        {
            Contract.Requires(sheet != null);
            Contract.Requires(region != null);
            Contract.Requires(cellStyle != null);
            IRow row;
            ICell cell;

            int result = sheet.AddMergedRegion(region);

            for (int i = region.FirstRow; i <= region.LastRow; i++)
            {
                row = sheet.GetOrCreateRow(i);
                for (int j = region.FirstColumn; j <= region.LastColumn; j++)
                {
                    cell = row.GetOrCreateCell(j);
                    cell.CellStyle = cellStyle;
                }
            }

            return result;
        }

        #endregion

        #region IRow的扩展方法

        /// <summary>
        /// 获取ICell,没有则创建
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static ICell GetOrCreateCell(this IRow row, int column)
        {
            Contract.Requires(row != null);
            Contract.Requires(column >= 0);

            if (row.GetCell(column, MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
            {
                return row.CreateCell(column);
            }
            else
            {
                return row.GetCell(column);
            }
        }

        #endregion
    }
}