using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OfficeOpenXml;
using System.IO;
using System.Text;

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

    }

    [MenuItem("Tool/ExcelTool/生成表格.cs脚本")]
    public static void GenerateTableScript()
    {
        string readExcelDirectory = ExcelToolSettings.Instance.readExcelDirectory;
        //string path = @"C:\path\to\your\file.xlsx"; // 更新为你的Excel文件路径  

        string[] files = Directory.GetFiles(readExcelDirectory, "*.xlsx", SearchOption.TopDirectoryOnly);
        for (int i = 0; i < files.Length; i++)
        {
            string path = files[i];
            Debug.Log(path);
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // 选择第一个工作表  
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        var cell = worksheet.Cells[row, col];
                        if (cell == null || cell.Value == null)
                        {
                            Debug.Log($"[{row},{col}]null");
                        }
                        else
                        {
                            string str = cell.Value.ToString();
                            Debug.Log($"[{row},{col}]{str}");
                        }
                    }
                }
            }
        }
    }
}
