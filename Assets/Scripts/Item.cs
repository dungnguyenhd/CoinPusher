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
        // Kiểm tra xem đối tượng va chạm có tag là trigger mục tiêu không
        if (other.CompareTag("FallingChecker"))
        {
            if (isCoin)
            {
                TouchReceiver.Instance.coinLeft++;
                TouchReceiver.Instance.UpdateCoinText();
                Destroy(gameObject);
            }
        }
    }
}
