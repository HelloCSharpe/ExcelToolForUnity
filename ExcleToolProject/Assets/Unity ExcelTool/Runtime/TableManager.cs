using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Func<string, string> loadFunction = null;

    public void Init(Func<string,string> LoadTableFunc)
    {
        loadFunction = LoadTableFunc;
        Table.Init();
    }

    public void LoadTable<T>(string tablePath) where T : ITable, new()
    {
        string content = loadFunction.Invoke(tablePath);
        if (content != null)
        {
            var lines = content.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                new T().Load(line.Split('\t'));
            }
        }
    }

}
