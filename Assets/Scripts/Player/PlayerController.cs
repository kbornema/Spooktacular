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

    public InputController inputController;

    public GameObject SelectionArrowInstance;

    [SerializeField]
    private int _selectedSquadId = -1;

    public void Setup(int playerId, int playerInputId, GameObject selectionArrowPrefab, InputController _inputController)
    {
        this._playerId = playerId;
        this._playerInputId = playerInputId;
        _color = GetColor(playerId);
        stats = new PlayerStats();
        stats.Reset();

        inputController = _inputController;

        if(SelectionArrowInstance)
        {
            Destroy(SelectionArrowInstance);
            SelectionArrowInstance = null;
        }

        SelectionArrowInstance = Instantiate(selectionArrowPrefab);
        SelectionArrowInstance.SetActive(false);

        SelectionArrowInstance.GetComponent<SpriteRenderer>().color = _color;
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

            if (SelectionArrowInstance)
            {
                SelectionArrowInstance.SetActive(true);
                SelectionArrowInstance.transform.SetParent(squads[i].transform);
                SelectionArrowInstance.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            }
        }
    }

    private void Update()
    {
        if (inputController.GetPlayerButtonInput("LB", _playerInputId))
        {
            _selectedSquadId = (_selectedSquadId - 1 + squads.Length) % squads.Length;
            SelectSquad(_selectedSquadId);
        }
        if (inputController.GetPlayerButtonInput("RB", _playerInputId))
        {
            _selectedSquadId = (_selectedSquadId + 1) % squads.Length;
            SelectSquad(_selectedSquadId);
        }


        Vector2 move =  inputController.GetMoveVector(this) * GetActiveSquad().CurMoveSpeed;

        Vector2 newPos = GetActiveSquad().transform.position;
        newPos += move * Time.deltaTime;

        GetActiveSquad().transform.position = newPos;

        GetActiveSquad().SetMoving(move);

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
            squads[i] = squad;
        }

        SelectSquad(0);
    }

    public void InitSquads()
    {
        for (int i = 0; i < squads.Length; i++)
            squads[i].Init(this, _color);
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

    public Squad GetActiveSquad()
    {
        if (_selectedSquadId == -1)
            SelectSquad(0);

        return squads[_selectedSquadId];
    }
}
