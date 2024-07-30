using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Record Key", menuName = "Record Key/Player Record Key")]
public class RecordKeyConfig : ScriptableObject
{
    [Header("Move Left")]
    public KeyCode moveLeft;
    [Header("Move Right")]
    public KeyCode moveRight;
    [Header("Jump")]
    public KeyCode jump;
    [Header("Attack")]
    public KeyCode attack;
}
