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

    int gold;

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
        //���� �ᵵ �̰� �θ���
        gold += value;

        //gold���� ui ����ȭ
    }

<<<<<<< HEAD:Assets/Scripts/System/Player.cs
    public void AddUnit(Unit unit)
=======
    public void AddUnit(UnitInfo unit)
>>>>>>> parent of 177f88b (Shop 구현 완료):Assets/Scripts/Player/Player.cs
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
        if(coMoveRoom != null)
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

        //�ִϸ��̼� �޸��� ���

        while (runtime <= duration)
        {
            runtime += Time.deltaTime;
            transform.position = Vector2.Lerp(firstPosition, room.transform.position, runtime/duration);
            yield return null;
        }

        coMoveRoom = null;
        //�ִϸ��̼� �����ִ� ���
    }
}
