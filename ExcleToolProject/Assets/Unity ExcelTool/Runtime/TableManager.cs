using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private static TableManager _instance;
    public static TableManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = GameObject.Find("TableManager");
                if (go == null)
                    go = new GameObject("TableManager");
                _instance = go.GetComponent<TableManager>();
                if (_instance == null)
                    _instance = go.AddComponent<TableManager>();
            }
            return _instance;
        }
    }

    public void Init()
    {
        Table.Init();
    }

    public void LoadTable<T>(string tablePath) where T: ITable
    {

    }

}
