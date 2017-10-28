using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : AManager<UiManager> 
{
    [SerializeField]
    private GameObject[] _playerScoreUi;
    private Text[] _playerScoreTexts;

    [SerializeField]
    private Text _timer;
    public Text Timer { get { return _timer; } }


    protected override void OnAwake()
    {
    }

	// Use this for initialization
	void Start ()
    {
        int iPlayerCount = GameManager.Instance.NumberOfPlayer;
        Debug.Log(iPlayerCount + " Players joined the game.");

        _playerScoreTexts = new Text[iPlayerCount];

        for (int i = 0; i < _playerScoreUi.Length; i++)
            _playerScoreUi[i].SetActive(false);

        for (int i = 0; i < iPlayerCount; i++)
        {
            _playerScoreTexts[i] = _playerScoreUi[i].GetComponentInChildren<Text>();
            _playerScoreUi[i].SetActive(true);
        }
    }


    public void UpdateScore(int playerId, int score)
    {
        _playerScoreTexts[playerId].text = score.ToString();
    }
}
