using Excel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TDFramework;
using TDFramework.Extention;
public class ExcelHandle
{
    private string m_DataPath;
   public ExcelHandle(string dataPath)
    {
        m_DataPath = dataPath;
        LoadData();
    }

    private void LoadData()
    {
        if (m_DataPath.IsNull())
            return;
        // 获得表数据
        int columnNum = 0, rowNum = 0;
        DataRowCollection collect = ReadExcel(m_DataPath, ref columnNum, ref rowNum);

        //根据excel的定义，第四行开始才是数据
        for (int i = 1; i < rowNum; i++)
        {
            for (int j = 1; j < columnNum; j++)
            {
                string data=collect[i][j].ToString();

            }
        }
    }


    /// <summary>
    /// 读取excel文件内容
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="columnNum">行数</param>
    /// <param name="rowNum">列数</param>
    /// <returns></returns>
    static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //Tables[0] 下标0表示excel文件中第一张表的数据
        columnNum = result.Tables[0].Columns.Count;
        rowNum = result.Tables[0].Rows.Count;
        return result.Tables[0].Rows;
    }


}
