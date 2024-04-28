using System.Collections.Generic;
using UnityEngine;

public class Arena
{
    public const byte FLOOR_SIZE = 11;
    public const byte WALL_AMOUNT = 4;
    public const byte CONNECTOR_PER_WALL = 5;
    private const byte MINIMUM_ACTIVE_FLOOR = 5;
    private const byte MAXIMUM_ACTIVE_FLOOR = 30;

    private readonly FloorCell[,] floor = new FloorCell[FLOOR_SIZE, FLOOR_SIZE];
    private readonly WallConnector[,] wall = new WallConnector[WALL_AMOUNT, CONNECTOR_PER_WALL];

    private int _activeConnectorAmount = 0;
    private int _northConnectorIndex = -1;
    private int _southConnectorIndex = -1;
    private int _westConnectorIndex = -1;
    private int _eastConnectorIndex = -1;

    private int _activeFloorTiles = 0;

    public Arena()
    {
        ResetArena();
    }

    public void ResetArena()
    {
        InitFloor();
        InitWall();
        ResetConnectorIndex();
        ResetActiveFloorTile();
    }

    private void InitFloor()
    {
        for (int i = 0; i < FLOOR_SIZE; i++)
        {
            for (int j = 0; j < FLOOR_SIZE; j++)
            {
                floor[i, j] = new FloorCell(i, j);
            }
        }
    }

    private void InitWall()
    {
        for (int i = 0; i < WALL_AMOUNT; i++)
        {
            for (int j = 0; j < CONNECTOR_PER_WALL; j++)
            {
                wall[i, j] = new WallConnector();
            }
        }
    }

    private void ResetConnectorIndex()
    {
        _northConnectorIndex = -1;
        _southConnectorIndex = -1;
        _westConnectorIndex = -1;
        _eastConnectorIndex = -1;

        _activeConnectorAmount = 0;
    }

    private void ResetActiveFloorTile()
    {
        _activeFloorTiles = 0;
    }

    /*
     * Return the amount of active connectors
     * Can be used to render at the right of the board/scene
    */
    public int GetActiveConnectorAmount()
    {
        return _activeConnectorAmount;
    }

    /*
     * Active one connector on each wall if they should be active
     */
    public void TurnOnRandomWallConnector()
    {
        EColor color = (EColor)Random.Range(0, (int)EColor.Count);

        int isNorthWallActive;
        int isSouthWallActive;
        int isWestWallActive;
        int isEastWallActive;

        (_northConnectorIndex, isNorthWallActive) = TurnOnSpecificWall(color, 0);
        (_southConnectorIndex, isSouthWallActive) = TurnOnSpecificWall(color, 1);
        (_westConnectorIndex, isWestWallActive) = TurnOnSpecificWall(color, 2);
        (_eastConnectorIndex, isEastWallActive) = TurnOnSpecificWall(color, 3);

        _activeConnectorAmount = isNorthWallActive + isSouthWallActive + isWestWallActive + isEastWallActive;

        if (_activeConnectorAmount < 2)
        {
            TurnOnRandomWallConnector();
        }
    }

    private (int, int) TurnOnSpecificWall(EColor color, int wallSide)
    {
        int isWallActive = Random.Range(0, 2);
        int index = -1;

        if (isWallActive == 1)
        {
            index = Random.Range(0, CONNECTOR_PER_WALL);
            wall[wallSide, index].TurnOnConnector(color);
        }

        return (index, isWallActive);
    }

    /*
     * Used to reset all the walls connector that where active
     */
    public void TurnOffActiveWallConnector()
    {
        wall[0, _northConnectorIndex].TurnOffConnector();
        wall[1, _southConnectorIndex].TurnOffConnector();
        wall[2, _westConnectorIndex].TurnOffConnector();
        wall[3, _eastConnectorIndex].TurnOffConnector();

        ResetConnectorIndex();
    }

    /*
     * Define the amount of tiles that should be activated
     */
    private void SetRandomActiveFloorTile()
    {
        _activeFloorTiles = Random.Range(MINIMUM_ACTIVE_FLOOR, MAXIMUM_ACTIVE_FLOOR + 1);
    }

    /*
     * Begin the sequence
     * Charge random tiles through the floor
     */
    public void ChargeUpRandomFloorTile()
    {
        SetRandomActiveFloorTile();

        for (int i = 0; i < _activeFloorTiles; i++)
        {
            int line = Random.Range(0, FLOOR_SIZE);
            int row = Random.Range(0, FLOOR_SIZE);

            floor[line, row].ChargeFloor();
        }
    }

    public void BurstActiveFloorTile()
    {
        if (_activeFloorTiles == 0)
        {
            return;
        }

        for (int i = 0; i < FLOOR_SIZE; i++)
        {
            for (int j = 0; j < FLOOR_SIZE; j++)
            {
                if (floor[i, j].GetFloorState() == EFloorState.Charging)
                {
                    floor[i, j].BurstFloor();
                }
            }
        }
    }

    /*
     * Reset all the active floor tile
     */
    public void TurnOffActiveFloorTile()
    {
        if (_activeFloorTiles == 0)
        {
            return;
        }

        for (int i = 0; i < FLOOR_SIZE; i++)
        {
            for (int j = 0; j < FLOOR_SIZE; j++)
            {
                if (floor[i, j].GetFloorState() == EFloorState.Burst)
                {
                    floor[i, j].DischargeFloor();
                }
            }
        }

        ResetActiveFloorTile();
    }

    /*
     * Return a list of FloorCell that are active
     * Their _xPos and _yPos can be used to match with the tilemap or grid system
     */
    public List<FloorCell> GetActiveFloorTile()
    {
        List<FloorCell> list = new();

        for (int i = 0; i < FLOOR_SIZE; i++)
        {
            for (int j = 0; j < FLOOR_SIZE; j++)
            {
                if (floor[i, j].IsActive())
                {
                    list.Add(floor[i, j]);
                }
            }
        }

        return list;
    }

    /*
     * Return the list of colors that can be used to know which color the connectors should be
     * Direction 0 represent the north walls
     * Direction 1 represent the south walls
     * Direction 2 represent the west walls
     * Direction 3 represent the east walls
     */
    public List<Color> GetWallConnectors(int direction)
    {
        List<Color> list = new();

        for (int j = 0; j < CONNECTOR_PER_WALL; j++)
        {
            list.Add(wall[direction, j].GetColor());
        }

        return list;
    }
}
