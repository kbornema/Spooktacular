using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    public PlayerController[] player;

    [SerializeField]
    float gameLength;

    float timer;

	// Use this for initialization
	void Start () {
        for(int i = 0; i < player.Length; i++)
        {
            player[i].playerIndex = i;
            player[i].gameController = this;
        }

        timer = gameLength;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            EndGame();
        }
	}

    void EndGame()
    {
        int winnerIndex = FindPlayerWithHighestScore();
        Debug.Log("Player " + winnerIndex + " won with " + player[winnerIndex].stats.score + " points! Congrats!");
    }

    // returns index of Best Player
    int FindPlayerWithHighestScore()
    {
        int index = -1;
        int currentScore = 0;
        for(int i = 0; i < player.Length; i++)
        {
            if (currentScore < player[i].stats.score)
                currentScore = player[i].stats.score;
        }

        return index;
    }
}
