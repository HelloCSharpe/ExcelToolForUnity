using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OfficeOpenXml;
using System.IO;
using System.Text;
using static TableConfigData;

public class ExcelEditorTool
{
    [MenuItem("Tool/ExcelTool/生成表格txt文件")]
    public static void GenerateTxt()
    {
        string readExcelDirectory = ExcelToolSettings.Instance.readExcelDirectory;
        string exportExcelDirectory = ExcelToolSettings.Instance.exportExcelDirectory;
        //string path = @"C:\path\to\your\file.xlsx"; // 更新为你的Excel文件路径  

        string[] files = Directory.GetFiles(readExcelDirectory, "*.xlsx", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i];
            var fi = new FileInfo(path);
            string fileName = fi.Name;
            string ext = fi.Extension;
            fileName = fileName.Replace(ext, "");
            Debug.Log(path);
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // 选择第一个工作表  
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                int _col = colCount;
                while (true)
                {
                    var cell = worksheet.Cells[1, _col];
                    if (cell == null || cell.Value == null)
                        _col--;
                    else
                        break;
                }
                colCount = _col;

                int _row = rowCount;
                while (true)
                {
                    var cell = worksheet.Cells[_row, 1];
                    if (cell == null || cell.Value == null)
                        _row--;
                    else
                        break;
                }
                rowCount = _row;

                string txtSavePath = $"{exportExcelDirectory}/{fileName}.txt";
                using (StreamWriter sw = new StreamWriter(txtSavePath))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int row = 5; row <= rowCount; row++)
                    {
                        stringBuilder.Clear();
                        bool ignore = false;
                        for (int col = 1; col <= colCount; col++)
                        {
                            var cell = worksheet.Cells[row, col];
                            if (cell == null || cell.Value == null)
                            {
                                if (col == 1)
                                {
                                    ignore = true;
                                    break;
                                }
                            }
                            else
                            {
                                string str = cell.Value.ToString();
                                //Debug.Log($"[{row},{col}]{str}");
                                stringBuilder.Append(str);
                            }
                            if (col != colCount)
                            {
                                stringBuilder.Append('\t');
                            }
                        }
                        if (!ignore)
                        {
                            if (row != rowCount)
                            {
                                sw.WriteLine(stringBuilder.ToString());
                            }
                            else
                            {
                                sw.Write(stringBuilder.ToString());
                            }
                        }

                    }
                }
            }
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Tool/ExcelTool/生成表格.cs脚本")]
    public static void GenerateTableScript()
    {
        string readExcelDirectory = ExcelToolSettings.Instance.readExcelDirectory;
        //string path = @"C:\path\to\your\file.xlsx"; // 更新为你的Excel文件路径  

        string[] files = Directory.GetFiles(readExcelDirectory, "*.xlsx", SearchOption.TopDirectoryOnly);
        List<TableConfigData> configs = new List<TableConfigData>();
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i];
            Debug.Log(path);
            var fi = new FileInfo(path);
            string fileName = fi.Name;
            string ext = fi.Extension;
            fileName = fileName.Replace(ext, "");
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // 选择第一个工作表  
                //int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                int _col = colCount;
                while (true)
                {
                    var cell = worksheet.Cells[1, _col];
                    if (cell == null || cell.Value == null)
                        _col--;
                    else
                        break;
                }
                colCount = _col;
                TableConfigData tableConfigData = new TableConfigData();
                tableConfigData.tableName = fileName;
                for (int col = 1; col <= colCount; col++)
                {
                    var fieldName = worksheet.Cells[1, col].Value.ToString();
                    if (TableFieldData.FieldVisible(fieldName))
                    {
                        var typeName = worksheet.Cells[2, col].Value.ToString();
                        var cell3 = worksheet.Cells[3, col];
                        string cell3Value = null;
                        if (cell3 == null || cell3.Value == null)
                        {
                        }
                        else
                        {
                            cell3Value = cell3.Value.ToString();
                        }
                        tableConfigData.AddFieldData(col, fieldName, typeName, cell3Value);
                    }

                }
                configs.Add(tableConfigData);
                string tablePath = tableConfigData.GetTableSavePath();
                string tableContent = tableConfigData.GetTableContent();
                File.WriteAllText(tablePath, tableContent);

            }
        }
        if (configs.Count > 0)
        {
            var prev = ExcelToolSettings.Instance.scriptGenerateDirectory;
            string tablecsPath = $"{prev}/Table.cs";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("public class Table");
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("    public static void Init()");
            stringBuilder.AppendLine("    {");

            for (int i = 0; i < configs.Count; i++)
            {
                stringBuilder.AppendLine(configs[i].GetInitString());
            }
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");

            string tablecsContent = stringBuilder.ToString();
            File.WriteAllText(tablecsPath, tablecsContent);
        }
        AssetDatabase.Refresh();
    }
}
/*
 
public class Table
{
    public static void Init()
    {
        TableManager.Instance.LoadTable<Table_Plot>("Tables/Plot");
    }
}
 
 */