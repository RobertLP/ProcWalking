using UnityEngine;
#if UNITY_EDITOR //|| UNITY_EDITOR_64 || UNITY_EDITOR_WIN 
using UnityEditor;
#endif
public class ReadOnlyAttribute : PropertyAttribute
{

}
#if UNITY_EDITOR //|| UNITY_EDITOR_64 || UNITY_EDITOR_WIN 
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property,
		GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position,
		SerializedProperty property,
		GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = true;
	}
}
#endif