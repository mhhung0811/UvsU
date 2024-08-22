using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FSMTransition 
{
    public IFSMDecision decide;
    public string true_state;
    public string false_state;
}
