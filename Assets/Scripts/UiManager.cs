using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : AManager<UiManager> 
{
    [SerializeField]
    private UiPlayerScore[] _playerScoreUi;

    [SerializeField]
    private Text _timer;
    public Text Timer { get { return _timer; } }

    [SerializeField]
    private GameObject _gameOverRoot;

    public Text winnerText;


    protected override void OnAwake()
    {
        _gameOverRoot.SetActive(false);
    }

	// Use this for initialization
	void Start ()
    {
        SetupUi();
    }

    public void SetupUi()
    {
        var players = GameManager.Instance.GetPlayers();
        Debug.Log(players.Length + " Players joined the game.");

        for (int i = 0; i < _playerScoreUi.Length; i++)
            _playerScoreUi[i].gameObject.SetActive(false);

        for (int i = 0; i < players.Length; i++)
        {
            _playerScoreUi[i].SetColor(players[i].PlayerColor);
            _playerScoreUi[i].gameObject.SetActive(true);

            for (int j = 0; j < _playerScoreUi[i].mashings.Length; j++)
                _playerScoreUi[i].mashings[j].playerInputId = players[i].PlayerInputId;

            UpdateScore(i, 0);
        }
    }

    public void UpdateScore(int playerId, int score)
    {
        _playerScoreUi[playerId].SetScore(score);
    }

    public void GameOver(int winnerIndex, PlayerController[] players)
    {
        if(winnerIndex == -1)
        {
            winnerText.text = "Draw! No Winner";
        }

        else
        {
            winnerText.text = "Player " + (players[winnerIndex].PlayerId + 1) + " won with " + players[winnerIndex].Stats.Score + " points!";
        }

        _gameOverRoot.SetActive(true);
    }
}
