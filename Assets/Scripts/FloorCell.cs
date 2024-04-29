using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCell : MonoBehaviour
{
    private readonly int _xPos;
    private readonly int _yPos;
    private EFloorState _floorState;

    public FloorCell(int xPos, int yPos)
    {
        _xPos = xPos;
        _yPos = yPos;
        _floorState = EFloorState.Off;
    }
    
    public int GetXPos()
    {
        return _xPos;
    }

    public int GetYPos()
    {
        return _yPos;
    }

    public EFloorState GetFloorState()
    {
        return _floorState;
    }

    public void DischargeFloor()
    {
        _floorState = EFloorState.Off;
    }

    public void ChargeFloor()
    {
        _floorState = EFloorState.Charging;
    }

    public void BurstFloor()
    {
        _floorState = EFloorState.Burst;
    }

    public bool IsActive()
    {
        return _floorState == EFloorState.Charging || _floorState == EFloorState.Burst;
    }

    private void Start()
    {
        SpawnNewTile();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pesun");
    }

    private IEnumerator SpawnNewTile()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
