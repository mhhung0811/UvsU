using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor);
}
