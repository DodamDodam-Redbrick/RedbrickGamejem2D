using System.Collections;
using System.Collections.Generic;
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

    List<UnitInfo> unitList = new List<UnitInfo>();

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

    public void AddUnit(UnitInfo unit)
    {
#if UNITY_EDITOR
        Debug.Log($"{unit.unitType.ToString()} is added to unitList");
#endif
        unitList.Add(unit);
    }

    public void RemoveUnit(UnitInfo unit)
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
