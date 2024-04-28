using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyMapData keyMapData;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    //[SerializeField] private float dashDuration;
    //[SerializeField] private float dashSpeed;

    [TextArea][SerializeField] private string description = "Be wary, the line array order is case dependant. 0: Up, 1: Down, 2: Left, 3: Right";
    [SerializeField] private Line[] lines = new Line[4];

    private bool _lockPlayerMovement = false;
    private bool _isDashing = false;
    private float _dashCooldownTimer;

    private void Update()
    {
        if (_lockPlayerMovement) return;
        HandleMovement();
        HandleRopeShooting();

        if (keyMapData.menuKey.IsKeyDown())
        {
            DeactivateAllLines();
        }
    }

    private void HandleMovement()
    {
        Vector2 direction = Vector2.zero;
        if (keyMapData.upKey.IsKeyDown()) direction += Vector2.up;
        if (keyMapData.downKey.IsKeyDown()) direction += Vector2.down;
        if (keyMapData.leftKey.IsKeyDown()) direction += Vector2.left;
        if (keyMapData.rightKey.IsKeyDown()) direction += Vector2.right;

        direction.Normalize();

        _rigidbody2D.velocity = speed * 20 * Time.deltaTime * (Vector3)direction;
    }

    private void HandleRopeShooting()
    {
        if (keyMapData.ropeUpKey.IsKeyDown()) lines[0].gameObject.SetActive(true);
        if (keyMapData.ropeDownKey.IsKeyDown()) lines[1].gameObject.SetActive(true);
        if (keyMapData.ropeLeftKey.IsKeyDown()) lines[2].gameObject.SetActive(true);
        if (keyMapData.ropeRightKey.IsKeyDown()) lines[3].gameObject.SetActive(true);
    }

    /*private void HandleDash()
    {
        if (_isDashing) {
            _dashCooldownTimer -= Time.deltaTime;
            if (_dashCooldownTimer <= 0) {
                _isDashing = false;
            }
        }

        switch (_isDashing)
        {
            case false when keyMapData.upKey.HasKeyDoubleTapped():
                PerformDash(Vector2.up);
                break;
            case false when keyMapData.downKey.HasKeyDoubleTapped():
                PerformDash(Vector2.down);
                break;
            case false when keyMapData.leftKey.HasKeyDoubleTapped():
                PerformDash(Vector2.left);
                break;
            case false when keyMapData.rightKey.HasKeyDoubleTapped():
                PerformDash(Vector2.right);
                break;
        }
    }
    
    private void PerformDash(Vector2 direction) {
        _rigidbody2D.velocity = dashSpeed * direction;
        _isDashing = true;
        _dashCooldownTimer = dashDuration;
    }*/

    public void LockPlayerMovement()
    {
        _lockPlayerMovement = true;
    }

    public void UnlockPlayerMovement()
    {
        _lockPlayerMovement = false;
    }

    public void DeactivateAllLines()
    {
        foreach (Line line in lines)
        {
            line.gameObject.SetActive(false);
        }
    }
}

