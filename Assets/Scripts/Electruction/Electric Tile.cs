using System.Collections;
using UnityEngine;

public class ElectricTile : MonoBehaviour
{
    GameObject _player;
    PlayerController _playerController;
    private float _lifeTime = 2f;
    float _chargeTime = 1f;
    bool _isCharged = false;
    Animator _animator;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _playerController = _player.GetComponent<PlayerController>();
        StartCoroutine(DeleteTile());
        StartCoroutine(ChargeTile());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isCharged) { return; }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            _playerController.DeactivateAllLines();
            _playerController.LockPlayerMovement();
        }
    }

    IEnumerator DeleteTile()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    IEnumerator ChargeTile() { 
        yield return new WaitForSeconds(_chargeTime);
        _isCharged = true;
        _animator.SetBool("isZapping", _isCharged);
    }
}
