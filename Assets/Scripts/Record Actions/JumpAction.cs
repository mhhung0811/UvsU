using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor)
    {
        Debug.Log("Start Jump");
        float i = 0;
        //while (i < actionTime)
        //{
        //    i += Time.deltaTime;

        //    //Debug.Log("Running Left");
        //    obj.GetComponent<PlayerMovement>().HandleJump();

        //    yield return new WaitForEndOfFrame();
        //}
        actor.GetComponent<PlayerMovement>().HandleJump();
        Debug.Log("End Jump");
        yield return null;
    }

    public JumpAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;        
    }
}
