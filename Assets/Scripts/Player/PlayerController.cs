using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    [SerializeField]
    private int _playerId;
    public int PlayerId { get { return _playerId; } }

    [SerializeField]
    private int _playerInputId;
    public int PlayerInputId { get { return _playerInputId; } }

    [SerializeField]
    private PlayerStats stats;
    public PlayerStats Stats { get { return stats; } }
    
    [SerializeField]
    private Squad[] squads;
    public Squad[] Squads { get { return squads; } }

    [SerializeField]
    private Color _color;
    public Color PlayerColor { get { return _color; } }

    public GameObject SelectionArrowInstance;

    [SerializeField]
    private int _selectedSquadId = -1;

    public void Setup(int playerId, int playerInputId, GameObject selectionArrowPrefab)
    {
        this._playerId = playerId;
        this._playerInputId = playerInputId;
        _color = GetColor(playerId);
        stats = new PlayerStats();
        stats.Reset();

        if(SelectionArrowInstance)
        {
            Destroy(SelectionArrowInstance);
            SelectionArrowInstance = null;
        }

        SelectionArrowInstance = Instantiate(selectionArrowPrefab);
        SelectionArrowInstance.SetActive(false);

        SelectionArrowInstance.GetComponent<SpriteRenderer>().color = _color;
        
        CreateSquads(3);

        SelectSquad(0);
    }

    public void SelectSquad(int i)
    {
        if(i < 0 || i >= squads.Length)
        {   
            _selectedSquadId = -1;
            SelectionArrowInstance.SetActive(false);
        }

        else
        {
            _selectedSquadId = i;
            SelectionArrowInstance.SetActive(true);
            SelectionArrowInstance.transform.SetParent(squads[i].transform);

            SelectionArrowInstance.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
        }
    }

    public void CreateSquads(int num)
    {
        if(squads != null)
        {
            for (int i = 0; i < squads.Length; i++)
                Destroy(squads[i].gameObject);
        }
        
        squads = new Squad[num];

        for (int i = 0; i < squads.Length; i++)
        {
            var squad = Instantiate(GameManager.Instance.SquadPrefab);
            squad.Init(this, _color);

            squads[i] = squad;
        }
    }

    private static Color GetColor(int playerId)
    {
        if (playerId == 0)
            return Color.red;

        else if (playerId == 1)
            return Color.green;

        else if (playerId == 2)
            return Color.blue;

        else if (playerId == 3)
            return Color.yellow;

        return Color.magenta;
    }
}
