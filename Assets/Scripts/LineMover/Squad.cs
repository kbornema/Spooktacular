using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour 
{

    /*
    [Header("Movement")]
    public  mPath currentPath;
    [SerializeField]
    public WayPoint currentPoint;

    public DIRECTION direction;


    public float interpolation;

    [SerializeField]
    private float snappingDistance;


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


    [Header("Interaction")]
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
    public int CurrentGroupLoot { get { return currentGroupLoot; } set { currentGroupLoot = value; } }

    // Current movement speed of the group
    [SerializeField]
    private int movementSpeed = 5;
    public int MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

    [Header("Debug")]

    [SerializeField, ReadOnly]
    private PlayerController _player;
    public PlayerController Player { get { return _player; } }


    */

    [Header("Movement")]
    public mPath currentPath;
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
    public DoorController CurrentDoor = null;
    public Squad CurrentOpponent = null;

    // Group gets invulnerable at spawn and after a lost fight for a short time
    public bool isInvulnerable = false;

    // Group is looting right now
    public bool isLooting = false;

    // Group is already in a fight
    public bool isFighting = false;

    [SerializeField]
    private float _normalMoveSpeed = 5.0f;

    [SerializeField]
    private float curMoveSpeed = 5.0f;
    public float CurMoveSpeed { get { return curMoveSpeed; } set { curMoveSpeed = value; } }

    [Header("LootParams")]
    [SerializeField]
    private float _lootDelay = 1.0f;

    [SerializeField]
    private int _lootPerLoot = 3;

    // Limit of loot a group can carry
    [SerializeField]
    private int maxGroupLootLimit = 25;

    // Current gathered loot
    [SerializeField, ReadOnly]
    private int currentGroupLoot = 0;
    public int CurrentGroupLoot { get { return currentGroupLoot; } set { currentGroupLoot = value; } }

    [Header("Other")]
    [SerializeField, ReadOnly]
    private PlayerController _player;

    public int playerID;

    // Current allowed (rolled) candy
    private int allowed_candy = 3;
    private IEnumerator LootingRountine()
    {
        while (true)
        {
            if(isLooting)
            {
                yield return new WaitForSeconds(1.0f);
                CurrentGroupLoot += 3;
                allowed_candy--;
            }

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
    }

    FightManager fightManager;

    // Update is called once per frame
    void Update()
    {

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
        else
        {
            Vector3 vecToTarget = currentPath.GetFirstPoint().transform.position - transform.position;
            if (vecToTarget.magnitude < snappingDistance)
            {
                transform.position = currentPath.GetFirstPoint().transform.position;
                if (currentPath.NumberOfWayPointsInPath() > 1)
                {
                    currentPath.RemoveFirstPoint();
                }
                else
                {
                    
                }
            }
        }
    }

    public WayPoint getCurrentPoint()
    {
        if (currentPoint == null)
            Debug.Log("There Should always be a current point");
        return currentPoint;
    }

    public void setPath(mPath newPath)
    {
        currentPath = newPath;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("GOT A DOOR!");
        if (coll.gameObject.tag == "Door")
            //coll.gameObject.SendMessage("ApplyDamage", 10);
            Debug.Log("GOT A DOOR!");

    }

    // When at a door
    // TODO needs house gameobjects
    void atDoor()
    {
        // Group is not at max loot
        if (CurrentGroupLoot < maxGroupLootLimit)

            // There is more then zero loot in the house
            // if house.currentLoot > 0
            startDoorLoot();
    }

    void setInvulnerable()
    {
        // TODO
        // at spawn
        // after a lost fight
        isInvulnerable = true;
        // set really invulnerable
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Door")
        {
            var newFoundDoor = coll.gameObject.GetComponent<DoorController>();

            // Group is not at max loot
            if (CurrentGroupLoot < maxGroupLootLimit)

                // There is more then zero loot in the house
                if (newFoundDoor.House.CurrentLoot > 0)
                {
                    CurrentDoor = newFoundDoor;
                    startDoorLoot();
                }


        }

        var otherSquad = coll.GetComponent<Squad>();

        // Check if colliding objects is an opponent
        if (otherSquad && otherSquad._player != _player && isFighting == false && otherSquad.isFighting == false)
        {
            CurrentOpponent = otherSquad;
            fightManager.newFight(this, CurrentOpponent);
        }
        //coll.gameObject.SendMessage("ApplyDamage", 10);


    }

    public void startFight()
    {
        isFighting = true;
        // Stop gathering
        if (isLooting)
        {
            isLooting = false;
            CurrentDoor.doorIsClosed = true;
        }

        // Set speed to 0
        curMoveSpeed = 0.0f;
    }

    public void lostFight()
    {

        // Set speed to normal
        curMoveSpeed = _normalMoveSpeed;

        // set invulnerable
        isInvulnerable = true;

        // Lose candycorn
        CurrentGroupLoot -= 5;
    }



    public void wonFight()
    {
        // not fighting anymore
        isFighting = false;

        // Set speed to normal
        curMoveSpeed = _normalMoveSpeed;

        // Win candycorn
        CurrentGroupLoot += 5;

    }

    void startDoorLoot()
    {
        // TODO was passiert im kampf???

        // Random count for rolling how much candy we are allowed to get at this door
        allowed_candy = Random.Range(2, 4);

        // Set speed to 0
        curMoveSpeed = 0.0f;

        // Per seconds are gathered 3 candycorns
        isLooting = true;
    }

    public void endDoorLoot()
    {
        // Stop gathering
        isLooting = false;

        // Set speed to normal
        curMoveSpeed = _normalMoveSpeed;

        CurrentDoor.doorIsClosed = true;
    }


    public void Init(PlayerController player, Color _color)
    {
        _player = player;
        _flag.SetColor(_color);

        for (int i = 0; i < _coloredSprites.Length; i++)
            _coloredSprites[i].color = _color;

        playerID = _player.PlayerId;
        Debug.Log("ID: " + playerID);
    }

    public PlayerController Player { get { return _player; } }
}

