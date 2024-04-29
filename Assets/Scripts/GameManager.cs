using QFSW.QC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CommandPrefix("gameManager.")]
public class GameManager : MonoBehaviour
{
    private const byte CONNECTOR_PER_WALL = 5;
    private const byte MINIMUM_ACTIVE_FLOOR = 5;
    private const byte MAXIMUM_ACTIVE_FLOOR = 30;
    private const float TIME_PER_CONNECTOR = 1.0f;
    private const float START_TIME = 30.0f;
    private const byte FLOOR_SIZE = 11;

    private Vector3 START_POSITION = new Vector3(0, 0, -0.1f);

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
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _pauseScreen;
    [SerializeField]
    private SoundManager _soundManager;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private TMP_Text _highestScoreText;
    [SerializeField]
    private TMP_Text _currentScoreText;
    [SerializeField]
    private TMP_Text _remainingTimeText;

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

    private bool _isPlaying;
    private bool _isGamePaused = false;
    [Command("time")]
    private float _remainingTime;
    private int _connectedLights = 0;
    private int _score = 0;
    private int _highscore = 0;
    private float _floorTimer = 0.0f;

   
    // Start is called before the first frame update
    void Start()
    {
        //Render the Main Menu Panel
        RenderMainMenu();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isPlaying)
        {
            PauseGame();
        }

        if (_isGamePaused && Input.GetKeyDown(KeyCode.Q))
        {
            PauseGame();
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Return) && !_isPlaying)
        {
            StartGame();
        }

        if (_isPlaying)
        {
            Playing();
        }
    }

    private void StartGame()
    {
        _player.transform.position = START_POSITION;

        _remainingTime = START_TIME;
        _score = 0;

        _mainMenu.SetActive(false);
        _player.GetComponent<PlayerController>().DeactivateAllLines();

        _isPlaying = true;
    }

    private void EndGame()
    {
        _isPlaying = false;

        if (_score > _highscore)
        {
            _highscore = _score;
        }
        _soundManager.StopMusic();
        
        RenderMainMenu();
    }

    private void PauseGame()
    {
        _isGamePaused = !_isGamePaused;

        Time.timeScale = Convert.ToInt32(!_isGamePaused);
        _pauseScreen.SetActive(_isGamePaused);
    }

    private void RenderMainMenu()
    {
        _mainMenu.SetActive(true);
        _highestScoreText.text = _highscore.ToString();
        //TODO Walid do your magic here!
    }

    private void RenderScoreAndTimer()
    {
        int minutes = (int)_remainingTime / 60;
        int seconds = (int)_remainingTime % 60;
        string displayMinutes = (minutes < 10) ? "0" + minutes : minutes.ToString();
        string displaySeconds = (seconds < 10) ? "0" + seconds : seconds.ToString();

        _remainingTimeText.text = displayMinutes + ":" + displaySeconds;

        _currentScoreText.text = _score.ToString();
    }

    private void Playing()
    {
        _soundManager.PlayMusic();
        _remainingTime -= Time.deltaTime;
        _floorTimer += Time.deltaTime;

        if (_floorTimer >= 4.0f && _floorTimer < 5.0f)
        {
            ChargeUpRandomFloorTile();
        }

        if (_floorTimer >= 5.0f && _floorTimer < 6.0f)
        {
            BurstActiveFloorTile();
        }

        if (_floorTimer >= 6.0f)
        {
            TurnOffActiveFloorTile();
            _floorTimer -= 6.0f;
        }

        if (_activeConnectorAmount == 0)
        {
            WallSequence();
        }

        CalculateConnections();

        RenderTiles();
        RenderScoreAndTimer();

        if (_remainingTime < 0.1)
        {
            GameOver();
        }
    }

    private void GameOver()
        {
            _soundManager.PlaySFX("GameOver");
            EndGame();
        }
    
    
    public List<GameObject> GetGameField()
    {
        return _floorTile;
    }

    private void CalculateConnections()
    {
        if (GetAllConnectedWalls() == _activeConnectorAmount)
        {
            _remainingTime += _activeConnectorAmount * TIME_PER_CONNECTOR;

            _score += _activeConnectorAmount;
            _activeConnectorAmount = 0;
            ResetAllWalls();

            _player.GetComponent<PlayerController>().DeactivateAllLines();
        }
    }

    

    private void WallSequence()
    {
        ResetAllWalls();

        TurnOnRandomWallConnector();

        RenderWallConnectors(_northConnectorWall);
        RenderWallConnectors(_southConnectorWall);
        RenderWallConnectors(_westConnectorWall);
        RenderWallConnectors(_eastConnectorWall);

        RenderTargetColor();
    }

    private void ResetAllWalls()
    {
        ResetWall(_northConnectorWall);
        ResetWall(_southConnectorWall);
        ResetWall(_westConnectorWall);
        ResetWall(_eastConnectorWall);
    }

    private int GetAllConnectedWalls()
    {
        int amount = 0;

        if (_northConnectorIndex > -1)
        {
            amount += (_northConnectorWall[_northConnectorIndex].GetComponent<WallConnector>().GetConnectionState()) ? 1 : 0;
        }
        if (_southConnectorIndex > -1)
        {
            amount += (_southConnectorWall[_southConnectorIndex].GetComponent<WallConnector>().GetConnectionState()) ? 1 : 0;
        }
        if (_westConnectorIndex > -1)
        {
            amount += (_westConnectorWall[_westConnectorIndex].GetComponent<WallConnector>().GetConnectionState()) ? 1 : 0;
        }
        if (_eastConnectorIndex > -1)
        {
            amount += (_eastConnectorWall[_eastConnectorIndex].GetComponent<WallConnector>().GetConnectionState()) ? 1 : 0;
        }

        return amount;
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

    private void RenderTiles()
    {
        List<FloorCell> floorCells = GetActiveFloorTile();

        foreach (var floorCell in floorCells)
        {
            RenderTile(_floorTile[floorCell.GetXPos() * FLOOR_SIZE + floorCell.GetYPos()]);
        }
    }

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

    public void ResetActiveWallConnectors()
    {
        if (_northConnectorIndex > -1) _northConnectorWall[_northConnectorIndex].GetComponent<WallConnector>().SetConnectionState(false);
        if (_southConnectorIndex > -1) _southConnectorWall[_southConnectorIndex].GetComponent<WallConnector>().SetConnectionState(false);
        if (_westConnectorIndex > -1) _westConnectorWall[_westConnectorIndex].GetComponent<WallConnector>().SetConnectionState(false);
        if (_eastConnectorIndex > -1) _eastConnectorWall[_eastConnectorIndex].GetComponent<WallConnector>().SetConnectionState(false);
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

            var floor = _floorTile[line * 11 + row].GetComponent<FloorCell>();
            floor.ChargeFloor();
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
                if (_floorTile[i * 11 + j].GetComponent<FloorCell>().GetFloorState() == EFloorState.Charging)
                {
                    _floorTile[i * 11 + j].GetComponent<FloorCell>().BurstFloor();
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
                if (_floorTile[i * 11 + j].GetComponent<FloorCell>().GetFloorState() == EFloorState.Burst)
                {
                    _floorTile[i * 11 + j].GetComponent<FloorCell>().DischargeFloor();
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
                if (_floorTile[i * 11 + j].GetComponent<FloorCell>().IsActive())
                {
                    list.Add(_floorTile[i * 11 + j].GetComponent<FloorCell>());
                }
            }
        }

        return list;
    }
}
