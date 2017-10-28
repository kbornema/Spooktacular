using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseProperties : MonoBehaviour {

    public int maxLootStorage = 25;

    public int minStart = 12;
    public int maxStart = 21;

    public float _regenerateTime = 1.0f;

    public float LootPercent { get { return ((float)currentLoot / (float)maxLootStorage); } }

    [SerializeField]
    List<Collider2D> doorList;

    [SerializeField, ReadOnly]
    private int currentLoot = 0;
    public int CurrentLoot
    {
        get
        {
            return currentLoot;
        }
        set
        {
            currentLoot = value;
            if (currentLoot <= 0)
                currentLoot = 0;
        }
    }

    // Use this for initialization
    void Start () {
        // Starts with between 12 and 20 candycorns
        CurrentLoot = Random.Range(minStart, maxStart);
        StartCoroutine(RegenerationCoroutine());
    }
	

    // Houses slowly regenerate candycorn
    private IEnumerator RegenerationCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_regenerateTime);
            if (CurrentLoot < maxLootStorage)
                CurrentLoot += 1;
        }
        
        //yield break; // beendet Coroutine
    }

}
