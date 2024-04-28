using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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


    private Arena _arena;

    // Start is called before the first frame update
    void Start()
    {
        _arena = new Arena();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetTile();
            _arena.TurnOnRandomWallConnector();
            _arena.ChargeUpRandomFloorTile();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {

            RenderWallConnectors();
        }
        RenderTiles();
    }

    private void RenderWallConnectors()
    {
        
    }

    private void ResetTile()
    {
        foreach (var floorCell in _floorTile)
        {
            floorCell.SetActive(false);
        }
    }

    private void RenderTiles()
    {
        List<FloorCell> floorCells = _arena.GetActiveFloorTile();

        foreach (var floorCell in floorCells)
        {
            RenderTile(_floorTile[floorCell.GetXPos() * Arena.FLOOR_SIZE + floorCell.GetYPos()]);
        }
    }

    private void RenderTile(GameObject tile)
    {
        gameObject.SetActive(true);
        tile.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
