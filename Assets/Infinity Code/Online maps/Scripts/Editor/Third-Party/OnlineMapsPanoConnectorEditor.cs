﻿/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsPanoConnector))]
public class OnlineMapsPanoConnectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
#if !UPANO
        if (GUILayout.Button("Enable uPano"))
        {
            if (EditorUtility.DisplayDialog("Enable uPano", "You have uPano in your project?", "Yes, I have uPano", "Cancel"))
            {
                OnlineMapsEditor.AddCompilerDirective("UPANO");
            }
        }
#else
        base.OnInspectorGUI();
#endif
    }

    private void OnEnable()
    {
        OnlineMapsPanoConnector connector = target as OnlineMapsPanoConnector;
        if (connector.shader == null) connector.shader = Shader.Find("Unlit/Texture");
    }
}