using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartJumpAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }
    public IEnumerator Execute(GameObject actor)
    {
        //Debug.Log("Start Jump");
        float i = 0;
        while (i < actionTime)
        {
            i += Time.deltaTime;

            //Debug.Log("Running Left");
            actor.GetComponent<PlayerMovement>().HandleStartJump();

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    public StartJumpAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
    }
}
