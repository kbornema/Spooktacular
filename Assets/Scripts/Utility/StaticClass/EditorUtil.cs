#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public static class EditorUtil
{
    private static System.Type m_annotationUtility;
    private static System.Reflection.PropertyInfo m_showGridProperty;

    private static System.Reflection.PropertyInfo ShowGridProperty
    {
        get
        {
            if (m_showGridProperty == null)
            {
                m_annotationUtility = System.Type.GetType("UnityEditor.AnnotationUtility,UnityEditor.dll");
                m_showGridProperty = m_annotationUtility.GetProperty("showGrid", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            }
            return m_showGridProperty;
        }
    }

    public static bool ShowGrid
    {
        get { return (bool)ShowGridProperty.GetValue(null, null); }
        set { ShowGridProperty.SetValue(null, value, null); }
    }

    public static bool TagExists(string tag)
    {
        var editorTags = UnityEditorInternal.InternalEditorUtility.tags;

        for (int j = 0; j < editorTags.Length; j++)
        {
            if (editorTags[j] == tag)
                return true;
        }

        return false;
    }

    [MenuItem("Utility/Create Prefabs From Selection")]
    static void CreatePrefabsFromSelection()
    {   
        GameObject[] objects = Selection.gameObjects;

        for (int i = 0; i < objects.Length; i++)
        {
            PrefabUtility.CreatePrefab(string.Concat("Assets/", objects[i].name, ".prefab"), objects[i], ReplacePrefabOptions.ConnectToPrefab);

        }
    }

    [MenuItem("Utility/Make Prefabs From Sprite Selection")]
    static void InstantiateTextureSelection()
    {
        Object[] selectedObjects = Selection.objects;

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            Texture2D texture = selectedObjects[i] as Texture2D;

            if (texture)
            {
                string spritesheetPath = AssetDatabase.GetAssetPath(texture);
                Object[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(spritesheetPath);

                for (int j = 0; j < objects.Length; j++)
                {
                    Sprite sprite = objects[j] as Sprite;

                    if (sprite)
                    {
                        GameObject tmp = new GameObject(sprite.name);
                        SpriteRenderer tmpRenderer = tmp.AddComponent<SpriteRenderer>();
                        tmpRenderer.sprite = sprite;

                        PrefabUtility.CreatePrefab(string.Concat("Assets/", sprite.name, ".prefab"), tmp, ReplacePrefabOptions.ConnectToPrefab);

                        GameObject.DestroyImmediate(tmp);
                    }
                }
            }
        }

    }

    public static void Collapse(GameObject go, bool collapse)
    {
        // bail out immediately if the go doesn't have children
        if (go.transform.childCount == 0) 
            return;

        // get a reference to the hierarchy window
        var hierarchy = GetFocusedWindow("Hierarchy");

        // select our go
        SelectObject(go);

        // create a new key event (RightArrow for collapsing, LeftArrow for folding)
        var key = new Event { keyCode = collapse ? KeyCode.LeftArrow : KeyCode.RightArrow, type = EventType.keyDown };

        // finally, send the window the event
        hierarchy.SendEvent(key);
    }
    public static void SelectObject(Object obj)
    {
        Selection.activeObject = obj;
    }
    public static EditorWindow GetFocusedWindow(string window)
    {
        FocusOnWindow(window);
        return EditorWindow.focusedWindow;
    }

    public static void FocusOnWindow(string window)
    {
        EditorApplication.ExecuteMenuItem("Window/" + window);
    }

    public static void SetExpandedRecursive(GameObject go, bool expand)
    {
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var methodInfo = type.GetMethod("SetExpandedRecursive");

        EditorApplication.ExecuteMenuItem("Window/Hierarchy");
        var window = EditorWindow.focusedWindow;

        methodInfo.Invoke(window, new object[] { go.GetInstanceID(), expand });
    }

    public static void DrawWireRectanglePosSize(Vector2 pos, Vector2 size)
    {
        DrawWireRectangleMinMax(pos - size * 0.5f, pos + size * 0.5f);
    }

    public static void DrawWireRectangleMinMax(Vector2 min, Vector2 max)
    {
        DrawWireRectangleMinMax(min.x, min.y, max.x, max.y);
    }

    public static void DrawWireRectangleMinMax(float minX, float minY, float maxX, float maxY)
    {
        UnityEditor.Handles.DrawLine(new Vector3(minX, minY, 0.0f), new Vector3(maxX, minY, 0.0f));
        UnityEditor.Handles.DrawLine(new Vector3(maxX, minY, 0.0f), new Vector3(maxX, maxY, 0.0f));
        UnityEditor.Handles.DrawLine(new Vector3(maxX, maxY, 0.0f), new Vector3(minX, maxY, 0.0f));
        UnityEditor.Handles.DrawLine(new Vector3(minX, maxY, 0.0f), new Vector3(minX, minY, 0.0f));
    }
    
    public static void FindScriptsOfType<T>(List<T> list) where T : MonoBehaviour
    {
        List<GameObject> goList = new List<GameObject>();
        FindObjectsOfType<GameObject>(goList, ".prefab");

        for (int i = 0; i < goList.Count; i++)
        {
            T[] scripts = goList[i].GetComponentsInChildren<T>();

            for (int j = 0; j < scripts.Length; j++)
            {
                list.Add(scripts[j]);
            }
        }
    }

    public static void FindObjectsOfType<T>(List<T> list, string fileEnding) where T : class
    {
        FindObjectsOfType<T>(Application.dataPath, fileEnding, list, null);
    }

    public static void FindAssetsOfType<T>(List<T> list, System.Predicate<T> pred = null) where T : class
    {
        FindObjectsOfType<T>(Application.dataPath, ".asset", list, pred);
    }

    private static void FindObjectsOfType<T>(string path, string fileEnding, List<T> list, System.Predicate<T> pred) where T : class
    {
        string[] files = System.IO.Directory.GetFiles(path);

        foreach (string file in files)
        {   
            if (file.EndsWith(fileEnding))
            {
                string assetPath = "Assets" + file.Replace(Application.dataPath, "");
                assetPath = assetPath.Replace("\\", "/");

                var element = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;

                if (element != null && (pred == null || pred(element)))
                    list.Add(element);
            }
        }

        string[] directories = System.IO.Directory.GetDirectories(path);

        for (int i = 0; i < directories.Length; i++)
            FindObjectsOfType(directories[i], fileEnding, list, pred);
    }

}
#endif
