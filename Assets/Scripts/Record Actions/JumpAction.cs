using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }
    private GameObject obj;

    public IEnumerator Execute()
    {
        Debug.Log("Start Jump");
        float i = 0;
        while (i < actionTime)
        {
            i += Time.deltaTime;

            //Debug.Log("Running Left");
            //obj.GetComponent<PlayerMovement>().HandleJump(true);

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("End Jump");
        yield return null;
    }

    public JumpAction(float startTime, float actionTime, GameObject obj)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
        this.obj = obj;
    }
}
