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


    protected override void OnAwake()
    {
    }

	// Use this for initialization
	void Start ()
    {
        var players = GameManager.Instance.GetPlayers();
        Debug.Log(players.Length + " Players joined the game.");

        for (int i = 0; i < _playerScoreUi.Length; i++)
            _playerScoreUi[i].gameObject.SetActive(false);

        for (int i = 0; i < players.Length; i++)
        {
            _playerScoreUi[i].SetColor(players[i].PlayerColor);
            _playerScoreUi[i].gameObject.SetActive(true);
        }
    }

    void Update()
    {

    }


    public void UpdateScore(int playerId, int score)
    {
        _playerScoreUi[playerId].SetScore(score);
    }
}
