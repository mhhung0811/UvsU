using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
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
}
