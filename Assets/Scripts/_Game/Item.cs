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
        if (other.CompareTag("Prize_Receiver"))
        {
            if (isCoin)
            {
                TouchReceiver.Instance.coinLeft++;
                TouchReceiver.Instance.UpdateCoinText();
                Destroy(gameObject);
                TouchReceiver.Instance.PlayRandomPrizeFX();
            }
        }
        else if (other.CompareTag("Lost_Receiver"))
        {
            Destroy(gameObject);
        }
    }
}
