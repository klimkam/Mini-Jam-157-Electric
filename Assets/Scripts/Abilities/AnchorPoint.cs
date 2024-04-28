using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ERelationType {
    NORTH,
    SOUTH,
    EAST,
    WEST,
    COUNT
}

public class AnchorPoint : MonoBehaviour
{
    [SerializeField] public ERelationType anchorPointType = ERelationType.COUNT;

    private void OnEnable() {
        if (anchorPointType == ERelationType.COUNT) {
            Debug.LogError("Object with name " + gameObject.name + " and position " + transform.position + " doesn't have a anchor point type setup.");
        }
    }

    public void OnHook()
    {
    }
}

