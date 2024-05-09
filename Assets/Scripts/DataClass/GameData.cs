using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string playerId;
    public int playerLevel;
    public int playerExp;
    public int playerCoin;
    public int playerCash;
    public int dayReward;
    public long lastCoinDropTime;


    public List<GameStateData> gameplayData = new();

    public GameData()
    {
        playerId = "";
        playerLevel = 1;
        playerExp = 0;
        playerCoin = 100;
        playerCash = 0;
        dayReward = 0;
        lastCoinDropTime = 0;
        gameplayData = new List<GameStateData>();
    }

    public GameData(GameData gameData)
    {
        playerId = gameData.playerId;
        playerLevel = gameData.playerLevel;
        playerExp = gameData.playerExp;
        playerCoin = gameData.playerCoin;
        playerCash = gameData.playerCash;
        dayReward = gameData.dayReward;
        lastCoinDropTime = gameData.lastCoinDropTime;
        gameplayData = gameData.gameplayData;
    }
}

[Serializable]
public class GameStateData
{
    public int id;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public GameStateData(int id, Vector3 position, Quaternion rotation)
    {
        this.id = id;
        this.position = new SerializableVector3(position);
        this.rotation = new SerializableQuaternion(rotation);
    }
}

[Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class SerializableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}