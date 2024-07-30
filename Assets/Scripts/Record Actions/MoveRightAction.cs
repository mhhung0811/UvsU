using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }
    private GameObject obj;

    public IEnumerator Execute()
    {
        Debug.Log("Start Right");
        float i = 0;
        while (i < actionTime)
        {
            i += Time.deltaTime;

            //Debug.Log("Running Left");
            //obj.GetComponent<PlayerMovement>().HandleMovement(1);

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("End Right");
        yield return null;
    }

    public MoveRightAction(float startTime, float actionTime, GameObject obj)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
        this.obj = obj;
    }
}
