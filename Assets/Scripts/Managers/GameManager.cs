using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private AudioConfigs audioConfigs;
    [SerializeField] private LevelConfigs levelConfigs;
    public bool isPauseGame { get; private set; }
    public bool isSoftPauseGame { get; private set; }

    [SerializeField] private int _current_level = 1;

    public int Current_level
    {
        get { return _current_level; }
        set { _current_level = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        isPauseGame = false;
        isSoftPauseGame = false;
    }

    public List<AudioConfig> GetAllAudioConfigs()
    {
        return audioConfigs._all_audio_configs;
    }

    public void Pause()
    {
        isPauseGame = !isPauseGame;
        if (isPauseGame)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void SoftPause()
    {
        isSoftPauseGame = true;
    }

    public void SoftContinue()
    {
        isSoftPauseGame = false;
    }
    public List<LevelConfig> GetAllLevelConfigs()
    {
        return levelConfigs.all_level_configs;
    }
}
