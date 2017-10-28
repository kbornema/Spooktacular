using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [SerializeField]
    LinkedList<WayPoint> currentPath;
    [SerializeField]
    WayPoint currentPoint;


    [SerializeField]
    private SquadFlag _flag;
    public SquadFlag Flag { get { return _flag; } }


    [SerializeField]
    private List<SpriteLookup> _skins;
    [SerializeField]
    private bool _randomizeChildren;

    [SerializeField]
    private AnimatedSpriteReplacer[] _childrenSpriteReplacer;

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

    private IEnumerator HousLootingRoutine()
    {
        while (true)
        {
            if(isLooting)
            {
                yield return new WaitForSeconds(1.0f);
                CurrentGroupLoot += 3;
                allowed_candy--;
            }
            
        }
        //yield break; // beendet Coroutine
    }

    private IEnumerator InvulnerableRoutine()
    {
        while (true)
        {
            if(isInvulnerable)
            {
                yield return new WaitForSeconds(8.0f);
                isInvulnerable = false;
            }
            
        }
        //yield break; // beendet Coroutine
    }

    private void Awake()
    {
        if(_randomizeChildren)
        {
            for (int i = 0; i < _childrenSpriteReplacer.Length; i++)
                _childrenSpriteReplacer[i].SetLookup(_skins[Random.Range(0, _skins.Count)]);
        }

        StartCoroutine(HousLootingRoutine());
        StartCoroutine(InvulnerableRoutine());
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

    public void setPath(LinkedList<WayPoint> newPath)
    {
        currentPath = newPath;
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

}
