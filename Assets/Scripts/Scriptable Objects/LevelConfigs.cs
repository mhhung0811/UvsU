using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevevConfigs", menuName = "Level/LevelConfigs")]
public class LevelConfigs : ScriptableObject
{
    public List<LevelConfig> all_level_configs;
}

[System.Serializable]
public class LevelConfig
{
    [Header("Level Id")]
    public int level_id;
    [Header("Map")]
    public GameObject map;
    [Header("Iteration times")]
    public List<float> iterTimes;
}
