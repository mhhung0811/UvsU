using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor)
    {   
        actor.GetComponent<PlayerAttack>().HandleAttack();
        yield return null;
    }

    public AttackAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
    }
}
