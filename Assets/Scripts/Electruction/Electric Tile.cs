using System.Collections;
using UnityEngine;

public class ElectricTile : MonoBehaviour
{
    [SerializeField]
    PlayerController _player;
    private float _lifeTime = 2f;

    private void Start()
    {
        StartCoroutine(DeleteTile());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            _player.DeactivateAllLines();
            _player.LockPlayerMovement();
        }
    }

    IEnumerator DeleteTile()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
