using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MovePlayer : MonoBehaviour {

    GameObject PathWalking;

    Transform targetPathNode;
    int pathNodeIndex = 0;

    public float speed = 1f;

    // Use this for initialization
    void Start()
    {
        PathWalking = GameObject.Find("Path");
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPathNode == null)
        {
            getNextNode();
            if (targetPathNode == null)
            {
                Destroy(gameObject);
            }
        }

        Vector2 dir = targetPathNode.position - this.transform.localPosition;
        float distThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distThisFrame)
            targetPathNode = null;
        else
        {
            transform.Translate(dir.normalized * distThisFrame);
            
        }
    }

    void getNextNode()
    {
        targetPathNode = PathWalking.transform.GetChild(pathNodeIndex);
        pathNodeIndex++;
    }

    //void ReachedGoal
}
