using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    private SerializedProperty _min;
    private SerializedProperty _max;
    private string _name;
    private bool _cache = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!_cache)
        {
            //get the name before it's gone
            _name = property.displayName;

            //get the X and Y values
            property.Next(true);
            _min = property.Copy();
            property.Next(true);
            _max = property.Copy();

            _cache = true;
        }

        Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(_name));

        //Check if there is enough space to put the name on the same line (to save space)
        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        float half = contentPosition.width / 2;
        GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);

        //show the X and Y from the point
        EditorGUIUtility.labelWidth = 14f;
        contentPosition.width *= 0.5f;
        EditorGUI.indentLevel = 0;

        // Begin/end property & change check make each field
        // behave correctly when multi-object editing.
        EditorGUI.BeginProperty(contentPosition, label, _min);
        {
            EditorGUI.BeginChangeCheck();
            float newMin = EditorGUI.FloatField(contentPosition, new GUIContent("-"), _min.floatValue);
            if (EditorGUI.EndChangeCheck())
                _min.floatValue = Mathf.Min(_max.floatValue, newMin);
        }
        EditorGUI.EndProperty();

        contentPosition.x += half;

        EditorGUI.BeginProperty(contentPosition, label, _max);
        {
            EditorGUI.BeginChangeCheck();
            float newMax = EditorGUI.FloatField(contentPosition, new GUIContent("+"), _max.floatValue);
            if (EditorGUI.EndChangeCheck())
                _max.floatValue = Mathf.Max(_min.floatValue, newMax);
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 16.0f;
    }
}
