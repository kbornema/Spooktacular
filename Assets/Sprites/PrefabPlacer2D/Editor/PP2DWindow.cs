using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PP2DWindow : EditorWindow
{

    [SerializeField]
    private PP2DToolType _toolType = PP2DToolType.Rectangle;
    [SerializeField]
    private APP2DLayer _selectedPrefabLayer;

    private bool _mouseIsDragging;
    private Vector2 _mouseWorldPos;
    private Vector2 _mouseDragStartPos;
    private int _currentMouseDragButton;

    [MenuItem("Tool/PrefabPlacerWindow2D _F1")]
    public static void Open()
    {
        var window = EditorWindow.GetWindow<PP2DWindow>();
        window.InitWindow();
    }

    private void InitWindow()
    {
        _selectedPrefabLayer = TryFindSelectedLayer(Selection.activeGameObject);
        //Prevent default behavior from Unitys current selected tool (like translation etc.):
        Tools.current = Tool.View;
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneUpdate;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneUpdate;
    }

    private void OnSceneUpdate(SceneView sceneView)
    {
        Event current = Event.current;

        if (current.isMouse)
            ProcessMouseEvents(sceneView, current);

        else if (current.isKey)
            ProcessKeyEvents(sceneView, current);

        if(current.type == EventType.Repaint)
            OnRepaint(sceneView, current);
    }

    private void OnRepaint(SceneView sceneView, Event current)
    {
        //if(_mouseIsDragging)
        //{
        //    EditorUtil.DrawDebugWireRectangle(_mouseDragStartPos, _mouseWorldPos);
        //}

        if(_selectedPrefabLayer)
        {
            _selectedPrefabLayer.OnRepaint(sceneView, _mouseIsDragging, _mouseDragStartPos, _mouseWorldPos, _currentMouseDragButton, _toolType);
        }
    }

    private void ProcessKeyEvents(SceneView sceneView, Event curEvent)
    {
       
    }

    private void ProcessMouseEvents(SceneView sceneView, Event curEvent)
    {
        _mouseWorldPos = GetCurWorldMousePos();

        int eventButton = curEvent.button;

        if (eventButton == 0 || eventButton == 1)
        {   
            if (curEvent.type == EventType.MouseDown)
                OnMouseClick(eventButton);

            else if(curEvent.type == EventType.MouseDrag)
                OnMouseDrag(eventButton);

            else if(curEvent.type == EventType.MouseUp)
                OnMouseRelease(eventButton);

            curEvent.Use();
        }
    }

    private void OnMouseClick(int button)
    {
        _mouseDragStartPos = _mouseWorldPos;

        if (_selectedPrefabLayer)
            _selectedPrefabLayer.OnMouseClick(_mouseWorldPos, button, _toolType);
    }

    private void OnMouseDrag(int button)
    {
        _mouseIsDragging = true;
        _currentMouseDragButton = button;

        if (_selectedPrefabLayer)
            _selectedPrefabLayer.OnMouseDrag(_mouseDragStartPos, _mouseWorldPos, button, _toolType);
    }

    private void OnMouseRelease(int button)
    {
        _mouseIsDragging = false;

        if (_selectedPrefabLayer)
            _selectedPrefabLayer.OnMouseRelease(_mouseWorldPos, button, _toolType);
    }

    private void OnGUI()
    {
        _selectedPrefabLayer = (APP2DLayer)EditorGUILayout.ObjectField("PrefabLayer:", _selectedPrefabLayer, typeof(APP2DLayer), true);
        _toolType = (PP2DToolType)EditorGUILayout.EnumPopup("ToolType", _toolType);
    }

    private Vector2 GetCurWorldMousePos()
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        return new Vector2(ray.origin.x, ray.origin.y);
    }

    private APP2DLayer TryFindSelectedLayer(GameObject obj)
    {
        if (obj)
        {
            var layer = obj.GetComponent<APP2DLayer>();

            if (layer)
                return layer;

            else if (obj.transform.parent)
                return TryFindSelectedLayer(obj.transform.parent.gameObject);
        }

        return null;
    }
}


