using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHub
{
    void SendMessage(string message, Peer sender, GameObject obj);
    void Register(Peer component);
}
