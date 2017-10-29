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
                if (CurrentDoor.House.CurrentLoot > 0)
                {
                    yield return new WaitForSeconds(_lootDelay);
                    CurrentGroupLoot += _lootPerLoot;
                    allowed_candy--;
                    CurrentDoor.House.CurrentLoot -= _lootPerLoot;
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

        fightManager = GameManager.Instance.GetComponent<FightManager>();
        PathWalking = GameObject.Find("Path"); // TAKEOUT
    }

    FightManager fightManager;

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
        float distThisFrame = curMoveSpeed * Time.deltaTime;
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


        _flag.FillAmount = (float)currentGroupLoot / (float)maxGroupLootLimit;
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
        if(isLooting)
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
    }

    public PlayerController Player { get { return _player; } }
}
