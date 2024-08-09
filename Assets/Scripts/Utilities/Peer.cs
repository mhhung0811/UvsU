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

    public void Send(string message)
    {
        _hub.SendMessage(message, this);
    }

    public abstract void Receive(string message);
}
