using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ItemData;

public enum GameState
{
    Gameplay,
    GamePause,
}

public class GameController : MonoBehaviour
{
    public GameObject itemHolder;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject defaultPrefab;
    [SerializeField] private TMP_Text coinNumText;
    [SerializeField] private List<ParticleSystem> prizeFX;
    [SerializeField] private ItemData itemData;
    [SerializeField] private CollectionData collectionData;
    [SerializeField] private GameObject dropper;
    [SerializeField] private Toys_Gain_Tween_Animation Toys_Gain_Screen;
    [SerializeField] private Combo_Tween_Animation Combo_Screen;
    [SerializeField] private GameObject gameGuide;

    private bool isComboing = false;
    private int comboNum = 0;

    public static GameController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeGameData();
        TurnOnGuide();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag("TouchReceiver") && GameManager.Instance.gameData.playerCoin > 0)
                    {
                        gameGuide.SetActive(false);
                        Vector3 touchPosition = hit.point;
                        touchPosition.y += 1.5f;
                        Instantiate(coinPrefab, touchPosition, Quaternion.identity, itemHolder.transform);
                        GameManager.Instance.ConsumeCoins(-1);

                        int randomChance = Random.Range(0, 101);

                        if (randomChance <= 10)
                        {
                            DropItems();
                        }
                    }
                }
            }
        }
    }

    public void PlayRandomPrizeFX()
    {
        prizeFX[Random.Range(0, prizeFX.Count)].Play();
    }

    public List<GameStateData> GetGameStateDatas()
    {
        List<GameStateData> objectsData = new();
        foreach (Transform child in itemHolder.transform)
        {
            if (child.TryGetComponent<Item>(out var item))
            {
                objectsData.Add(new GameStateData(item.itemInfo.id, child.position, child.rotation));
            }
        }
        return objectsData;
    }

    private void InitializeGameData()
    {
        GameData gameData = GameManager.Instance.gameData;

        // Generate previous item position
        List<GameStateData> data = gameData.gameplayData;
        if (data != null && data.Count > 0)
        {
            foreach (GameStateData item in data)
            {
                SpawnItem(
                item.id,
                new Vector3(item.position.x, item.position.y, item.position.z),
                new Quaternion(item.rotation.x, item.rotation.y, item.rotation.z, item.rotation.w)
                );
            }
        }
        else
        {
            foreach (Transform child in defaultPrefab.transform)
            {
                Instantiate(child.gameObject, child.localPosition, child.localRotation, itemHolder.transform);
            }
        }
    }

    public void DropItems()
    {
        SpawnItem(itemData.items[Random.Range(0, itemData.items.Count)].id, GetRandomDropperPosition(), Quaternion.identity);
    }

    private void SpawnItem(int id, Vector3 position, Quaternion rotation)
    {
        ItemInfo itemInfo = itemData.FindById(id);
        GameObject itemPrefab = itemInfo.itemPrefab;
        GameObject IniItem = Instantiate(itemPrefab, position, rotation, itemHolder.transform);
        Item itemClass = IniItem.AddComponent<Item>();
        itemClass.itemInfo = itemInfo;
        if (id == 1) itemClass.isCoin = true;
    }

    private Vector3 GetRandomDropperPosition()
    {
        List<Vector3> VerticeList = new List<Vector3>(dropper.GetComponent<MeshFilter>().sharedMesh.vertices);
        Vector3 leftTop = dropper.transform.TransformPoint(VerticeList[0]);
        Vector3 rightTop = dropper.transform.TransformPoint(VerticeList[10]);
        Vector3 leftBottom = dropper.transform.TransformPoint(VerticeList[110]);
        Vector3 rightBottom = dropper.transform.TransformPoint(VerticeList[120]);
        Vector3 XAxis = rightTop - leftTop;
        Vector3 ZAxis = leftBottom - leftTop;
        Vector3 RndPointonPlane = leftTop + XAxis * Random.value + ZAxis * Random.value;

        return RndPointonPlane;
    }

    public void GainToys(GameObject prefab)
    {
        Toys_Gain_Screen.SetToysModel(prefab);
        Toys_Gain_Screen.gameObject.SetActive(true);
        Toys_Gain_Screen.MoveToCenter();
        StartCoroutine(MoveToButtonAndDisapppear());
    }

    IEnumerator MoveToButtonAndDisapppear()
    {
        yield return new WaitForSeconds(2.5f);
        Toys_Gain_Screen.MoveAndDisappear();
    }

    public void DisplayCombo()
    {
        if (!isComboing)
        {
            isComboing = true;
            comboNum++;
            Combo_Screen.DisplayComboSceen(comboNum);
            StartCoroutine(ResetCombo(3f));
        }
        else
        {
            comboNum++;
            Combo_Screen.UpdateComboText(comboNum);
        }
    }

    IEnumerator ResetCombo(float delay)
    {
        yield return new WaitForSeconds(delay);
        comboNum = 0;
        isComboing = false;
    }

    public void TurnOnGuide()
    {
        gameGuide.SetActive(true);
    }
}
