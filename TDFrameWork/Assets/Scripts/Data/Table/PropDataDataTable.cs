using LitJson;
using TDFramework.Table;
using TDFramework.Extention;
namespace TDFramework.Table
 {
    public partial class PropDataDataTable : AbstractDataTable<PropDataDataTable, PropDataTableEntity>
    {
        public override string FileName =>GameEntry.Config.m_LoadType==LoadType.Addressable?"PropData":"Data/PropData";
        public override PropDataTableEntity ReadData(JsonData data)
        {
            PropDataTableEntity entity = new PropDataTableEntity();
            entity.ID = data["id"].ToString().ToInt();
            entity.type = data["type"].ToString();
            entity.passlock = data["passlock"].ToString();
            return entity;
        }
    }
}