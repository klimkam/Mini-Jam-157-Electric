using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTester : MonoBehaviour
{
    [SerializeField]
    float _time;

    Timer _timer;

    private void Start()
    {
        _timer = new Timer(_time);
        _timer.timerFinished.AddListener(TimerDebugger);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _timer.ResetTimer();
        }
        _timer.Tick();
    }

    private void TimerDebugger() {
        Debug.Log("Timer Ended for Timer Tester with timer of " + _time);
    }
}
