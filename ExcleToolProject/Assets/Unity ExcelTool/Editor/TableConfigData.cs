using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;
using System.Text;

public class TableConfigData
{
    public string tableName;//Plot

    public List<TableFieldData> fieldDatas = new List<TableFieldData>();

    public void AddFieldData(int col,string fieldName,string typeName,string cell3Value)
    {
        if (TableFieldData.FieldVisible(fieldName))
        {
            TableFieldData tableFieldData = new TableFieldData();
            tableFieldData.fieldName = fieldName;
            tableFieldData.typeName = typeName;
            tableFieldData.className = className;
            tableFieldData.index = col - 1;
            if (cell3Value != null)
            {
                tableFieldData.isKey = cell3Value.Equals("key");
                tableFieldData.isIndex = cell3Value.Equals("index");
            }
            this.fieldDatas.Add(tableFieldData);
        }
    }
    public string className => $"Table_{tableName}";//Table_Plot

    public TableFieldData KeyField
    {
        get
        {
            foreach (var field in fieldDatas)
            {
                if (field.isKey)
                {
                    return field;
                }
            }
            return null;
        }
    }
    private List<TableFieldData> _IndexFields;
    public List<TableFieldData> IndexFields
    {
        get
        {
            if (_IndexFields == null)
            {
                _IndexFields = new List<TableFieldData>();
                foreach (var field in fieldDatas)
                {
                    if (field.isIndex)
                    {
                        _IndexFields.Add(field);
                    }
                }
            }
            return _IndexFields;
        }
    }
    public class TableFieldData
    {
        public int index;
        public string fieldName;
        public string typeName;
        public bool isKey;
        public bool isIndex;
        public string className;

        public bool isVisible
        {
            get
            {
                return FieldVisible(fieldName);
            }
        }

        public static bool FieldVisible(string _fieldName)
        {
            if (string.IsNullOrEmpty(_fieldName))
            {
                return false;
            }
            return char.IsUpper(_fieldName[0]);
        }

        public string fieldNameLower => fieldName.ToLower();

        public string getTypeFieldString()
        {
            return $"    public {typeName} {fieldName};";
        }
        public string getTypeString()
        {
            switch (typeName)
            {
                case "int":
                    return $"            if (!string.IsNullOrEmpty(columns[{index}])) {fieldName} = int.Parse(columns[{index}]);";
                case "int[]":
                    return $"            {fieldName} = ConvertUtils.ConvertArray<int>(columns[{index}]);";
                case "int[][]":
                    return $"            {fieldName} = ConvertUtils.ConvertArrayList<int>(columns[{index}]);";

                case "bool":
                    return $"            if (!string.IsNullOrEmpty(columns[{index}])) {fieldName} = int.Parse(columns[{index}]) == 1;";
                //case "bool[]":
                //    return $"            {fieldName} = ConvertUtils.ConvertArray<bool>(columns[{index}]);";
                //case "bool[][]":
                //    return $"            {fieldName} = ConvertUtils.ConvertArrayList<bool>(columns[{index}]);";

                case "string":
                    return $"            {fieldName} = columns[{index}];";
                case "string[]":
                    return $"            {fieldName} = ConvertUtils.ConvertArray<string>(columns[{index}]);";
                case "string[][]":
                    return $"            {fieldName} = ConvertUtils.ConvertArrayList<string>(columns[{index}]);";

                case "float":
                    return $"            if (!string.IsNullOrEmpty(columns[{index}])) {fieldName} = float.Parse(columns[{index}]);";
                case "float[]":
                    return $"            {fieldName} = ConvertUtils.ConvertArray<float>(columns[{index}]);";
                case "float[][]":
                    return $"            {fieldName} = ConvertUtils.ConvertArrayList<float>(columns[{index}]);";
                default:
                    throw new Exception($"Error Type [{typeName}] {fieldName}");
            }
            return "";
        }


        public string GetIndexListAddString()
        {
            return $"            if (!ms_{fieldName}Map.ContainsKey({fieldName})) {{ ms_{fieldName}Map.Add({fieldName}, new List<{className}>()); }}";
        }

        public string GetIndexListAddString2()
        {
            return $"            ms_{fieldName}Map[{fieldName}].Add(this);";
        }
        public string GetKeyAddString()
        {
            return $"            ms_{fieldName}Map.Add({fieldName}, this);";
        }
    }

    public string GetInitString()
    {
        string _path = string.Format(ExcelToolSettings.Instance.loadPathFormation, tableName);
        return $"        TableManager.Instance.LoadTable<{className}>(\"{_path}\");";
    }

