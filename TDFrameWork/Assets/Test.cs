using UnityEngine;
using System.Collections.Generic;
using TDFramework;
public class Test : MonoBehaviour
{

    private List<GameObject> m_ObjList = new List<GameObject>();
    private void Start()
    {
        GameEntry.Instance.Init();
    }
    private void OnGUI()
    {
        if(GUI.Button(new Rect(100, 100, 100, 100), "set"))
        {
            GameEntry.Pool.PopPrefab("PlayerCube", p => 
            { 
                p.transform.localPosition = new Vector3(UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10));  
                Debug.Log(p.name);
                m_ObjList.Add(p);
            });
        }

        if(GUI.Button(new Rect(100, 500, 100, 100), "out"))
        {
            foreach (var item in m_ObjList)
            {
                GameEntry.Pool.PushPrefab(item);
            }
        }
    }
}


 



