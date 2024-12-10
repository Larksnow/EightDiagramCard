using UnityEditor;
using UnityEngine;

public class ScreenshotEditor : EditorWindow
{
    [MenuItem("Tools/Take Screenshot")]
    static void TakeScreenshot()
    {
        string path = $"Assets/Art/ScreenShots/{Random.Range(0, 1000)}.png";
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log("Screenshot saved to: " + path);
    }
}