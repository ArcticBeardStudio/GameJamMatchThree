using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SearchPattern))]
public class SearchPatternDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Name
        SerializedProperty nameProp = property.FindPropertyRelative("name");
        Rect nameRect = new Rect(position.x, position.y, position.width, position.height);
        EditorGUI.PropertyField(nameRect, nameProp, new GUIContent("Name"));

        // Pattern
        SerializedProperty patternProp = property.FindPropertyRelative("pattern");
        int numLines = patternProp.stringValue.Split('\n').Length;
        Rect patternRect = new Rect(position.x, position.y + position.height, position.width, (position.height) + (position.height - 3) * (numLines - 1));
        
        patternProp.stringValue = EditorGUI.TextArea(patternRect, patternProp.stringValue);

        EditorGUI.EndProperty();
    }
}
