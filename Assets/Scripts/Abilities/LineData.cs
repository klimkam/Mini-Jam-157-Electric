using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newLineData", menuName = "Data/LineData")]
public class LineData : ScriptableObject {
    [Header("Line Data")] 
    [TextArea] [SerializeField] private string _description;

    [field: SerializeField] public AnimationCurve RopeAnimationCurve { get; private set; }
    [field: SerializeField] public AnimationCurve RopeProgressionCurve { get; private set; }
    [field: SerializeField] public float StartWazeSize { get; private set; }
    [field: SerializeField] public float RopeProgressionSpeed { get; private set; }
    [field: SerializeField] public int RopeDetailAmount { get; private set; }
    [field: SerializeField] public float StraightenLineSpeed { get; private set; }
}
