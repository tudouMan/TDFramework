using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TDFramework.Resource;

namespace TDFramework.Data
{
   
    public abstract class AbstractDBModel<T,P>where T:class,new() where P:AbstractEntity 
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new T();
                return mInstance;
            }
        }

        protected List<P> mDataList = new List<P>();

        protected Dictionary<int, P> mDataDic = new Dictionary<int, P>();

        public AbstractDBModel()
        {
            Load();
        }


        public abstract string FileName { get; }

        public abstract P ReadData(JsonData jsonData);

        private void Load()
        {

            Res.LoadAssetAsync<TextAsset>(string.Format("Data/{0}.json",FileName) ,fileStr=> 
            {
                string jsonData = Tool.StringEncryption.DecryptDES(fileStr.text);
                JsonData data = JsonMapper.ToObject(jsonData);
                foreach (JsonData item in data)
                {
                    P p = ReadData(item);
                    mDataList.Add(p);
                    mDataDic.Add(p.ID, p);
                }

                Res.ReleaseAsset<TextAsset>(fileStr);

            });
          
        }


        public List<P> GetList()
        {
            return mDataList;
        }

        public P Get(int id)
        {
            if (mDataDic.ContainsKey(id))
                return Copy<P>(mDataDic[id]);
            else
                return null;
        }

        public  P Copy<P>(P RealObject)where P:AbstractEntity
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
