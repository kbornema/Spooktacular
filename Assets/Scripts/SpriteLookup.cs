using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteLookup", menuName = "Custom/SpriteLookup")]
public class SpriteLookup : ScriptableObject
{   
    [SerializeField]
    private List<EntryList> _replacements;

    private Dictionary<Sprite, Sprite> _dict;

    
    private void Awake()
    {
        _dict = null;
    }

    public Dictionary<Sprite, Sprite> GetReplacementDict()
    {   
        if(_dict == null)
        {
            _dict = new Dictionary<Sprite,Sprite>();

            for (int i = 0; i < _replacements.Count; i++)
            {
                var curList = _replacements[i]._entries;

                for (int j = 0; j < curList.Count; j++)
                {
                    Debug.Assert(curList[j]._original, string.Format("Original in {0} Animation not set up!", _replacements[i]._animName), this);
                    Debug.Assert(curList[j]._replacement, string.Format("Replacement in {0} Animation not set up!", _replacements[i]._animName), this);

                    _dict.Add(curList[j]._original, curList[j]._replacement);
                }
            }
        }

        return _dict;
    }


    [System.Serializable]
    private class EntryList
    {
        public string _animName = "";
        public List<Entry> _entries = null;
    }

    [System.Serializable]
    private class Entry
    {
        public Sprite _original = null;
        public Sprite _replacement = null;
    }

}
