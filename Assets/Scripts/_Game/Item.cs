using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class Item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public bool isCoin = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prize_Receiver"))
        {
            if (isCoin)
            {
                GameManager.Instance.ConsumeCoins(1);
                DestroyObject();
            }
            else
            {
                GameController.Instance.GainToys(itemInfo.itemPrefab);
                DestroyObject();
            }
            GameController.Instance.DisplayCombo();
        }
        else if (other.CompareTag("Lost_Receiver"))
        {
            Destroy(gameObject);
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
        GameController.Instance.PlayRandomPrizeFX();
    }
}
