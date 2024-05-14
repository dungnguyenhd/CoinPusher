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
    [SerializeField] private Transform dropper;
    [SerializeField] private Transform rainDropper;
    [SerializeField] private Toys_Gain_Tween_Animation Toys_Gain_Screen;
    [SerializeField] private Combo_Tween_Animation Combo_Screen;
    [SerializeField] private GameObject gameGuide;
    [SerializeField] private float explosionFieldOfImpact;
    [SerializeField] private float explosionForce;
    [SerializeField] private ParticleSystem explosionFx;

    private bool isComboing = false;
    private int comboNum = 0;
    private int delayTime = 2;

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
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f);
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

                        if (randomChance <= 8 && randomChance > 1)
                        {
                            int ran = Random.Range(0, 2);
                            if (ran == 0)
                            {
                                DropItems();
                            }
                            else
                            {
                                ReplaceItem();
                            }
                        }
                        else if (randomChance <= 1)
                        {
                            StartCoroutine(CoinRain());
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
        SpawnItem(itemData.items[Random.Range(1, itemData.items.Count)].id, GetRandomDropperPosition(), Quaternion.identity);
    }

    private void SpawnItem(int id, Vector3 position, Quaternion rotation)
    {
        ItemInfo itemInfo = itemData.FindById(id);
        GameObject itemPrefab = itemInfo.itemPrefab;
        GameObject IniItem = Instantiate(itemPrefab, position, rotation, itemHolder.transform);
        Item itemClass = IniItem.AddComponent<Item>();
        itemClass.itemInfo = itemInfo;
        if (id == 1 || id == 2) itemClass.isCoin = true;
    }

    private Vector3 GetRandomDropperPosition()
    {
        List<Vector3> VerticeList = new(dropper.GetComponent<MeshFilter>().sharedMesh.vertices);
        Vector3 leftTop = dropper.TransformPoint(VerticeList[0]);
        Vector3 rightTop = dropper.TransformPoint(VerticeList[10]);
        Vector3 leftBottom = dropper.TransformPoint(VerticeList[110]);
        Vector3 rightBottom = dropper.TransformPoint(VerticeList[120]);
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

    private void ReplaceItem()
    {
        Transform randomCoin = GetRandomCoin();

        if (randomCoin != null)
        {
            randomCoin.GetPositionAndRotation(out Vector3 pos, out Quaternion rot);

            ExplodeCoin(randomCoin.gameObject);

            int id = itemData.items[Random.Range(1, itemData.items.Count)].id;
            ItemInfo itemInfo = itemData.FindById(id);
            GameObject itemPrefab = itemInfo.itemPrefab;
            GameObject IniItem = Instantiate(itemPrefab, pos, rot, itemHolder.transform);
            Item itemClass = IniItem.AddComponent<Item>();
            itemClass.itemInfo = itemInfo;
            if (id == 1 || id == 2) itemClass.isCoin = true;
        }
    }

    private IEnumerator CoinRain()
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.05f);
            SpawnItem(1, rainDropper.position, rainDropper.rotation);
        }
    }

    private void ExplodeCoin(GameObject coin)
    {
        Collider[] colliders = Physics.OverlapSphere(coin.transform.position, explosionFieldOfImpact);
        explosionFx.transform.position = coin.transform.position;
        explosionFx.Play();
        foreach (Collider target in colliders)
        {
            if (target.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddExplosionForce(explosionForce, coin.transform.position, explosionFieldOfImpact);
            }
        }
        Destroy(coin);
    }

    private Transform GetRandomCoin()
    {
        int itemNumber = itemHolder.transform.childCount;
        int loopCount = 0;
        Transform randomCoin = null;
        while (loopCount < itemNumber)
        {
            int randomIndex = Random.Range(0, itemNumber);
            Transform child = itemHolder.transform.GetChild(randomIndex);
            Item item = child.GetComponent<Item>();
            if (item != null && item.isCoin)
            {
                randomCoin = child;
                break;
            }
            loopCount++;
        }
        return randomCoin;
    }

    public void CountingCombo()
    {
        if (!isComboing)
        {
            isComboing = true;
            comboNum++;
            delayTime = 2;
            Combo_Screen.DisplayComboSceen(comboNum);
            StartCoroutine(ResetCombo());
        }
        else
        {
            comboNum++;
            delayTime++;
            if (delayTime > 5) delayTime = 5;
            Debug.Log(comboNum);
            Combo_Screen.UpdateComboText(comboNum);
        }
    }

    IEnumerator ResetCombo()
    {
        while (delayTime > 0)
        {
            yield return new WaitForSeconds(1f);
            delayTime--;
        }
        comboNum = 0;
        delayTime = 0;
        isComboing = false;
    }

    public void TurnOnGuide()
    {
        gameGuide.SetActive(true);
    }
}
