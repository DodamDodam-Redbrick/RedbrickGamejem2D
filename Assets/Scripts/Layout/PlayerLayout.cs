using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLayout : MonoBehaviour
{
    [SerializeField]
    GameObject unitGroup;

    [SerializeField]
    GameObject unitCardPrefab;

    [SerializeField]
    Image playerPortrait;

    [SerializeField]
    TextMeshProUGUI nowCost;

    [SerializeField, Tooltip("마우스 기준 유닛이 어떻게 표시 될 지 오프셋")]
    Vector3 offset = new Vector3(0f, 10f, 0f);

    Player player;

    List<UnitCard> unitCards = new List<UnitCard>();

    Unit selectedUnit;

    Camera mainCam;

    Vector3 buttonDownMousePosition;

    int step = 0;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {

            if(selectedUnit != null)
            {
                Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                selectedUnit.transform.position = mousePosition + offset;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if(hit.collider != null)
                {
                    UnitCard unitCard = hit.collider.GetComponent<UnitCard>();

                    if (unitCard != null)
                    {
                        selectedUnit = unitCard.unit;
                        unitCard.DeactiveUnitCard();
                    }

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                BattleManager battleMap = GameSystem.Instance.battleMap;

                MapGrid mapGrid = battleMap.mapGrid;

                //if(mapGrid.GetNodeFromVector(mousePosition).type) {

                selectedUnit = null;
            }
            //클릭 감지해서 설치 진행
        }
    }
    
    public void Init()
    {
        if (player == null)
            player = Player.Instance;

        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        SetUnitCards();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Init();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void SetUnitCards()
    {
        for (int i = 0; i < player.UnitList.Count; i++)
        {
            UnitCard unitCard = GetUnUseUnitCardPool();

            if (unitCard != null)
            {
                unitCard.gameObject.SetActive(true);
            }
            else
            {
                unitCard = Instantiate(unitCardPrefab, unitGroup.transform).GetComponent<UnitCard>();
                unitCards.Add(unitCard);
            }

            unitCard.Set(player.UnitList[i]);
        }
    }

    void HideUnitCards()
    {
        foreach (UnitCard unitCard in unitCards)
        {
            unitCard.gameObject.SetActive(false);
        }
    }

    UnitCard GetUnUseUnitCardPool()
    {
        foreach (UnitCard unitCard in unitCards)
        {
            if (unitCard.gameObject.activeInHierarchy == false)
            {
                return unitCard;
            }
        }

        return null;
    }

    public void SetNowCost(int value)
    {
        nowCost.text = value.ToString();
    }
}
