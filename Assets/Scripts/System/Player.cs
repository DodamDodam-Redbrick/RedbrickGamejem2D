using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerCharacter
{
    firstCharacter,
    secondCharacter,
}

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField]
    Animator animator;

    public PlayerCharacter playerCharacter = PlayerCharacter.firstCharacter;

    public int gold;

    public List<Unit> UnitList { get { return unitList; } }

    List<Unit> unitList = new List<Unit>();

    Coroutine coMoveRoom;
    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;
    }

    private void Start()
    {

    }

    public void ChangeGold(int value)
    {
        //돈을 써도 이걸 부르기
        gold += value;

        //gold관련 ui 동기화
    }

    public void AddItem()
    {
        // 아이템 관련 (포션 등)
    }
    public void AddUnit(Unit unit)
    {
#if UNITY_EDITOR
        Debug.Log($"{unit.unitInfo.unitType.ToString()} is added to unitList");
#endif
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public void MoveRoom(Room room)
    {
        if (coMoveRoom != null)
        {
            StopCoroutine(coMoveRoom);
            coMoveRoom = null;
        }

        coMoveRoom = StartCoroutine(CoMoveRoom(room));
    }

    IEnumerator CoMoveRoom(Room room)
    {
        Vector3 firstPosition = transform.position;
        float runtime = 0f;
        float duration = GameSystem.Instance.MapMoveTime;

        //애니메이션 달리는 모습

        while (runtime <= duration)
        {
            runtime += Time.deltaTime;
            transform.position = Vector2.Lerp(firstPosition, room.transform.position, runtime / duration);
            yield return null;
        }

        coMoveRoom = null;
        //애니메이션 멈춰있는 모습
    }
}