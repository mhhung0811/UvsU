using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Peer : MonoBehaviour
{
    protected IHub _hub;

    public void SetMediator(IHub hub)
    {
        _hub = hub;
    }

    public void Send(string message, GameObject obj)
    {
        if(_hub == null)
        {
            Debug.Log("can not find hub");
        }
        _hub.SendMessage(message, this, obj);
    }

    public abstract void Receive(string message);
}
