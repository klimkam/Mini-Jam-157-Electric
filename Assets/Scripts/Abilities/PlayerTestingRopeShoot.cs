using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestingRopeShoot : MonoBehaviour
{
    public Line upLine;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upLine.OnRopeShoot();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            upLine.OnRopeRetract();
        }
    }
}
