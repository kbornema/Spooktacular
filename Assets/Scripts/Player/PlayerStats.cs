using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

    public int score;
    int collectedCandy;
    int stolenCandy;
    int itemsUsed;
    float distanceTravelled;
    float[] squadTime;
    int totalInputs;

    public PlayerStats()
    {
        squadTime = new float[] { 0, 0, 0 };
    }

    public void CollectedCandy(int amount) { collectedCandy += amount; }

    public void StoleCandy(int amount) { collectedCandy += amount; }

    public void UseItem() { itemsUsed++; }

    public void AddInput() { totalInputs++; }

    public void AddTravelDistance(float amount) { distanceTravelled += amount; }

    public void AddSquadTime(int squadIndex, float time) { squadTime[squadIndex] += time; }
}
