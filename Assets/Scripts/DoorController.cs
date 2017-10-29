using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    [SerializeField]
    private float _doorCooldown = 10.0f;

    [SerializeField]
    private HouseProperties _house;
    public HouseProperties House { get { return _house; } }

    [SerializeField]
    private SpriteRenderer _houseLight;

    public bool doorIsClosed = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(DoorClosingRoutine());
    }

    // Door closes and opens after some time
    private IEnumerator DoorClosingRoutine()
    {
        while (true)
        {
            if(doorIsClosed)
            {
                gameObject.GetComponent<Collider2D>().enabled = false;
                _houseLight.enabled = false;
                yield return new WaitForSeconds(_doorCooldown);
                gameObject.GetComponent<Collider2D>().enabled = true;
                _houseLight.enabled = true;
                doorIsClosed = false;
            }
            else
                yield return new WaitForEndOfFrame();

        }

        //yield break; // beendet Coroutine
    }




}
