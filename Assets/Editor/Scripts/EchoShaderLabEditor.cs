using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EchoShaderLab.UI.UIBuilder))]
public class EchoShaderLabEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EchoShaderLab.UI.UIBuilder builder = (EchoShaderLab.UI.UIBuilder)target;
		if (builder != null && GUILayout.Button("生成UI"))
		{
			builder.BuildUI();
		}
	}
}
