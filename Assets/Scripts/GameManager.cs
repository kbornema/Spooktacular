using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : AManager<GameManager> 
{
    [SerializeField]
    private string playScene = "03_Master";

    [Header("Common")]

    [SerializeField]
    private float gameLength = 1800.0f;

    [SerializeField]
    private WorldText worldTextPrefab;

    [SerializeField]
    private Squad _squadPrefab;
    public Squad SquadPrefab { get { return _squadPrefab; } }
    private TileMapcontroller Map;
    public InputController inputController;

    public FightManager fightManager;

    [SerializeField]
    private GameObject _selectionArrowPrefab;
    
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

    public List<Fight> FightList;

    private int[] Score;
    public int[] _Score { get { return Score; } }

    protected override void OnAwake()
    {
        remainingTime = gameLength;
        FightList = new List<Fight>();

        //Only setup the game of the scene is the gameplay scene and there are no players yet:
        if (NumberOfPlayer == 0 )
        {   
            if(SceneManager.GetActiveScene().name.Equals(playScene))
                SetupGame(_playerIds);
            else
                Debug.LogWarning("Could not automatically start game. Did you setup playScene in GameManager?", this);
        }
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name.Equals(playScene))
            StartGame();
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

            players[i].Setup(i, playerIndices[i], _selectionArrowPrefab, inputController);

            //mLineMover lineMover = playerControllerObj.AddComponent<mLineMover>();
        }

        remainingTime = gameLength;

    }

    public void StartGame()
    {
        Score = new int[players.Length];
        CreateSquads();
        _gameIsRunning = true;
    }

    public void CreateSquads()
    {
        List<WayPoint> WPList = Map.GetWaypointList();

        for (int i = 0; i < players.Length; i++)
        {
            players[i].CreateSquads(3);

            for (int j = 0; j < players[i].Squads.Length; j++)
            {
                var a = ChooseWayPoint(WPList);
                players[i].Squads[j].transform.position = a.transform.position;
                players[i].Squads[j].SetFirstWaypoint(a);
            }

            players[i].InitSquads();
        }
    }

    private WayPoint ChooseWayPoint(List<WayPoint> wPList)
    {
        int var = UnityEngine.Random.Range(0, wPList.Count);
        //Debug.Log("Random:"+ var  +" = "+ wPList[var].transform.position);
        WayPoint result = wPList[var];
        wPList.Remove(wPList[var]);

        return result;
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
        //TODO:
        //int winnerIndex = FindPlayerWithHighestScore();
        //Debug.Log("Player " + winnerIndex + " won with " + players[winnerIndex].Stats.Score + " points! Congrats!");
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

    public void AddToScore(int playerID, int points)
    {
        Score[playerID] += points;
        UiManager.Instance.UpdateScore(playerID, Score[playerID]);
    }
    
    public PlayerController[] GetPlayers()
    {
        return players;
    }

    public WorldText SpawnText(Vector3 startPos, int value, Color color)
    {
        var instance = Instantiate(worldTextPrefab);
        instance.transform.position = startPos;
        instance.Set(value.ToString(), color);
        return instance;
    }

    public void SetMap(TileMapcontroller tileMapcontroller)
    {
        this.Map = tileMapcontroller;
        StartGame();
    }
}
