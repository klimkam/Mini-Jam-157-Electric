using UnityEngine;

public class WallConnector : MonoBehaviour
{
    private EColor _color;

    public WallConnector()
    {
        _color = EColor.Count;
    }

    public void TurnOnConnector(EColor color)
    {
        _color = color;
    }

    public void TurnOffConnector()
    {
        _color = EColor.Count;
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
