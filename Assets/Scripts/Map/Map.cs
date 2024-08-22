using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Map : MonoBehaviour
{
    public List<Transform> list_spawn_point;
    public abstract void ReLoadMap();
}
