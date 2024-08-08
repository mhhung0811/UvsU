﻿using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private RecordManager recordManager;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject iterator;
    [SerializeField] private RecordKeyConfig playerKeys;

    private GameObject actor;

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
        //bool has_click_left = false;
        //bool has_click_right = false; 
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(recordManager.StartRecord(5));
        }
        //else if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    recordManager.EndRecord();
        //    Debug.Log("End Record");
        //}
        if (Input.GetKeyDown(KeyCode.Y))
        {
            iterator.SetActive(true);
            recordManager.RunRecord(recordManager.Records[0], iterator);
        }

        if (Input.GetKey(playerKeys.jump))
        {
            player.GetComponent<PlayerMovement>().HandleStartJump();
        }

        if (Input.GetKeyUp(playerKeys.jump))
        {
            player.GetComponent<PlayerMovement>().HandleEndJump();
        }

        bool moveLeft = Input.GetKey(playerKeys.moveLeft);
        bool moveRight = Input.GetKey(playerKeys.moveRight);

        
        if( moveLeft ^ moveRight )
        {
            // Kiểm tra phím di chuyển trái
            if (moveLeft)
            {
                player.GetComponent<PlayerMovement>().HandleMovement(-1);
                //Debug.Log("Left");
            }

            // Kiểm tra phím di chuyển phải
            if (moveRight)
            {
                player.GetComponent<PlayerMovement>().HandleMovement(1);
                //Debug.Log("Right");
            }
        }
        if (Input.GetKeyDown(playerKeys.attack))
        {
            player.GetComponent<PlayerAttack>().HandleAttack();
        }


        /*if(!has_click_left  &&  !has_click_right) 
        {
            player.GetComponent<PlayerAnimation>().SetBoolRunning(false);
        }
        bool ground_check = player.GetComponent<PlayerMovement>().GroundCheck();
        if (ground_check)
        {
            player.GetComponent<PlayerAnimation>().SetBoolGround(true);
        }*/
    }
}
