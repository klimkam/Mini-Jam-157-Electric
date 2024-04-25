using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    public UnityEvent timerFinished;
    
    float _maxTime = 0f;
    float _currentTime = 0f;

    bool _finishedFirstTime = false;

    public Timer(float maxTime) {
        timerFinished = new UnityEvent();
        MaxTime = maxTime;
    }

    public float MaxTime { 
        get { return _maxTime; }
        set {
            if (value <= 0) { 
                _maxTime = 0;
                return;
            }
            _maxTime = value; 
        }
    }

    public float CurrentTime { 
        get { return _currentTime; }
        set
        {
            if (value <= 0)
            {
                if( _finishedFirstTime )
                {
                    _finishedFirstTime = false;
                    timerFinished.Invoke();
                }
                _currentTime = 0;
                return;
            }
            _currentTime = value;
        }
    }

    public void ResetTimer()
    {
        CurrentTime = MaxTime;
        _finishedFirstTime = true;
    }

    public void Tick() {
        if (CurrentTime <= 0f) return;
        CurrentTime -= Time.deltaTime;
    }
}
