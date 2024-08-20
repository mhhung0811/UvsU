using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightAction : IAction
{
    public float startTime { get; set; }
    public float actionTime { get; set; }

    public IEnumerator Execute(GameObject actor)
    {
        //Debug.Log("Start Right");
        float i = 0;
        while (i < actionTime)
        {
            i += Time.deltaTime;

            //Debug.Log("Running Left");
            bool has_click_right;
            actor.GetComponent<PlayerMovement>().HandleMovement(1);
            has_click_right = true;

            if (!has_click_right)
            {
                actor.GetComponent<PlayerAnimation>().SetBoolRunning(false);
            }
            bool ground_check = actor.GetComponent<PlayerMovement>().GroundCheck();
            if (ground_check)
            {
                actor.GetComponent<PlayerAnimation>().SetBoolGround(true);
            }

            yield return new WaitForEndOfFrame();
        }
        //Debug.Log("End Right");
        yield return null;
    }

    public MoveRightAction(float startTime, float actionTime)
    {
        this.startTime = startTime;
        this.actionTime = actionTime;
    }
}
