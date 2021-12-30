﻿using TDFramework.EventSystem;
using TDFramework.Table;
using TDFramework.Pool;
using TDFramework.Audio;
using TDFramework.UI;
using TDFramework.Runtime;
using TDFramework.Cache;
using TDFramework.Localization;
using TDFramework.Resource;

namespace TDFramework
{
    [MonoSingletonPath("GameEntry")]
    public class GameEntry : MonoSingleton<GameEntry>
    {

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
          
        }

        public static DebugManager Debug { get; private set; }
        public static LoggerManager Logger { get; private set; }
        public static EventCenter Event { get;private set; }
        public static TimeManager Time { get; private set; }
        public static PoolManager Pool { get; private set; }

        public static FsmManager FSM { get; private set; }

        public static TableDataManager Table { get; private set; }

        public static SceneLoaderManager Scene { get; private set; }
        
        public static SoundManager Sound { get; private set; }

        public static UIManager UI { get; private set; }

        public static RuntimeMgr Runtime { get; private set; }

        public static LocalCacheMgr LocalCache { get; private set; }


        public static LocalizationMgr Localization { get; private set; }

        public static ResManager Res { get; private set; }

        public static FrameWorkPathConfig Config { get; set; }

        public void Init()
        {
            InitManager();
        }

        
        private void InitManager()
        {
            Config = UnityEngine.Resources.Load<FrameWorkPathConfig>("PathConfig");
            Logger = new LoggerManager();
            Debug = new DebugManager();
            Event = new EventCenter();
            Time = new TimeManager();
            Pool = new PoolManager();
            FSM = new FsmManager();
            Table = new TableDataManager();
            Scene = new SceneLoaderManager();
            Sound = new SoundManager();
            UI = UIManager.Instance;
        
            //IL

            Runtime = new RuntimeMgr();
             
            Localization = new LocalizationMgr();
            Res = new ResManager();
            LocalCache = new LocalCacheMgr();
            Res.Init();
            Logger.Init();
            Event.Init();
            Time.Init();
            Pool.Init();
            Table.Init();
            Scene.Init();
            Debug.Init();
            Sound.Init();
            FSM.Init();
            Runtime.Init();
            Localization.Init();
            LocalCache.Init();
            Debug.Log("GameEntry Init");
        }



        private void Update()
        {
            Time.Update();
            Scene.Update();
            FSM.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Logger.SyncLog();
        }

        protected override void OnApplicationQuit()
        {
            LocalCache.Save();
            base.OnApplicationQuit();
        }
    }

}
