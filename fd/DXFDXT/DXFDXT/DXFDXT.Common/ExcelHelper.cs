using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXFDXT.Common
{
    /// <summary>
    /// 操作excel
    /// </summary>
    public class ExcelHelper : IDisposable
    {
        private string fileName = null; //文件名
        private IWorkbook workbook = null;
        private FileStream fs = null;
        private bool disposed;

        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
            disposed = false;
        }

        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;

            fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="datatableSource">源DataTable</param>
        /// <param name="strFileName">后缀名字</param>
        public static byte[] Export(List<DataTable> datatableSource, string strFileName)
        {
            bool is2007up = strFileName.IndexOf(".xlsx") > 0 ? true : false;
            using (MemoryStream ms = Export(datatableSource, is2007up))
            {
                //using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                //{
                byte[] data = ms.ToArray();
                //    fs.Write(data, 0, data.Length);
                //    fs.Flush();
                //}
                return data;
            }
        }
        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="datatableSource">源DataTable</param>
        /// <returns>流信息</returns>
        public static MemoryStream Export(List<DataTable> datatableSources, bool is2007up)
        {
            IWorkbook workbook = new HSSFWorkbook(); ;
            if (is2007up)
            {
                workbook = new XSSFWorkbook(); ;
            }

            foreach (DataTable datatableSource in datatableSources)
            {
                ISheet sheet = CreateSheet(workbook, datatableSource.TableName);
                ICellStyle dateStyle = workbook.CreateCellStyle();
                IDataFormat format = workbook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                ////取得列宽
                int[] arrColWidth = new int[datatableSource.Columns.Count];
                foreach (DataColumn item in datatableSource.Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                }

                for (int i = 0; i < datatableSource.Rows.Count; i++)
                {
                    for (int j = 0; j < datatableSource.Columns.Count; j++)
                    {
                        int intTemp = Encoding.GetEncoding(936).GetBytes(datatableSource.Rows[i][j].ToString()).Length;
                        if (intTemp > arrColWidth[j])
                        {
                            arrColWidth[j] = intTemp;
                        }
                    }
                }

                int rowIndex = 0;
                foreach (DataRow row in datatableSource.Rows)
                {
                    //// 新建表，填充表头，填充列头，样式
                    if (rowIndex == 65535 || rowIndex == 0)
                    {
                        if (rowIndex != 0)
                        {
                            sheet = CreateSheet(workbook, datatableSource.TableName) as HSSFSheet;
                        }
                        //// 列头及样式
                        {
                            IRow headerRow = sheet.CreateRow(0);
                            ICellStyle headStyle = workbook.CreateCellStyle();
                            headStyle.Alignment = HorizontalAlignment.Center;
                            IFont font = workbook.CreateFont();
                            font.FontHeightInPoints = 10;
                            font.Boldweight = 700;
                            headStyle.IsLocked = true;
                            headStyle.SetFont(font);
                            foreach (DataColumn column in datatableSource.Columns)
                            {
                                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                ////设置列宽
                                int tempwidth = (arrColWidth[column.Ordinal] + 1) >= 255 ? 255 : (arrColWidth[column.Ordinal] + 1);
                                sheet.SetColumnWidth(column.Ordinal, tempwidth * 256);
                            }

                            sheet.CreateFreezePane(0, 1);
                        }

                        rowIndex = 1;
                    }

                    //// 填充内容
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in datatableSource.Columns)
                    {
                        ICell newCell = dataRow.CreateCell(column.Ordinal);
                        string drvalue = row[column].ToString();
                        switch (column.DataType.ToString())
                        {
                            case "System.String": ////字符串类型
                                newCell.SetCellValue(drvalue);
                                break;
                            case "System.DateTime": ////日期类型
                                DateTime dateV;
                                DateTime.TryParse(drvalue, out dateV);
                                newCell.SetCellValue(dateV);
                                newCell.CellStyle = dateStyle; ////格式化显示
                                break;
                            case "System.Boolean": ////布尔型
                                bool boolV = false;
                                bool.TryParse(drvalue, out boolV);
                                newCell.SetCellValue(boolV);
                                break;
                            case "System.Int16": ////整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                if (drvalue != string.Empty)
                                {
                                    int intV = 0;
                                    int.TryParse(drvalue, out intV);
                                    newCell.SetCellValue(intV);
                                }
                                else
                                {
                                    newCell.SetCellValue(string.Empty);
                                }
                                break;
                            case "System.Decimal": ////浮点型
                            case "System.Double":
                                if (drvalue != string.Empty)
                                {
                                    double doubV = 0;
                                    double.TryParse(drvalue, out doubV);
                                    newCell.SetCellValue(doubV);
                                }
                                else
                                {
                                    newCell.SetCellValue(string.Empty);
                                }
                                break;
                            case "System.DBNull": ////空值处理
                                newCell.SetCellValue(string.Empty);
                                break;
                            default:
                                newCell.SetCellValue(string.Empty);
                                break;
                        }
                    }

                    rowIndex++;
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        /// <summary>
        /// 创建Sheet
        /// </summary>
        /// <param name="workBook">工作目录</param>
        /// <param name="sheetName">Sheet名字</param>
        /// <returns></returns>
        private static ISheet CreateSheet(IWorkbook workBook, string sheetName)
        {
            ISheet sheet = workBook.CreateSheet(sheetName);
            return sheet;
        }
        public static DataSet ImportExcel(string filetype, Stream filestream)
        {
            bool is2007up = filetype.IndexOf(".xlsx") > 0 ? true : false;
            DataSet ds = new DataSet();
            IWorkbook hssfworkbook;
            //using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            //{
            if (is2007up)
            {
                hssfworkbook = new XSSFWorkbook(filestream);
            }
            else
            {
                hssfworkbook = new HSSFWorkbook(filestream);
            }
            //}
            int sheetCount = hssfworkbook.NumberOfSheets;
            for (int k = 0; k < sheetCount; k++)
            {
                DataTable dt = new DataTable();
                ISheet sheet = hssfworkbook.GetSheetAt(k);

                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                IRow headerRow = sheet.GetRow(0);
                if (headerRow == null)
                {
                    continue;
                }
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    dt.Columns.Add(cell.ToString());
                }

                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            if (j == 2)
                            {
                                if (row.Cells[j].ToString().IndexOf('月') > -1)
                                {
                                    dataRow[j] = row.Cells[j].DateCellValue;
                                }
                                else
                                {
                                    dataRow[j] = row.Cells[j].ToString();
                                }

                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }

                    dt.Rows.Add(dataRow);
                }
                dt.TableName = sheet.SheetName;
                ds.Tables.Add(dt);
            }
            return ds;
        }
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }
    }
}
