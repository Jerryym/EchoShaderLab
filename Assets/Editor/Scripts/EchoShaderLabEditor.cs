using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIBuilder))]
public class EchoShaderLabEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		UIBuilder builder = (UIBuilder)target;
		if (builder != null && GUILayout.Button("生成UI"))
		{
			builder.BuildUI();
		}
	}
}
