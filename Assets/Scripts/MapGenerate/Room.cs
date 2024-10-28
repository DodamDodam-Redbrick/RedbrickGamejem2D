using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum RoomDirection
{
    left,
    right,
    top,
    bottom,
}

public enum RoomType
{
    battle,
    start,
    boss,
    randomEvent,
    shop,
}

public enum wallType
{
    L = 0b0001,
    R = 0b0010,
    T = 0b0100,
    B = 0b1000,

    LR = L|R,
    LT = L|T,
    LB = L|B,
    RT = R|T,
    RB = R|B,
    TB = T|B,

    LTB = L|T|B,
    LRB = L|R|B,
    LRT = L|R|T,
    RTB = R|T|B,

    LRTB = L|R|T|B,
}

public class Room : MonoBehaviour
{
    [SerializedDictionary("wallType", "wallPrefab")]
    public SerializedDictionary<wallType, GameObject> wallDic;

    // Start is called before the first frame update
    // 이미 스폰포인트에 방이 있으면 잇지 않는다
    [SerializeField]
    GameObject roomPrefab;

    [SerializeField]
    SpriteRenderer roomTypeSprite;

    [SerializeField, Tooltip("left, right, top, bottom 순으로 정렬해야함")]
    SpawnPoint[] spawnPoints;

    [SerializeField, Tooltip("방이 생성되는 확률, 기준은 할")]
    int connectRate = 6;

    RoomType _roomType;

    public RoomType roomType
    {
        get { return _roomType; }
        set
        {
            _roomType = value;
            OnRoomTypeChanged(_roomType);
        }
    }

    bool[] connectedRoom = new bool[4];
    RoomDirection[] reverseDirection = { RoomDirection.right, RoomDirection.left, RoomDirection.bottom, RoomDirection.top };

    void Start()
    {
        RoomManager.Instance.rooms.Add(this.gameObject);

        StartCoroutine(SpawnRoom());
    }

    IEnumerator SpawnRoom()
    {
        for (int dir = 0; dir < 4; dir++)
        {
            yield return null;

            if (RoomManager.Instance.IsSpawnComplete)
            {
                RoomManager.Instance.SpawnComplete();
                yield break;
            }

            if (connectedRoom[dir])
            {
                continue;
            }

            if (spawnPoints[dir].isRoom) //이게 오류일 수도 있음
            {
                continue;
            }

            int rand = UnityEngine.Random.Range(0, 10);

            if(rand < connectRate)
            {
                GameObject inst = Instantiate(roomPrefab, transform.parent);
                inst.transform.position = spawnPoints[dir].transform.position;
                inst.transform.rotation = Quaternion.identity;
                inst.GetComponent<Room>().ConnectRoom(reverseDirection[dir]);
                connectedRoom[dir] = true;
            }

            yield return null;
        }
    }

    public void SetWall()
    {
        int wallMask = 0;
        int binary = 1;
        for(int i=0; i<4; i++)
        {
            if (connectedRoom[i])
            {
                wallMask |= binary;
            }

            binary *= 2;
        }

        Instantiate(wallDic[(wallType)wallMask], this.transform);
    }

    public void ConnectRoom(RoomDirection dir)
    {
        connectedRoom[(int)dir] = true;
    }

    public void OnRoomTypeChanged(RoomType newValue)
    {
        switch (newValue)
        {
            case RoomType.shop:
                roomTypeSprite.sprite = DataManager.ImageData[ImageIndex.map_shop];
            break;
            case RoomType.start:
                roomTypeSprite.sprite = DataManager.ImageData[ImageIndex.map_start];
                break;
            case RoomType.boss:
                roomTypeSprite.sprite = DataManager.ImageData[ImageIndex.map_boss];
                break;
            case RoomType.battle:
                roomTypeSprite.sprite = DataManager.ImageData[ImageIndex.map_battle];
                break;
            case RoomType.randomEvent:
                roomTypeSprite.sprite = DataManager.ImageData[ImageIndex.map_randomEvent];
                break;
        }
    }
}
