using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour 
{
    private const float WRLD_TEXT_OFFSET = 0.5f;

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
    public int CurrentGroupLoot { get { return currentGroupLoot; } set { SetCurrenetGroupLoot(value);} }

    private void SetCurrenetGroupLoot(int value)
    {
        if (value < 0)
            return;

        int delta = value - currentGroupLoot;

        Vector2 up = new Vector2(0, 1);

        float range = Mathf.PI * 0.5f;

        Vector2 newUp = up.Rotate((Random.value - 0.5f) * range);

        GameManager.Instance.SpawnText(transform.position, delta.ToString(), _player.PlayerColor).SetMoveAxis(newUp);
        currentGroupLoot = value;
    }

    [Header("Other")]
    [SerializeField, ReadOnly]
    private PlayerController _player;

    public int playerID;
    
    private bool unloading = false;

    // Current allowed (rolled) candy
    private int allowed_candy = 3;
    private IEnumerator LootingRoutine()
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

    private IEnumerator UnloadLootRoutine()
    {
        while (true)
        {
            if (unloading)
            {
                yield return new WaitForSeconds(0.25f);
                CurrentGroupLoot -= 1;
                if (currentGroupLoot <= 0)
                {
                    unloading = false;
                    curMoveSpeed = _normalMoveSpeed;
                }
                    
                else
                    GameManager.Instance.AddToScore(playerID,1);
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
        if (coll.gameObject.tag == "Door")
        {
            var newFoundDoor = coll.gameObject.GetComponent<DoorController>();

            // Group is not at max loot
            if (CurrentGroupLoot < maxGroupLootLimit)
            {
                // There is more then zero loot in the house
                if (newFoundDoor.House.CurrentLoot > 0)
                {
                    CurrentDoor = newFoundDoor;
                    startDoorLoot();
                }

                else
                {
                    GameManager.Instance.SpawnText(newFoundDoor.transform.position + new Vector3(0, WRLD_TEXT_OFFSET, 0), "House empty!", _player.PlayerColor, 0.66f);
                }
            }
            else
            {
                GameManager.Instance.SpawnText(transform.position + new Vector3(0, WRLD_TEXT_OFFSET, 0), "Squad full!", _player.PlayerColor, 0.66f);
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
        allowed_candy = UnityEngine.Random.Range(2, 4);

        // Set speed to 0
        curMoveSpeed = 0.0f;

        // Per seconds are gathered 3 candycorns
        isLooting = true;

        GameManager.Instance.SpawnText(transform.position + new Vector3(0.0f, WRLD_TEXT_OFFSET, 0.0f), "Start Loot", _player.PlayerColor, 0.66f);
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
        GameManager.Instance.SpawnText(transform.position + new Vector3(0, WRLD_TEXT_OFFSET, 0), "Save Loot!", _player.PlayerColor, 0.66f);

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

