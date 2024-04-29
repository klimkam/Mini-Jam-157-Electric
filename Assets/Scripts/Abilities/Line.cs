using System;
using UnityEngine;

public class Line : MonoBehaviour {
    [SerializeField] private Transform origin;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineData lineData;
    [SerializeField] private ERelationType directionType;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform Hook;
    
    private Vector2 _grapplePoint, _grappleDistanceVector;
    private float _moveTime, _waveSize;
    private bool _canGrapple, _canStartRopeAnimation;

    private AnchorPoint _connection;
    
    private void OnEnable() {
        Initialize();
        SetGrapplePoint();
    }

    private void Initialize() {
        if (_playerController == null) {
            _playerController = transform.parent.GetComponent<PlayerController>();
        }

        Hook.position = origin.position;
        _connection = null;
        _moveTime = 0;
        lineRenderer.positionCount = lineData.RopeDetailAmount;
        _waveSize = lineData.StartWazeSize;
        _grapplePoint = Vector2.zero;
        _canStartRopeAnimation = false;
    }

    private void Update() {
        if (!_canStartRopeAnimation) return;
        
        DrawRope();
        _moveTime += Time.fixedDeltaTime;
    }
    
    private void SetGrapplePoint() {
        //var hit = Physics2D.Raycast(origin.position, GetShootingDirection(), 100, ~origin.transform.gameObject.layer); //Arbitrary distance 

        var hits = Physics2D.RaycastAll(origin.position, GetShootingDirection(), 100);

        Debug.DrawLine(origin.position,GetShootingDirection() + (Vector2)origin.position,Color.magenta);
        
        if (hits.Length > 1) {
            foreach (var hit in hits) {
                if ((hit.transform != null) && (hit.transform.gameObject.layer != origin.gameObject.layer) && hit.transform.gameObject.layer != LayerMask.NameToLayer("Walls")) {
                    var objectHit = hit.transform.GetComponent<AnchorPoint>();

                    if (objectHit) {
                        _grapplePoint = objectHit.transform.position; //TODO Change it to match the center or wanted position of the anchor/wall connector
                        _connection = objectHit;
                        print("Hooked wall connector");
                        _grappleDistanceVector = _grapplePoint - (Vector2)origin.position;
                        ShootRope();
                        return;
                    }
                }
            }
            
        }
        _playerController.DeactivateAllLines();
        
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
        
        if ((Vector2)lineRenderer.GetPosition(lineData.RopeDetailAmount - 1) == _grapplePoint) {
            _connection.OnHook(_playerController);
        }

    }
    
    private void DrawRopeWaves() {
        for (int i = 0; i < lineData.RopeDetailAmount; i++) {
            float delta = i / (lineData.RopeDetailAmount - 1f); //So we don't go out of bounds
            Vector2 offset = Vector2.Perpendicular(_grappleDistanceVector).normalized * (lineData.RopeAnimationCurve.Evaluate(delta) * _waveSize);
            Vector2 targetPosition = Vector2.Lerp((Vector2)origin.position + offset, _grapplePoint + offset, delta);
            Vector2 currentPosition = Vector2.Lerp(origin.position, targetPosition, lineData.RopeProgressionCurve.Evaluate(_moveTime) * lineData.RopeProgressionSpeed);

            Vector3 currentPos = new Vector3(currentPosition.x, currentPosition.y, -1);
            lineRenderer.SetPosition(i, currentPos);
            Hook.transform.position = currentPosition;

            UpdateHookPosition(GetShootingDirection());
        }
    }

    private void UpdateHookPosition(Vector2 dir) {
        var direction = dir.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Hook.transform.eulerAngles = new Vector3(0, 0, angle - 90);
    }
}

