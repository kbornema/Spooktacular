using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : MonoBehaviour {

    public int playerID;
    public GameObject B, X, Y;

    private List<Fight> BattleList;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
        ResetLabels();
    }

    private void ResetLabels()
    {
        B.SetActive(false);
        X.SetActive(false);
        Y.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
        img.color = gameObject.transform.parent.GetComponent<Image>().color;
        img.canvasRenderer.SetAlpha(0);
        BattleList = new List<Fight>();
        //Debug.LogWarning("Hello Panel " +playerID); 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (BattleList.Count == 0)
            img.canvasRenderer.SetAlpha(0);
        else
            img.canvasRenderer.SetAlpha(1);

        //Delete Fight, which aren't acitve any more
        foreach (Fight F in BattleList)
        {
            if (F.fightIsDone || GameManager.Instance.FightList.IndexOf(F) == -1)
            {
                HandleLabels(F,false);
                BattleList.Remove(F);
                //Debug.Log("deactivate Button"); 
            }
        }

        //Add new Fights do Routine
        foreach (Fight f in GameManager.Instance.FightList)
        {
            //Debug.Log("Search for fights " + playerID);
            if (f.firstPlayer.playerID == playerID
                || f.secondPlayer.playerID == playerID)
            {
                if (BattleList.IndexOf(f) == -1)
                    BattleList.Add(f);

                //Debug.Log("Button active");
            }            
        }

        //activate button
        foreach (Fight ff in BattleList)
        {
            HandleLabels(ff, true);
        }
    }

    private void HandleLabels(Fight F, bool b)
    {
        switch (F.FightMode)
        {
            case 0:
                B.SetActive(b);
                break;
            case 1:
                X.SetActive(b);
                break;
            case 2:
                Y.SetActive(b);
                break;
        }
    }
}
