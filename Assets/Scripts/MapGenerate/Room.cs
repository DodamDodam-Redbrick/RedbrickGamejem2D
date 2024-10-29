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
    // �̹� ��������Ʈ�� ���� ������ ���� �ʴ´�
    [SerializeField]
    GameObject roomPrefab;

    [SerializeField]
    SpriteRenderer roomTypeSprite;

    [SerializeField, Tooltip("left, right, top, bottom ������ �����ؾ���")]
    SpawnPoint[] spawnPoints;

    [SerializeField, Tooltip("left, right, top, bottom ������ �����ؾ���")]
    GameObject[] arrowPrefabs;

    [SerializeField]
    GameObject arrowParent;

    [SerializeField, Tooltip("���� �����Ǵ� Ȯ��, ������ ��")]
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

    Room[] connectedRoom = new Room[4];
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

            if (spawnPoints[dir].isRoom) //�̰� ������ ���� ����
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowArrow();
            ShowNearbyRoom();
            //���⿡�� ��Ʋ �����ϸ� �ɵ�
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