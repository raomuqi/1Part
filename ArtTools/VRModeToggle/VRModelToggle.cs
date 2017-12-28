using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;
using System.Xml.Linq;

public class VRModelToggle : EditorWindow
{
    private string mode;
    private string useVR = "useVRInput";
    private bool bVRMode;

    // XML
    XmlDocument xmlDoc;
    XmlElement node;
    string fullPath;

    [MenuItem("JackRao/VRMode Toggle", false, 0)]
    static void ShowWindow()
    {
        Rect pos = SceneView.lastActiveSceneView.position;
        pos.x += 10; pos.y += 10; pos.width = 200; pos.height = 115;
        EditorWindow.GetWindowWithRect(typeof(VRModelToggle), pos, true, "VR模式与普通模式切换");
    }

    private void OnEnable()
    {
        InitXMLFile();
        bVRMode = GetVRMode();
        SetModeText();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal("box");
        GUI.color = bVRMode ? Color.green : Color.white;
        if (GUILayout.Button(mode, GUILayout.Height(80)))
        {
            bVRMode = !bVRMode;
            SetModeText();
            SetupVRMode(bVRMode.ToString());
        }
        GUILayout.EndHorizontal();
        GUILayout.Label(string.Format("当前模式： {0}", mode));
    }

    private void SetupVRMode(string args)
    {
        node.SetAttribute(useVR, args);
        xmlDoc.Save(fullPath);      // Save the modification
    }

    private void SetModeText()
    {
        mode = bVRMode ? "VR模式" : "普通模式";
    }

    private bool GetVRMode()
    {
        return bool.Parse(node.GetAttribute(useVR));
    }

    private void InitXMLFile()
    {
        xmlDoc = new XmlDocument();
        fullPath = Application.streamingAssetsPath + "/setup.xml";
        if (!File.Exists(fullPath))
        {
            Debug.LogError("setup.xml not exist in StreamingAssets folder");
            return;
        }
        xmlDoc.Load(fullPath);
        node = (XmlElement)xmlDoc.SelectSingleNode("Setup/Public");
        if (node == null)
        {
            Debug.LogError("[Setup/Public] node not exist");
        }
    }

    private void OnDisable()
    {
        
    }
}
