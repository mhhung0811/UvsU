using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    [SerializeField] private RecordKeyConfig recordKeys;

    private Dictionary<KeyCode, int> keyPos;
    private Dictionary<KeyCode, bool> keyPressed;

    private List<float> recordTimes;
    private List<IAction> actions;
    public bool isStopRecording { private get; set; }


    void Start()
    {
        isStopRecording = false;

        actions = new List<IAction>();

        recordTimes = new List<float>();

        keyPos = new Dictionary<KeyCode, int>()
        {
            {recordKeys.moveLeft, 0 },
            {recordKeys.moveRight, 0 },
            {recordKeys.jump, 0 },
            {recordKeys.attack, 0 }
        };
        keyPressed = new Dictionary<KeyCode, bool>()
        {
            {recordKeys.moveLeft, false },
            {recordKeys.moveRight, false },
            {recordKeys.jump, false },
            {recordKeys.attack, false }
        };
    }

    public IEnumerator StartRecord(float time, List<List<IAction>> records)
    {
        isStopRecording = false;

        float timer = 0;

        //Debug.Log("Start Record");
        recordTimes.Add(time);

        //Debug.Log(recordTimes.Count);
        while (timer < time && !isStopRecording)
        {
            //Debug.Log(timer);
            if (Input.GetKey(recordKeys.moveLeft) && !keyPressed[recordKeys.moveLeft])
            {
                keyPressed[recordKeys.moveLeft] = true;
                StartCoroutine(EndAction(recordKeys.moveLeft, timer, time));
            }
            if (Input.GetKey(recordKeys.moveRight) && !keyPressed[recordKeys.moveRight])
            {
                //Debug.Log("record move right");
                keyPressed[recordKeys.moveRight] = true;
                StartCoroutine(EndAction(recordKeys.moveRight, timer, time));
            }
            if (Input.GetKey(recordKeys.jump) && !keyPressed[recordKeys.jump])
            {
                keyPressed[recordKeys.jump] = true;
                StartCoroutine(EndAction(recordKeys.jump, timer, time));
            }

            if (Input.GetKey(recordKeys.attack) && !keyPressed[recordKeys.attack])
            {
                keyPressed[recordKeys.attack] = true;
                StartCoroutine(EndAction(recordKeys.attack, timer, time));
            }
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // error occur when hold key out of end time
        // hot fix, might need improve
        yield return new WaitForSeconds(0.5f);
        EndRecord(records);

        //Debug.Log("End Record");
    }

    public void EndRecord(List<List<IAction>> records)
    {
        records.Add(new List<IAction>(actions));        
        actions.Clear();
    }

    IEnumerator StartAction(KeyCode keycode, float startTime, float endTime, Action<float> callback)
    {
        IAction action;
        float timer = 0f;
        //Debug.Log("Recording");
        if (keycode == recordKeys.moveLeft)
        {
            action = new MoveLeftAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.moveRight)
        {
            action = new MoveRightAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.jump)
        {
            action = new JumpAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        else if (keycode == recordKeys.attack)
        {
            action = new AttackAction(startTime, 0f);
            keyPos[keycode] = actions.Count;
            actions.Add(action);
        }
        while (Input.GetKey(keycode) && timer < endTime && !isStopRecording)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        callback(timer);
    }

    IEnumerator EndAction(KeyCode keycode, float startTime, float endTime)
    {
        float timer = 0;
        yield return StartCoroutine(StartAction(keycode, startTime, endTime, result => { timer = result; }));
        if (keycode == recordKeys.moveLeft ||
            keycode == recordKeys.moveRight ||
            keycode == recordKeys.jump ||
            keycode == recordKeys.attack)
        {
            actions[keyPos[keycode]].actionTime = timer;
            keyPressed[keycode] = false;
        }
        //Debug.Log("End action");
    }

    IEnumerator Run(List<IAction> actions, GameObject actor, Action<Coroutine> callback)
    {
        //Debug.Log("Run");

        float timer = 0;
        int i = 0;
        Coroutine actionCoroutine;

        while (i < actions.Count)
        {
            timer += Time.deltaTime;
            if (timer >= actions[i].startTime)
            {
                actionCoroutine = StartCoroutine(actions[i].Execute(actor));
                if (actionCoroutine != null)
                {
                    callback(actionCoroutine);
                }
                i++;
            }
            yield return new WaitForEndOfFrame();
        }
        actor.GetComponent<PlayerModel>().ChangeState();

        //Debug.Log("End Run");
    }

    public List<Coroutine> RunRecord(List<IAction> listAction, GameObject actor)
    {
        //Debug.Log(listAction.Count);
        List<Coroutine> listCoroutines = new List<Coroutine>();
        if (listAction.Count > 0)
        {
            Coroutine record = StartCoroutine(Run(listAction, actor, res => { listCoroutines.Add(res); }));
            Debug.Log("Run record, record count : " + listCoroutines.Count);
            listCoroutines.Add(record);
            
        }
        return listCoroutines;
    }
}
