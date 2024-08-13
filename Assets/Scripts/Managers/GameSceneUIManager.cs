using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngineInternal;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField] private IngameManager _ingameManager;

    [SerializeField] private TextMeshProUGUI _iteration;
    [SerializeField] private TextMeshProUGUI _goal_text;
    [SerializeField] private GameObject _upper_text;
    [SerializeField] private TextMeshProUGUI _restart_text;
    [SerializeField] private TextMeshProUGUI _count_down_timer;
    [SerializeField] private GameObject _failure_text;
    [SerializeField] private GameObject _win_text;
    [SerializeField] private Image _all_ui_panel;
    [SerializeField] private TextMeshProUGUI _lower_text;

    [SerializeField] private Image _time_slider;

    [SerializeField] private Image _angel_icon_arrow;
    [SerializeField] private RectTransform _devil_icon_prefab;

    [SerializeField] private GridLayoutGroup _grid_layout_group;

    
    [SerializeField] private List<RectTransform> _list_devils_icon = new List<RectTransform>();

    [Header("Pause Panel")]
    [SerializeField] private Image _pause_panel;
    public Image PausePanel
    {
        get { return _pause_panel; }
        set { _pause_panel = value; }
    }
    [SerializeField] private TextMeshProUGUI _music_text;
    [SerializeField] private TextMeshProUGUI _sfx_text;
    [SerializeField] private List<Image> _list_icon_arrow = new List<Image>();

    private int _current_option = 0;

    public void SetTimeSlider(float value, float max_timer)
    {
        _time_slider.fillAmount = value / max_timer;
    }
    public void SetTimerText(float value)
    {
        int timer = (int)value;
        _count_down_timer.text = $"{timer}";

    }
    public void SetGoalText(string text)
    {
        _goal_text.text = text;
    }
    public void SetIterText(int curent_iter , int max_iter)
    {
        string text = $"Iteration {curent_iter } / {max_iter}";
        _iteration.text = text;
    }
    public void SetIconIter(int current_iter)
    {
        int num_devil_current  = current_iter / 2;
        int max_devil_icon = _list_devils_icon.Count;

        _angel_icon_arrow.gameObject.SetActive(false);

        for (int i = 0; i < max_devil_icon; i++)
        {
            if(i<num_devil_current)
            {
                _list_devils_icon[i].transform.Find("Disable").gameObject.SetActive(false);
            }
            else
            {
                _list_devils_icon[i].transform.Find("Disable").gameObject.SetActive(true);
            }
        }
        // Even is devil
        if (current_iter % 2 == 0) 
        {
            _list_devils_icon[num_devil_current-1].transform.Find("Arrow").gameObject.SetActive(true);
        }
        //Odd is angel
        else
        {
            _angel_icon_arrow.gameObject.SetActive(true);
        }
    }
    public void InitiateIconIter(int max_iter)
    {
        _angel_icon_arrow.gameObject.SetActive(true);

        int num_devil = max_iter / 2;
        Debug.Log("Num devil : " + num_devil);
        for(int i = 0; i< num_devil; i++)
        {
            RectTransform image = Instantiate(_devil_icon_prefab);
            image.transform.SetParent(_grid_layout_group.gameObject.transform, false);
            Transform arrow_obj = image.transform.Find("Arrow");
            arrow_obj.gameObject.SetActive(false);
            _list_devils_icon.Add(image);
        }
    }

    public void DisableUpperText()
    {
        _upper_text.gameObject.SetActive(false);
        _restart_text.gameObject.SetActive(true);
    }

    public void EnableUpperText()
    {
        _upper_text.gameObject.SetActive(true);
        _restart_text.gameObject.SetActive(false);
    }
    public void EnableFailedText()
    {
        _failure_text.gameObject.SetActive(true);
    }
    public void DisableFailedText()
    {
        _failure_text.gameObject.SetActive(false);
    }
    public void EnableWinText()
    {
        _win_text.gameObject.SetActive(true);
    }
    public void DisableWinText()
    {
        _win_text.gameObject.SetActive(false);
    }
    public void LevelCompleted()
    {
        _all_ui_panel.gameObject.SetActive(false);
        _win_text.gameObject.SetActive(true);
        _lower_text.gameObject.SetActive(true);
    }
    public void ChangePauseState()
    {
        bool panel_state = _pause_panel.gameObject.activeSelf;
        _pause_panel.gameObject.SetActive(!panel_state);
        if (panel_state)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
        

    }
    void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game is paused");
    }
    void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game is resumed");
    }
    public void HandleOption(int value)
    {
        if (_pause_panel.gameObject.activeSelf)
        {
            _current_option += value;
            _current_option = Mathf.Min(Mathf.Max(0, _current_option), _list_icon_arrow.Count - 1);
            Debug.Log("Current option : " + _current_option);
            for(int i = 0; i < _list_icon_arrow.Count; i++)
            {
                _list_icon_arrow[i].gameObject.SetActive(false);
            }
            _list_icon_arrow[_current_option].gameObject.SetActive(true);
        }
    }
    public void ChooseCurrentOption()
    {
        if (_pause_panel.gameObject.activeSelf)
        {
            switch (_current_option)
            {
                case 0:
                    ChangePauseState();
                    break;
                case 1:
                    ChangePauseState();
                    _ingameManager.BackToPreviousIteration();
                    break;
                case 2:
                    ChangePauseState();
                    _ingameManager.RedoIteration();
                    break;
                case 3:
                    ChangePauseState();
                    _ingameManager.RestartLevel();
                    break;
                case 4:
                    ChangePauseState();
                    AudioManager.Instance.AudioSourceBGM.Stop();
                    SceneManager.LoadSceneAsync("Main Menu");
                    break;
                case 7:
                    ChangePauseState();
                    Application.Quit();
                    break;
                default:
                    ChangePauseState();
                    break;


            }
        }
    }
    public void ChangeVolume(float value)
    {
        if(_current_option == 5)
        {
            AudioManager.Instance.AudioSourceBGM.volume += value * 0.1f;
            _music_text.text = $"Music volume: {Mathf.Round(AudioManager.Instance.AudioSourceBGM.volume * 100f)}%";
        }
        else if(_current_option == 6)
        {
            AudioManager.Instance.AudioSourceFX.volume += value * 0.1f;
            _sfx_text.text = $"SFX volume: {Mathf.Round(AudioManager.Instance.AudioSourceFX.volume * 100f)}%";
        }

    }
}
