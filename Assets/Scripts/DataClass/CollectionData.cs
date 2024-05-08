using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionData", menuName = "GameData/CollectionData", order = 1)]
public class CollectionData : ScriptableObject
{
    public List<CollectionInfo> collections;

    [System.Serializable]
    public class CollectionInfo
    {
        public int id;
        public string name;
    }

    public CollectionInfo FindById(int id)
    {
        return collections.FirstOrDefault(item => item.id == id);
    }

    public List<CollectionInfo> FindAllByEquipmentIds(List<int> listIds)
    {
        List<CollectionInfo> foundItemList = collections.FindAll(item => listIds.Contains(item.id));
        return foundItemList;
    }
}