using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

public class GameSystem : MonoBehaviour
{
    public Transform canvasTrans;

    public int maxCold = 10;
    public int coldLevel = 0;
    public float coldIncreaseAmount = 1f;

    public float deBuffColdPercent;

    public static GameSystem Instance;

    [SerializeField]
    public int difficultRoomCount = 5;

    [SerializeField]
    public float outlineSize = 100f;

    [SerializeField, Tooltip("초단위")]
    float mapMoveTime = 2;

    [SerializeField]
    public float bulletSpeed = 10f;

    [SerializeField]
    int rewardAmount = 3;

    [SerializeField]
    int minRewardGold = 5;

    [SerializeField]
    int maxRewardGold = 10;

    [Header("Panels")]

    [SerializeField]
    public EventPanel eventPanel;

    [SerializeField]
    public RewardPanel rewardPanel;

    [SerializeField]
    public ShopPanel shopPanel;

    [SerializeField]
    public GameObject defeatedPanel;
    
    public List<RewardType> shopList;
    public int shopAmount = 6;

    [HideInInspector]
    public List<Unit> placedUnit = new List<Unit>();

    [HideInInspector]
    public UnitInfo mainCharacter;

    [SerializeField]
    GameObject loadingPanel;

    [SerializeField]
    IceLayout icePanel;

    [Header("Prefabs")]
    [SerializeField]
    GameObject minimapPrefab;
    RoomManager minimap;

    [Header("Layouts")]
    [SerializeField]
    PlayerLayout playerLayout;

    //int stage = 1;
    [HideInInspector]
    public BattleManager battleMap;

    int startRewardCount = 0;

    public float MapMoveTime { get { return mapMoveTime; } }

