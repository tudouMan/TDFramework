using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TDFramework.Resource;

namespace TDFramework.Table
{
    public abstract class AbstractTable<T,P>where T:class,new() where P:TableBase 
    {
        protected List<P> mDataList = new List<P>();
        protected Dictionary<int, P> mDataDic = new Dictionary<int, P>();

        public AbstractTable()
        {
            Load();
        }


        public abstract string FileName { get; }

        public abstract P ReadData(JsonData jsonData);

        private void Load()
        {

            GameEntry.Res.LoadAssetAsync<TextAsset>(string.Format("Data/{0}.json",FileName) ,fileStr=> 
            {
                string jsonData = Tool.StringEncryption.DecryptDES(fileStr.text);
                JsonData data = JsonMapper.ToObject(jsonData);
                foreach (JsonData item in data)
                {
                    P p = ReadData(item);
                    mDataList.Add(p);
                    mDataDic.Add(p.ID, p);
                }

                GameEntry.Res.ReleaseAsset<TextAsset>(fileStr);

            });
          
        }


        public List<P> GetList()
        {
            return mDataList;
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="isCopy">是否序列化出来一份新的</param>
        /// <returns></returns>
        public P Get(int id,bool isCopy=true)
        {
            if (mDataDic.ContainsKey(id))
                return Copy<P>(mDataDic[id]);
            else
                return null;
        }


      

        public  P Copy<P>(P RealObject)where P:TableBase
        {
            using (Stream objectStream = new MemoryStream())
            {
                //利用 System.Runtime.Serialization序列化与反序列化完成引用对象的复制     
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, RealObject);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (P)formatter.Deserialize(objectStream);
            }
        }

       
    }
}
