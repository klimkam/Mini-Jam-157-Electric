using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class LineCast : MonoBehaviour
{
    public Transform origin;
    public LineRenderer LineRenderer;
    public LineData LineData;
    private Vector2 _grapplePoint;
    private bool _canGrapple;
    private Vector2 _grappleDistanceVector;
    private bool _canStartRopeAnimation;
    private bool _straightLine;
    private double _waveSize;
    private float _moveTime;


    private void OnEnable()
    {
        _moveTime = 0;
        LineRenderer.positionCount = LineData.RopeDetailAmount;
        _waveSize = LineData.StartWazeSize;
        _straightLine = false;
        SetGrapplePoint();
    }

    private void Update()
    {
        if (_canStartRopeAnimation)
        {
            DrawRope();
            _moveTime += Time.fixedDeltaTime;
        }
    }
    
    private void SetGrapplePoint()
    {
        //var ray = Vector3.up * 10;

        var hit = Physics2D.Raycast(origin.position,Vector3.up, 100);

        if (hit.transform.GetComponent<AnchorPoint>()) {
            _canGrapple = true;
        }

        _grapplePoint = hit.collider.ClosestPoint(hit.point);
        _grappleDistanceVector = _grapplePoint - (Vector2)origin.position;
        ShootRope();
    }
    
    private void ShootRope() //Start the rope shooting
    {
        LinePointsToFirePoint();
        LineRenderer.enabled = true;
        _canStartRopeAnimation = true;
    }

    private void LinePointsToFirePoint() //Sets the point of the precision of the rope
    {
        for (int i = 0; i < LineData.RopeDetailAmount; i++)
        {
            LineRenderer.SetPosition(i, origin.position);
        }
    }

    private void DrawRope() //Sequences the activation of the gravity
    {
        if (!_straightLine)
        {
            if (LineRenderer.GetPosition(LineData.RopeDetailAmount - 1).x == _grapplePoint.x)
            {
                _straightLine = true;
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (_canGrapple)
            {
                _canGrapple = false;
            }
            else
            {
                DrawRopeWaves();
            }
            
            if (_waveSize > 0)
            {
                _waveSize -= Time.deltaTime * LineData.StraightenLineSpeed;
                DrawRopeWaves();
            }
        }
    }
    
    private void DrawRopeWaves() //Draw the rope depending on the number of point of precision, reducing the precision each time it loops
    {
        for (int i = 0; i < LineData.RopeDetailAmount; i++)
        {
            float delta = i / (LineData.RopeDetailAmount - 1f);
            Vector2 offset = Vector2.Perpendicular(_grappleDistanceVector).normalized * (float)(LineData.RopeAnimationCurve.Evaluate(delta) * _waveSize);
            Vector2 targetPosition = Vector2.Lerp(origin.position, _grapplePoint, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(origin.position, targetPosition, LineData.RopeProgressionCurve.Evaluate(_moveTime) * LineData.RopeProgressionSpeed);
    
            LineRenderer.SetPosition(i, currentPosition);
            //Player.Hook.transform.position = currentPosition;

            //UpdateHookPosition(_ray);
        }
    }
    
}
