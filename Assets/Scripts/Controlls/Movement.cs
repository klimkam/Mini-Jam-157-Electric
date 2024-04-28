using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField]
    float m_speed = 5f;

    Rigidbody2D m_Rigidbody;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        OnMove();
    }

    private void OnMove()
    {
        float xMovement = Input.GetAxisRaw("Horizontal") / 10;
        float yMovement = Input.GetAxisRaw("Vertical") / 10;

        Vector3 movement = new Vector3 (xMovement, yMovement, 0);
        m_Rigidbody.MovePosition(transform.position + movement);
    }
}
