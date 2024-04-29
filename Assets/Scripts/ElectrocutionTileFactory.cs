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

    [SerializeField]
    GameObject _randomTile;
    GameObject RandomField { 
        get { return _randomTile; }
        set { 
            _randomTile = value;
            GenerateElectrocutionTile();
        }
    }

    void Start()
    {
        _gameField = _gameManager.GetGameField();
        StartCoroutine(SpawnNewTile());
    }

    void PickRandom() {
        RandomField = _gameField[Random.Range(0, _gameField.Count - 1)];
    }

    void GenerateElectrocutionTile()
    {
        Instantiate(_electrocutionTile, RandomField.transform);
    }

    private IEnumerator SpawnNewTile() {

        while (true) { 
            yield return new WaitForSeconds(1);
            PickRandom();
        }
    }
}
