using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : AManager<GameManager> 
{
    [Header("Common")]
    [SerializeField]
    private float gameLength = 600.0f;


    [Header("Debug")]

    [SerializeField, ReadOnly]
    private int _numPlayers = 1;
    public int NumberOfPlayer { get { return _numPlayers; } }

    [SerializeField, ReadOnly]
    private bool _gameIsRunning = false;

    [SerializeField, ReadOnly]
    public PlayerController[] players;

    [SerializeField, ReadOnly]
    private float remainingTime = 0.0f;

    protected override void OnAwake()
    {
        SetupGame(_numPlayers);
    }

    public void SetupGame(int numPlayers)
    {
        for (int i = 0; i < players.Length; i++)
            Destroy(players[i].gameObject);

        this._numPlayers = numPlayers;

        players = new PlayerController[_numPlayers];

        for (int i = 0; i < _numPlayers; i++)
        {
            GameObject playerControllerObj = new GameObject("Player_" + i);
            players[i] = playerControllerObj.AddComponent<PlayerController>();
            players[i].Setup(i);
        }

        remainingTime = gameLength;
    }
	
	// Update is called once per frame
	private void Update () 
    {   
        if (_gameIsRunning)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0.0f)
                EndGame();
        }
	}

    private void EndGame()
    {
        int winnerIndex = FindPlayerWithHighestScore();
        Debug.Log("Player " + winnerIndex + " won with " + players[winnerIndex].Stats.Score + " points! Congrats!");
    }

    // returns index of Best Player
    int FindPlayerWithHighestScore()
    {
        int index = -1;
        int currentScore = 0;
        for(int i = 0; i < players.Length; i++)
        {
            if (currentScore < players[i].Stats.Score)
                currentScore = players[i].Stats.Score;
        }

        return index;
    }

}
