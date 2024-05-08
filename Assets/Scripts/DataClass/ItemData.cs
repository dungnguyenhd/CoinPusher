using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "GameData/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public List<ItemInfo> items = new();

    [System.Serializable]
    public class ItemInfo
    {
        public int id;
        public string name;
        public GameObject itemPrefab;
        public int collectionId;
    }

    public ItemInfo FindById(int id)
    {
        return items.FirstOrDefault(item => item.id == id);
    }

    public List<ItemInfo> FindAllByEquipmentIds(List<int> listIds)
    {
        List<ItemInfo> foundItemList = items.FindAll(item => listIds.Contains(item.id));
        return foundItemList;
    }
}