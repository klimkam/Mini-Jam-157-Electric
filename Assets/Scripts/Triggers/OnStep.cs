using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStep : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Entered");
    }
}
