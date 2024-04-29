using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ElectricTile : MonoBehaviour
{
    GameObject _player;
    PlayerController _playerController;
    private float _lifeTime = 2f;
    float _chargeTime = 0f;

    private void Start()
    {
        _player = GameObject.Find("Player");

        _playerController = _player.GetComponent<PlayerController>();
        StartCoroutine(DeleteTile());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
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
}
