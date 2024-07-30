using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    [SerializeField] private RecordKeyConfig recordKeys;
    [SerializeField] private GameObject player;

    private Dictionary<KeyCode, int> keyPos;
    private List<IAction> actions;
    private float timer;
    private bool isRecord;

    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        isRecord = false;

        keyPos = new Dictionary<KeyCode, int>()
        {
            {recordKeys.moveLeft, 0 },
            {recordKeys.moveRight, 0 },
            {recordKeys.jump, 0 },
            {recordKeys.attack, 0 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRecord) return;

        timer += Time.deltaTime;

        if (Input.GetKeyDown(recordKeys.moveLeft))
        {
            StartCoroutine(EndAction(recordKeys.moveLeft, timer));
        }
        if (Input.GetKeyDown(recordKeys.moveRight))
        {
            StartCoroutine(EndAction(recordKeys.moveRight, timer));
        }
        if (Input.GetKeyDown(recordKeys.jump))
        {
            StartCoroutine(EndAction(recordKeys.jump, timer));
        }
        if (Input.GetKeyDown(recordKeys.attack))
        {
            StartCoroutine(EndAction(recordKeys.attack, timer));
        }
    }

    public void StartRecord()
    {
        isRecord = true;
    }

    public void EndRecord()
    {
        isRecord = false;
    }

    public void RunRecord(List<IAction> actions)
    {
        if (actions.Count > 0)
        {
            StartCoroutine(Run(actions));
            //foreach (IAction action in actions)
            //{
                //StartCoroutine(action.Execute());
                //if (action.GetType() == typeof(MoveRight))
                //{
                //    Debug.Log("Move right");
                //}
                //else if (action.GetType() == typeof(MoveLeft))
                //{
                //    Debug.Log("Move left");
                //}
                //Debug.Log(action.time);
            //}
        }
    }

    IEnumerator StartAction(KeyCode keycode, float startTime, Action<float> callback)
    {
        IAction action;
        float timer = 0f;
        Debug.Log("Recording");
        if (keycode == recordKeys.moveLeft)
        {
            action = new MoveLeftAction(startTime, 0f, player);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.moveRight)
        {
            action = new MoveRightAction(startTime, 0f, player);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.jump)
        {
            action = new JumpAction(startTime, 0f, player);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.attack)
        {
            action = new AttackAction(startTime, 0f, player);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        
        while (Input.GetKey(keycode))
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //yield return new WaitUntil(() => Input.GetKeyUp(keycode));
        callback(timer);
    }

    IEnumerator EndAction(KeyCode keycode, float startTime)
    {
        float timer = 0;
        yield return StartCoroutine(StartAction(keycode, startTime, result => { timer = result; }));
        if (keycode == recordKeys.moveLeft ||
            keycode == recordKeys.moveRight ||
            keycode == recordKeys.jump ||
            keycode == recordKeys.attack)
        {
            actions[keyPos[keycode]].actionTime = timer;
        }        
        Debug.Log("End record");
    }

    IEnumerator Run(List<IAction> actions)
    {
        float timer = 0;
        int i = 0;

        while (i < actions.Count)
        {
            timer += Time.deltaTime;
            if (timer >= actions[i].startTime)
            {
                StartCoroutine(actions[i].Execute());
                i++;
            }
            yield return new WaitForEndOfFrame();
        }

        //foreach (var action in actions)
        //{
        //    yield return new WaitForSeconds(action.startTime);
        //    StartCoroutine(action.Execute());
        //}
    }
}
