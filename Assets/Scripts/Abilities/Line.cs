using System;
using UnityEngine;

public class Line : MonoBehaviour {
    [SerializeField] private Transform origin;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineData lineData;
    [SerializeField] private ERelationType directionType;
    
    private Vector2 _grapplePoint, _grappleDistanceVector;
    private float _moveTime, _waveSize;
    private bool _canGrapple, _canStartRopeAnimation;

    private void OnEnable() {
        Initialize();
        SetGrapplePoint();
    }

    private void Initialize() {
        _moveTime = 0;
        lineRenderer.positionCount = lineData.RopeDetailAmount;
        _waveSize = lineData.StartWazeSize;
    }

    private void Update() {
        if (!_canStartRopeAnimation) return;
        
        DrawRope();
        _moveTime += Time.fixedDeltaTime;
    }
    
    private void SetGrapplePoint() {
        var hit = Physics2D.Raycast(origin.position, GetShootingDirection(), 100); //Arbitrary distance 

        if (hit.transform != null) {
            var objectHit = hit.transform.GetComponent<AnchorPoint>();

            if (objectHit) {
                objectHit.OnHook();
                _grapplePoint = objectHit.transform.position; //TODO Change it to match the center or wanted position of the anchor/wall connector
            }
            else {
                _grapplePoint = hit.collider.ClosestPoint(hit.point);
            }
        }
        else {
            _grapplePoint = origin.position * (GetShootingDirection() * 10);
        }
        
        _grappleDistanceVector = _grapplePoint - (Vector2)origin.position;
        ShootRope();
    }

    private Vector2 GetShootingDirection() {
        return directionType switch {
            ERelationType.NORTH => Vector2.up,
            ERelationType.SOUTH => Vector2.down,
            ERelationType.EAST => Vector2.left,
            ERelationType.WEST => Vector2.right,
            ERelationType.COUNT => throw new ArgumentOutOfRangeException(),
            _ => Vector2.zero
        };
    }

    private void ShootRope() {
        LinePointsToFirePoint();
        lineRenderer.enabled = true;
        _canStartRopeAnimation = true;
    }

    private void LinePointsToFirePoint() {
        for (int i = 0; i < lineData.RopeDetailAmount; i++) {
            lineRenderer.SetPosition(i, origin.position);
        }
    }

    private void DrawRope() {
        DrawRopeWaves();
        if (!(_waveSize > 0)) return;
        
        _waveSize -= Time.deltaTime * lineData.StraightenLineSpeed;
    }
    
    private void DrawRopeWaves() {
        for (int i = 0; i < lineData.RopeDetailAmount; i++) {
            float delta = i / (lineData.RopeDetailAmount - 1f); //So we don't go out of bounds
            Vector2 offset = Vector2.Perpendicular(_grappleDistanceVector).normalized * (lineData.RopeAnimationCurve.Evaluate(delta) * _waveSize);
            Vector2 targetPosition = Vector2.Lerp((Vector2)origin.position + offset, _grapplePoint + offset, delta);
            Vector2 currentPosition = Vector2.Lerp(origin.position, targetPosition, lineData.RopeProgressionCurve.Evaluate(_moveTime) * lineData.RopeProgressionSpeed);
    
            lineRenderer.SetPosition(i, currentPosition);
            //Player.Hook.transform.position = currentPosition;

            UpdateHookPosition(GetShootingDirection());
        }
    }

    private void UpdateHookPosition(Vector2 dir) {
        var direction = dir.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Player.Hook.transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }
}

