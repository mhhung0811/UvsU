using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [SerializeField] private List<FSMState> fsmStates = new List<FSMState>();
    private FSMState _curent_state;
    private string _init_state = "Idle";
    private void Start()
    {
        LoadInitState();
    }
    void LoadInitState()
    {
        _curent_state = GetState(_init_state);
    }
    public void ChangeState(string state_id)
    {
        FSMState state = GetState(state_id);
        if(state == null)
        {
            return;
        }
        _curent_state = state;
    }
    private FSMState GetState(string state_id)
    {
        foreach(FSMState state in fsmStates)
        {
            if(state.IDstate == state_id)
            {
                return state;
            }
        }
        return null;
    }

}
