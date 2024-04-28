using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestingRopeShoot : MonoBehaviour
{
    public KeyMapData KeyMapData;
    public Line upLine;

    private void Update() {
        if (KeyMapData.upKey.IsKeyDown())
        {
            upLine.gameObject.SetActive(true);
        }
        if (KeyMapData.menuKey.IsKeyDown())
        {
            upLine.gameObject.SetActive(false);
        }
    }
}
