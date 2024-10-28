using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField]
    public int roomCount = 10;

    [SerializeField]
    int minRandomRange = 3;

    [SerializeField]
    int maxRandomRange = 5;

    [SerializeField, Tooltip("방이 다 만들어지는데 검사하는 시간")]
    float mapGenerateTime = 0.3f;

    [SerializeField]
    Transform minimapPanel;

    [SerializeField]
    Transform player;

    [SerializeField]
    GameObject mapGeneratorPrefab;

    GameObject mapGenerator;

    [HideInInspector]
    public List<GameObject> rooms;

    bool isSecond;

    public bool IsSpawnComplete { get { return roomCount <= rooms.Count; } }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateRandomMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateRandomMap()
    {
        if(mapGenerator != null)
        {
            Destroy(mapGenerator);
            mapGenerator = null;
        }

        rooms.Clear();
        mapGenerator = Instantiate(mapGeneratorPrefab, minimapPanel);
        mapGenerator.transform.localPosition = Vector3.zero;
        //mapGenerator.transform.localScale = new Vector3(15, 15, 0);

        StartCoroutine(CoCheckMapeGenerateComplete());
    }

    IEnumerator CoCheckMapeGenerateComplete()
    {
        yield return new WaitForSeconds(mapGenerateTime);

        if(IsSpawnComplete == false)
        {
            GenerateRandomMap();
        }
    }

    public void SpawnComplete()
    {
        if (isSecond)
        {
            return;
        }

        isSecond = true;

        //각 방의 타입을 정해줌

        int rand = Random.Range(1, rooms.Count - 1);
        rooms[rand].GetComponent<Room>().roomType = RoomType.shop;

        int specialCount = Random.Range(minRandomRange, maxRandomRange);
        for (int i=0;i<specialCount; i++)
        {
            rand = Random.Range(1, rooms.Count - 1); //첫번째방은 start방이고 마지막방은 boss방
            Room room = rooms[rand].GetComponent<Room>();
            if (room.roomType != RoomType.battle)
            {
                i--;
                continue;
            }

            room.roomType = RoomType.randomEvent;
        }

        rooms[0].GetComponent<Room>().roomType = RoomType.start;
        rooms[rooms.Count - 1].GetComponent<Room>().roomType = RoomType.boss;

        //방에 걸맞는 벽을 세워줌
        for (int i = 0; i < rooms.Count; i++)
        {
            Room room = rooms[i].GetComponent<Room>();
            room.SetRoom();
            room.HideArrow();
            room.HideRoom();
        }

        //플레이어를 시작방으로 이동
        player.transform.position = rooms[0].transform.position;
        rooms[0].GetComponent<Room>().ShowRoom();

    }

}
