using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TDFramework
{
    public class JsonExporter
    {
        /// <summary>
        /// 将DataTable对象，转换成JSON string，并保存到文件中
        /// </summary>

        string m_Context = "";
        int m_HeaderRows = 0;

        public string Context
        {
            get
            {
                return m_Context;
            }
        }

        /// <summary>
        /// 构造函数：完成内部数据创建
        /// </summary>
        /// <param name="excel">ExcelLoader Object</param>
        public JsonExporter(ExcelLoader excel, bool exportArray = true, int headerRows = 3, bool cellJson = true)
        {
            m_HeaderRows = headerRows - 1;
            List<DataTable> validSheets = new List<DataTable>();
            for (int i = 0; i < excel.Sheets.Count; i++)
            {
                DataTable sheet = excel.Sheets[i];
                if (sheet.Columns.Count > 0 && sheet.Rows.Count > 0)
                    validSheets.Add(sheet);
            }

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            if (validSheets.Count > 0)
            {   // single sheet

                //-- convert to object
                object sheetValue = convertSheet(validSheets[0], exportArray, cellJson);

                //-- convert to json string
                m_Context = JsonConvert.SerializeObject(sheetValue,jsonSettings);
                Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
                m_Context = reg.Replace(m_Context, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });

                UnityEngine.Debug.Log(string.Format("表格名: {0} \nTableName: {1}  \n解析数据为: {2})", excel.DataName, excel.Sheets[0].TableName, m_Context));
            }

        }

        private object convertSheet(DataTable sheet, bool exportArray, bool cellJson = true)
        {
            if (exportArray)
                return convertSheetToArray(sheet, cellJson);
            else
                return convertSheetToDict(sheet, cellJson);
        }

        private object convertSheetToArray(DataTable sheet, bool cellJson)
        {
            List<object> values = new List<object>();

            int firstDataRow = m_HeaderRows;
            for (int i = firstDataRow; i < sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];

                values.Add(
                    convertRowToDict(sheet, row, firstDataRow, cellJson)
                    );
            }

            return values;
        }

        /// <summary>
        /// 以第一列为ID，转换成ID->Object的字典对象
        /// </summary>
        private object convertSheetToDict(DataTable sheet, bool cellJson)
        {
            Dictionary<string, object> importData =
                new Dictionary<string, object>();

            int firstDataRow = m_HeaderRows;
            for (int i = firstDataRow; i < sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];
                string ID = row[sheet.Columns[0]].ToString();
                if (ID.Length <= 0)
                    ID = string.Format("row_{0}", i);

                var rowObject = convertRowToDict(sheet, row, firstDataRow, cellJson);
                // 多余的字段
                // rowObject[ID] = ID;
                importData[ID] = rowObject;
            }

            return importData;
        }

        /// <summary>
        /// 把一行数据转换成一个对象，每一列是一个属性
        /// </summary>
        private Dictionary<string, object> convertRowToDict(DataTable sheet, DataRow row, int firstDataRow, bool cellJson)
        {
            var rowData = new Dictionary<string, object>();
            int col = 0;
            foreach (DataColumn column in sheet.Columns)
            {


                object value = row[column];

                // 尝试将单元格字符串转换成 Json Array 或者 Json Object
                if (cellJson)
                {
                    string cellText = value.ToString().Trim();
                    if (cellText.StartsWith("[") || cellText.StartsWith("{"))
                    {
                        try
                        {
                            object cellJsonObj = JsonConvert.DeserializeObject(cellText);
                            if (cellJsonObj != null)
                                value = cellJsonObj;
                        }
                        catch (Exception exp)
                        {
                        }
                    }
                }

                if (value.GetType() == typeof(System.DBNull))
                {
                    value = getColumnDefault(sheet, column, firstDataRow);
                }
                else if (value.GetType() == typeof(double))
                { // 去掉数值字段的“.0”
                    double num = (double)value;
                    if ((int)num == num)
                        value = (int)num;
                }

                string fieldName = column.ToString();
                if (string.IsNullOrEmpty(fieldName))
                    fieldName = string.Format("col_{0}", col);

                rowData[fieldName] = value;
                col++;
            }

            return rowData;
        }

        /// <summary>
        /// 对于表格中的空值，找到一列中的非空值，并构造一个同类型的默认值
        /// </summary>
        private object getColumnDefault(DataTable sheet, DataColumn column, int firstDataRow)
        {
            for (int i = firstDataRow; i < sheet.Rows.Count; i++)
            {
                object value = sheet.Rows[i][column];
                Type valueType = value.GetType();
                if (valueType != typeof(System.DBNull))
                {
                    if (valueType.IsValueType)
                    {
                       return Activator.CreateInstance(valueType);
                    }
                       
                    break;
                }
            }
            return "";
        }

        /// <summary>
        /// 将内部数据转换成Json文本，并保存至文件
        /// </summary>
        /// <param name="jsonPath">输出文件路径</param>
        public void SaveToFile(string filePath, Encoding encoding)
        {

            if (File.Exists(filePath))
                File.Delete(filePath);
            //-- 保存文件
            using (FileStream file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                {
                    m_Context = TDFramework.Tool.StringEncryption.EncryptDES(m_Context);
                    writer.Write(m_Context);
                }
                    
            }
        }

       
    }
}
