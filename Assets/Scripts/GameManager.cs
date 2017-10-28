using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : AManager<GameManager> 
{
    private const string GAME_SCENE_NAME = "03_Master";

    [Header("Common")]

    [SerializeField]
    private float gameLength = 600.0f;

    [SerializeField]
    private Squad _squadPrefab;
    public Squad SquadPrefab { get { return _squadPrefab; } }
    
    [Header("Debug")]

    [SerializeField]
    private bool _gameIsRunning = false;
    public bool GameIsRunning { get { return _gameIsRunning; } }

    [SerializeField, ReadOnly]
    public PlayerController[] players;

    [SerializeField, ReadOnly]
    private float remainingTime = 0.0f;
    public float RemainingTime { get { return remainingTime; } }

    private int _lastTimeInt = -1;

    [SerializeField]
    private bool[] _playerIds;

    public int NumberOfPlayer { get { return players.Length; } }

    protected override void OnAwake()
    {
        remainingTime = gameLength;

        //Only setup the game of the scene is the gameplay scene and there are no players yet:
        if (NumberOfPlayer == 0 && SceneManager.GetActiveScene().name.Equals(GAME_SCENE_NAME))
        {
            Debug.Log("B");
            SetupGame(_playerIds);
        }
    }

    public void SetupGame(bool[] joinedPlayers)
    {
        this._playerIds = joinedPlayers;

        List<int> playerIndices = new List<int>();

        for (int i = 0; i < joinedPlayers.Length; i++)
        {
            if(joinedPlayers[i])
                playerIndices.Add(i);
        }

        for (int i = 0; i < players.Length; i++)
            Destroy(players[i].gameObject);

        players = new PlayerController[playerIndices.Count];

        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerControllerObj = new GameObject("Player_" + i);
            players[i] = playerControllerObj.AddComponent<PlayerController>();
            players[i].Setup(i, playerIndices[i]);
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

            int curTimeInt = (int)remainingTime;

            if(_lastTimeInt != curTimeInt)
            {
                _lastTimeInt = curTimeInt;

                if (UiManager.Instance)
                    UiManager.Instance.Timer.text = GetTimeString(remainingTime);
            }
        }
	}

    private string GetTimeString(float time)
    {
        int totalSeconds = (int)time;

        int minutes = totalSeconds / 60;

        int seconds = totalSeconds % 60;

        string minString = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();;
        string secString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

        return minString + ":" + secString;
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
