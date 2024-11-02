using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    [SerializeField, Tooltip("초단위")]
    float mapMoveTime = 2;

    [SerializeField]
    int rewardAmount = 3;
    int shopAmount = 5;

    [SerializeField]
    int minRewardGold = 5;

    [SerializeField]
    int maxRewardGold = 10;

    int stage = 1;

    [Header("Panels")]
    [SerializeField]
    public EventPanel eventPanel;

    [SerializeField]
    public RewardPanel rewardPanel;

    [SerializeField]
    public ShopPanel shopPanel;

    [SerializeField]
    GameObject loadingPanel;

    [Header("Prefabs")]
    [SerializeField]
    GameObject minimapPrefab;
    RoomManager minimap;

    [SerializeField]
    GameObject battleMapPrefab;
    BattleManager battleMap;

    [Header("Layouts")]
    [SerializeField]
    PlayerLayout playerLayout;

    //int stage = 1;

    public BattleManager battleMap;

    public float MapMoveTime { get { return mapMoveTime; } }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(CoStartGame());

#if UNITY_EDITOR
        //GetReward();
        //GetEvent();

        GetUnitReward(UnitType.sword, StartBattle);
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
            if(hit.collider != null)
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
        //게임 시작하면
        //1. 맵 랜덤생성
        if(minimap != null)
        {
            Destroy(minimap.gameObject);
            minimap = null;
        }

        minimap = Instantiate(minimapPrefab).GetComponent<RoomManager>();
        minimap.Init();
        minimap.GenerateRandomMap();
    }

    public void EnterNewRoom(RoomType roomType)
    {
        switch (roomType)
        {
            case RoomType.battle:
                StartBattle();
                break;
            case RoomType.boss:
                StartBossBattle();
                break;

            case RoomType.randomEvent:
                GetEvent();
                break;

            case RoomType.shop:
                GetShop();
                break;

        }
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
        playerLayout.SetActive(true);


        //랜덤맵 정해서 배틀맵 보여주기
        MapType mapType = GetRandomEnumType<MapType>();
        battleMap = Instantiate(DataManager.Instance.mapDatas[mapType]).GetComponent<BattleManager>();
    }

    public void StartBossBattle()
    {
        playerLayout.SetActive(true);

        //보스맵으로 배틀맵 보여주기
    }

    public void FinishBattle()
    {
        SetMinimapLayout();

        GetReward(CloseBattleMap);
    }

    void GetUnitReward(UnitType unitType, UnityAction endAction = null)
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
    public void GetReward(UnityAction endAction = null)
    {
        //랜덤으로 리워드 정하기
        List<Reward> rewards = new List<Reward>();

        for(int i = 0; i < rewardAmount; i++)
        {
            RewardType rewardType = GetRandomEnumType<RewardType>();
            Reward reward = new Reward(DataManager.Instance.rewardData[rewardType].thumbnail, DataManager.Instance.rewardData[rewardType].description
                        , rewardType);

            if(rewardType == RewardType.gold)
            {
                reward.gold = Random.Range(minRewardGold, maxRewardGold);
            }

            switch (rewardType)
            {
                case RewardType.gold:
                    break;
                case RewardType.unit_sword:
                    UnitType unitType = GetRandomEnumType<UnitType>();
                    UnitInfo originUnitInfo = DataManager.Instance.unitData[unitType]; //얕은 복사
                    UnitInfo unit = new UnitInfo(originUnitInfo.entityStats, unitType, originUnitInfo.thumbnail, originUnitInfo.entityPrefab, originUnitInfo.cost, originUnitInfo.placeNodeType); //깊은 복사
                    reward.unit = unit;
                    break;
            }

            rewards.Add(reward);
        }

        rewardPanel.ShowPopupPanel(rewards, endAction);
    }

    public void FinishGetReward(UnityAction endAction = null)
    {
        rewardPanel.HidePopupPanel();

        if(endAction != null)
        {
            endAction();
        }
    }

    public void DefeatedBattle()
    {

        //할거 다하고 메인메뉴로 가기 (가기전에 통계표 보여주는것도 ㄱㅊ을듯
    }

    public void ShowRandomRewardPopup(int count)
    {
        List<Reward> rewards = new List<Reward>();

        for (int i = 0; i < count; i++)
        {
            //보상 중 랜덤하게 rewards에 추가
        }

        ShowRewardPopup(rewards);
    }

    public void ShowRewardPopup(List<Reward> rewards, UnityAction endAction = null)
    {
        rewardPanel.ShowPopupPanel(rewards, endAction);
    }
    public void GetEvent()
    {
        EventType eventType = GetRandomEnumType<EventType>();
        Event currentEvent = DataManager.eventData[eventType];

        eventPanel.ShowEventPanel(currentEvent);

    }
    public void FinishGetEvent()
    {
        eventPanel.HidePopUpPanel();
    }

    public void GetShop()
    {
        if(shops != null)
            shops.Clear();
        shops = new List<Reward>();
        List<RewardType> shopType = GetRandomShopEnum();
        int index = 0;
        while(index <= shopAmount)
        {
            RewardType rewardType = shopType[Random.Range(0, shopType.Count)];
            bool isUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => rewardType == (RewardType)unit);

            if (index > 2)
            {
                if (isUnitType)
                    continue;

            }

            else
            {
                if (!isUnitType)
                    continue;
            }

            Reward shop = new Reward(DataManager.rewardData[rewardType].thumbnail, DataManager.rewardData[rewardType].description, rewardType, DataManager.rewardData[rewardType].shopPrice);
            switch (rewardType)
            {
                case RewardType.unit_sword:
                    UnitType unitType = GetRandomEnumType<UnitType>();
                    UnitInfo originUnitInfo = (UnitInfo)DataManager.entityData[(EntityType)unitType]; //얕은 복사
                    UnitInfo unit = new UnitInfo(originUnitInfo.entityStats, unitType, originUnitInfo.thumbnail, originUnitInfo.entityPrefab); //깊은 복사
                    shop.unit = unit;
                    break;
            }

            shops.Add(shop);
            index++;
        }
        shopPanel.ShowShopPanel(shops);

    }

    T GetRandomEnumType<T>()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(T));
        return (T)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }

    List<RewardType> GetRandomShopEnum()
    {
        if(shopList != null)
            return shopList;

        shopList = new List<RewardType>();


        foreach (RewardType rT in System.Enum.GetValues(enumType: typeof(RewardType)))
        {
            shopList.Add(rT);
        }


        subList = new List<RewardType>();
        subList.Add(RewardType.gold);
        
        shopList = shopList.Except(subList).ToList();

        Debug.Log($"{shopList.Count} shops");
        return shopList;    

    }

    // Loading Panel
    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void FinishLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}
