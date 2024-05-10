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
            StartCoroutine(CountingDelay());
            if (isCoin)
            {
                GameManager.Instance.ConsumeCoins(1);
                GameManager.Instance.GainExp(5);
                StartCoroutine(DestroyObject());
            }
            else
            {
                GameController.Instance.GainToys(itemInfo.itemPrefab);
                GameManager.Instance.GainExp(10);
                StartCoroutine(DestroyObject());
            }
        }
        else if (other.CompareTag("Lost_Receiver"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator CountingDelay()
    {
        yield return new WaitForSeconds(Random.Range(1, 6) / Random.Range(10, 15));
        GameController.Instance.CountingCombo();
    }

    private IEnumerator DestroyObject()
    {
        GameController.Instance.PlayRandomPrizeFX();
        yield return new WaitForSeconds(Random.Range(6, 10) / 10f);
        Destroy(gameObject);
    }
}
