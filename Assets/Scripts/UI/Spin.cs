using UnityEngine;
using EasyUI.PickerWheelUI;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
// using System;
public class Spin : MonoBehaviour
{
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Button uiSpinButtonAds;
    [SerializeField] private TMP_Text uiSpinButtonText;
    [SerializeField] private TMP_Text uiSpinButtonAdsText;
    [SerializeField] private PickerWheel pickerWheel;
    [SerializeField] private GameObject spinNotification;
    [SerializeField] ParticleSystem coinDropFX;
    [SerializeField] Sprite coin;
    [SerializeField] Sprite coinX3;
    [SerializeField] Sprite diamond;
    [SerializeField] Sprite diamondX3;
    [SerializeField] Sprite ticket;
    [SerializeField] Image uiDiamond;
    public int priceOnSpin = 1;

    private const string spinTimeKey = "SpinWaitTime";
    // private const string spinAdTimeKey = "SpinAdWaitTime";
    private const float countDownDuration = 1800f;
    private DateTime targetTime;
    // private DateTime targetAdTime;
    private bool spinAvailable = true;
    private bool spinAdRewarded = false;

    private List<WheelPieceData> wheelData = new();
    private List<WheelPiece> listWheelPieces = new();

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void Start()
    {
        CheckSpiner();
    }

    private void CheckSpiner()
    {
        if (PlayerPrefs.HasKey(spinTimeKey))
        {
            long targetTimeTicks = long.Parse(PlayerPrefs.GetString(spinTimeKey));
            targetTime = new DateTime(targetTimeTicks);
            if (targetTime < DateTime.Now)
            {
                PlayerPrefs.DeleteKey(spinTimeKey);
                spinAvailable = true;
                spinNotification.SetActive(true);
                uiSpinButtonText.text = "Tap to spin";
                uiSpinButton.interactable = true;
            }
            else
            {
                if (!spinAdRewarded)
                {
                    spinAvailable = false;
                    spinNotification.SetActive(false);
                    uiSpinButton.interactable = false;

                    TimeSpan remainingTime = targetTime - DateTime.Now;

                    if (remainingTime.TotalSeconds <= 0)
                    {
                        PlayerPrefs.DeleteKey(spinTimeKey);
                    }
                    else
                    {
                        StartCountdown(remainingTime);
                    }
                }
                else
                {
                    uiSpinButton.interactable = true;
                    uiSpinButtonText.text = "Tap to spin";
                }
            }
        }
        else
        {
            spinAvailable = true;
            spinNotification.SetActive(true);
            uiSpinButtonText.text = "Tap to spin";
            uiSpinButton.interactable = true;
        }
    }

    private void SetCountdownTime()
    {
        targetTime = DateTime.Now.AddSeconds(countDownDuration);
        PlayerPrefs.SetString(spinTimeKey, targetTime.Ticks.ToString());
        StartCountdown(TimeSpan.FromSeconds(countDownDuration));
    }

    private void StartCountdown(TimeSpan duration)
    {
        StartCoroutine(CountdownCoroutine(duration));
    }

    private IEnumerator CountdownCoroutine(TimeSpan duration)
    {
        while (duration.TotalSeconds > 0 && !spinAdRewarded)
        {
            duration -= TimeSpan.FromSeconds(Time.deltaTime);
            uiSpinButtonText.text = $"{(int)duration.TotalMinutes:00}:{duration.Seconds:00}";
            yield return null;
        }

        if (duration.TotalSeconds <= 0)
        {
            spinAvailable = true;
            PlayerPrefs.DeleteKey(spinTimeKey);
        }
        uiSpinButton.interactable = true;
        uiSpinButtonText.text = "Tap to spin";
    }

