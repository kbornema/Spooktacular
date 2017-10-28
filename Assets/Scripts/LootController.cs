using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {

    public HouseProperties house;

   // [Range(0, 1)]
    private float fCurrentLoot;

    public Sprite[] CandyAnimation;

    [SerializeField]
    private SpriteRenderer SpRender;
    private int lastLoot = -1;
   
	
	// Update is called once per frame
	void Update ()
    {

        int curLoot = house.CurrentLoot;
        // fCurrentLoot = House.fCurrentloot;
        if (lastLoot != fCurrentLoot)
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
