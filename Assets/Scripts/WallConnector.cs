using System;
using UnityEngine;

public class WallConnector : MonoBehaviour
{
    private EColor _color;
    private bool _isConnected = false;

    public WallConnector()
    {
        _color = EColor.Count;
    }

    public bool GetConnectionState()
    {
        return _isConnected;
    }

    public void SetConnectionState(bool value)
    {
        _isConnected = value;
    }

    public void TurnOnConnector(EColor color)
    {
        _color = color;
    }

    public void TurnOffConnector()
    {
        _color = EColor.Count;
        _isConnected = false;
    }

    public Color GetColor()
    {
        return _color switch
        {
            EColor.Red => Color.red,
            EColor.Green => Color.green,
            EColor.Blue => Color.blue,
            _ => Color.white,
        };
    }
}
