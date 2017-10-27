using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public static class UnityInputChanger
{
    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) 
                return child;
        }
        while (child.Next(false));
        return null;
    }

    public static void ChangeFloatProperty(string axisName, string propertyName, float value)
    {
        SerializedObject serializedObject = null;
        var property = GetProperty(axisName, propertyName, ref serializedObject);

        if(property != null)
        {
            property.floatValue = value;
            serializedObject.ApplyModifiedProperties();
        }
    }

    public static SerializedProperty GetProperty(string axisName, string propertyName, ref SerializedObject inputManagerRef)
    {
        inputManagerRef = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = inputManagerRef.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            if(axesProperty.displayName == axisName)
            {   
                var test = GetChildProperty(axesProperty, propertyName);

                if (test == null)
                    Debug.LogWarning("Could not find property " + propertyName + " on axis " + axisName);

                return test;
            } 
        }

        Debug.LogWarning("Axis  " + axisName + " not found in Input!");
        return null;
    }

    public static void WriteInput(int ignoreCount, List<JoystickCreator.Axis> axises)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        int totalCount = ignoreCount + axises.Count;

        if (axesProperty.arraySize != totalCount)
            axesProperty.arraySize = totalCount;

        serializedObject.ApplyModifiedProperties();

        int axisCount = 0;
        for (int i = ignoreCount; i < totalCount; i++)
        {
            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(i);
            var axis = axises[axisCount];

            GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            GetChildProperty(axisProperty, "type").intValue = (int)axis.axisType;
            GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
            GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;
            axisCount++;
        }

        /*
         *     axesProperty.arraySize++;
    serializedObject.ApplyModifiedProperties();



        */
        /*
        for (int i = 0; i < axises.Count; i++)
        {
            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex();

            
        }
        */


        serializedObject.ApplyModifiedProperties();
    }
}
#endif
