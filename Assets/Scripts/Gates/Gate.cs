using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Peer
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Receive(string message)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        { 
            Debug.Log(collision.gameObject.name);
        }
        if (collision.CompareTag("Player"))
        {
            Send("win");
        }
    }
}
