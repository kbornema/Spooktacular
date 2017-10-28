using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPanel : MonoBehaviour {

    public int playerID;
    public GameObject B, X, Y;

    private List<GameObject> BattleList;
    private GameObject parent;
    private bool isB, isX, isY;

    private void Awake()
    {
        if (transform.parent.gameObject != null)
            parent = transform.parent.gameObject;

    }

    // Use this for initialization
    void Start ()
    {
        parent.SetActive(false);
        BattleList = new List<GameObject>(); 
	}
	
	// Update is called once per frame
	void Update ()
    {
        //BattleList = Alle Battles mit payerindex
        foreach (GameObject G in BattleList)
        {
            if ((int)Time.time % 10 == 0)
            {
                Debug.LogWarning("Deactivate");
                X.SetActive(false);
                Y.SetActive(false);
                B.SetActive(false);
            }
            
            HandleFights(G);
        }

    }

    private void HandleFights(GameObject fight)
    {
        parent.SetActive(true);
        int mode = 1; //lies mode des Fights (Fightklasse bruacht property "mode" (X,Y, oder B => 1,2,3)
        switch (mode)
        {
            case 0:
                X.SetActive(true);
                break;
            case 1:
                B.SetActive(true);
                break;
            case 2:
                Y.SetActive(true);
                break;
        }
    }

    private GameObject ChoseActionButton()
    {
        int k = UnityEngine.Random.Range(0, 3);
        switch (k)
        {
            case 0: return X;
            case 1: return Y;
            case 2: return B;
            default: return X;
        }
    }
}
