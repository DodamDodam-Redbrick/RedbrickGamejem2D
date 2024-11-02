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
    GameObject mapGeneratorPrefab;

    GameObject mapGenerator;

    Player player;

    [HideInInspector]
    public List<Room> rooms;

    bool isSecond;

    public bool IsSpawnComplete { get { return roomCount <= rooms.Count; } }

    public void Init()
    {
        Instance = this;

        player = Player.Instance;
    }

    public void GenerateRandomMap()
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

        if (IsSpawnComplete == false)
        {
            GenerateRandomMap();
        }

        else
        {
            StartCoroutine(GameSystem.Instance.FinishLoading(0f));
            GameSystem.Instance.GetReward();
        }
    }

    public void SpawnComplete()
    {
        if (isSecond)
        {
            return;
        }

        isSecond = true;


        SetEachRoomType();

        //방에 걸맞는 벽을 세워줌
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetRoom();
            rooms[i].HideArrow();
            rooms[i].HideRoom();
        }

        rooms[0].ShowRoom();
        //플레이어를 시작방으로 이동
        player.transform.position = rooms[0].transform.position;
    }


    void SetEachRoomType()
    {
        //각 방의 타입을 정해줌
        int rand = Random.Range(1, rooms.Count - 1);
        rooms[rand].roomType = RoomType.shop;

        //기본이 battle이기 때문에 한번 초기화해줌
        foreach (Room room in rooms)
        {
            room.roomType = RoomType.battle;
        }

        int specialCount = Random.Range(minRandomRange, maxRandomRange);
        for (int i = 0; i < specialCount; i++)
        {
            rand = Random.Range(1, rooms.Count - 1); //첫번째방은 start방이고 마지막방은 boss방
            Room room = rooms[rand];
            if (room.roomType != RoomType.battle)
            {
                i--;
                continue;
            }

            room.roomType = RoomType.randomEvent;
        }

        rooms[0].roomType = RoomType.start;
        rooms[rooms.Count - 1].roomType = RoomType.boss;
    }
}