    public bool isOnPanel = false;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine(CoStartGame());
        //shopList = null;
        //GetShop();
#if UNITY_EDITOR
        mainCharacter = DataManager.Instance.unitData[UnitType.mainCharacter].DeepCopy();
        //shopList = null;
        //GetShop();
        //GetReward();
        //StartRandomEvent();
        //GetUnitReward(UnitType.sword_2, StartTestBattle);
#endif
    }

    IEnumerator CoStartGame()
    {
        while (!DataManager.Instance.isFinishLoad)
        {
            yield return null;
        }

        OnStartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if (hit.collider != null)
            {
                Arrow arrowHit = hit.collider.GetComponent<Arrow>();
                if (arrowHit != null)
                {
                    arrowHit.OnClickArrow();
                }
            }
        }
    }

    void OnStartGame()
    {
        ShowLoading();


        //게임 시작하면
        //1. 맵 랜덤생성
        if (minimap != null)
        {
            Destroy(minimap.gameObject);
            minimap = null;
        }

        minimap = Instantiate(minimapPrefab, new Vector2(-100, -100), Quaternion.identity).GetComponent<RoomManager>();
        minimap.Init();
        minimap.GenerateRandomMap();
        minimap.transform.SetParent(canvasTrans, false);

        //메인캐릭터 딥카피
        mainCharacter = DataManager.Instance.unitData[UnitType.mainCharacter].DeepCopy();
    }

    public void ShowIcePanel()
    {
        icePanel.gameObject.SetActive(true);
        icePanel.SetIceLevel(coldLevel);
    }

    public void HideIcePanel()
    {
        icePanel.gameObject.SetActive(false);
    }

    public void EnterStartRoom()
    {
        if(startRewardCount >= 3)
        {
            return;
        }

        GetRandomReward(true, EnterStartRoom);

        startRewardCount += 1;
    }

    public void EnterNewRoom(RoomType roomType)
    {
        ShowLoading();
        Invoke("FinishLoading", 0.5f);
        switch (roomType)
        {
            case RoomType.start:
                
                EnterStartRoom();

                break;

            case RoomType.battle:

                StartBattle();

                break;
            case RoomType.boss:
                StartBossBattle();
                break;

            case RoomType.randomEvent:
                StartRandomEvent();
                break;
            case RoomType.shop:
                GetShop();
                break;

        }
    }


    public void GoToMainMenu()
    {
        SceneName changeSceneName = SceneName.MainMenu;

        SceneManager.LoadScene(changeSceneName.ToString());
    }

    public void CloseBattleMap()
    {
        if (battleMap != null)
        {
            Destroy(battleMap.gameObject);
            battleMap = null;
        }
    }

    public void StartRandomEvent()
    {
        EventType eventType = GetRandomEnumType<EventType>();
        Event currentEvent = DataManager.Instance.eventData[eventType];

        eventPanel.ShowEventPanel(currentEvent);
    }

    void SetMinimapLayout()
    {
        Player.Instance.GetComponentInChildren<AudioListener>().enabled = true;
        playerLayout.Hide();
    }

    void SetBattleLayout()
    {
        Player.Instance.GetComponentInChildren<AudioListener>().enabled = false;
        playerLayout.Show();
    }

    public void StartBattle()
    {
        //랜덤맵 정해서 배틀맵 보여주기
        MapType mapType = GetRandomEnumType<MapType>();
        battleMap = Instantiate(DataManager.Instance.mapDatas[mapType]).GetComponent<BattleManager>();
        battleMap.Init();
        battleMap.StartBattle();

        //배틀맵 세팅하고 레이아웃 켜야함
        SetBattleLayout();
    }

    public void StartTestBattle()
    {
        battleMap = Instantiate(DataManager.Instance.mapDatas[MapType.firstStage_one]).GetComponent<BattleManager>();
        battleMap.Init();
        battleMap.StartBattle();

        //배틀맵 세팅하고 레이아웃 켜야함
        SetBattleLayout();
    }

    public void StartBossBattle()
    {
        //스테이지에 맞는 보스맵
        
        SetBattleLayout();
        //보스맵으로 배틀맵 보여주기
    }

    public void FinishBattle()
    {
        SetMinimapLayout();

        SyncUnitData();

        GetRandomReward(false ,CloseBattleMap);
    }

    void SyncUnitData()
    {
        List<UnitInfo> unitList = Player.Instance.unitList;

        foreach (Unit unit in placedUnit)
        {
            unitList[unit.cardIndex] = unit.unitInfo;
        }
    }

    public void GetUnitReward(UnitType unitType, UnityAction endAction = null)
    {
        List<Reward> rewards = new List<Reward>();
        RewardType rewardType = (RewardType)unitType;

        Reward reward = new Reward(DataManager.Instance.rewardData[rewardType].thumbnail, DataManager.Instance.rewardData[rewardType].description
            , rewardType);

        UnitInfo originUnitInfo = (UnitInfo)DataManager.Instance.unitData[unitType]; //얕은 복사
        UnitInfo unit = new UnitInfo(originUnitInfo.entityStats, unitType, originUnitInfo.thumbnail, originUnitInfo.entityPrefab, originUnitInfo.cost, originUnitInfo.placeNodeType); //깊은 복사
        reward.unit = unit;

        rewards.Add(reward);

        rewardPanel.ShowPopupPanel(rewards, endAction);
    }

    public Reward GetReward(RewardType rewardType, bool onlyUnit = false)
    {
        Reward reward = new Reward(DataManager.Instance.rewardData[rewardType].thumbnail, DataManager.Instance.rewardData[rewardType].description
                        , rewardType);

        bool isUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => rewardType == (RewardType)unit);

        if (onlyUnit && !isUnitType)
            return null;

        if (rewardType == RewardType.reward_gold)
        {
            reward.gold = Random.Range(minRewardGold, maxRewardGold);
        }

        return reward;
    }

    public void GetRandomReward(bool isOnlyUnit = false, UnityAction endAction = null)
    {
        // 랜덤으로 리워드 정하기
        List<Reward> rewards = new List<Reward>();
        while (rewards.Count < rewardAmount)
        {
            RewardType rewardType = GetRandomRewardType();
            Reward reward = GetReward(rewardType, isOnlyUnit);

            if (reward == null)
                continue;

            rewards.Add(CopyUnitType(reward));
        }

        rewardPanel.ShowPopupPanel(rewards, endAction);
    }

    private RewardType GetRandomRewardType()
    {
        float totalWeight = DataManager.Instance.rewardChanceDatas
                            .Where(pair => !IsShopReward(pair.Key)) // shop_potion 타입 제외
                            .Sum(pair => pair.Value);
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var rewardChance in DataManager.Instance.rewardChanceDatas)
        {
            if (IsShopReward(rewardChance.Key))
                continue;

            cumulativeWeight += rewardChance.Value;
            if (randomValue <= cumulativeWeight)
            {
                return rewardChance.Key;
            }
        }

        return RewardType.reward_gold; // Default return value, should not reach here
    }

    private bool IsShopReward(RewardType rewardType)
    {
        return rewardType == RewardType.shop_potion_one
            || rewardType == RewardType.shop_potion_two
            || rewardType == RewardType.shop_potion_three;
    }

    public void FinishGetReward(UnityAction endAction = null)
    {
        rewardPanel.HidePopupPanel();

        if (endAction != null)
        {
            endAction();
        }
    }

    public void DefeatedBattle()
    {

        //할거 다하고 메인메뉴로 가기 (가기전에 통계표 보여주는것도 ㄱㅊ을듯
        defeatedPanel.SetActive(true);
    }

    public void ShowRandomRewardPopup(int count)
    {
        List<Reward> rewards = new List<Reward>();

        for (int i = 0; i < count; i++)
        {
            //보상 중 랜덤하게 rewards에 추가
        }


        ShowRewardPopup(rewards); //endAction도 추가
    }

    public void ShowRewardPopup(List<Reward> rewards, UnityAction endAction = null)
    {
        rewardPanel.ShowPopupPanel(rewards, endAction);
    }

    public void FinishGetEvent()
    {
        eventPanel.HidePopUpPanel();
    }

    public T GetRandomEnumType<T>()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(T));
        return (T)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }

    public void GetShop()
    {
        List<Reward> shops = new List<Reward>();
        List<RewardType> shopType = GetRandomShopEnum();
        while (shops.Count < shopAmount)
        {
            RewardType rewardType = shopType[Random.Range(0, shopType.Count)];
            bool isUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => rewardType == (RewardType)unit);

            // 인덱스에 따른 조건
            if ((shops.Count > 2 && isUnitType) || (shops.Count <= 2 && !isUnitType))
                continue;

            Reward shop = new Reward(DataManager.Instance.rewardData[rewardType].thumbnail, DataManager.Instance.rewardData[rewardType].description, rewardType, DataManager.Instance.rewardData[rewardType].shopPrice);

            CopyUnitType(shop);
            shops.Add(shop);
        }
        shopPanel.ShowShopPanel(shops);
    }

    List<RewardType> GetRandomShopEnum()
    {
        if (shopList != null)
            return shopList;

        shopList = new List<RewardType>();

        foreach (RewardType rT in System.Enum.GetValues(enumType: typeof(RewardType)))
        {
            shopList.Add(rT);
        }

        List<RewardType> subList = new List<RewardType>();
        subList.Add(RewardType.reward_gold);

        shopList = shopList.Except(subList).ToList();

        return shopList;

    }


    // 추위 증가
    public void IncreaseCold() 
    {
        coldLevel += (int)coldIncreaseAmount;
        SetCold();

        if(coldLevel < 2)
        {
            HideIcePanel();
        }
        else
        {
            ShowIcePanel();
        }

    }

    public void SetCold()
    {
        if (coldLevel >= 2)
        {
            deBuffColdPercent = 0.85f;
        }
        else if (coldLevel >= 4)
        {
            deBuffColdPercent = 0.7f;

        }
        else if (coldLevel >= 6)
        {
            deBuffColdPercent = 0.55f;

        }
        else if (coldLevel >= 8)
        {
            deBuffColdPercent = 0.40f;

        }
        else if (coldLevel >= maxCold)
        {
            deBuffColdPercent = 0.25f;
            coldLevel = maxCold;
        }
    }

    public void ShowLoading()
    {
        loadingPanel.SetActive(true);
    }

    public void FinishLoading()
    {
        loadingPanel.SetActive(false);
    }

    public Reward CopyUnitType(Reward reward)
    {
        UnitType unitType = (UnitType)reward.rewardType;
        RewardType rewardType = reward.rewardType;
        UnitInfo unit;
        // mainCharacter = DataManager.Instance.unitData[UnitType.mainCharacter].DeepCopy();

        switch (rewardType)
        {
            case RewardType.reward_gold:
                break;

            case RewardType.unit_sword_1:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;

            case RewardType.unit_sword_2:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                Debug.Log($"{reward.unit.unitType}");
                break;


            case RewardType.unit_sword_3:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;

            case RewardType.unit_archer_1:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;

            case RewardType.unit_archer_2:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;


            case RewardType.unit_archer_3:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;


            case RewardType.unit_wizard_1:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;

            case RewardType.unit_wizard_2:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;


            case RewardType.unit_wizard_3:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;

            case RewardType.unit_soldier_1:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;

            case RewardType.unit_soldier_2:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;


            case RewardType.unit_soldier_3:
                unit = DataManager.Instance.unitData[unitType].DeepCopy();
                reward.unit = unit;
                break;


        }

        return reward;
    }

    public bool OnPanels()
    {
        if(eventPanel.gameObject.activeSelf == true || shopPanel.gameObject.activeSelf == true || rewardPanel.gameObject.activeSelf == true)
            isOnPanel = true;

        else
            isOnPanel = false;

        return isOnPanel;

        
    }
}