    public string GetTableSavePath()
    {
        var prev = ExcelToolSettings.Instance.scriptGenerateDirectory;
        return $"{prev}/{className}.cs";
    }

    public string GetTableContent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("using System;");
        stringBuilder.AppendLine("using System.Collections.Generic;");
        stringBuilder.AppendLine("using UnityEngine;");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"public partial class {className} : ITable");
        stringBuilder.AppendLine("{");
        for (int i = 0; i < fieldDatas.Count; i++)
        {
            stringBuilder.AppendLine(fieldDatas[i].getTypeFieldString());
        }
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("    public override void Load(string[] columns)");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine("        try");
        stringBuilder.AppendLine("        {");
        for (int i = 0; i < fieldDatas.Count; i++)
        {
            stringBuilder.AppendLine(fieldDatas[i].getTypeString());
        }
        var keyField = KeyField;
        var idxFields = IndexFields;
        for (int i = 0; i < idxFields.Count; i++)
        {
            stringBuilder.AppendLine(idxFields[i].GetIndexListAddString());
            stringBuilder.AppendLine(idxFields[i].GetIndexListAddString2());
        }
        stringBuilder.AppendLine(keyField.GetKeyAddString());
        stringBuilder.AppendLine("            OnLoaded();");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("        catch (Exception e)");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine("            Debug.LogErrorFormat(\"load " + className + " Fail,ID:{0},msg:{1},trace:{2}\", Id, e.Message, e.StackTrace);");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("    }");
        stringBuilder.AppendLine();

        //keyfield
        stringBuilder.AppendLine($"    private static Dictionary<{keyField.typeName}, {className}> ms_{keyField.fieldName}Map = new Dictionary<{keyField.typeName}, {className}>();");
        stringBuilder.AppendLine($"    public static {className} getBy{keyField.fieldName}({keyField.typeName} {keyField.fieldNameLower})");
        stringBuilder.AppendLine("    {");
        stringBuilder.AppendLine($"        if (ms_{keyField.fieldName}Map.ContainsKey({keyField.fieldNameLower}))");
        stringBuilder.AppendLine("        {");
        stringBuilder.AppendLine($"            return ms_{keyField.fieldName}Map[{keyField.fieldNameLower}];");
        stringBuilder.AppendLine("        }");
        stringBuilder.AppendLine("        return null;");
        stringBuilder.AppendLine("    }");

        //fields
        for (int i = 0; i < idxFields.Count; i++)
        {
            var idField = idxFields[i];
            stringBuilder.AppendLine($"    private static Dictionary<{idField.typeName}, List<{className}>> ms_{idField.fieldName}Map = new Dictionary<{idField.typeName}, List<{className}>>();");
            stringBuilder.AppendLine($"    public static List<{className}> getListBy{idField.fieldName}({idField.typeName} {idField.fieldNameLower})");
            stringBuilder.AppendLine("    {");
            stringBuilder.AppendLine($"        if (ms_{idField.fieldName}Map.ContainsKey({idField.fieldNameLower}))");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine($"            return ms_{idField.fieldName}Map[{idField.fieldNameLower}];");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("        return null;");
            stringBuilder.AppendLine("    }");
        }

        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}

/*
 using System;
using System.Collections.Generic;
using UnityEngine;

public partial class Table_Plot : ITable
{
    public int Id;
    public string Script;
    public int[] Descs;
    public int[][] Condition;
    public float FValue;
    public float[] FValues;
    public bool IsMine;
    public int Order;

    public override void Load(string[] columns)
    {
        try
        {
            if (!string.IsNullOrEmpty(columns[0])) Id = int.Parse(columns[0]);
            Script = columns[1];
            Descs = ConvertUtils.ConvertArray<int>(columns[2]);
            Condition = ConvertUtils.ConvertArrayList<int>(columns[3]);
            if (!string.IsNullOrEmpty(columns[0])) FValue = float.Parse(columns[4]);
            FValues = ConvertUtils.ConvertArray<float>(columns[5]);
            if (columns[6] != "") IsMine = int.Parse(columns[6]) == 1;
            if (columns[7] != "") Order = int.Parse(columns[7]);

            if (!ms_OrderMap.ContainsKey(Order)) { ms_OrderMap.Add(Order, new List<Table_Plot>()); }
                
            ms_OrderMap[Order].Add(this);
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

    private static Dictionary<int, List<Table_Plot>> ms_OrderMap = new Dictionary<int, List<Table_Plot>>();
    public static List<Table_Plot> getListByOrder(int order)
    {
        if (ms_OrderMap.ContainsKey(order))
        {
            return ms_OrderMap[order];
        }
        return null;
    }


}

 
 
 */