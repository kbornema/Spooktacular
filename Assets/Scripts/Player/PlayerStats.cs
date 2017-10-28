using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats 
{   
    [SerializeField]
    private int score;
    public int Score { get { return score; } }

    [SerializeField]
    private int collectedCandy;
    public int CollectedCandy { get { return collectedCandy; } }

    [SerializeField]
    private int stolenCandy;
    public int StolenCandy { get { return stolenCandy; } }

    [SerializeField]
    private int itemsUsed;
    public int ItemsUsed { get { return itemsUsed; } }

    [SerializeField]
    private float distTravelled;
    public float DistTravelled { get { return distTravelled; } }

    [SerializeField]
    private float[] squadTime;
    public float[] SquadTime { get { return squadTime; } }

    [SerializeField]
    private int totalInputs;
    public int TotalInputs { get { return totalInputs; } }

    public void Reset()
    {
        score = 0;
        collectedCandy = 0;
        stolenCandy = 0;
        itemsUsed = 0;
        distTravelled = 0.0f;
        squadTime = new float[] { 0, 0, 0 };
        totalInputs = 0;
    }

    public void OnCandyCollected(int amount) { collectedCandy += amount; }

    public void OnCandyStolen(int amount) { collectedCandy += amount; }

    public void OnUseItem() { itemsUsed++; }

    public void OnAddInput() { totalInputs++; }

    public void OnAddTravelDistance(float amount) { distTravelled += amount; }

    public void OnAddSquadTime(int squadIndex, float time) { squadTime[squadIndex] += time; }
}
