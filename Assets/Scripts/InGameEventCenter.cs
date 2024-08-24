using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEventCenter : MonoBehaviour
{
    private List<IObserver> observers = new List<IObserver>();

    public void Attach(IObserver item)
    {
        observers.Add(item);
    }
    public void Remove(IObserver item)
    {
        observers.Remove(item);
    }
    public void Notify()
    {
        foreach(IObserver item in observers)
        {
            item.UpdateState();
        }
    }
}
