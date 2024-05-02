using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "GameData/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    List<ItemInfo> items;

    [System.Serializable]
    public class ItemInfo
    {
        public int id;
    }
}
