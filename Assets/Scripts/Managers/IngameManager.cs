using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngameManager : MonoBehaviour, IHub
{
    private float _max_time;
    private float _timer;

    private int _max_iteration;
    private int _max_iterator;
    private int _current_iteration;
    private int _current_iterator;

    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private List<Peer> _peers;

    [SerializeField] private GameSceneUIManager _gameSceneUIManager;
    [SerializeField] private InputManager _inputManager;
    private RecordManager _recordManager;

    private List<GameObject> _iterators;
    private List<List<IAction>> _whiteRecords;
    private List<List<IAction>> _blackRecords;

    // Start is called before the first frame update
    void Start()
    {
        LoadComponent();
        PrepareIteration();
        _gameSceneUIManager.InitiateIconIter(_max_iteration);
        AudioManager.Instance.PlayBGM(0);
    }

    private void LoadComponent()
    {
        _current_iteration = -1;
        _current_iterator = -1;
        _max_iteration = _levelConfig.iterTimes.Count;
        _max_iterator = _levelConfig.iterPositions.Count;

        _recordManager = GetComponent<RecordManager>();
        _iterators = new List<GameObject>();
        _whiteRecords = new List<List<IAction>>();
        _blackRecords = new List<List<IAction>>();

        foreach (Peer item in _peers)
        {
            Register(item);
        }
    }

    // Update is called once per frame
    void Update()
    {

        CountDownTimer();
    }

    private void CountDownTimer()
    {
        _timer -= Time.deltaTime;
        _gameSceneUIManager.SetTimerText(_timer);
        _gameSceneUIManager.SetTimeSlider(_timer, _max_time);
    }

    public void PrepareIteration()
    {
        _current_iteration++;
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

        // Set timer
        _max_time = _levelConfig.iterTimes[_current_iteration];
        _timer = _max_time;

        if (_current_iteration >= _max_iteration)
        {
            Debug.Log("You win");
        }
    }

    public void BackToPreviousIteration()
    {
        _current_iteration -= 2;
        _current_iterator = (_current_iteration % 2 == 0) ? 0 : (_current_iteration + 1) / 2;

        PrepareIteration();
    }

    public void StartIteration()
    {                
        // Even is white
        if (_current_iteration % 2 == 0)
        {
            // Start recording
            StartCoroutine(_recordManager.StartRecord(_max_time, _whiteRecords));

            // Run record if has
            for (int i = 0; i < _blackRecords.Count; i++)
            {
                _recordManager.RunRecord(_blackRecords[i], _iterators[i + 1]);
            }
        }
        // Odd is black
        else
        {
            // Start recording
            StartCoroutine(_recordManager.StartRecord(_max_time, _blackRecords));

            // Run record if has
            _recordManager.RunRecord(_whiteRecords[0], _iterators[0]);
            for (int i = 0; i < _blackRecords.Count - 1; i++)
            {
                _recordManager.RunRecord(_blackRecords[i], _iterators[i + 1]);
            }
        }

        _gameSceneUIManager.DisableUpperText();
    }

    public void EndIteration()
    {
        _inputManager.FreePlayer();
        _recordManager.isStop = true;
        PrepareIteration();
        _gameSceneUIManager.EnableUpperText();
        StartCoroutine(_inputManager.WaitToStartGame());
    }

    public GameObject GetCurrentPlayer()
    {
        if (_iterators.Count >= _current_iterator)
        {
            Debug.Log(_iterators.Count);
            Debug.Log(_iterators[_current_iterator]);
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
            Debug.Log("Complete");
            EndIteration();
        }
    }

    public void Register(Peer peer)
    {
        //_components.Add(component);
        peer.SetMediator(this);
    }
}
