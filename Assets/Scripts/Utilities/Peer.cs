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
        _hub.SendMessage(message, this, obj);
    }

    public abstract void Receive(string message);
}
