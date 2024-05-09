using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Combo_Tween_Animation : MonoBehaviour
{
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private GameObject holder;
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem combo1_3_Fx;
    [SerializeField] private ParticleSystem combo3_5_Fx;
    [SerializeField] private ParticleSystem combo5_plus_Fx;

    [SerializeField] private GameObject twinkFX;

    private Vector3 initialPosition;
    private Vector3 initialScale;

    void Start()
    {
        initialPosition = holder.transform.position;
        initialScale = holder.transform.localScale;
    }

    public void DisplayComboSceen(int amount)
    {
        gameObject.SetActive(true);
        twinkFX.SetActive(true);
        holder.transform.localScale = new Vector3(1, 1, 1);
        LeanTween.move(holder, target.position, 0.5f).setEaseOutQuad().setOnComplete(() =>
                {
                    UpdateComboText(amount);
                    StartCoroutine(ResetHolder(3f));
                });
    }

    IEnumerator ResetHolder(float delay)
    {
        yield return new WaitForSeconds(delay);
        twinkFX.SetActive(false);
        LeanTween.scale(holder, Vector3.zero, 0.5f).setEaseOutQuad().setOnComplete(() =>
        {
            gameObject.SetActive(false);
            holder.transform.position = initialPosition;
        });
        combo1_3_Fx.gameObject.SetActive(false);
        combo3_5_Fx.gameObject.SetActive(false);
        combo5_plus_Fx.gameObject.SetActive(false);
    }

    public void UpdateComboText(int amount)
    {
        comboText.text = "COMBO X" + amount;
        if (amount <= 3)
        {
            combo1_3_Fx.gameObject.SetActive(true);
            combo1_3_Fx.Play();
        }
        else if (amount > 3 && amount <= 6)
        {
            combo3_5_Fx.gameObject.SetActive(true);
            combo3_5_Fx.Play();
        }
        else if (amount > 6)
        {
            combo5_plus_Fx.gameObject.SetActive(true);
            combo5_plus_Fx.Play();
        }

        LeanTween.scale(comboText.gameObject, Vector3.one * 1.2f, 0.3f).setEaseOutQuad().setOnComplete(() =>
        {
            LeanTween.scale(comboText.gameObject, Vector3.one, 0.3f).setEaseOutQuad();
        });
    }
}
