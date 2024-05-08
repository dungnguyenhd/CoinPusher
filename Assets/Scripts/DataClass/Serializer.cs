using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Serializer
{
    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/60games.CoinPusher";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGameData()
    {
        string path = Application.persistentDataPath + "/60games.CoinPusher";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            return data;
        } else
        {
            Debug.Log("Save file not found");
            return null;
        }
    }
}
