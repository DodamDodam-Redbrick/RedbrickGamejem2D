using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    [SerializeField]
    public RewardPanel rewardPanel;

    [SerializeField, Tooltip("초단위")]
    float mapMoveTime = 2;

    [SerializeField]
    GameObject playerLayout;

    [SerializeField]
    GameObject minimapPrefab;
    RoomManager minimap;

    [SerializeField]
    GameObject battleMapPrefab;
    BattleManager battleMap;

    int stage = 1;

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
#if UNITY_EDITOR
        //리워드 창 만드는 예시
        //List<Reward> rewards = new List<Reward>();
        
        //UnitInfo unit = (UnitInfo)DataManager.entityData[EntityType.sword];

        //Reward reward = new Reward(DataManager.imageData[ImageIndex.unit_sword_thumbnail], "hi", RewardType.unit, unit);

        //rewards.Add(reward);
        //rewards.Add(reward);

        //ShowRewardPopup(rewards);
#endif

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
        }
    }

    public void StartBattle()
    {
        playerLayout.SetActive(true);


        //랜덤맵 정해서 배틀맵 보여주기
    }

    public void StartBossBattle()
    {
        playerLayout.SetActive(true);

        //보스맵으로 배틀맵 보여주기
    }

    public void FinishBattle()
    {
        playerLayout.SetActive(false);

        //할거 다하고 메인메뉴로 가기 (가기전에 통계표 보여주는것도 ㄱㅊ을듯
    }

    public void DefeatedBattle()
    {

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

    public void ShowRewardPopup(List<Reward> rewards)
    {
        rewardPanel.ShowPopupPanel(rewards);
    }
}
