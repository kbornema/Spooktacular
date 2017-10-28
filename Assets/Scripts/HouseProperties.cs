using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseProperties : MonoBehaviour {

    public int maxLootStorage = 25;

    [SerializeField]
    List<Collider2D> doorList;

    [SerializeField]
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
        }
    }

    // Use this for initialization
    void Start () {
        // Starts with between 12 and 20 candycorns
		CurrentLoot = Random.Range(12, 21);
        StartCoroutine(RegenerationCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Houses slowly regenerate candycorn
    private IEnumerator RegenerationCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if (CurrentLoot < maxLootStorage)
                CurrentLoot += 1;
        }
        
        //yield break; // beendet Coroutine
    }

}
