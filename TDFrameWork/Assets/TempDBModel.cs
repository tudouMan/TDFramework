using LitJson;
using TDFramework.Table;

namespace TDFramework.Table
{
    public partial class TempDataTable : AbstractDataTable<TempDataTable, TempTableEntity>
    {
        public override string FileName => "XXXX";

        public override TempTableEntity ReadData(JsonData data)
        {
            TempTableEntity entity = new TempTableEntity();
            
            return entity;
        }
    }





}
