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
    [SerializeField] private Slider coinSlider;
    [SerializeField] private Slider expSlider;
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
        CoinIncreamentProgress(GameManager.Instance.gameData.playerCoin);
        StartCoroutine(UpdateSliderValue(coinSlider, targetCoinProgress, true));
        StartCoroutine(UpdateSliderValue(expSlider, targetExpProgress, true));
    }

    IEnumerator UpdateSliderValue(Slider slider, float targetValue, bool isIncreasing)
    {
        float currentValue = slider.value;
        while ((isIncreasing && currentValue < targetValue && currentValue < slider.maxValue) ||
               (!isIncreasing && currentValue > targetValue && currentValue > slider.minValue))
        {
            float step = fillSpeed * Time.deltaTime;
            if (isIncreasing)
            {
                currentValue = Mathf.Lerp(currentValue, targetValue, step);
            }
            else
            {
                currentValue = Mathf.Lerp(currentValue, targetValue, step);
            }
            slider.value = currentValue;
            yield return null;
        }
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
    }

    public void ExpIncreamentProgress(int newProgress)
    {
        bool increasing = true;
        if (newProgress < 0)
        {
            increasing = false;
        }

        targetExpProgress = coinSlider.value + newProgress;
        StartCoroutine(UpdateSliderValue(expSlider, targetExpProgress, increasing));
    }

    public void UpdateCoinText()
    {
        foreach (var text in listCoinText)
        {
            text.text = GameManager.Instance.gameData.playerCoin.ToString();
        }
    }

    public void UpdateDollarText()
    {
        foreach (var text in listCashText)
        {
            text.text = GameManager.Instance.gameData.playerCoin.ToString();
        }
    }

    public void UpdateExpText()
    {
        foreach (var text in listExpText)
        {
            text.text = GameManager.Instance.gameData.playerCoin.ToString();
        }
    }
}
