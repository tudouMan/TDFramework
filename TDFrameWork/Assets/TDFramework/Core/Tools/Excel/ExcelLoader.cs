using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TDFramework;
using TDFramework.Extention;
using ExcelDataReader;

namespace TDFramework
{
    public class ExcelLoader
    {
        private DataSet mData;
        public string DataName { get; set; }

        //TODO: add Sheet Struct Define

        public ExcelLoader(string filePath, int headerRow,string dataName)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using var reader = ExcelReaderFactory.CreateReader(stream);
                // Use the AsDataSet extension method
                // The result of each spreadsheet is in result.Tables
                var result = reader.AsDataSet(createDataSetReadConfig(headerRow));
                DataName = dataName;
                this.mData = result;
            }

            if (this.Sheets.Count < 1)
            {
                throw new Exception("Excel file is empty: " + filePath);
            }
        }

        public DataTableCollection Sheets
        {
            get
            {
                return this.mData.Tables;
            }
        }

        public DataSet MData { get => mData; set => mData = value; }

        private ExcelDataSetConfiguration createDataSetReadConfig(int headerRow)
        {
            var tableConfig = new ExcelDataTableConfiguration()
            {
                // Gets or sets a value indicating whether to use a row from the 
                // data as column names.
                UseHeaderRow = true,

                // Gets or sets a callback to determine whether to include the 
                // current row in the DataTable.
                //FilterRow = (rowReader) => {
                //    return rowReader.Depth > headerRow - 1;
                //},
            };

            return new ExcelDataSetConfiguration()
            {
                // Gets or sets a value indicating whether to set the DataColumn.DataType
                // property in a second pass.
                UseColumnDataType = true,

                // Gets or sets a callback to obtain configuration options for a DataTable. 
                ConfigureDataTable = (tableReader) => { return tableConfig; },
            };
        }
    }

}
