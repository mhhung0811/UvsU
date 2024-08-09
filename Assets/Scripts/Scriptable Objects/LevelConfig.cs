using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Record Key", menuName = "Level Config/Basic Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Iteration times")]
    public List<float> iterTimes;
    [Header("Iteration positions")]
    public List<Vector3> iterPositions;
}
