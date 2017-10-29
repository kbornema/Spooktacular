using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fight
{
    public Squad firstPlayer;
    public Squad secondPlayer;

    public bool fightIsDone = false;

     public Fight(Squad _firstPlayer, Squad _secondPlayer)
    {
        firstPlayer = _firstPlayer;
        secondPlayer = _secondPlayer;
    }



}
