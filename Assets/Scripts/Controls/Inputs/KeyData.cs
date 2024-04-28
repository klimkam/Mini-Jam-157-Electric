using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// This is a wrapper class for Keys, Keys are created as an asset that can be linked to a <c>KeyMapData</c>.
/// </summary>

[CreateAssetMenu(fileName = "newKey", menuName = "Data/Inputs/Key")]
public class KeyData : ScriptableObject {
    [SerializeField] private KeyCode _key;
    
    private float doubleTapCooldown = 0;

    public bool IsKeyDown() {
        return Input.GetKey(_key);
    }

    public bool IsKeyLifted() {
        return Input.GetKeyUp(_key);
    }

    public bool IsKeyDownThisFrame() {
        return Input.GetKeyDown(_key);
    }

    public bool HasKeyDoubleTapped() {
        if (doubleTapCooldown > 0) {
            doubleTapCooldown -=  Time.deltaTime;
        }

        if (Input.GetKeyDown(_key)) {
            if (doubleTapCooldown > 0)
            {
                doubleTapCooldown = 0;
                return true;
            }

            doubleTapCooldown = 0.3f;
        }

        return false;
    }
    
}
