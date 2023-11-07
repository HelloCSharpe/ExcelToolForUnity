using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Table_Plot : ITable
{
    public int Id;
    public int Script;
    public int Title;
    public string Synopsis;

    public override void Load(string[] columns)
    {
        try
        {
            if (!string.IsNullOrEmpty(columns[0])) Id = int.Parse(columns[0]);
            if (!string.IsNullOrEmpty(columns[1])) Script = int.Parse(columns[1]);
            if (!string.IsNullOrEmpty(columns[2])) Title = int.Parse(columns[2]);
            Synopsis = columns[3];
            ms_IdMap.Add(Id, this);
            OnLoaded();
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("load Table_Plot Fail,ID:{0},msg:{1},trace:{2}", Id, e.Message, e.StackTrace);
        }
    }

    private static Dictionary<int, Table_Plot> ms_IdMap = new Dictionary<int, Table_Plot>();
    public static Table_Plot getById(int id)
    {
        if (ms_IdMap.ContainsKey(id))
        {
            return ms_IdMap[id];
        }
        return null;
    }
}
