using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newKeyMap", menuName = "Data/Inputs/KeyMap")]
public class KeyMapData : ScriptableObject {
    public KeyData upKey;
    public KeyData downKey;
    public KeyData leftKey;
    public KeyData rightKey;

    public KeyData menuKey;

    public KeyData ropeUpKey;
    public KeyData ropeDownKey;
    public KeyData ropeLeftKey;
    public KeyData ropeRightKey;
}
