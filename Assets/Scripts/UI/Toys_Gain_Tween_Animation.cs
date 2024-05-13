using UnityEngine;
using System.Collections;

public class Toys_Gain_Tween_Animation : MonoBehaviour
{
    [SerializeField] private GameObject holder;
    [SerializeField] private Transform toysButton;
    [SerializeField] private Transform target;
    [SerializeField] private Transform model;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        initialPosition = holder.transform.position;
        initialRotation = holder.transform.rotation;
        initialScale = holder.transform.localScale;

        RotateHolder();
    }

    void RotateHolder()
    {
        LeanTween.rotateAround(holder, Vector3.up, 720f, 2f).setLoopClamp();
    }

    public void MoveToCenter()
    {
        LeanTween.moveLocalY(holder, 0f, 2f).setEaseOutQuad();
    }

    public void MoveAndDisappear()
    {
        LeanTween.move(holder, target.position, 1f).setEaseOutQuad().setOnComplete(() =>
        {
            LeanTween.scale(toysButton.gameObject, Vector3.one * 1.2f, 0.3f).setEaseOutQuad().setOnComplete(() =>
            {
                LeanTween.scale(toysButton.gameObject, Vector3.one, 0.3f).setEaseOutQuad();
            });
        });

        LeanTween.scale(holder, Vector3.zero, 1f).setEaseOutQuad().setOnComplete(ResetHolder);
    }

    void ResetHolder()
    {
        gameObject.SetActive(false);
        Destroy(model.GetChild(0).gameObject);
        holder.transform.SetPositionAndRotation(initialPosition, initialRotation);
        holder.transform.localScale = initialScale;
    }

    public void SetToysModel(GameObject prefab)
    {
        GameObject ini = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, model.transform);
        Destroy(ini.GetComponent<Rigidbody>());
        ini.transform.localPosition = new Vector3(0, 0, 0);
    }
}
