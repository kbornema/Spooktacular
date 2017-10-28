using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour {

    [SerializeField]
    List<Fight> fightList;

    public void newFight(Squad one, Squad two)
    {
        if (checkIfFightAlreadyExists(one, two))
            return;
        else
            fightList.Add(new Fight(one, two));
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var item in fightList)
        {

        }
	}

    private IEnumerator FightingRoutine(Fight currentFight)
    {
        bool teamOneWins = false;
        // TODO startClap
        yield return new WaitForSeconds(4.5f);
        // Our chance to win starts at 50
        int likelihood = 50;

        bool clapWon = false; // from view of player one
        // If we win the clap, we get an additional 20
        // startClap
        if (clapWon)
            likelihood += 10;
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

        int haveWeWon = Random.Range(1, 101);
        if (haveWeWon < likelihood)
            teamOneWins = true;

        if (teamOneWins)
        {
            currentFight.firstPlayer.wonFight();
            currentFight.secondPlayer.lostFight();
        }
        else
        {
            currentFight.secondPlayer.wonFight();
            currentFight.firstPlayer.lostFight();
        }
            

        yield break; // beendet Coroutine
    }

    bool checkIfFightAlreadyExists(Squad one, Squad two)
    {
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
