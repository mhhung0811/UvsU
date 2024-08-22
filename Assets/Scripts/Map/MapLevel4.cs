using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLevel4 : Map
{
    [SerializeField] private List<TriggerWallState> _trigger_walls;
    [SerializeField] private List<GameObject> _trigger_point;
    public override void ReLoadMap()
    {
        foreach(TriggerWallState trigger_wall in _trigger_walls)
        {
            trigger_wall.trigger_wall.SetActive(trigger_wall.state);
        }
        foreach(GameObject trigger_point in _trigger_point)
        {
            trigger_point.SetActive(true);
        }
    }
}
