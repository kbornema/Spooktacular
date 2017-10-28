using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

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
                yield return new WaitForSeconds(10.0f);
                gameObject.GetComponent<Collider2D>().enabled = true;
                doorIsClosed = false;
            }
            else
                yield return new WaitForEndOfFrame();

        }

        //yield break; // beendet Coroutine
    }

    // Update is called once per frame
    void Update () {
		
	}


}
