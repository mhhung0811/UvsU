using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor)
    {
        //Debug.Log("Start Attack");
        //float i = 0;
        //while (i < actionTime)
        //{
        //    i += Time.deltaTime;

        //    //Debug.Log("Running Left");
        //    obj.GetComponent<PlayerAttack>().HandleAttack();

        //    yield return new WaitForEndOfFrame();
        //}
        
        actor.GetComponent<PlayerAttack>().HandleAttack();
        
        //Debug.Log("End Attack");
        yield return null;
    }

    public AttackAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
    }
}
