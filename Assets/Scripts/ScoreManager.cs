using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{   
    public GameObject[] Player;
    private Text[] _playerScoreTexts;


	// Use this for initialization
	void Start ()
    {
        int iPlayerCount = GameManager.Instance.NumberOfPlayer;
        Debug.Log(iPlayerCount + " Players joined the game.");

        _playerScoreTexts = new Text[iPlayerCount];

        for (int i = 0; i < iPlayerCount; i++)
        {
            _playerScoreTexts[i] = Player[i].GetComponentInChildren<Text>();
            Player[i].SetActive(true);
        }
    }


    public void UpdateScore(int playerId, int score)
    {
        _playerScoreTexts[playerId].text = score.ToString();
    }
}
