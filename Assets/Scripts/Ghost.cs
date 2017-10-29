using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public float speed;

    private Vector2 velocity;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
    }

    private void Move()
    {
        //float flipVal = Mathf.Sign(moveDir.x);
        sprite.flipX = velocity.x < 0 ? true: false;
    }
}
