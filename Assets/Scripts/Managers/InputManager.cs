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
        HandleInput();
    }
    public void HandleInput()
    {
        bool has_click_left = false;
        bool has_click_right = false;
        if (Input.GetKeyDown(KeyCode.T))
        {
            recordManager.StartRecord();
            Debug.Log("Start Record");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            recordManager.EndRecord();
            Debug.Log("End Record");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            recordManager.RunRecord(recordManager.actions);
        }

        if (Input.GetKey(playerKeys.moveLeft))
        {
            player.GetComponent<PlayerMovement>().HandleMovement(-1);
            has_click_left = true;
        }
        if (Input.GetKey(playerKeys.moveRight))
        {
            player.GetComponent<PlayerMovement>().HandleMovement(1);
            has_click_right = true;
        }
        if (Input.GetKeyDown(playerKeys.jump))
        {
            player.GetComponent<PlayerMovement>().HandleJump();
        }
        if (Input.GetKeyDown(playerKeys.attack))
        {
            player.GetComponent<PlayerAttack>().HandleAttack();
        }


        if(!has_click_left  &&  !has_click_right) 
        {
            player.GetComponent<PlayerAnimation>().SetBoolRunning(false);
        }
        bool ground_check = player.GetComponent<PlayerMovement>().GroundCheck();
        if (ground_check)
        {
            player.GetComponent<PlayerAnimation>().SetBoolGround(true);
        }
    }
}
