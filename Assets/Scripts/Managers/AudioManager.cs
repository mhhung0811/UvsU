using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private AudioSource audioSourceFX;

    [SerializeField] private List<AudioConfig> list_audio_configs;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadComponent();
    }
    private void LoadComponent()
    {
        this.list_audio_configs = GameManager.Instance.GetAllAudioConfigs();
       
    }

    public void PlayBGM(int audio_id)
    {
        if(list_audio_configs == null)
        {
            Debug.Log("There is no config for audio");
            return;
        }

        AudioClip audioClip = GetAudioClip(audio_id);

        if (audioClip == null)
        {
            Debug.Log($"Can not find audio with id {audio_id} in configs");
            return;
        }
        audioSourceBGM.clip = audioClip;
        audioSourceBGM.Play();
    }
    public void PlayFX(int audio_id)
    {
        if (list_audio_configs == null)
        {
            Debug.Log("There is no config for audio");
            return;
        }

        AudioClip audioClip = GetAudioClip(audio_id);

        if (audioClip == null)
        {
            Debug.Log($"Can not find audio with id {audio_id} in configs");
            return;
        }
        audioSourceFX.clip = audioClip;
        audioSourceFX.Play();
    }
    public AudioClip GetAudioClip(int audio_id)
    {
        foreach (AudioConfig config in list_audio_configs)
        {
            if (config.id == audio_id)
            {
                return config.audio_clip;
            }
        }
        return null;
    }

}
