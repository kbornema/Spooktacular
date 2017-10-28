using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalSet", menuName = "PP2D/NormalSet", order = 0)]
public class PP2DNormalSet : APP2DSet
{
    [SerializeField]
    private List<GameObject> _prefabs;
    public List<GameObject> GetPrefabs() { return _prefabs; }

    public override List<GameObject> GetObjectPrefabs(int tileId, APP2DSet.GetOptions mode)
    {
        List<GameObject> result = new List<GameObject>();

        if(tileId == -1)
        {
            result.Add(_prefabs[Random.Range(0, _prefabs.Count)]);
            return result;
        }

        result.Add(_prefabs[tileId]);
        return result;
    }


}
