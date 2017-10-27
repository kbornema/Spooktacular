using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour {

    public GameObject House;

   // [Range(0, 1)]
    private float fCurrentLoot;

    public Sprite[] CandyAnimation;


    private SpriteRenderer SpRender;
    private float fLastLoot, fHouseLoot;
    private Sprite actualSprite;
    

    private bool switchLoot = false;
	// Use this for initialization
	void Start ()
    {
        SpRender = gameObject.GetComponent<SpriteRenderer>();
        // fHouseLoot = House.GetComponent <???>.fCurrentLoot;
	}
	
	// Update is called once per frame
	void Update ()
    {
        generateLoot(); // nur für Testzwecke

        // fCurrentLoot = House.fCurrentloot;
        if (fLastLoot != fCurrentLoot)
            UpdateLoot();
	
	}

    private void generateLoot()
    {
        if (!switchLoot && fCurrentLoot > 0.95f)
            switchLoot = true;

        if (switchLoot && fCurrentLoot < 0.01f)
            switchLoot = false;

        if (!switchLoot && fCurrentLoot < 0.95f)
            fCurrentLoot += 0.01f;
        if (switchLoot && fCurrentLoot > 0.01f)
            fCurrentLoot -= 0.01f;
    }

    private void UpdateLoot()
    {

        //if (fCurrentLoot <= 0.01f)
            // nulll
        if (fCurrentLoot < 0.1f)
            actualSprite = CandyAnimation[0];
        if (fCurrentLoot > 0.1f)
            actualSprite = CandyAnimation[1];
        if (fCurrentLoot > 0.2f)
            actualSprite = CandyAnimation[2];
        if (fCurrentLoot > 0.3f)
            actualSprite = CandyAnimation[3];
        if (fCurrentLoot > 0.4f)
            actualSprite = CandyAnimation[4];
        if (fCurrentLoot > 0.5f)
            actualSprite = CandyAnimation[5];
        if (fCurrentLoot > 0.6f)
            actualSprite = CandyAnimation[6];
        if (fCurrentLoot > 0.7f)
            actualSprite = CandyAnimation[7];
        if (fCurrentLoot > 0.8f)
            actualSprite = CandyAnimation[8];
        if (fCurrentLoot > 0.9f)
            actualSprite = CandyAnimation[9];

        fLastLoot = fCurrentLoot;
        SpRender.sprite = actualSprite;
    }
}
