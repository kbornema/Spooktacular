using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APP2DSet : ScriptableObject
{
    public enum GetOptions { Mains, Alternatives, Mixed }


    public abstract List<GameObject> GetObjectPrefabs(int tileBit, GetOptions mode);
}
