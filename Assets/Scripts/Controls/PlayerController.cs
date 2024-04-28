using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private KeyMapData keyMapData;
    [SerializeField] private float speed;
    
    private bool _lockPlayerMovement = false;


    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        
    }

    private void HandleRopeShooting() {
        
    }

    public void LockPlayerMovement() {
        _lockPlayerMovement = true;
    }

    public void UnlockPlayerMovement() {
        _lockPlayerMovement = false;
    }
}

