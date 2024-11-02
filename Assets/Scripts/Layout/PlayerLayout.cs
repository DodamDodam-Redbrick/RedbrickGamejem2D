using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField]
    GraphicRaycaster canvasRaycaster;

    PointerEventData pointerEventData;

    EventSystem eventSystem;

    [SerializeField, Tooltip("마우스 기준 유닛이 어떻게 표시 될 지 오프셋")]
    Vector3 offset = new Vector3(0f, 10f, 0f);

    Player player;

    List<UnitCard> unitCards = new List<UnitCard>();

    Unit selectedUnit;

    UnitCard selectedUnitCard;

    Camera mainCam;

    Vector3 buttonDownMousePosition;

    int step = 0;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            if (selectedUnit != null)
            {
                Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                MapGrid mapGrid = GameSystem.Instance.battleMap.mapGrid;
                mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

                if (mapGrid.GetNodeFromVector(mousePosition).type == selectedUnit.unitInfo.placeNodeType)
                {
                    selectedUnit.transform.position = mapGrid.GetNodeFromVector(mousePosition).myPos;
                }
                else
                {
                    selectedUnit.transform.position = mousePosition;
                }

            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                canvasRaycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult hit in results)
                {
                    UnitCard unitCard = hit.gameObject.GetComponent<UnitCard>();

                    if (unitCard != null)
                    {
                        selectedUnitCard = unitCard;
                        selectedUnit = Instantiate(selectedUnitCard.unit.entityPrefab, GameSystem.Instance.battleMap.transform).GetComponent<Unit>();
                        selectedUnit.Init(selectedUnitCard.unit);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                MapGrid mapGrid = GameSystem.Instance.battleMap.mapGrid;

                if (selectedUnit != null)
                {
                    if (mapGrid.GetNodeFromVector(mousePosition).type == selectedUnit.unitInfo.placeNodeType)
                    {
                        selectedUnit.transform.position = mapGrid.GetNodeFromVector(mousePosition).myPos;
                        selectedUnitCard.DeactiveUnitCard();
                    }
                    else
                    {
                        Destroy(selectedUnit.gameObject);
                    }

                    selectedUnit = null;
                    selectedUnitCard = null;
                }
            }
        }
    }
    
    public void Init()
    {
        if (player == null)
            player = Player.Instance;

        if (mainCam == null)
        {
            mainCam = GameSystem.Instance.battleMap.cam;
        }

        eventSystem = GetComponent<EventSystem>();

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
