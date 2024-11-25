using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIDistortionController))]
public class UIDistortionControllerEditor : Editor
{
    private GUIStyle labelStyle;

    public override void OnInspectorGUI()
    {
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle();
            labelStyle.font = GUI.skin.label.font;
            labelStyle.fontSize = 20;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.white;
        }

        GUILayout.Label("powered by <b>fridvince</b>", labelStyle);

        GUILayout.Space(4);

        GUIStyle greyLabelStyle = new GUIStyle();
        greyLabelStyle.font = GUI.skin.label.font;
        greyLabelStyle.fontSize = 16;
        greyLabelStyle.alignment = TextAnchor.MiddleCenter;
        greyLabelStyle.normal.textColor = Color.gray;

        GUILayout.Label("fridvince@gmx.us", greyLabelStyle);

        GUILayout.Space(10);

        DrawDefaultInspector();
    }
}