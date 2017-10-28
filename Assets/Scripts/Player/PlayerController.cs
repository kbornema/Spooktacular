using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    [SerializeField]
    private int playerIndex;
    public int PlayerIndex { get { return playerIndex; } }

    [SerializeField]
    private PlayerStats stats;
    public PlayerStats Stats { get { return stats; } }


    [SerializeField]
    private Squad[] squads;
    public Squad[] Squads { get { return squads; } }
	

    public void Setup(int playerIndex)
    {
        this.playerIndex = playerIndex;
        stats = new PlayerStats();
        stats.Reset();
    }
}
