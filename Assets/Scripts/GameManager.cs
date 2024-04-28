using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const byte FLOOR_SIZE = 11;
    private const byte WALL_AMOUNT = 4;
    private const byte CONNECTOR_PER_WALL = 5;
    private const byte MINIMUM_ACTIVE_FLOOR = 5;
    private const byte MAXIMUM_ACTIVE_FLOOR = 30;

    [SerializeField]
    private List<GameObject> _northConnectorWall;
    [SerializeField]
    private List<GameObject> _southConnectorWall;
    [SerializeField]
    private List<GameObject> _westConnectorWall;
    [SerializeField]
    private List<GameObject> _eastConnectorWall;

    [SerializeField]
    private List<GameObject> _floorTile;

    [SerializeField]
    private int _activeConnectorAmount = 0;
    [SerializeField]
    private int _northConnectorIndex = -1;
    [SerializeField]
    private int _southConnectorIndex = -1;
    [SerializeField]
    private int _westConnectorIndex = -1;
    [SerializeField]
    private int _eastConnectorIndex = -1;

    [SerializeField]
    private int _activeFloorTiles = 0;

    [SerializeField]
    private GameObject _targetColor;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetTile();
            ResetWall(_northConnectorWall);
            ResetWall(_southConnectorWall);
            ResetWall(_westConnectorWall);
            ResetWall(_eastConnectorWall);


            TurnOnRandomWallConnector();
            //ChargeUpRandomFloorTile();


            RenderWallConnectors(_northConnectorWall);
            RenderWallConnectors(_southConnectorWall);
            RenderWallConnectors(_westConnectorWall);
            RenderWallConnectors(_eastConnectorWall);

            RenderTargetColor();
        }
    }

    private void RenderWallConnectors(List<GameObject> connectors)
    {
        foreach (GameObject connector in connectors)
        {
            connector.GetComponent<SpriteRenderer>().color = connector.GetComponent<WallConnector>().GetColor();
        }
    }

    private void ResetTile()
    {
        foreach (var floorCell in _floorTile)
        {
            floorCell.SetActive(false);
        }
    }

    private void ResetWall(List<GameObject> connectors)
    {
        foreach (var connector in connectors)
        {
            connector.GetComponent<WallConnector>().TurnOffConnector();
        }
    }

    /*private void RenderTiles()
    {
        List<FloorCell> floorCells = GetActiveFloorTile();

        foreach (var floorCell in floorCells)
        {
            RenderTile(_floorTile[floorCell.GetXPos() * FLOOR_SIZE + floorCell.GetYPos()]);
        }
    }*/

    private void RenderTile(GameObject tile)
    {
        gameObject.SetActive(true);
        //tile.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void ResetArena()
    {
        ResetConnectorIndex();
        ResetActiveFloorTile();
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

    private void RenderTargetColor()
    {
       _targetColor.GetComponent<Image>().color = _targetColor.GetComponent<WallConnector>().GetColor();
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

        _targetColor.GetComponent<WallConnector>().TurnOnConnector(color);

        int isNorthWallActive;
        int isSouthWallActive;
        int isWestWallActive;
        int isEastWallActive;

        (_northConnectorIndex, isNorthWallActive) = TurnOnSpecificWall(color, _northConnectorWall);
        (_southConnectorIndex, isSouthWallActive) = TurnOnSpecificWall(color, _southConnectorWall);
        (_westConnectorIndex, isWestWallActive) = TurnOnSpecificWall(color, _westConnectorWall);
        (_eastConnectorIndex, isEastWallActive) = TurnOnSpecificWall(color, _eastConnectorWall);

        _activeConnectorAmount = isNorthWallActive + isSouthWallActive + isWestWallActive + isEastWallActive;

        if (_activeConnectorAmount < 2)
        {
            TurnOffActiveWallConnector();
            TurnOnRandomWallConnector();
        }
    }

    private (int, int) TurnOnSpecificWall(EColor color, List<GameObject> walls)
    {
        int isWallActive = Random.Range(0, 2);
        int index = -1;

        if (isWallActive == 1)
        {
            index = Random.Range(0, CONNECTOR_PER_WALL);
            walls[index].GetComponent<WallConnector>().TurnOnConnector(color);
        }

        return (index, isWallActive);
    }

    /*
     * Used to reset all the walls connector that where active
     */
    public void TurnOffActiveWallConnector()
    {
        if (_northConnectorIndex > -1) _northConnectorWall[_northConnectorIndex].GetComponent<WallConnector>().TurnOffConnector();
        if (_southConnectorIndex > -1) _southConnectorWall[_southConnectorIndex].GetComponent<WallConnector>().TurnOffConnector();
        if (_westConnectorIndex > -1) _westConnectorWall[_westConnectorIndex].GetComponent<WallConnector>().TurnOffConnector();
        if (_eastConnectorIndex > -1) _eastConnectorWall[_eastConnectorIndex].GetComponent<WallConnector>().TurnOffConnector();

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
    /*public void ChargeUpRandomFloorTile()
    {
        SetRandomActiveFloorTile();

        for (int i = 0; i < _activeFloorTiles; i++)
        {
            int line = Random.Range(0, FLOOR_SIZE);
            int row = Random.Range(0, FLOOR_SIZE);

            floor[line, row].ChargeFloor();
        }
    }*/

    /*public void BurstActiveFloorTile()
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
    }*/

    /*
     * Reset all the active floor tile
     */
    /*public void TurnOffActiveFloorTile()
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
    }*/

    /*
     * Return a list of FloorCell that are active
     * Their _xPos and _yPos can be used to match with the tilemap or grid system
     */
    /*public List<FloorCell> GetActiveFloorTile()
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
    }*/

    /*
     * Return the list of colors that can be used to know which color the connectors should be
     * Direction 0 represent the north walls
     * Direction 1 represent the south walls
     * Direction 2 represent the west walls
     * Direction 3 represent the east walls
     */
    /*public List<Color> GetWallConnectors(int direction)
    {
        List<Color> list = new();

        for (int j = 0; j < CONNECTOR_PER_WALL; j++)
        {
            list.Add(wall[direction, j].GetColor());
        }

        return list;
    }*/
}
