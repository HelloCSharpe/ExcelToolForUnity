using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Table_Shop : ITable
{
    public int Id;
    public string Name;
    public int Tap;
    public int Type;
    public string[] Descs;
    public string[][] Currency;
    public int[] RandCounts;
    public int[][] AutoFresh;
    public float Ticket;
    public float[] Setting;
    public float[][] TapIcon;

    public override void Load(string[] columns)
    {
        try
        {
            if (!string.IsNullOrEmpty(columns[0])) Id = int.Parse(columns[0]);
            Name = columns[1];
            if (!string.IsNullOrEmpty(columns[2])) Tap = int.Parse(columns[2]);
            if (!string.IsNullOrEmpty(columns[3])) Type = int.Parse(columns[3]);
            Descs = ConvertUtils.ConvertArray<string>(columns[4]);
            Currency = ConvertUtils.ConvertArrayList<string>(columns[5]);
            RandCounts = ConvertUtils.ConvertArray<int>(columns[6]);
            AutoFresh = ConvertUtils.ConvertArrayList<int>(columns[7]);
            if (!string.IsNullOrEmpty(columns[8])) Ticket = float.Parse(columns[8]);
            Setting = ConvertUtils.ConvertArray<float>(columns[9]);
            TapIcon = ConvertUtils.ConvertArrayList<float>(columns[10]);
            if (!ms_TapMap.ContainsKey(Tap)) { ms_TapMap.Add(Tap, new List<Table_Shop>()); }
            ms_TapMap[Tap].Add(this);
            if (!ms_TypeMap.ContainsKey(Type)) { ms_TypeMap.Add(Type, new List<Table_Shop>()); }
            ms_TypeMap[Type].Add(this);
            ms_IdMap.Add(Id, this);
            OnLoaded();
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("load Table_Shop Fail,ID:{0},msg:{1},trace:{2}", Id, e.Message, e.StackTrace);
        }
    }

    private static Dictionary<int, Table_Shop> ms_IdMap = new Dictionary<int, Table_Shop>();
    public static Table_Shop getById(int id)
    {
        if (ms_IdMap.ContainsKey(id))
        {
            return ms_IdMap[id];
        }
        return null;
    }
    private static Dictionary<int, List<Table_Shop>> ms_TapMap = new Dictionary<int, List<Table_Shop>>();
    public static List<Table_Shop> getListByTap(int tap)
    {
        if (ms_TapMap.ContainsKey(tap))
        {
            return ms_TapMap[tap];
        }
        return null;
    }
    private static Dictionary<int, List<Table_Shop>> ms_TypeMap = new Dictionary<int, List<Table_Shop>>();
    public static List<Table_Shop> getListByType(int type)
    {
        if (ms_TypeMap.ContainsKey(type))
        {
            return ms_TypeMap[type];
        }
        return null;
    }
}
