using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TableManager.Instance.Init(LoadTableFunc);
    }

    string LoadTableFunc(string path)
    {
        string str = "";
        using (StreamReader sr = new StreamReader("Assets/Game/"+ path+".txt"))
        {
            str = sr.ReadToEnd();
        }
        return str;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var tb = Table_Shop.getById(1002);
            Debug.Log(tb.Name);
        }
    }
}
