using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDFramework
{
    public enum DebugColoyType
    {
        Red,
        Green,
        Yello,
        White,
    }

    public  class DebugManager:ManagerBase,IDisposable
    {
        private  Dictionary<DebugColoyType, string> m_ColorTypeDic=new Dictionary<DebugColoyType, string> 
        {
                { DebugColoyType.Red,   "FF0000" },
                { DebugColoyType.Green, "26FF00" },
                { DebugColoyType.Yello, "FFFF00" },
                { DebugColoyType.White, "FFFFFF" },
        };

        private  bool m_LogEnabled;
        public  bool LogEnabled
        {
            get { return m_LogEnabled; }
            set
            {
                m_LogEnabled = value;
                UnityEngine.Debug.unityLogger.logEnabled = m_LogEnabled;
               
            }
        }

        internal override void Init()
        {
            m_ColorTypeDic = new Dictionary<DebugColoyType, string>
             {
                { DebugColoyType.Red,   "FF0000" },
                { DebugColoyType.Green, "26FF00" },
                { DebugColoyType.Yello, "FFFF00" },
                { DebugColoyType.White, "FFFFFF" },
             };
        }


        public void Dispose()
        {
            
        }

        public  void Log(object message, DebugColoyType colorType = DebugColoyType.White, int size = 11)
        {
            UnityEngine.Debug.Log($"<size={size}><color=#{m_ColorTypeDic[colorType]}>{message}</color></size>");
        }

        public  void LogError(object message, int size = 10)
        {
            UnityEngine.Debug.LogError($"<size={size}>{message}</size>");
        }

        public  void LogWarning(object message, int size = 10)
        {
            UnityEngine.Debug.LogError($"<size={size}>{message}</size>");
        }

       
    }
}


