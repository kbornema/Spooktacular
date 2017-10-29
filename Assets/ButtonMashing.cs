using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMashing : MonoBehaviour {

    public int playerInputId;
    public int Button;
    public Sprite Up, Down;

    private Image img;
	// Use this for initialization
	void Start ()
    {
        img = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Button" + (Button) + "_" + playerInputId))
        {
            img.sprite = Down;
        }
        else
            img.sprite = Up;


    }
}
