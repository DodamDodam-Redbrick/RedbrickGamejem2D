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
    SpriteRenderer roomTypeSpriteRenderer;

    [SerializeField, Tooltip("left, right, top, bottom 순으로 정렬해야함")]
    SpawnPoint[] spawnPoints;

    [SerializeField, Tooltip("left, right, top, bottom 순으로 정렬해야함")]
    GameObject[] arrowPrefabs;

    [SerializeField]
    GameObject arrowParent;

    [SerializeField, Tooltip("방이 생성되는 확률, 기준은 할")]
    int connectRate = 6;

    Sprite roomTypeSprite;

    RoomType _roomType;

    bool isVisited;

    public RoomType roomType
    {
        get { return _roomType; }
        set
        {
            _roomType = value;
            OnRoomTypeChanged(_roomType);
        }
    }

    Room[] connectedRoom = new Room[4];
    RoomDirection[] reverseDirection = { RoomDirection.right, RoomDirection.left, RoomDirection.bottom, RoomDirection.top };

    void Start()
    {
        RoomManager.Instance.rooms.Add(this);

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
                Room roomInst = Instantiate(roomPrefab, transform.parent).GetComponent<Room>();
                roomInst.transform.position = spawnPoints[dir].transform.position;
                roomInst.transform.rotation = Quaternion.identity;
                roomInst.ConnectRoom(reverseDirection[dir], this);
                connectedRoom[dir] = roomInst;
            }

            yield return null;
        }
    }

    public void HideArrow()
    {
        arrowParent.gameObject.SetActive(false);
    }

    void ShowArrow()
    {
        arrowParent.gameObject.SetActive(true); 
    }

    public void SetRoom()
    {
        int wallMask = 0;
        int binary = 1;
        for(int i=0; i<4; i++)
        {
            if (connectedRoom[i] != null)
            {
                wallMask |= binary;

                Arrow arrowInst = Instantiate(arrowPrefabs[i], arrowParent.transform).GetComponent<Arrow>();
                arrowInst.MoveRoom = connectedRoom[i];
            }

            binary *= 2;
        }

        Instantiate(wallDic[(wallType)wallMask], this.transform);
    }

    public void HideRoom()
    {
        gameObject.SetActive(false);
    }

    public void ShowRoom()
    {
        gameObject.SetActive(true);
        if (!isVisited)
        {
            roomTypeSpriteRenderer.sprite = DataManager.imageData[ImageIndex.map_unknown];
         
        }
        else
        {
            roomTypeSpriteRenderer.sprite = roomTypeSprite;
        }
    }

    public void ShowNearbyRoom()
    {
        foreach (var room in connectedRoom) {
            if(room != null)
            {
                room.ShowRoom();
            }
        }
    }

    public void ConnectRoom(RoomDirection dir, Room room)
    {
        connectedRoom[(int)dir] = room;
    }

    public void OnRoomTypeChanged(RoomType newValue)
    {
        switch (newValue)
        {
            case RoomType.shop:
                roomTypeSprite = DataManager.imageData[ImageIndex.map_shop];
            break;
            case RoomType.start:
                roomTypeSprite = DataManager.imageData[ImageIndex.map_start];
            break;
            case RoomType.boss:
                roomTypeSprite = DataManager.imageData[ImageIndex.map_boss];
            break;
            case RoomType.battle:
                roomTypeSprite = DataManager.imageData[ImageIndex.map_battle];
            break;
            case RoomType.randomEvent:
                roomTypeSprite = DataManager.imageData[ImageIndex.map_randomEvent];
            break;
            default:
                roomTypeSprite = null;
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(isVisited == false)
            {
                isVisited = true; //무조건 ShowRoom보다 먼저
                //자기 방 타입에 맞는 이벤트 진입
                GameSystem.Instance.EnterNewRoom(roomType);
            }

            ShowRoom();
            ShowArrow();
            ShowNearbyRoom();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideArrow();
        }
    }
}
