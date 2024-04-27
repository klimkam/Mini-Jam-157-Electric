using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Line : MonoBehaviour
{
    public LineData data;
    public LineRenderer line;
    public Transform playerTransform;
    private Vector2 _originPoint;

    
    
    public bool hooked;

    public ERelationType lineType = ERelationType.COUNT;
    private bool _straightLine;
    private float _waveSize;
    private float _moveTime;
    private Vector2 _grappleDistanceVector;

    /*private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * data.RopeProgressionSpeed;

        //Destroy(gameObject, lifetime);

        lineRenderer.positionCount = 2; // Set the number of points in the line to 2
        lineRenderer.startWidth = 0.1f; // Set the width of the line
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position); // Set the start position of the line
        lineRenderer.SetPosition(1, player.transform.position);
    }*/


    private void SetLineLastPosition()
    {
        line.SetPosition(0, playerTransform.position);
    }

    private void SetLineFirstPosition()
    {
        line.SetPosition(data.RopeDetailAmount - 1,transform.position);
    }


/*public Line(Transform player, ERelationType line) {
    _playerTransform = player;
    lineType = line;
}*/

    private void Update() {
        MoveLine();
        DrawRope();
        SetLineLastPosition();
        SetLineFirstPosition();
        _grappleDistanceVector = transform.position - playerTransform.position;
    }

    private void MoveLine() {
        if (hooked) return;
        switch (lineType) {
            case ERelationType.NORTH:
                transform.position += Vector3.up * Time.deltaTime * data.RopeProgressionSpeed;
                break;
            case ERelationType.SOUTH:
                transform.position += Vector3.down * Time.deltaTime * data.RopeProgressionSpeed;
                break;
            case ERelationType.EAST:
                transform.position += Vector3.left * Time.deltaTime * data.RopeProgressionSpeed;
                break;
            case ERelationType.WEST:
                transform.position += Vector3.right * Time.deltaTime * data.RopeProgressionSpeed;
                break;
            case ERelationType.COUNT:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void LateUpdate() {
        _originPoint = playerTransform.position;
    }

    private void DrawRope() {
        if (_waveSize > 0)
        {
            _waveSize -= Time.deltaTime * data.StraightenLineSpeed;
            DrawRopeWaves();
        }
    }

    public void OnRopeShoot() {
        _moveTime = 0;
        line.positionCount = data.RopeDetailAmount;
        _waveSize = data.StartWazeSize;
        _straightLine = false;
        hooked = false;
        transform.position = playerTransform.position;
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
        gameObject.SetActive(true);
    }

    public void OnRopeRetract() {
        gameObject.SetActive(false);
    }

    private void OnLineHook() {
        hooked = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        AnchorPoint point = other.gameObject.GetComponent<AnchorPoint>();

        if (point == null) return;
        
        if (lineType == point.anchorPointType) {
            OnLineHook();
        }
    }

    /// <summary>
    /// Animations
    /// </summary>
    private void LinePointsToFirePoints() {
        for (ushort i = 0; i < data.RopeDetailAmount; i++) {
            line.SetPosition(i, _originPoint);
        }
    }
    
    private void DrawRopeWaves() {
        for (ushort i = 0; i < data.RopeDetailAmount; i++) {
            float delta = i / (data.RopeDetailAmount - 1f);
            Vector2 offset = Vector2.Perpendicular(_grappleDistanceVector).normalized * (data.RopeAnimationCurve.Evaluate(delta) * _waveSize);
            Vector2 targetPosition = Vector2.Lerp(playerTransform.position, transform.position * 5, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(playerTransform.position, targetPosition, data.RopeProgressionCurve.Evaluate(_moveTime) * data.RopeProgressionSpeed);
            line.SetPosition(i, currentPosition);
        }
    }
}
