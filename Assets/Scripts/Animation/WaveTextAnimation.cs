using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveTextAnimation : MonoBehaviour
{
    [SerializeField] private Transform _text_root;
    [SerializeField] private List<TextMeshProUGUI> letters;
    [SerializeField, Range(10f,20f)] private float amplitude = 10f;
    [SerializeField, Range(1f, 5f)] private float frequency = 1f;
    [SerializeField, Range(1f, 5f)] private float duration = 1f;

    void Start()
    {
        foreach(Transform letter in _text_root)
        {
            letters.Add(letter.GetComponent<TextMeshProUGUI>());
        }
        for (int i = 0; i < letters.Count; i++)
        {
            AnimateLetter(letters[i], i);
        }
    }

    void AnimateLetter(TextMeshProUGUI letter, int index)
    {
        float startDelay = index * 0.1f;
        Vector3 originalPosition = letter.transform.localPosition;

        DOTween.To(() => 0f, x =>
            letter.transform.localPosition = originalPosition + new Vector3(0, Mathf.Sin(x * frequency) * amplitude, 0),
            Mathf.PI * 2, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetDelay(startDelay);
    }
}
