using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SearchPattern))]
public class SearchPatternDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Name
        Rect nameRect = new Rect(position.x, position.y, position.width - 100, position.height);
        SerializedProperty nameProp = property.FindPropertyRelative("name");
        EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);

        // Size
        Rect sizeWRect = new Rect(position.x + position.width - 100, position.y, 100, position.height);
        int sizeW = EditorGUI.IntField(sizeWRect, "W", 0);

        EditorGUI.EndProperty();
    }
}
