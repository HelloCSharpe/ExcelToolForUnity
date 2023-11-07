using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ExcelToolSettings : ScriptableObject
{
    [Header("��ȡ����·��")]
    public string readExcelDirectory = "tables";
    [Header("��񵼳���·��")]
    public string exportExcelDirectory = "Assets/Game/Tables";
    [Header("�ű����ɵ�·��")]
    public string scriptGenerateDirectory = "Assets/Scripts/Tables";
    [Header("�ű����ɵ�·��")]
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
                    m_SerializedObject.ApplyModifiedProperties();  // Ӧ���޸��˵�����ֵ
                    Instance.Save(); //�洢�������ݵ� ProjectSettings �ļ���
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
