using JetBrains.Annotations;
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

    public List<UnitInfo> UnitList { get { return unitList; } }

    public List<UnitInfo> unitList = new List<UnitInfo>();

    Coroutine coMoveRoom;
    private void Awake()
    {

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
    public void AddUnit(UnitInfo unit)
    {
        Debug.Log($"{unit.unitType.ToString()} is added to unitList");
        unitList.Add(unit);

        UpgradeUnit();
    }

    public void RemoveUnit(UnitInfo unit)
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

        GameSystem.Instance.IncreaseCold();
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

    public void UpgradeUnit()
    {
        

        // 인덱스를 감소시키면서 역순으로 진행
        for (int i = unitList.Count - 1; i > 0; i--)
        {
            UnitInfo unitA = unitList[i];
            UnitInfo unitB = unitList[i - 1]; // unitB는 unitA의 이전 유닛
            if (unitA.unitType == unitB.unitType) // 타입이 같고 업그레이드할 수 있는 경우 진행
            {
                UnitType upgradeUnitType = (UnitType)((int)unitA.unitType + 1);

                if (DataManager.Instance.unitData.ContainsKey(upgradeUnitType))
                {
                    // 새로운 유닛 추가
           
                    // 기존 유닛 제거
                    RemoveUnit(unitA);
                    RemoveUnit(unitB);

                    Debug.Log("Upgrade Complete");
                    AddUnit(DataManager.Instance.unitData[upgradeUnitType]);
                    break;
                    

                }
            }
        }

        
    }

}