using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour, IHub
{
    [Header("Timer")]
    [SerializeField] private float _max_time;
    [SerializeField] private float _timer;

    [Header("Iteration")]
    [SerializeField] private int _max_iteration;
    [SerializeField] private int _current_iteration;

    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private List<Peer> _peers;

    [SerializeField] private GameSceneUIManager _gameSceneUIManager;

    // Start is called before the first frame update
    void Start()
    {
        LoadComponent();
        _gameSceneUIManager.InitiateIconIter(_max_iteration);
        AudioManager.Instance.PlayBGM(0);
    }

    private void LoadComponent()
    {
        _max_time = 15f;
        _timer = _max_time;
        _max_iteration = 5;
        _current_iteration = 1;

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
    public void NextIteration()
    {
        _current_iteration++;
        _timer = _max_time;

        if (_current_iteration >= _max_iteration)
        {
            Debug.Log("You win");
        }
        else
        {
            ConfigMap();

        }
    }
    public void ConfigMap()
    {
        // Hàm này sẽ config lại map khi mà reset map hoặc tiến đến iteration tiếp theo


    }
    public void ConfigPlayerEnemyPosition()
    {

    }
    public void BackToPreviousIteration()
    {
        _current_iteration--;
        _timer = _max_time;
        ConfigMap();
    }

    public void SendMessage(string message, Peer sender)
    {
        // Resolve message here
        if (message == "win")
        {
            Debug.Log("win");
        }
    }

    public void Register(Peer peer)
    {
        //_components.Add(component);
        peer.SetMediator(this);
    }
}
