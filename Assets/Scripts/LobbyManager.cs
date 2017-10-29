using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour 
{
    private const int MAX_INPUT_DEVICES = 5;
    private const int MAX_PLAYERS = 4;
    private const int MIN_PLAYERS = 1;

    public int CountDown = 10;
    public Text cd;
    public Text info;
    public GameObject Container;
    public GameObject AvatarPrefeb;

    private int playerCount;
    private int MaxCount;
    private float t0;
    private int time;
    private bool[] playerJoined;

    private bool _started = false;

	// Use this for initialization
	void Start ()
    {
        info.text = "";
        t0 = Time.time;
        MaxCount = CountDown;

        playerJoined = new bool[MAX_INPUT_DEVICES];
	}
	
	// Update is called once per frame
	void Update ()
    {
        time = (int) (Time.time - t0);
        CountDown = Mathf.Max( 0, MaxCount - time);
        cd.text = CountDown.ToString();

        if (CountDown > 0 && playerCount < MAX_PLAYERS)
        {
            UpdatePlayers();
        }

        if (playerCount < MIN_PLAYERS)
        {
            info.text = "must be at least " + MIN_PLAYERS + " players";
        }

        if (playerCount >= MIN_PLAYERS && CountDown <= 0 && !_started)
        {
            _started = true;
            CountDown = 0;
            GameManager.Instance.SetupGame(playerJoined);
            SceneManager.LoadScene("03_Master");
        }
    }

    private void UpdatePlayers()
    {
        for (int i = 0; i < MAX_INPUT_DEVICES; i++)
        {
            if(!playerJoined[i] && Input.GetButton("Start_" + i))
            {
                GameObject clone = Instantiate(AvatarPrefeb);
                clone.transform.parent = Container.transform;
                //TODO: different colors:
                clone.GetComponent<Image>().color = Color.green;
                clone.GetComponentInChildren<Text>().text = "Player " + (i + 1);
                playerJoined[i] = true;
                playerCount++;
            }
        }
    }
}
