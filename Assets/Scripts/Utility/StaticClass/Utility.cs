using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>Static class that holds several functionalities that might be used on many games.</summary>
public static class Utility
{
    /// <summary> Best color ever!!! :) </summary>
    public static Color CornflowerBlue { get { return new Color(100.0f / 255.0f, 149.0f / 255.0f, 237.0f / 255.0f); } }

    public static void SetPixel(this Texture2D texture, Vec2i pos, Color color)
    {
        texture.SetPixel(pos.x, pos.y, color);
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static Vector2 ToVec2(this Vector3 v)
    {
        return v;
    }
    
    public static string CreateTimeStamp(DateTime time)
    {
        string dayString = time.Day < 10 ? ("0" + time.Day.ToString()) : time.Day.ToString();
        string monthString = time.Month < 10 ? ("0" + time.Month.ToString()) : time.Month.ToString();
        string hourString = time.Hour < 10 ? ("0" + time.Hour.ToString()) : time.Hour.ToString();
        string minuteString = time.Minute < 10 ? ("0" + time.Minute.ToString()) : time.Minute.ToString();

        return string.Concat(dayString, ".", monthString, ".", time.Year, " - ", hourString, ":", minuteString);
    }

    public static string CreateTimeStamp()
    {
        return CreateTimeStamp(DateTime.Now);
    }

    public static void DestroyAllChildren(this Transform trans)
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < trans.childCount; i++)
                GameObject.Destroy(trans.GetChild(i).gameObject);
        }

        else
        {
            while (trans.childCount > 0)
                GameObject.DestroyImmediate(trans.GetChild(0).gameObject);
        }
    }

    public static void DrawScreenFilledQuad(Material material)
    {
        material.SetPass(0);

        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);

        GL.TexCoord2(0.0f, 0.0f);
        GL.Vertex3(0, 0, -1);

        GL.TexCoord2(0.0f, 1.0f);
        GL.Vertex3(0, 1, -1);

        GL.TexCoord2(1.0f, 1.0f);
        GL.Vertex3(1, 1, -1);

        GL.TexCoord2(1.0f, 0.0f);
        GL.Vertex3(1, 0, -1);

        GL.End();
        GL.PopMatrix();
    }

    public static void Shuffle<T>(this IList<T> list, System.Random random)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /*
    public static T CreateAsset<T>(string name, string folder = "") where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        if (!FolderExists(folder))
        {
            if (CreateFolder(folder))
                AssetDatabase.Refresh();
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets" + Path.DirectorySeparatorChar + folder + Path.DirectorySeparatorChar + name + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return asset;
    }

    public static bool CreateFolder(string relativePath)
    {
        if (!FolderExists(relativePath))
        {
            string globalPath = GetGlobalPathFrom(relativePath);
            DirectoryInfo info = Directory.CreateDirectory(globalPath);

            if (info.Exists)
                AssetDatabase.Refresh();

            return info.Exists;
        }

        return false;
    }

    public static bool FolderExists(string relativePath)
    {
        bool result = Directory.Exists(GetGlobalPathFrom(relativePath));
        return result;
    }

    public static string GetGlobalPathFrom(string relativePath)
    {
        return Application.dataPath + Path.DirectorySeparatorChar + relativePath;
    }

    public static string CreatePath(params string[] folders)
    {
        string path = "";

        for (int i = 0; i < folders.Length; i++)
            path = path + folders[i] + Path.DirectorySeparatorChar;

        return path;
    }
    */
}
