using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tween_Animation : MonoBehaviour
{
    [SerializeField] private bool isMenu;
    [SerializeField] private GameObject screen;
    [SerializeField] private List<GameObject> listFadeOutItem;
    [SerializeField] private List<RectTransform> listLeftButton;
    [SerializeField] private List<RectTransform> listRightButton;
    [SerializeField] private List<RectTransform> listTopButton;
    [SerializeField] private List<RectTransform> listBottomButton;

    [SerializeField]
    private float
    leftInAmount, leftOutAmount, rightInAmount, rightOutAmount,
    bottomInAmount, bottomOutAmount, topInAmount, topOutAmount;

    void Start()
    {
        if (isMenu) FadeIn();
    }

    public void FadeIn()
    {
        ActiveScreen();
        LeanTween.init();

        foreach (var button in listLeftButton)
        {
            LeanTween.moveX(button, leftInAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }

        foreach (var button in listRightButton)
        {
            LeanTween.moveX(button, rightInAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }

        foreach (var button in listBottomButton)
        {
            LeanTween.moveX(button, bottomInAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }

        foreach (var button in listFadeOutItem)
        {
            LeanTween.scale(button, new Vector3(1f, 1f, 1f), 1).setDelay(0.3f).setEase(LeanTweenType.easeOutElastic);
        }

        foreach (var button in listTopButton)
        {
            LeanTween.moveY(button, topInAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }
    }

    public void FadeOut()
    {
        LeanTween.init();

        foreach (var button in listLeftButton)
        {
            LeanTween.moveX(button, leftOutAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }

        foreach (var button in listRightButton)
        {
            LeanTween.moveX(button, rightOutAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }

        foreach (var button in listBottomButton)
        {
            LeanTween.moveX(button, bottomOutAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }

        for (int i = 0; i < listFadeOutItem.Count; i++)
        {
            if (i != listFadeOutItem.Count - 1)
            {
                LeanTween.scale(listFadeOutItem[i], new Vector3(0f, 0f, 0f), 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeInElastic);
            }
            else
            {
                LeanTween.scale(listFadeOutItem[i], new Vector3(0f, 0f, 0f), 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeInElastic).setOnComplete(InactiveScreen);
            }
        }

        foreach (var button in listTopButton)
        {
            LeanTween.moveY(button, topOutAmount, 0.5f).setDelay(0.3f).setEase(LeanTweenType.easeOutCubic);
        }
    }

    void InactiveScreen()
    {
        if (screen != null) screen.SetActive(false);
    }

    void ActiveScreen()
    {
        if (!isMenu && screen != null) screen.SetActive(true);
    }
}
