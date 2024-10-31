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
    GameObject minimapPrefab;
    GameObject minimap;

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
            Destroy(minimap);
            minimap = null;
        }

        minimap = Instantiate(minimapPrefab);
        RoomManager minimapManager = minimap.GetComponent<RoomManager>();
        minimapManager.Init();
        minimapManager.GenerateRandomMap();
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
