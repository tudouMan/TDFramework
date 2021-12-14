using MySql.Data.MySqlClient;
using System;
using System.Data;
using TDFramework;
using TDFramework.Net.Mysql;
using UnityEngine;


public class Test : MonoBehaviour
{


    public GameObject obj;
    public  string CONNECTIONSTRING = "datasource=127.0.0.1;port=3306;database=game;user=root;pwd=123456;";

    private void Awake()
    {
        GameEntry.Instance.Init();
    }




    [ContextMenu("Test")]
    private void TestData()
    {
        DatabaseManager data = new DatabaseManager();
        DatabaseManager.Init();

        bool isConnect = data.TestConnection();
        Debug.Log("Connect Is :" + isConnect);
        
    }
}


 



