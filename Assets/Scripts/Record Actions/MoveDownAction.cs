using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor)
    {
        actor.GetComponent<PlayerMovement>().FallDown();
        yield return null;
    }

    public MoveDownAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
    }
}
