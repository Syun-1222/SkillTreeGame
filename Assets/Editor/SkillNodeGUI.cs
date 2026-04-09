using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SkillNodeEntry))]
public class SkillNodeGUI : PropertyDrawer
{
    // inspectorで描画処理を入力
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // SkillNodeEntryを取得
        var skillProp = property.FindPropertyRelative("skill");
        var posProp = property.FindPropertyRelative("position");
        var offsetProp = property.FindPropertyRelative("iconOffset");
        var sizeProp = property.FindPropertyRelative("iconSize");   
        var scaleProp = property.FindPropertyRelative("iconScale"); 

        string title = "None";

        if (skillProp.objectReferenceValue != null)
        {
            var skill = skillProp.objectReferenceValue as SkillData;
            title = $"{skill.skillName} ({skill.id})";
        }

        EditorGUI.LabelField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            title
        );

        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            skillProp
        );

        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            posProp
        );

        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            offsetProp
        );

        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            sizeProp
        );

        position.y += EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            scaleProp
        );
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 6行に増える
        return EditorGUIUtility.singleLineHeight * 6;
    }
}