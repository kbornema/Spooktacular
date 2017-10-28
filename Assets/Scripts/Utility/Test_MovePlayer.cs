using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_MovePlayer : MonoBehaviour {

    GameObject PathWalking;

    Transform targetPathNode;
    int pathNodeIndex = 0;

    [SerializeField]
    private int experience = 0;
    public int Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
        }
    }

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

    void atLootLocation()
    {
        // falls nicht lootlimit der gruppe erreicht
        // falls loot an tuer vorhanden
        gatherLoot();
    }

    void gatherLoot()
    {
        // was passiert im kampf???

        // speed von gruppe auf 0 setzen
        // pro Sekunde werden ... 3 suessigkeiten gesammelt
        // laeuft, solange gruppe nicht neue anweisungen bekommt
        // laeuft, bis max loot der gruppe erreicht
        // laeuft, bis haus leer
        // laeuft, bis einzellootlimit von 3-5 erreicht
        // tuer schließt nach loot fuer 10 sekunden



    }

    //void ReachedGoal
}
