using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> listCoinText;
    [SerializeField] private List<TMP_Text> listExpText;
    [SerializeField] private List<TMP_Text> listCashText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Slider coinSlider;
    [SerializeField] private Slider expSlider;
    [SerializeField] private List<TMP_Text> expText;
    [SerializeField] private List<TMP_Text> coinText;
    [SerializeField] private List<TMP_Text> cashText;
    private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeCurve;
    public float fillSpeed = 3f;
    private float targetCoinProgress = 0f;
    private float targetExpProgress = 0f;

    public static GUI_Manager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateExpSliderMaxValue();
        UpdateCashText();
        UpdateLevelText();
        CoinIncreamentProgress(GameManager.Instance.gameData.playerCoin);
        ExpIncreamentProgress(GameManager.Instance.gameData.playerExp);
    }

    IEnumerator UpdateSliderValue(Slider slider, float targetValue, bool isIncreasing)
    {
        float currentValue = slider.value;
        float step = fillSpeed * Time.deltaTime;

        while ((isIncreasing && currentValue < targetValue) ||
               (!isIncreasing && currentValue > targetValue))
        {
            if (isIncreasing)
            {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, step);
            }
            else
            {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, step);
            }

            slider.value = currentValue;
            yield return null;
        }

        UpdateSliderValue();
    }

    public void CoinIncreamentProgress(int newProgress)
    {
        bool increasing = true;
        if (newProgress < 0)
        {
            increasing = false;
        }
        targetCoinProgress = coinSlider.value + newProgress;
        StartCoroutine(UpdateSliderValue(coinSlider, targetCoinProgress, increasing));
        UpdateCoinText();
        DisplayCoinGain(newProgress);
    }

    public void ExpIncreamentProgress(int newProgress)
    {
        bool increasing = true;
        if (newProgress < 0)
        {
            increasing = false;
        }

        targetExpProgress = expSlider.value + newProgress;
        StartCoroutine(UpdateSliderValue(expSlider, targetExpProgress, increasing));
        UpdateExpText();
        DisplayExpGain(newProgress);
    }

    public void UpdateCoinText()
    {
        foreach (var text in listCoinText)
        {
            text.text = GameManager.Instance.gameData.playerCoin.ToString();
        }
    }

    public void UpdateCashText()
    {
        foreach (var text in listCashText)
        {
            text.text = GameManager.Instance.gameData.playerCash.ToString();
        }
    }

    public void UpdateExpText()
    {
        foreach (var text in listExpText)
        {
            text.text = GameManager.Instance.gameData.playerExp.ToString();
        }
    }

    public void UpdateLevelText()
    {
        levelText.text = GameManager.Instance.gameData.playerLevel.ToString();
    }

    public void UpdateSliderValue()
    {
        expSlider.value = GameManager.Instance.gameData.playerExp;
    }

    public void DisplayExpGain(int amount)
    {
        foreach (var text in expText)
        {
            if (amount > 0)
            {
                text.text = "+" + amount.ToString();

                text.gameObject.SetActive(true);

                LeanTween.value(0f, 1f, fadeInDuration)
                    .setEase(fadeCurve)
                    .setOnUpdate((float alpha) =>
                    {
                        text.alpha = alpha;
                    })
                    .setOnComplete(() =>
                    {
                        LeanTween.value(1f, 0f, fadeOutDuration).setEase(fadeCurve)
                            .setDelay(displayDuration).setOnUpdate((float alpha) =>
                            {
                                text.alpha = alpha;
                            })
                            .setOnComplete(() =>
                            {
                                text.gameObject.SetActive(false);
                            });
                    });
            }
        }
    }

    public void DisplayCoinGain(int amount)
    {
        foreach (var text in coinText)
        {
            if (amount > 0)
            {
                text.text = "+" + amount.ToString();

                text.gameObject.SetActive(true);

                LeanTween.value(0f, 1f, fadeInDuration)
                    .setEase(fadeCurve)
                    .setOnUpdate((float alpha) =>
                    {
                        text.alpha = alpha;
                    })
                    .setOnComplete(() =>
                    {
                        LeanTween.value(1f, 0f, fadeOutDuration).setEase(fadeCurve)
                            .setDelay(displayDuration).setOnUpdate((float alpha) =>
                            {
                                text.alpha = alpha;
                            })
                            .setOnComplete(() =>
                            {
                                text.gameObject.SetActive(false);
                            });
                    });
            }
        }
    }

    public void DisplayCashGain(int amount)
    {
        foreach (var text in cashText)
        {
            if (amount > 0)
            {
                text.text = "+" + amount.ToString();

                text.gameObject.SetActive(true);

                LeanTween.value(0f, 1f, fadeInDuration)
                    .setEase(fadeCurve)
                    .setOnUpdate((float alpha) =>
                    {
                        text.alpha = alpha;
                    })
                    .setOnComplete(() =>
                    {
                        LeanTween.value(1f, 0f, fadeOutDuration).setEase(fadeCurve)
                            .setDelay(displayDuration).setOnUpdate((float alpha) =>
                            {
                                text.alpha = alpha;
                            })
                            .setOnComplete(() =>
                            {
                                text.gameObject.SetActive(false);
                            });
                    });
            }
        }
    }

    public void UpdateExpSliderMaxValue()
    {
        expSlider.maxValue = GameManager.Instance.gameData.playerLevel * 10;
    }
}
