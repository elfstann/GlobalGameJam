using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroItem : MonoBehaviour
{
    [SerializeField] private Image image;
    public float StartTime = 5.0f;

    [Header("Fade in")]
    [SerializeField] private float fromValue = 0.5f;
    [SerializeField] private float timeFadeIn = 0.5f;
    [SerializeField] private Ease easeFadeIn;

    //[Header("Between")]
    //[SerializeField] private float delay = 2.0f;

    //[Header("Fade out")]
    //[SerializeField] private float timeFadeOut = 1.0f;
    //[SerializeField] private Ease easeFadeOut;
    //[SerializeField] private float targetValue = 0.2f;

    private Sequence sequence;

    public void Play()
    {
        gameObject.SetActive(true);
        image.color = new Color(image.color.r, image.color.g, image.color.b, fromValue);

        sequence = DOTween.Sequence()
            .Append(image.DOFade(1.0f, timeFadeIn).SetEase(easeFadeIn));
            //.AppendInterval(delay)
            //.Append(image.DOFade(targetValue, timeFadeOut).SetEase(easeFadeOut))
            //.OnComplete(() =>
            //{
            //    // Make it inactive               
            //    End(false);
            //})
            //.Play();
    }

    private void End(bool hasClicked)
    {
        sequence.Kill();
        gameObject.SetActive(false);
    }
}
