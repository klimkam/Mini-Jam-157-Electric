using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This is a wrapper class for Keys, Keys are created as an asset that can be linked to a <c>KeyMapData</c>.
/// </summary>

[CreateAssetMenu(fileName = "newKey", menuName = "Data/Inputs/Key")]
public class KeyData : ScriptableObject {
    [SerializeField] private KeyCode _key;
    [SerializeField] private float _lastInputTime;
    private const float _doubleTapThreshold = 0.5f;

    private bool lastFrame;
    
    public bool IsKeyDown() {
        SetLastInputTime();
        return Input.GetKeyDown(_key);
    }

    public bool IsKeyLifted() {
        return Input.GetKeyUp(_key);
    }

    public bool HasKeyDoubleTapped() {
        float timeSinceLastPress = Time.time - _lastInputTime;
        return timeSinceLastPress <= _doubleTapThreshold;
    }
    

    private void SetLastInputTime() {
        if (Input.GetKeyDown(_key) && lastFrame == false) {
            _lastInputTime = Time.time;
        }
        lastFrame = Input.GetKeyDown(_key);
    }
}
