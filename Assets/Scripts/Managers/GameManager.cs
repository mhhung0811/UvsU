using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private AudioConfigs audioConfigs;     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<AudioConfig> GetAllAudioConfigs()
    {
        return audioConfigs._all_audio_configs;
    }
}
