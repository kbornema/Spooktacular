using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour {

    [SerializeField]
    List<Fight> fightList;

    private int ID0, ID1;
    private int[] Score;

    public void newFight(Squad one, Squad two)
    {
        if (checkIfFightAlreadyExists(one, two))
            return;
        else
        {
            // Add to list. This is to avoid double fights with the same squads
            Fight f = new Fight(one, two);
            ID0 = f.firstPlayer.playerID;
            ID1 = f.secondPlayer.playerID;
            Score = new int[2];
            fightList.Add(f);

            // Start the coroutine
            StartCoroutine(FightingRoutine(f));
        }            
    }

    
    // Update is called once per frame
    void Update ()
    {
        GameManager.Instance.FightList = fightList;

        for (int i = fightList.Count - 1; i >= 0; i--)
        {
            if (fightList[i].fightIsDone)
                fightList.RemoveAt(i);

            HandleFight(fightList[i].FightMode+1);          
        }
	}

    private void HandleFight(int mode)
    {
        //TODO: Zähle Buttonmashes von Player 1 und 2 
        if (Input.GetButtonDown("Button" + (mode) + "_" + ID0))
        {
            Score[0]++;
        }
            

        if (Input.GetButtonDown("Button" + (mode) + "_" + ID1))
        {
            Score[0]++;
        }
            Score[1]++;
    }

    private IEnumerator FightingRoutine(Fight currentFight)
    {
        currentFight.firstPlayer.startFight();
        currentFight.secondPlayer.startFight();

        print("start fight" + currentFight.firstPlayer.Player.name + " vs "+  currentFight.secondPlayer.Player.name);
        bool teamOneWins = false;
        // TODO startClap
        yield return new WaitForSeconds(4.5f);
        // Our chance to win starts at 50
        int likelihood = 50;

        bool clapWon = Score[0] > Score[1] ? true: false; 
        // If we win the clap, we get an additional 20
        // startClap
        if (clapWon)
            likelihood += 15;
        else
            likelihood -= 15;
        int differenceInLoot = currentFight.secondPlayer.CurrentGroupLoot - currentFight.firstPlayer.CurrentGroupLoot;
        
        // The greater the difference in loot, the higher the winning chance
        if (differenceInLoot > 4)
        {
            likelihood += 5;
        }

        if (differenceInLoot > 12)
        {
            likelihood += 5;
        }

        if (differenceInLoot > 18)
        {
            likelihood += 5;
        }

        if (differenceInLoot < -4)
        {
            likelihood -= 5;
        }
        if (differenceInLoot < -12)
        {
            likelihood -= 5;
        }
        if (differenceInLoot < -18)
        {
            likelihood -= 5;
        }

        int haveWeWon = UnityEngine.Random.Range(1, 101);
        if (haveWeWon < likelihood)
            teamOneWins = true;

        if (teamOneWins)
        {
            currentFight.firstPlayer.wonFight();
            currentFight.secondPlayer.lostFight();
            GameManager.Instance.AddToScore(ID0,1);
        }
        else
        {
            currentFight.secondPlayer.wonFight();
            currentFight.firstPlayer.lostFight();
            GameManager.Instance.AddToScore(ID1, 1);
        }

        // end fight
        currentFight.fightIsDone = true;    

        yield break; // beendet Coroutine
    }

    bool checkIfFightAlreadyExists(Squad one, Squad two)
    {
        Debug.Assert(one != null);
        Debug.Assert(two != null);

        foreach (var item in fightList)
        {
            if (item.firstPlayer == one)
                if (item.secondPlayer == two)
                    return true;

            if (item.secondPlayer == one)
                if (item.firstPlayer == two)
                    return true;
        }
        return false;
    }
}
