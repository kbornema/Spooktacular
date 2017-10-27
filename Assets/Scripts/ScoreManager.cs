using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public GameObject[] Player ;

    private Text Score1, Score2, Score3, Score4;

    public int iPlayerCount = 2;

    private int ilastKing,inewKing;
    // Playerscore
    private int pl1, pl2, pl3, pl4;

	// Use this for initialization
	void Start ()
    {
        Score1 = Player[0].GetComponentInChildren<Text>();
        Score2 = Player[1].GetComponentInChildren<Text>();
        Score3 = Player[2].GetComponentInChildren<Text>();
        Score4 = Player[3].GetComponentInChildren<Text>();
        Debug.Log("scores1 " + Score1.name);
        Debug.Log("scores2 " + Score2.name);
        Debug.Log("scores3 " + Score3.name);
        Debug.Log("scores4 " + Score4.name);

        Score1.text = "10";
        Score2.text = "17";
        Score4.text = "500";
        Score3.text = "33";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (iPlayerCount >= 3)
            Player[2].SetActive(true);
        if (iPlayerCount >= 4)
            Player[3].SetActive(true);


    }


    public void UpdateScore(int Player, int Score)
    {
        switch (Player)
        {
            case 1:
                Score1.text = Score.ToString();
                break;
            case 2:
                Score2.text = Score.ToString();
                break;
            case 3:
                Score3.text = Score.ToString();
                break;
            case 4:
                Score4.text = Score.ToString();
                break;
        }

    }
}
