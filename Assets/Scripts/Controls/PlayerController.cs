using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyMapData keyMapData;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager _soundManager;

    [TextArea][SerializeField] private string description = "Be wary, the line array order is case dependant. 0: Up, 1: Down, 2: Left, 3: Right";
    [SerializeField] private Line[] lines = new Line[4];
    [SerializeField] float _lockTime = 1f;
    [SerializeField]
    Animator _animator;

    private bool _lockPlayerMovement = false;

    private void Update()
    {
        if (_lockPlayerMovement) return;
        HandleMovement();
        HandleRopeShooting();
    }

    private void HandleMovement()
    {
        Vector2 direction = Vector2.zero;
        if (keyMapData.upKey.IsKeyDown()) direction += Vector2.up;
        if (keyMapData.downKey.IsKeyDown()) direction += Vector2.down;
        if (keyMapData.leftKey.IsKeyDown()) direction += Vector2.left;
        if (keyMapData.rightKey.IsKeyDown()) direction += Vector2.right;

        direction.Normalize();

        _rigidbody2D.velocity = speed * 10 * (Vector3)direction * Time.deltaTime;
    }

    private void HandleRopeShooting()
    {
        if (keyMapData.ropeUpKey.IsKeyDownThisFrame()) lines[0].gameObject.SetActive(true);
        if (keyMapData.ropeDownKey.IsKeyDownThisFrame()) lines[1].gameObject.SetActive(true);
        if (keyMapData.ropeLeftKey.IsKeyDownThisFrame()) lines[2].gameObject.SetActive(true);
        if (keyMapData.ropeRightKey.IsKeyDownThisFrame()) lines[3].gameObject.SetActive(true);
    }

    public void LockPlayerMovement()
    {
        _lockPlayerMovement = true;
        _rigidbody2D.velocity = Vector2.zero;
        _animator.SetBool("isElectrocuted", true);
        StartCoroutine(UnlockPlayerMovement());
    }

    public IEnumerator UnlockPlayerMovement()
    {
        yield return new WaitForSeconds(_lockTime);
        _lockPlayerMovement = false;
        _animator.SetBool("isElectrocuted", false);
    }

    public void DeactivateAllLines()
    {
        bool playRetract = false;
        
        foreach (Line line in lines)
        {
            if (line.gameObject.activeInHierarchy) playRetract = true;
        }
        
        if (playRetract) _soundManager.PlaySFX("RetractRope");
        
        gameManager.ResetActiveWallConnectors();
        foreach (Line line in lines)
        {
            line.gameObject.SetActive(false);
        }
    }
}

