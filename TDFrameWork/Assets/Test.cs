using MySql.Data.MySqlClient;
using System;
using System.Data;
using TDFramework;
using TDFramework.Net.Mysql;
using UnityEngine;


public class Test : MonoBehaviour
{



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
        DataSet userDataTable=  DatabaseManager.Select("User");
        DataTableCollection users = userDataTable.Tables;
        for (int i = 0; i < users.Count; i++)
        {
            var name = users[i].Rows[0]["name"];
            var level= users[i].Rows[0]["level"];
            Debug.Log($"user name:{name} user level:{level}");
        }
       

    }
}


 



