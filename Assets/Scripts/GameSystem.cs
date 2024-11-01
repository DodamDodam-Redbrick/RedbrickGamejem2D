using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    [SerializeField]
    public RewardPanel rewardPanel;

    [SerializeField, Tooltip("�ʴ���")]
    float mapMoveTime = 2;

    [SerializeField]
    GameObject playerLayout;

    [SerializeField]
    GameObject minimapPrefab;
    RoomManager minimap;

    [SerializeField]
    GameObject battleMapPrefab;
    BattleManager battleMap;

    [SerializeField]
    int rewardAmount = 3;

    [SerializeField]
    int minRewardGold = 5;

    [SerializeField]
    int maxRewardGold = 10;

    int stage = 1;

    [Header("Room Panels")]
    [SerializeField]
    public EventPanel eventPanel;


    Dictionary<int, List<MapType>> stageMaps = new Dictionary<int, List<MapType>>()
    {
        {1, new List<MapType>(){MapType.firstStage_one, MapType.firstStage_two } },
    };

    public float MapMoveTime { get { return mapMoveTime; } }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStartGame();

#if UNITY_EDITOR
        // GetReward();
        GetEvent();
#endif
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
        //���� �����ϸ�
        //1. �� ��������
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

    }
    public void StartBattle()
    {
        playerLayout.SetActive(true);


        //������ ���ؼ� ��Ʋ�� �����ֱ�
    }

    public void StartBossBattle()
    {
        playerLayout.SetActive(true);

        //���������� ��Ʋ�� �����ֱ�
    }

    public void FinishBattle()
    {
        playerLayout.SetActive(false);

        GetReward();
        //������ �ް� ȭ�� ������ ���ϸ� Coroutine����
        CloseBattleMap();
    }

    public void GetReward()
    {
        //�������� ������ ���ϱ�
        List<Reward> rewards = new List<Reward>();

        for(int i = 0; i < rewardAmount; i++)
        {
            RewardType rewardType = GetRandomRewardType();
            Reward reward = new Reward(DataManager.rewardData[rewardType].thumbnail, DataManager.rewardData[rewardType].description
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
                    UnitType unitType = GetRandomUnitType();
                    UnitInfo originUnitInfo = (UnitInfo)DataManager.entityData[(EntityType)unitType]; //���� ����
                    UnitInfo unit = new UnitInfo(originUnitInfo.entityStats, unitType, originUnitInfo.thumbnail, originUnitInfo.entityPrefab); //���� ����
                    reward.unit = unit;
                    break;
            }

            rewards.Add(reward);
        }

        rewardPanel.ShowPopupPanel(rewards);
    }

    public void FinishGetReward()
    {
        rewardPanel.HidePopupPanel();
        //�̴ϸ� ����
    }

    public void DefeatedBattle()
    {

        //�Ұ� ���ϰ� ���θ޴��� ���� (�������� ���ǥ �����ִ°͵� ��������
    }

    public void ShowRandomRewardPopup(int count)
    {
        List<Reward> rewards = new List<Reward>();

        for (int i = 0; i < count; i++)
        {
            //���� �� �����ϰ� rewards�� �߰�
        }

        ShowRewardPopup(rewards);
    }

    public void ShowRewardPopup(List<Reward> rewards)
    {
        rewardPanel.ShowPopupPanel(rewards);
    }

    RewardType GetRandomRewardType()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(RewardType));
        return (RewardType)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }

    UnitType GetRandomUnitType()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(UnitType));
        return (UnitType)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }


    public void GetEvent()
    {
        EventType eventType = GetRandomEventType();
        Event currentEvent = DataManager.eventData[eventType];

        eventPanel.ShowEventPanel(currentEvent);

    }

    EventType GetRandomEventType()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(EventType));
        return (EventType)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }
    
    public void FinishGetEvent()
    {
        eventPanel.HidePopUpPanel();
    }
}
