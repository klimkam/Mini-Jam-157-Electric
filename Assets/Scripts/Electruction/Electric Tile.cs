using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTile : MonoBehaviour
{
    private float _lifeTime = 1f;

    private void Start()
    {


        StartCoroutine(DeleteTile());
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pesun");
    }

    IEnumerator DeleteTile()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}
