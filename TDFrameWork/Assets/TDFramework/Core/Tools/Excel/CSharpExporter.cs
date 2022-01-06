using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDFramework.Extention;

namespace TDFramework
{
    public class CSharpExporter
    {
        private DataTable m_Table;
        private string m_DataName;
        public CSharpExporter(DataTable table,string dataName)
        {
            m_Table = table;
            m_DataName = dataName;
        }

        public void CreatCSharpScript()
        {
            if (m_Table == null || m_DataName.IsNull())
            {
                UnityEngine.Debug.LogError("Table is null or dataName is null");
                return;
            }

            FrameWorkPathConfig pathConfig=UnityEngine.Resources.Load<FrameWorkPathConfig>("PathConfig");

            StringBuilder content = new StringBuilder();

            #region Entity
            content.Append("using TDFramework.Table;");
            content.Append("\n");
            content.Append("\n");
            content.Append("namespace TDFramework");
            content.Append("\n");
            content.Append("{");
            content.Append("\n");
            content.Append("    [System.Serializable]");
            content.Append("\n");
            content.Append($"    public partial class {m_DataName.UppercaseFirst()}TableEntity : TableDataBase");
            content.Append("\n");
            content.Append("    {");
            content.Append("\n");
            for (int i = 0; i < m_Table.Columns.Count; i++)
            {
                string propertityName = m_Table.Columns[i].ToString();
                string propertityType = m_Table.Rows[0][i].ToString();
                content.Append($"        public {propertityType} {propertityName};");
                content.Append("\n");

            }
            content.Append("    }");
            content.Append("\n");
            content.Append("}");


            $"{pathConfig.m_CShapScriptsSavaPath}/Entity".CreateDirIfNotExists();
            $"{pathConfig.m_CShapScriptsSavaPath}/Table".CreateDirIfNotExists();
            string savaEntityPath = $"{pathConfig.m_CShapScriptsSavaPath}/Entity/{m_DataName.UppercaseFirst()}TableEntity.cs";
            if (System.IO.File.Exists(savaEntityPath))
                System.IO.File.Delete(savaEntityPath);
            System.IO.File.WriteAllText(savaEntityPath, content.ToString());

            content.Clear();
            content.Append("using TDFramework.Table;");
            content.Append("\n");
            content.Append("\n");
            content.Append("namespace TDFramework");
            content.Append("\n");
            content.Append("{");
            content.Append("\n");
            content.Append($"    public partial class {m_DataName.UppercaseFirst()}TableEntity : TableDataBase");
            content.Append("\n");
            content.Append("    {");
            content.Append("\n");
            content.Append("    }");
            content.Append("\n");
            content.Append("}");


            string savaDesignEntityPath = $"{pathConfig.m_CShapScriptsSavaPath}/Entity/{m_DataName.UppercaseFirst()}DesignTableEntity.cs";
            if (!System.IO.File.Exists(savaDesignEntityPath))
                System.IO.File.WriteAllText(savaDesignEntityPath, content.ToString());
            #endregion



            #region Table
            content.Clear();
            content.Append("using LitJson;");
            content.Append("\n");
            content.Append("using TDFramework.Table;");
            content.Append("\n");
            content.Append("using TDFramework.Extention;");
            content.Append("\n");
            content.Append("namespace TDFramework.Table");
            content.Append("\n");
           

            content.Append(" {");
            content.Append("\n");
            content.Append($"    public partial class {m_DataName.UppercaseFirst()}DataTable : AbstractDataTable<{m_DataName.UppercaseFirst()}DataTable, {m_DataName.UppercaseFirst()}TableEntity>");
            content.Append("\n");
            content.Append("    {");
            content.Append("\n");
            content.Append($"        public override string FileName =>GameEntry.Config.m_LoadType==LoadType.Addressable?\"{m_DataName}\":\"Data/{m_DataName}\";");
            content.Append("\n");
            content.Append($"        public override {m_DataName.UppercaseFirst()}TableEntity ReadData(JsonData data)");
            content.Append("\n");
            content.Append("        {");
            content.Append("\n");
            content.Append($"            {m_DataName.UppercaseFirst()}TableEntity entity = new {m_DataName.UppercaseFirst()}TableEntity();");
            content.Append("\n");

            for (int i = 0; i < m_Table.Columns.Count; i++)
            {
                string propertityName = m_Table.Columns[i].ToString();
                string propertityType = m_Table.Rows[0][i].ToString();

                string jsonToType = string.Empty;
                if (propertityType.Equals("int", StringComparison.CurrentCultureIgnoreCase))
                    jsonToType = ".ToInt()";
                else if (propertityType.Equals("float", StringComparison.CurrentCultureIgnoreCase))
                    jsonToType = ".ToFloat()";



                if (i == 0)
                    content.Append($"            entity.ID = data[\"{propertityName}\"].ToString().ToInt();");
                else
                    content.Append($"            entity.{propertityName} = data[\"{propertityName}\"].ToString(){jsonToType};");

                content.Append("\n");
            }


            content.Append("            return entity;");
            content.Append("\n");
            content.Append("        }");
            content.Append("\n");
            content.Append("    }");
            content.Append("\n");
            content.Append("}");

            string savatablePath = $"{pathConfig.m_CShapScriptsSavaPath}/Table/{m_DataName.UppercaseFirst()}DataTable.cs";
            if (System.IO.File.Exists(savatablePath))
                System.IO.File.Delete(savatablePath);

            System.IO.File.WriteAllText(savatablePath, content.ToString());



            content.Clear();
            content.Append("using LitJson;");
            content.Append("\n");
            content.Append("using TDFramework;");
            content.Append("\n");
            content.Append("using TDFramework.Extention;");
            content.Append("\n");
            content.Append("namespace TDFramework.Table");
            content.Append("\n");
            content.Append("{");
            content.Append("\n");
            content.Append($"    public partial class {m_DataName.UppercaseFirst()}DataTable");
            content.Append("\n");
            content.Append("    {");
            content.Append("\n");
            content.Append($"        public void ReadData({m_DataName.UppercaseFirst()}TableEntity entity,JsonData data)");
            content.Append("\n");
            content.Append("        {");
            content.Append("\n");
            content.Append("\n");
            content.Append("        }");
            content.Append("\n");
            content.Append("    }");
            content.Append("\n");
            content.Append("}");

            string savaDesigntablePath = $"{pathConfig.m_CShapScriptsSavaPath}/Table/{m_DataName.UppercaseFirst()}DataTable.Design.cs";
            if(!System.IO.File.Exists(savaDesigntablePath))
                System.IO.File.WriteAllText(savaDesigntablePath, content.ToString());

            #endregion
        }


    }

}
