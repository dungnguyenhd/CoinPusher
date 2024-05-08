using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private string deviceId;
    public GameData gameData;

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
        gameData.playerCoin += amount;
        GUI_Manager.Instance.CoinIncreamentProgress(amount);
    }

    public void GainExp(int amount)
    {
        gameData.playerExp += amount;
    }
}
