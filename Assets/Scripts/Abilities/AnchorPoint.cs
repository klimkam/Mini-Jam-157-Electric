using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ERelationType {
    NORTH,
    SOUTH,
    EAST,
    WEST,
    COUNT
}

public class AnchorPoint : MonoBehaviour {
    [SerializeField] public ERelationType anchorPointType = ERelationType.COUNT;
    [SerializeField] private WallConnector connector;

    private void OnEnable() {
        if (anchorPointType == ERelationType.COUNT) {
            Debug.LogError("Object with name " + gameObject.name + " and position " + transform.position + " doesn't have a anchor point type setup.");
        }

        connector = gameObject.transform.parent.gameObject.GetComponentInChildren<WallConnector>();
    }

    public void OnHook(PlayerController player) {
        if (connector.GetColor() != Color.white) {
            print(connector.GetColor() + " " + connector.transform.parent.name);
            
            return;
        }
        player.DeactivateAllLines();
    }
}

