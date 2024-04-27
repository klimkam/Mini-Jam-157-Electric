using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELineDirectionType {
    NORTH,
    SOUTH,
    EAST,
    WEST,
    COUNT
}

public class Line : MonoBehaviour {
    public LineData data;
    private LineRenderer _line;
    private readonly Transform _playerTransform;
    private Vector2 _originPoint;
    private Vector2 _grapplePoint;
    private Vector2 _grappleDistanceVector;
    private bool _straightLine;
    private float _waveSize;
    private float _moveTime;
    private bool _canStartRopeAnimation;
    private bool _canGrapple;
    private Vector2 _ray;

    public ELineDirectionType lineType = ELineDirectionType.COUNT;

    public Line(Transform player) {
        _playerTransform = player;
    }

    private void Update()
    {
        
    }

    private void LateUpdate() {
        _originPoint = _playerTransform.position;
    }

    public void ShootRope() {
        _line.positionCount = data.RopeDetailAmount;
        _waveSize = data.StartWazeSize;
        _straightLine = false;
        
    }

    public void RetractRope() {
        
    }

    /// <summary>
    /// Animations
    /// </summary>
    private void LinePointsToFirePoints() {
        for (ushort i = 0; i < data.RopeDetailAmount; i++) {
            _line.SetPosition(i, _originPoint);
        }
    }
    
    private void DrawRopeWaves() {
        for (ushort i = 0; i < data.RopeDetailAmount; i++) {
            float delta = i / (data.RopeDetailAmount - 1f);
            Vector2 offset = Vector2.Perpendicular(_grappleDistanceVector).normalized * (data.RopeAnimationCurve.Evaluate(delta) * _waveSize);
            Vector2 targetPosition = Vector2.Lerp(_originPoint, _grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(_originPoint, targetPosition, data.RopeAnimationCurve.Evaluate(_moveTime) * data.RopeProgressionSpeed);
    
            _line.SetPosition(i, currentPosition);
        }
    }
}
