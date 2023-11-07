using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ExcelToolSettings : ScriptableObject
{
    [Header("读取表格的路径")]
    public string readExcelDirectory = "tables";
    [Header("表格导出的路径")]
    public string exportExcelDirectory = "Assets/Game/Tables";
    [Header("脚本生成的路径")]
    public string scriptGenerateDirectory = "Assets/Scripts/Tables";
    [Header("脚本生成的路径")]
    public string loadPathFormation = "Tables/{0}";


    private const string PATH = "Assets/Editor/Resources/ExcelToolSettings.asset";
    private static ExcelToolSettings _instance;
    public static ExcelToolSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = AssetDatabase.LoadAssetAtPath<ExcelToolSettings>(PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<ExcelToolSettings>();
                    AssetDatabase.CreateAsset(_instance, PATH);
                }
            }
            return _instance;
        }
    }


    private static bool isTest = false;
    private static SerializedObject m_SerializedObject;
    [SettingsProvider]
    public static SettingsProvider MyExcelToolSettings()
    {
        return new SettingsProvider("Project/ExcelToolSettings", SettingsScope.Project)
        {
            label = "ExcelToolSettings",
            guiHandler = (searchContext) =>
            {
                m_SerializedObject?.Dispose();
                m_SerializedObject = new SerializedObject(Instance);

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(m_SerializedObject.FindProperty("readExcelDirectory"));
                EditorGUILayout.PropertyField(m_SerializedObject.FindProperty("exportExcelDirectory"));
                EditorGUILayout.PropertyField(m_SerializedObject.FindProperty("scriptGenerateDirectory"));
                EditorGUILayout.PropertyField(m_SerializedObject.FindProperty("loadPathFormation"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_SerializedObject.ApplyModifiedProperties();  // 应用修改了的属性值
                    Instance.Save(); //存储单例数据到 ProjectSettings 文件夹
                }
                GUILayout.Space(30);
            }
        };
    }

    public void Save()
    {
        AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(PATH));
    }

}
