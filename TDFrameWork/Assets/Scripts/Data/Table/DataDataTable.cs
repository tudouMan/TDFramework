using LitJson;
using TDFramework.Table;
using TDFramework.Extention;
namespace TDFramework.Table
 {
    public partial class DataDataTable : AbstractDataTable<DataDataTable, DataTableEntity>
    {
        public override string FileName => "Data";
        public override DataTableEntity ReadData(JsonData data)
        {
            DataTableEntity entity = new DataTableEntity();
            entity.ID = data["id"].ToString().ToInt();
            entity.set = data["set"].ToString();
            return entity;
        }
    }
}