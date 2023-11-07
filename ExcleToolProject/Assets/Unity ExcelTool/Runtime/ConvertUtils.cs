using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ConvertUtils
{
    private static readonly char SplitChar = ',';//·Ö¸ô·û£¨Ä¬ÈÏÊÇ,£©
    public static List<int> ToListInt(string vale)
    {
        List<int> ret = new List<int>();
        string[] splits = vale.Split(SplitChar);
        foreach (var v in splits)
            ret.Add(int.Parse(v));
        return ret;
    }

    public static int[] ToArrayInt(string vale)
    {
        string[] splits = vale.Split(SplitChar);
        int[] ret = new int[splits.Length];
        for (int i = 0; i < splits.Length; i++)
            ret[i] = int.Parse(splits[i]);
        return ret;
    }

    public static List<T> ConvertList<T>(string vec)
    {
        List<T> list = new List<T>();
        string strPos = vec;
        string[] resut = strPos.Split(SplitChar);

        try
        {
            if (resut != null && resut.Length > 0 && resut[0] != "")
            {
                for (int index = 0; index < resut.Length; index++)
                {
                    list.Add((T)Convert.ChangeType(resut[index], typeof(T)));
                }
            }
        }
        catch (System.Exception ex)
        {
            string info = string.Format("ConvertNumericList vec:{0} ex:{1} stacktrace:{2}",
              vec, ex.Message, ex.StackTrace);
            Debug.Log(info);

            list.Clear();
        }

        return list;
    }
    public static T[] ConvertArray<T>(string vec)
    {
        List<T> list = new List<T>();
        string strPos = vec;
        string[] resut = strPos.Split(SplitChar);

        try
        {
            if (resut != null && resut.Length > 0 && resut[0] != "")
            {
                for (int index = 0; index < resut.Length; index++)
                {
                    list.Add((T)Convert.ChangeType(resut[index], typeof(T)));
                }
            }
        }
        catch (System.Exception ex)
        {
            string info = string.Format("ConvertNumericList vec:{0} ex:{1} stacktrace:{2}",
              vec, ex.Message, ex.StackTrace);
            Debug.LogError(info);

            list.Clear();
        }

        return list.ToArray();
    }
    public static T[][] ConvertArrayList<T>(string vec)
    {
        List<T[]> list = new List<T[]>();
        string strPos = vec;
        string[] resut = strPos.Split(';');

        try
        {
            if (resut != null && resut.Length > 0 && resut[0] != "")
            {
                for (int index = 0; index < resut.Length; index++)
                {
                    list.Add(ConvertArray<T>(resut[index]));
                }
            }
        }
        catch (System.Exception ex)
        {
            string info = string.Format("ConvertNumericList vec:{0} ex:{1} stacktrace:{2}",
              vec, ex.Message, ex.StackTrace);
            Debug.LogError(info);

            list.Clear();
        }

        return list.ToArray();
    }
    public static string ToString<T>(this List<T> vec)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < vec.Count; i++)
        {
            if (i != 0)
                sb.Append(",");
            sb.Append(vec[i].ToString());
        }
        return sb.ToString();
    }
    public static string ToString<T>(this T[] vec)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < vec.Length; i++)
        {
            if (i != 0)
                sb.Append(",");
            sb.Append(vec[i].ToString());
        }
        return sb.ToString();
    }
}
