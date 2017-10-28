using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour 
{   
    [Header("Movement")]
    private LinkedList<WayPoint> currentPath;
    [SerializeField]
    private WayPoint currentPoint;


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

    // Update is called once per frame
    void Update()
    {
        // if door found // TODO
        // atDoor();

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


            // If house.currentcandy < 1 TODO
            // endDoorLoot();
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
    public void setPath( Path newPath )
    {
        current = newPath;
    }

    public void setPath(LinkedList<WayPoint> newPath)
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

    void startDoorLoot()
    {
        // TODO was passiert im kampf???

        // Random count for rolling how much candy we are allowed to get at this door
        allowed_candy = Random.Range(3, 6);

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

        // Close door for 10 seconds
        // TODO
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
    }
}
