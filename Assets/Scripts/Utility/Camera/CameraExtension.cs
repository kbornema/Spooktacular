using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtension
{
    public static Vector2 GetCameraMin(this Camera cam)
    {
        return new Vector2(cam.GetLeftCamBorder(), cam.GetLowerCamBorder());
    }

    public static Vector2 GetCameraMax(this Camera cam)
    {
        return new Vector2(cam.GetRightCamBorder(), cam.GetUpperCamBorder());
    }

    public static float GetLeftCamBorder(this Camera cam)
    {
        return cam.transform.position.x - cam.orthographicSize * cam.aspect;
    }

    public static float GetRightCamBorder(this Camera cam)
    {
        return cam.transform.position.x + cam.orthographicSize * cam.aspect;
    }

    public static float GetUpperCamBorder(this Camera cam)
    {
        return cam.transform.position.y + cam.orthographicSize;
    }

    public static float GetLowerCamBorder(this Camera cam)
    {
        return cam.transform.position.y - cam.orthographicSize;
    }
}
