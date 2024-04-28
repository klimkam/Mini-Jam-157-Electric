using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private KeyMapData keyMapData;
    [SerializeField] private float speed;

    [TextArea] [SerializeField] private string description = "Be wary, the line array order is case dependant. 0: Up, 1: Down, 2: Left, 3: Right";
    [SerializeField] private Line[] lines = new Line[4];
    
    private bool _lockPlayerMovement = false;
    
    private void Update() {
        if (_lockPlayerMovement) return;
        HandleMovement();
        HandleRopeShooting();

        if (keyMapData.menuKey.IsKeyDown()) {
            lines[0].gameObject.SetActive(false);
            lines[1].gameObject.SetActive(false);
            lines[2].gameObject.SetActive(false);
            lines[3].gameObject.SetActive(false);
        }
    }

    private void HandleMovement() {
        Vector2 direction = Vector2.zero;
        if (keyMapData.upKey.IsKeyDown()) direction += Vector2.up;
        if (keyMapData.downKey.IsKeyDown()) direction += Vector2.down;
        if (keyMapData.leftKey.IsKeyDown()) direction += Vector2.left;
        if(keyMapData.rightKey.IsKeyDown()) direction += Vector2.right;

        direction.Normalize();

        transform.position += speed * Time.deltaTime * (Vector3)direction;
    }

    private void HandleRopeShooting() {
            if(keyMapData.ropeUpKey.IsKeyDown()) lines[0].gameObject.SetActive(true);
            if(keyMapData.ropeDownKey.IsKeyDown()) lines[1].gameObject.SetActive(true);
            if(keyMapData.ropeLeftKey.IsKeyDown()) lines[2].gameObject.SetActive(true);
            if(keyMapData.ropeRightKey.IsKeyDown()) lines[3].gameObject.SetActive(true);
    }

    public void LockPlayerMovement() {
        _lockPlayerMovement = true;
    }

    public void UnlockPlayerMovement() {
        _lockPlayerMovement = false;
    }
}

