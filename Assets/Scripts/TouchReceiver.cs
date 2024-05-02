using TMPro;
using UnityEngine;

public class TouchReceiver : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject coinHolder;
    [SerializeField] private TMP_Text coinNumText;
    public int coinLeft = 0;

    public static TouchReceiver Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        coinLeft = 100;
        UpdateCoinText();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {

                    if (hit.transform.tag == "TouchReceiver" && coinLeft > 0)
                    {
                        Vector3 touchPosition = hit.point;
                        touchPosition.y += 3f;
                        Instantiate(coinPrefab, touchPosition, Quaternion.identity, coinHolder.transform);
                        coinLeft--;
                        UpdateCoinText();
                    }
                }
            }
        }
    }

    public void UpdateCoinText()
    {
        coinNumText.text = coinLeft.ToString();
    }
}
