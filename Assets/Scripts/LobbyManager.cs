using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour {

    public int CountDown = 10;
    public Text cd;
    public Text info;
    public GameObject Container;
    public GameObject AvatarPrefeb;

    private int playerCount;
    private int MaxCount;
    private bool p1, p2, p3, p4;
    private float t0;
    private int time;
	// Use this for initialization
	void Start ()
    {
        info.text = "";
        t0 = Time.time;
        MaxCount = CountDown;
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        time = (int) (Time.time - t0);
        CountDown = Mathf.Max( 0, MaxCount - time);
        cd.text = CountDown.ToString();

        if (CountDown > 0 && playerCount < 4)
        {
            UpdatePlayers();
        }

        if (playerCount < 2)
        {
            info.text = "must be at least 2 players";
        }

        if (playerCount >= 2 && CountDown <= 0)
        {
            CountDown = 0;
            Brain.Instance.NumberOfPlayer = playerCount;
            //StartGame
        }
        

    }

    private void UpdatePlayers()
    {
        if (!p1 && Input.GetButtonDown("Start_0"))
        {
            GameObject clone = Instantiate(AvatarPrefeb);
            clone.transform.parent = Container.transform;
            clone.GetComponent<Image>().color = Color.green;
            clone.GetComponentInChildren<Text>().text = "Player 1";
            playerCount++;
            p1 = true;
        }

        if (!p2 && Input.GetButtonDown("Start_1"))
        {
            GameObject clone = Instantiate(AvatarPrefeb);
            clone.transform.parent = Container.transform;
            clone.GetComponent<Image>().color = Color.red;
            clone.GetComponentInChildren<Text>().text = "Player 2";
            playerCount++;
            p2 = true;
        }

        if (!p3 && Input.GetButtonDown("Start_2"))
        {
            GameObject clone = Instantiate(AvatarPrefeb);
            clone.transform.parent = Container.transform;
            clone.GetComponent<Image>().color = Color.yellow;
            clone.GetComponentInChildren<Text>().text = "Player 3";
            playerCount++;
            p3 = true;
        }

        if (!p4 && Input.GetButtonDown("Start_3"))
        {
            GameObject clone = Instantiate(AvatarPrefeb);
            clone.transform.parent = Container.transform;
            clone.GetComponent<Image>().color = Color.blue;
            clone.GetComponentInChildren<Text>().text = "Player 4";
            playerCount++;
            p4 = true;
        }
    }
}
