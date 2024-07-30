using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private RecordManager recordManager;
    [SerializeField] private GameObject player;
    [SerializeField] private RecordKeyConfig playerKeys;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            recordManager.StartRecord();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            recordManager.EndRecord();
        }

        if (Input.GetKey(playerKeys.moveLeft))
        {
            //player.GetComponent<PlayerMovement>().HandleMovement(-1);
        }
        if (Input.GetKey(playerKeys.moveRight))
        {
            //player.GetComponent<PlayerMovement>().HandleMovement(1);
        }
        if (Input.GetKeyDown(playerKeys.jump))
        {
            //player.GetComponent<PlayerMovement>().HandleJump(true);
        }
    }
}
