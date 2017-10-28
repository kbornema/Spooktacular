using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PP2DToolType { Single, Rectangle }


public abstract class APP2DLayer : MonoBehaviour 
{   
    public abstract void OnMouseClick(Vector2 worldPos, int button, PP2DToolType tool);
    public abstract void OnMouseDrag(Vector2 dragStart, Vector2 worldPos, int button, PP2DToolType tool);
    public abstract void OnMouseRelease(Vector2 worldPos, int button, PP2DToolType tool);


#if UNITY_EDITOR
    public abstract void OnRepaint(UnityEditor.SceneView view, bool isDragging, Vector2 dragStart, Vector2 curMousePos, int button, PP2DToolType tool);
#endif
}
