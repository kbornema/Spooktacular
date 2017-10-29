using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    public float speed = 1F;

    private Vector3 moveDirection;
    private SpriteRenderer sprite;

    Camera camera;

    Vector3 startPosition;
    Vector3 targetPosition;

	// Use this for initialization
	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();

        camera = Camera.main;

        Respawn();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Vector3.Distance(startPosition, targetPosition) < Vector3.Distance(startPosition, transform.position))
        {
            Respawn();
        }

        Move();
        
    }

    private void Move()
    {
        transform.position += moveDirection * speed;

        //float flipVal = Mathf.Sign(moveDir.x);
        sprite.flipX = moveDirection.x < 0 ? true: false;
    }

    private void Respawn()
    {
        float t = Random.Range(-1F, 1F);
        int borderSelection = Random.Range(0, 4);

        switch(borderSelection)
        {
            case 0: // top Border
                startPosition = GetMapPosition(t, 0.5F);
                break;

            case 1: // bottom Border
                startPosition = GetMapPosition(t, -0.5F);
                break;

            case 2: // bottom Border
                startPosition = GetMapPosition(0.5F, t);
                break;

            case 3: // bottom Border
                startPosition = GetMapPosition(-0.5F, t);
                break;

            default:
                startPosition = GetMapPosition(0F, 0F);
                break;
        }

        transform.position = startPosition;

        Vector3 randomCenterTarget = GetMapPosition(Random.Range(-0.1F, 0.1F), Random.Range(-0.1F, 0.1F));
        targetPosition = startPosition + 2.1F * (randomCenterTarget - startPosition);

        moveDirection = (targetPosition - startPosition).normalized;
    }

    Vector3 GetMapPosition(float xOffset, float yOffset)
    {
        Vector3 MapCenter = camera.transform.position;
        MapCenter.z = 0F;
        float MapWidth = camera.GetRightCamBorder() - camera.GetLeftCamBorder();
        float MapHeight = camera.GetUpperCamBorder() - camera.GetLowerCamBorder();

        return MapCenter + xOffset * MapWidth * Vector3.right + yOffset * MapHeight * Vector3.up;
    }
}
