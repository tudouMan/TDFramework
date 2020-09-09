using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDFramework.Cache;

[System.Serializable]

public class TempCache : LocalCache
{
    public string mName;
    public int mLevel;

    public override void FirstInitCache()
    {
        mName = "LQ";
        mLevel = 1;
        Debug.Log($"{this.GetType().Name}FirstInitCache");
    }

    public override void ReadCache()
    {
        Debug.Log($"{this.GetType().Name}ReadCache");
        Debug.Log($"Name:{mName},Level{mLevel}");
    }

    public override void WriteCache()
    {
        Debug.Log($"{this.GetType().Name}WriteCache");
    }
}
