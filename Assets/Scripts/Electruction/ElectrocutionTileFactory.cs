using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectrocutionTileFactory : MonoBehaviour
{
    const int MINTILESTOSPAWN = 5;
    const int MAXTILESTOSPAWN = 30;

    [SerializeField] 
    GameObject _electrocutionTile;

    [SerializeField] 
    GameManager _gameManager;

    List<GameObject> _gameField;
    int _howMuchTilesToSpawn = MINTILESTOSPAWN;

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

    void PickRandomTile() {
        RandomField = _gameField[Random.Range(0, _gameField.Count - 1)];
    }

    void GetHowMuchTiles() {
        _howMuchTilesToSpawn = Random.Range(MINTILESTOSPAWN, MAXTILESTOSPAWN);
    }

    void GenerateElectrocutionTile()
    {
        Instantiate(_electrocutionTile, RandomField.transform);
    }

    IEnumerator SpawnNewTile() {
        while (true) { 
            yield return new WaitForSeconds(5);

            GetHowMuchTiles();
            for (int i = 0; i < _howMuchTilesToSpawn; i++) {
                PickRandomTile();
            }
        }
    }
}
