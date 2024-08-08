using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndJumpAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor)
    {
        Debug.Log("End Jump");
        
        actor.GetComponent<PlayerMovement>().HandleEndJump();
        yield return null;
    }
    public EndJumpAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
    }
}
