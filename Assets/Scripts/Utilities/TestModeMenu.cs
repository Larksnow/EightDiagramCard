using UnityEditor;
using UnityEngine;

public static class TestModeMenu
{
    private const string EnableTestKey = "TestMode_EnableTest";

    // 属性：从 EditorPrefs 获取或设置状态
    private static bool EnableTest
    {
        get => EditorPrefs.GetBool(EnableTestKey, false); // 默认值为 false
        set => EditorPrefs.SetBool(EnableTestKey, value);
    }

    [MenuItem("Tools/Test Mode/Toggle Test Mode")]
    public static void ToggleTestMode()
    {
        EnableTest = !EnableTest;
        Debug.Log("Test Mode: " + (EnableTest ? "Enabled" : "Disabled"));
    }

    [MenuItem("Tools/Test Mode/Toggle Test Mode", true)]
    public static bool ToggleTestModeValidate()
    {
        Menu.SetChecked("Tools/Test Mode/Toggle Test Mode", EnableTest);
        return true;
    }

    public static bool IsTestModeEnabled()
    {
        return EnableTest; // 返回保存的值
    }
}