#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


[CustomEditor(typeof(MenuButton), true)]
public class MenuButtonEditor : ButtonEditor
{
    private SerializedProperty clickSound;
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        this.DrawConfigInfo();
        this.serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();  
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.clickSound = this.serializedObject.FindProperty("clickSound");

    }

    protected virtual void DrawConfigInfo()
    {
        EditorGUILayout.PropertyField(this.clickSound);
    }
}
#endif