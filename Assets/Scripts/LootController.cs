using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {

    public HouseProperties house;

    public Sprite[] CandyAnimation;

    [SerializeField]
    private SpriteRenderer SpRender;
    private int lastLoot = -1;
   
	
	// Update is called once per frame
	private void Update ()
    {
        int curLoot = house.CurrentLoot;

        if (lastLoot != curLoot)
            UpdateLoot(curLoot);
	}

    private void UpdateLoot(int curLoot)
    {   
        float percent = house.LootPercent;

        int index = (int)(percent * (CandyAnimation.Length - 1));
        lastLoot = curLoot;
        SpRender.sprite = CandyAnimation[index];
    }
}
