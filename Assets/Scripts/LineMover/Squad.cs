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

    public mPath currentPath;

    [Header("Movement")]
    [SerializeField]
    public WayPoint currentPoint;

    [SerializeField]
    float snappingDistance;


    [Header("Visuals")]
    [SerializeField]
    private SquadFlag _flag;
    public SquadFlag Flag { get { return _flag; } }


    [SerializeField]
    private List<SpriteLookup> _skins;
    [SerializeField]
    private bool _randomizeChildren = false;

    [SerializeField]
    private SquadMember[] _member;
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
    public float CurMoveSpeed
    {
        get { return curMoveSpeed * LootToSpeedModifier.Evaluate(1F - currentGroupLoot / maxGroupLootLimit); }
        set { curMoveSpeed = value / LootToSpeedModifier.Evaluate(1F - currentGroupLoot / maxGroupLootLimit); }
    }

    [SerializeField]
    private AnimationCurve LootToSpeedModifier = new AnimationCurve(new Keyframe(0F, 0.5F), new Keyframe(1F, 1F));

    [Header("LootParams")]
    [SerializeField]
    private float _lootDelay = 1.0f;

    [SerializeField]
    private int _lootPerLoot = 1;

    // Limit of loot a group can carry
    [SerializeField]
    private int maxGroupLootLimit = 25;

    // Current gathered loot
    [SerializeField, ReadOnly]
    private int currentGroupLoot = 0;
    public int CurrentGroupLoot { get { return currentGroupLoot; } set { SetCurrenetGroupLoot(value);} }

    private void SetCurrenetGroupLoot(int value)
    {
        if (value < 0)
            return;

        int delta = value - currentGroupLoot;

        Vector2 up = new Vector2(0, 1);

        float range = Mathf.PI * 0.5f;

        Vector2 newUp = up.Rotate((Random.value - 0.5f) * range);

        GameManager.Instance.SpawnText(transform.position, delta, _player.PlayerColor).SetMoveAxis(newUp);
        currentGroupLoot = value;
    }

    [Header("Other")]
    [SerializeField, ReadOnly]
    private PlayerController _player;

    public int playerID;
    
    private bool unloading = false;

    // Current allowed (rolled) candy
    private int allowed_candy = 0;

    // min candy at door
    [SerializeField]
    private int allowed_candy_min = 4;

    // max candy at door
    [SerializeField]
    private int allowed_candy_max = 6;

    private IEnumerator LootingRoutine()
    {
        while (true)
        {
            if(isLooting)
            {
                yield return new WaitForSeconds(1.0f);
                CurrentGroupLoot += _lootPerLoot;
                allowed_candy--;
            }

            yield return new WaitForEndOfFrame();
        }
        //yield break; // beendet Coroutine
    }

    private IEnumerator UnloadLootRoutine()
    {
        while (true)
        {
            if (unloading)
            {
                if (currentGroupLoot <= 0)
                {
                    currentGroupLoot = 0;
                    unloading = false;
                    curMoveSpeed = _normalMoveSpeed;
                }
                else
                {
                    yield return new WaitForSeconds(0.25f);
                    CurrentGroupLoot -= 1;
                    GameManager.Instance.AddToScore(playerID, 1);
                }           
            }
            yield return new WaitForEndOfFrame();
        }
        //yield break; // beendet Coroutine
    }

    private IEnumerator InvulnerableRoutine()
    {
        while (true)
        {

            if (isInvulnerable)
            {
                isFighting = true;
                yield return new WaitForSeconds(8.0f);
                isInvulnerable = false;
                isFighting = false;
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
            for (int i = 0; i < _member.Length; i++)
            {
                _member[i].Init(_skins[Random.Range(0, _skins.Count)]);
            }
        }

        
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LootingRoutine());
        StartCoroutine(InvulnerableRoutine());
        StartCoroutine(UnloadLootRoutine());
    }

    public void SetMoving(Vector2 moveDir)
    {
        bool moving = moveDir.sqrMagnitude > 0.05f;
        float flipVal = Mathf.Sign(moveDir.x);

        for (int i = 0; i < _member.Length; i++)
        {
            _member[i].SetMoving(moving);


            if(moveDir.x != 0.0f)
            {
                var scale = _member[i].transform.localScale;
                scale.x = Mathf.Abs(scale.x) * -flipVal;
                _member[i].transform.localScale = scale;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        _flag.FillAmount = (float)currentGroupLoot / (float)maxGroupLootLimit;

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

            if (CurrentGroupLoot > maxGroupLootLimit - 1)
                endDoorLoot();
        }
        else
        {
            //TODO:
            if (currentPath == null)
                return;

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


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ghost")
        {
            SetCurrenetGroupLoot(currentGroupLoot / 2);
        }


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

            GameManager.Instance.fightManager.newFight(this, CurrentOpponent);
        }
        //coll.gameObject.SendMessage("ApplyDamage", 10);

        if (coll.gameObject.tag == "Cauldron")
        {
            UnloadLoot();
        }

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

        if (unloading)
        {
            unloading = false;
        }

        // Set speed to 0
        curMoveSpeed = 0.0f;
    }

    public void lostFight(int pay)
    {

        // Set speed to normal
        curMoveSpeed = _normalMoveSpeed;

        // set invulnerable
        isInvulnerable = true;

        // Lose candycorn
        CurrentGroupLoot -= pay;
    }



    public void wonFight(int pay)
    {
        // not fighting anymore
        isFighting = false;

        // Set speed to normal
        curMoveSpeed = _normalMoveSpeed;

        // Win candycorn
        CurrentGroupLoot += pay;

    }

    void startDoorLoot()
    {
        // TODO was passiert im kampf???

        // Random count for rolling how much candy we are allowed to get at this door
        allowed_candy = UnityEngine.Random.Range(allowed_candy_min, allowed_candy_max+1);

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

    private void UnloadLoot()
    {
        // Loot leeren
        unloading = true;
        curMoveSpeed = 0.0f;
    }

    public void Init(PlayerController player, Color _color)
    {   
        _player = player;
        _flag.SetColor(_color);

        for (int i = 0; i < _coloredSprites.Length; i++)
            _coloredSprites[i].color = _color;

        playerID = _player.PlayerId;
    }

    public PlayerController Player { get { return _player; } }

    public void AddNewWaypoint(WayPoint a)
    {   
        Debug.Assert(currentPath != null);
        currentPath.AddNewWaypoint(a);
    }

    public void ClearPathUpToFirstElement()
    {
        if (currentPath == null)
            return;

        currentPath.ClearPathUpToFirstElement();
    }

    public void SetFirstWaypoint(WayPoint a)
    {
        currentPath = new mPath();
        currentPath.AddNewWaypoint(a);
        currentPoint = a;
    }
}

