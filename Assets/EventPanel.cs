using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPanel : MonoBehaviour {

    public int playerID;
    public GameObject B, X, Y;

    private List<Fight> BattleList;
    private GameObject parent;

    private void Awake()
    {
        if (transform.parent.gameObject != null)
            parent = transform.parent.gameObject;
    }

    // Use this for initialization
    void Start ()
    {
        parent.SetActive(false);         
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (BattleList.Count == 0)
            parent.SetActive(false);
        else
            parent.SetActive(true);

        //Delete Fight, which aren't acitve any more
        foreach (Fight F in BattleList)
        {
            if (GameManager.Instance.FightList.IndexOf(F) == -1)
            {
                HandleLabels(F,false);
                BattleList.Remove(F);
            }
        }

        //Add new Fights do Routine
        foreach (Fight f in GameManager.Instance.FightList)
        {
            if (f.firstPlayer.playerID == playerID
                || f.firstPlayer.playerID == playerID)
            {
                if (BattleList.IndexOf(f) == -1)
                    BattleList.Add(f);
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
                X.SetActive(b);
                break;
            case 1:
                B.SetActive(b);
                break;
            case 2:
                Y.SetActive(b);
                break;
        }
    }
}
