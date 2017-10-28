using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    public int playerIndex;

    [SerializeField]
    public GameController gameController;

    [SerializeField]
    public PlayerStats stats;

    [SerializeField]
    public Squad[] squads;

	// Use this for initialization
	void Start () {
        stats = new PlayerStats();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
