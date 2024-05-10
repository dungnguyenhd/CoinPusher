using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string deviceId;
    public GameData gameData;

    [SerializeField] private int coinDropTime = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            deviceId = SystemInfo.deviceUniqueIdentifier;
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        CheckCoinDropAfterOffline();
    }

    private void LoadGameData()
    {
        GameData data = Serializer.LoadGameData();
        if (data == null)
        {
            CreateDefaultDataSlot();
        }
        else
        {
            if (data.playerId != deviceId)
            {
                gameData = null;
            }
            else
            {
                gameData = data;
            }
        }
    }

    public void SaveGameData()
    {
        Serializer.SaveGameData(gameData);
    }

    public void CreateDefaultDataSlot()
    {
        GameData data = new();
        gameData = data;
        gameData.playerId = deviceId;
        Serializer.SaveGameData(data);
    }

    private void OnApplicationQuit()
    {
        UpdateGameState(GameController.Instance.GetGameStateDatas());
        SaveGameData();
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            UpdateGameState(GameController.Instance.GetGameStateDatas());
            SaveGameData();
        }
    }

    private void UpdateGameState(List<GameStateData> state)
    {
        gameData.gameplayData = state;
    }

    public void ConsumeCoins(int amount)
    {
        if (gameData.playerCoin == 100 && amount < 0)
        {
            SetCountdownTime();
        }

        gameData.playerCoin += amount;
        GUI_Manager.Instance.CoinIncreamentProgress(amount);
    }

    public void GainExp(int amount)
    {
        int target = gameData.playerExp + amount;
        if (target >= gameData.playerLevel * 10)
        {
            int prevExp = gameData.playerExp;
            gameData.playerExp = target - gameData.playerLevel * 10;
            gameData.playerLevel++;
            UpdateLevelText();
            GUI_Manager.Instance.UpdateExpSliderMaxValue();
            GUI_Manager.Instance.ExpIncreamentProgress(prevExp);
        }
        else
        {
            gameData.playerExp = target;
            GUI_Manager.Instance.ExpIncreamentProgress(amount);
        }
        // GUI_Manager.Instance.DisplayExpGain(amount);
    }

    public void UpdateLevelText()
    {
        GUI_Manager.Instance.UpdateLevelText();
    }

    private void CheckCoinDropAfterOffline()
    {
        if (gameData.lastCoinDropTime != 0 || gameData.playerCoin < 100)
        {
            TimeSpan timeSinceLastDrop = TimeSpan.FromTicks(DateTime.Now.Ticks - gameData.lastCoinDropTime);

            TimeSpan timeNeededForCoinDrop = TimeSpan.FromSeconds(coinDropTime);

            int numCoinDrops = (int)(timeSinceLastDrop.TotalSeconds / timeNeededForCoinDrop.TotalSeconds);

            int remainderSeconds = (int)(timeSinceLastDrop.TotalSeconds % timeNeededForCoinDrop.TotalSeconds);

            if ((numCoinDrops + gameData.playerCoin) > 100)
            {
                gameData.playerCoin = 100;
            }
            else
            {
                gameData.playerCoin += numCoinDrops;
                StartCountdown(TimeSpan.FromSeconds(remainderSeconds));
            }
        }
    }

    private void SetCountdownTime()
    {
        StartCountdown(TimeSpan.FromSeconds(coinDropTime));
    }

    private void StartCountdown(TimeSpan duration)
    {
        StartCoroutine(CountdownCoroutine(duration));
    }

    private IEnumerator CountdownCoroutine(TimeSpan duration)
    {
        while (duration.TotalSeconds > 0)
        {
            duration -= TimeSpan.FromSeconds(Time.deltaTime);
            // uiSpinButtonText.text = $"{(int)duration.TotalMinutes:00}:{duration.Seconds:00}";
            yield return null;
        }

        gameData.lastCoinDropTime = DateTime.Now.Ticks;
        ConsumeCoins(1);

        if (gameData.playerCoin < 100)
        {
            SetCountdownTime();
        }
    }
}
