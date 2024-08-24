using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FSMState
{
    public string id_state;
    public List<IFSMAction> _actions = new List<IFSMAction>();
    public List<FSMTransition> _transitions = new List<FSMTransition>();

    public void UpdateStateEnemy(EnemyCore core)
    {
        ExecuteAction();
        ExecuteTransition(core);
    }
    private void ExecuteAction()
    {
        foreach(IFSMAction action in _actions)
        {
            action.Action();
        }
    }
    private void ExecuteTransition(EnemyCore core)
    {
        foreach(FSMTransition transition in _transitions)
        {
            bool check = transition.decide.Decision();
            if(check)
            {
                core.ChangeState(transition.true_state);
            }
            else
            {
                core.ChangeState(transition.false_state);
            }
        }
    }
}
