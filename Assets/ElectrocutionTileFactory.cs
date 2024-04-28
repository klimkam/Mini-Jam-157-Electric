using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrocutionTileFactory : MonoBehaviour
{
    [SerializeField] 
    private GameObject _electrocutionTile;

    [SerializeField] 
    private GameManager _gameManager;

    private List<GameObject> _gameField;
    void Start()
    {
        _gameField = _gameManager.GetGameField();
    }

    void Update()
    {
        
    }
}
