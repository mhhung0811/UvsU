﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngameManager : MonoBehaviour, IHub
{
    private float _max_time;
    private float _timer;

    private int _max_iteration;
    private int _max_iterator;
    [SerializeField] private int _current_iteration;
    private int _current_iterator;

    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private List<Peer> _peers;

    [SerializeField] private GameSceneUIManager _gameSceneUIManager;
    [SerializeField] private InputManager _inputManager;
    private RecordManager _recordManager;

    private List<GameObject> _iterators;
    private List<List<IAction>> _whiteRecords;
    private List<List<IAction>> _blackRecords;

    private Coroutine _white_record;
    private List<Coroutine> _black_records;

    private Coroutine _timer_coroutine;

    // Start is called before the first frame update
    void Start()
    {
        LoadComponent();
        PrepareIteration();
        AudioManager.Instance.PlayBGM(0);
    }

    private void LoadComponent()
    {
        _current_iteration = 0;
        _current_iterator = -1;
        _max_iteration = _levelConfig.iterTimes.Count;
        _max_iterator = _levelConfig.iterPositions.Count;

        //Initiate list icon iteration
        _gameSceneUIManager.InitiateIconIter(_max_iteration);

        _recordManager = GetComponent<RecordManager>();
        _iterators = new List<GameObject>();
        _whiteRecords = new List<List<IAction>>();
        _blackRecords = new List<List<IAction>>();

        foreach (Peer item in _peers)
        {
            Register(item);
        }
    }

    IEnumerator CountDownTimer()
    {
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            SetTimerUI();
            yield return new WaitForEndOfFrame();
        }
        if(_timer <= 0)
        {
            /*RestartIteration();*/
            StartCoroutine(IterationFailure());

        }
        yield return null;
    }

    private void SetTimerUI()
    {
        _gameSceneUIManager.SetTimerText(_timer);
        _gameSceneUIManager.SetTimeSlider(_timer, _max_time);
    }

    public void PrepareIteration()
    {
        PrepareUI();
        _current_iterator = (_current_iteration % 2 == 0) ? 0 : (_current_iteration + 1) / 2;

        // Clear old iterator
        foreach (GameObject iter in _iterators)
        {
            IterSpawner.Instance.Despawn(iter.transform);
        }

        // Spawn new iterator
        for (int i = 0; i <= (_current_iteration + 1) / 2; i++)
        {
            GameObject obj = null;
            //Debug.Log(i);
            if (i == 0)
            {
                obj = IterSpawner.Instance.Spawn("IterWhite", _levelConfig.iterPositions[i], Vector3.zero, 1);
                if (obj != null)
                {
                    if (_iterators.Count > 0)
                    {
                        _iterators[0] = obj;
                    }
                    else
                    {
                        _iterators.Add(obj);
                    }
                }
            }
            else
            {
                obj = IterSpawner.Instance.Spawn("IterBlack", _levelConfig.iterPositions[i], Vector3.zero, -1);
                if (obj != null) _iterators.Add(obj);
            }
        }
        // Set player
        _inputManager.SetPlayer(GetCurrentPlayer());

        if (_current_iteration >= _max_iteration)
        {
            Debug.Log("You win");
        }
    }
    private void PrepareUI()
    {
        //Enable move to start
        _gameSceneUIManager.EnableUpperText();

        //Set iter text UI
        _gameSceneUIManager.SetIterText(_current_iteration + 1, _max_iteration);
        _gameSceneUIManager.SetIconIter(_current_iteration + 1);

        // Set timer
        _max_time = _levelConfig.iterTimes[_current_iteration];
        _timer = _max_time;
        SetTimerUI();
    }
    public void BackToPreviousIteration()
    {
        StopAnyLogicCoroutine();
        _current_iteration -= 1;
        //_current_iterator = (_current_iteration % 2 == 0) ? 0 : (_current_iteration + 1) / 2;
        PrepareIteration();
        StartCoroutine(_inputManager.WaitToStartGame());
    }
    private void StopAnyLogicCoroutine()
    {
        // End timer
        StopCoroutine(_timer_coroutine);
        //End record 
        _recordManager.isStop = true;
        // Disable keyboard
        _inputManager.FreePlayer();

        if(_white_record !=  null)
        {
            StopCoroutine(_white_record);
        }
        if(_black_records != null)
        {
            foreach (Coroutine record in _black_records)
            {
                StopCoroutine(record);
            }
            _black_records.Clear();
        }
        
        
    }
    public void StartIteration()
    {
        // Debug.Log(_whiteRecords.Count);
        // Debug.Log(_blackRecords.Count);
        Debug.Log("Start Iteration");
        // Even is white
        Debug.Log("Current iteration : " + _current_iteration);
        if (_current_iteration % 2 == 0)
        {
            // Start recording
            StartCoroutine(_recordManager.StartRecord(_max_time, _whiteRecords));

            // Run record if has
            for (int i = 0; i < _blackRecords.Count; i++)
            {
                Coroutine record =  _recordManager.RunRecord(_blackRecords[i], _iterators[i + 1]);
                _black_records.Add(record);
            }
        }
        // Odd is black
        else
        {
            // Start recording
            StartCoroutine(_recordManager.StartRecord(_max_time, _blackRecords));

            // Run record if has
            _white_record =  _recordManager.RunRecord(_whiteRecords[0], _iterators[0]);
            for (int i = 0; i < _blackRecords.Count - 1; i++)
            {
                Coroutine record = _recordManager.RunRecord(_blackRecords[i], _iterators[i + 1]);
                _black_records.Add(record);
            }
        }

        _gameSceneUIManager.DisableUpperText();
        _timer_coroutine = StartCoroutine(CountDownTimer());
    }

    public void EndIteration()
    {
        StopAnyLogicCoroutine();

        _current_iteration++;
        PrepareIteration();
        StartCoroutine(_inputManager.WaitToStartGame());
    }

    public GameObject GetCurrentPlayer()
    {
        if (_iterators.Count >= _current_iterator)
        {
            //Debug.Log(_iterators.Count);
            //Debug.Log(_iterators[_current_iterator]);
            Debug.Log("Current iterator : " + _current_iterator);
            return _iterators[_current_iterator];

        }
        else
        {
            Debug.Log("player null at getCurrentPlayer");
            return null;
        }
    }

    public void SendMessage(string message, Peer sender)
    {
        // Resolve message here
        if (message == IngameMessage.Complete.ToString())
        {
            //Iter white in even iteration reachs gate
            if (_current_iteration % 2 == 0)
            {
                Debug.Log("Complete");
                EndIteration();
            }
            //Iter white in odd iteration reachs gate
            else
            {
                RestartIteration();
            }
        }
    }

    public void Register(Peer peer)
    {
        //_components.Add(component);
        peer.SetMediator(this);
    }
    IEnumerator IterationFailure()
    {
        StopAnyLogicCoroutine();
        _gameSceneUIManager.EnableFailedText();
        yield return new WaitForSeconds(1f);
        _gameSceneUIManager.DisableFailedText();
        PrepareIteration();
        StartCoroutine(_inputManager.WaitToStartGame());
        yield break;
    }
    public void RestartIteration()
    {
        StartCoroutine(IterationFailure());
    }
    public void CheckBulletTrigger(GameObject obj)
    {
        if(obj == _iterators[_current_iterator])
        {
            RestartIteration();
        }
        else if((obj != _iterators[_current_iterator]) && (obj == _iterators[0]))
        {
            EndIteration();
            _whiteRecords.Clear();
            Debug.Log("End iteration by shooting");
        }
    }
}