    public void SpinBut()
    {
        if (spinAvailable || spinAdRewarded)
        {
            uiSpinButton.interactable = false;
            uiSpinButtonText.text = "Spinning";
            pickerWheel.Spin();
            // audioManager.PlaySFX(audioManager.onPayButton);
        }

        pickerWheel.OnSpinEnd(wheelPiece =>
        {
            // if (wheelPiece.Label == "Coin")
            // {
            //     GameSharedUI.Instance.UpdateCoinsUIText(GameManager.Instance.GetCoins(), wheelPiece.Amount);
            //     GameManager.Instance.AddCoins(wheelPiece.Amount);
            // }
            // if (wheelPiece.Label == "Diamond")
            // {
            //     GameSharedUI.Instance.UpdateDiamondsUIText(GameManager.Instance.GetDiamonds(), wheelPiece.Amount);
            //     GameManager.Instance.AddDiamons(wheelPiece.Amount); ;
            // }

            // audioManager.PlaySFX(audioManager.onResorceAdded);
            coinDropFX.Play();

            if (spinAdRewarded)
            {
                spinAdRewarded = false;
            }
            else
            {
                SetCountdownTime();
                spinNotification.SetActive(false);
            }
            ResetSpinerPieces();
            CheckSpiner();
        });
    }

    public void SpinAd()
    {
        AdmobManager.Instance.LoadRewardedAd();
        AdmobManager.Instance.ShowRewardedAd("HomeSpinAd", 0);
    }

    public void AddSpinReward()
    {
        spinAdRewarded = true;
        CheckSpiner();
        // uiSpinButtonAds.interactable = false;
    }

    public void ResetSpinerPieces()
    {
        pickerWheel.wheelPieces.Clear();

        // coin 0
        AddNewWheelPiece(coin, 500, "Coin", 40f);

        // diamond 0
        AddNewWheelPiece(diamond, UnityEngine.Random.Range(1, 5) * 50, "Diamond", 10f); /* 50 - 200*/

        // coin 2
        AddNewWheelPiece(coinX3, 2000, "Coin", 12f);

        // ticket 1
        AddNewWheelPiece(ticket, UnityEngine.Random.Range(1, 4), "Ticket", 3f);

        // coin 1
        AddNewWheelPiece(coin, 1000, "Coin", 20f);

        // diamond 1
        AddNewWheelPiece(diamondX3, UnityEngine.Random.Range(6, 11) * 50, "Diamond", 5f); /* 300 - 500 */

        // coin 3
        AddNewWheelPiece(coinX3, 5000, "Coin", 9f);

        Shuffle(listWheelPieces);
        pickerWheel.wheelPieces = listWheelPieces;

        foreach (var item in listWheelPieces)
        {
            wheelData.Add(new WheelPieceData()
            {
                Label = item.Label,
                Amount = item.Amount,
                Chance = item.Chance,
            });
        }

        // GameManager.Instance.playerData.currentWheel.Clear();
        // GameManager.Instance.playerData.currentWheel = wheelData;

        StartCoroutine(ResetWheel());
    }

    IEnumerator ResetWheel()
    {
        yield return new WaitForSeconds(1f);
        pickerWheel.ResetWheelPiece();
    }

    void AddNewWheelPiece(Sprite icon, int amount, string label, float rate, int itemId = 0)
    {
        WheelPiece wheelPiece = new()
        {
            Icon = icon,
            Amount = amount,
            Label = label,
            Chance = rate,
        };
        listWheelPieces.Add(wheelPiece);
    }

    public void InitiatePieceData(List<WheelPieceData> data)
    {
        foreach (var item in data)
        {
            Sprite icon = null;
            switch (item.Label)
            {
                case "Coin":
                    if (item.Amount >= 1500) icon = coinX3;
                    else icon = coin;
                    break;
                case "Diamond":
                    if (item.Amount >= 150) icon = diamondX3;
                    else icon = diamond;
                    break;
                case "Ticket":
                    icon = ticket;
                    break;
                default:
                    break;
            }

            pickerWheel.wheelPieces.Add(new WheelPiece()
            {
                Icon = icon,
                Amount = item.Amount,
                Label = item.Label,
                Chance = item.Chance,
            });
        }
    }

    void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = UnityEngine.Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }
}