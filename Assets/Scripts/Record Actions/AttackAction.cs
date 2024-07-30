using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }
    private GameObject obj;

    public IEnumerator Execute()
    {
        Debug.Log("Start Attack");
        float i = 0;
        while (i < actionTime)
        {
            i += Time.deltaTime;

            //Debug.Log("Running Left");
            //obj.GetComponent<PlayerAttack>().Attack();

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("End Attack");
        yield return null;
    }

    public AttackAction(float startTime, float actionTime, GameObject obj)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
        this.obj = obj;
    }
}
