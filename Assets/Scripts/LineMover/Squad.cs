using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [SerializeField]
    LinkedList<WayPoint> currentPath;
    [SerializeField]
    WayPoint currentPoint;


    [Header("Visuals")]
    [SerializeField]
    private SquadFlag _flag;
    public SquadFlag Flag { get { return _flag; } }


    [SerializeField]
    private List<SpriteLookup> _skins;
    [SerializeField]
    private bool _randomizeChildren = false;

    [SerializeField]
    private AnimatedSpriteReplacer[] _childrenSpriteReplacer;
    [SerializeField]
    private SpriteRenderer[] _coloredSprites;

    [Header("Interaction")]
    public GameObject CurrentDoor = null;

    // Group gets invulnerable at spawn and after a lost fight for a short time
    public bool isInvulnerable = false;

    // Group is looting right now
    public bool isLooting = false;

    // Limit of loot a group can carry
    private int maxGroupLootLimit = 25;

    // Current allowed (rolled) candy
    int allowed_candy = 3;

    // Current gathered loot
    [SerializeField]
    private int currentGroupLoot = 0;
    public int CurrentGroupLoot
    {
        get
        {
            return currentGroupLoot;
        }
        set
        {
            currentGroupLoot = value;
        }
    }

    // Current movement speed of the group
    [SerializeField]
    private int movementSpeed = 5;
    public int MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
        set
        {
            movementSpeed = value;
        }
    }

    [SerializeField, ReadOnly]
    private PlayerController _player;

    private IEnumerator LootingRountine()
    {
        while (true)
        {
            if(isLooting)
            {
                if (CurrentDoor.transform.parent.GetComponent<HouseProperties>().CurrentLoot > 0)
                {
                    yield return new WaitForSeconds(1.0f);
                    CurrentGroupLoot += 3;
                    allowed_candy--;
                    CurrentDoor.transform.parent.GetComponent<HouseProperties>().CurrentLoot-=3;
                }
                else
                    endDoorLoot();
            }
            else
                yield return new WaitForEndOfFrame();
        }
        //yield break; // beendet Coroutine
    }

    private IEnumerator InvulnerableRountine()
    {
        while (true)
        {

            if (isInvulnerable)
            {
                yield return new WaitForSeconds(8.0f);
                isInvulnerable = false;
                // TODO set invulnerable logic on and off
            }
            else
                yield return new WaitForEndOfFrame();
        }
        //yield break; // beendet Coroutine
    }

    private void Awake()
    {
        if(_randomizeChildren)
        {
            for (int i = 0; i < _childrenSpriteReplacer.Length; i++)
            {
                _childrenSpriteReplacer[i].SetLookup(_skins[Random.Range(0, _skins.Count)]);
            }
        }

        
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LootingRountine());
        StartCoroutine(InvulnerableRountine());

        PathWalking = GameObject.Find("Path"); // TAKEOUT
    }

    // TAKEOUT
    GameObject PathWalking;
    Transform targetPathNode;
    int pathNodeIndex = 0;


    // TAKEOUT
    void getNextNode()
    {
        targetPathNode = PathWalking.transform.GetChild(pathNodeIndex);
        pathNodeIndex++;
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
        float distThisFrame = movementSpeed * Time.deltaTime;
        if (dir.magnitude <= distThisFrame)
            targetPathNode = null;
        else
        {
            transform.Translate(dir.normalized * distThisFrame);

        }

        // Check if we can and want to loot further
        if (isLooting)
        {
            // If new route comes in TODO
            // endDoorLoot();

            // Allowed candysize reached
            if (allowed_candy < 1)
            {
                endDoorLoot();

                // Max loot limit of group reached
                if (CurrentGroupLoot > maxGroupLootLimit - 1)
                    endDoorLoot();
            }
        }
    }

    public WayPoint getCurrentPoint()
    {
        if(currentPoint == null)
        {
            currentPoint = currentPath.First.Value;
        }
        return currentPoint;
    }

    public void setPath(LinkedList<WayPoint> newPath)
    {
        currentPath = newPath;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Door")
        {
            GameObject newFoundDoor = coll.gameObject;

            // Group is not at max loot
            if (CurrentGroupLoot < maxGroupLootLimit)

                // There is more then zero loot in the house
                if (newFoundDoor.transform.parent.GetComponent<HouseProperties>().CurrentLoot > 0)
                {
                    CurrentDoor = newFoundDoor;
                    startDoorLoot();
                }
                

        }
            //coll.gameObject.SendMessage("ApplyDamage", 10);
            

    }

    void startDoorLoot()
    {
        // TODO was passiert im kampf???

        // Random count for rolling how much candy we are allowed to get at this door
        allowed_candy = Random.Range(2, 4);

        // Set speed to 0
        MovementSpeed = 0;

        // Per seconds are gathered 3 candycorns
        isLooting = true;
    }

    void endDoorLoot()
    {
        // Stop gathering
        isLooting = false;

        // Set speed to normal
        MovementSpeed = 5;

        CurrentDoor.GetComponent<DoorController>().doorIsClosed = true;
    }

    void setInvulnerable()
    {
        // TODO
        // at spawn
        // after a lost fight
        isInvulnerable = true;
        // set really invulnerable
    }


    public void Init(PlayerController player, Color _color)
    {   
        _player = player;
        _flag.SetColor(_color);

        for (int i = 0; i < _coloredSprites.Length; i++)
            _coloredSprites[i].color = _color;
    }
}
