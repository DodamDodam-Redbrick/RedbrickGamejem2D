using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerLayout : MonoBehaviour
{
    enum Dir
    {
        left,
        right,
        top,
        bottom,
    }

    enum Axis
    {
        horizontal,
        vertical,
    }

    [SerializeField]
    float addCoinTime = 1f;
    float t = 0;
    [SerializeField]
    int addCoinAmount = 1;

    [SerializeField]
    public int battleCoin;

    [SerializeField]
    float placeDistance = 1f;

    [SerializeField]
    GameObject unitGroup;

    [SerializeField]
    GameObject unitCardPrefab;

    [SerializeField]
    Image playerPortrait;

    [SerializeField]
    TextMeshProUGUI nowCost;

    [SerializeField]
    Image costFillImage;

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

    bool isButtonDown = false;

    float[] dirRotation = { 180, 0, 90, 270 }; //left, right, top, bottom

    int step = 0;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        costFillImage.fillAmount = t / addCoinTime;
        if (addCoinTime <= t)
        {
            t = 0f;
            battleCoin += addCoinAmount;
            
            SetNowCost();
        }
        if (gameObject.activeInHierarchy)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                MapGrid mapGrid = GameSystem.Instance.battleMap.mapGrid;
                Node nodePosition = mapGrid.GetNodeFromVector(mousePosition);

                Debug.Log($"x : {nodePosition.myX}, y: {nodePosition.myY}");
            }
#endif


            if (step == 0) //드래그해서 놓을 때 까지
            {
                if (selectedUnit != null)
                {
                    Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    MapGrid mapGrid = GameSystem.Instance.battleMap.mapGrid;
                    mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

                    if (mapGrid.IsNotOutOfBind(mousePosition))
                    {
                        if (mapGrid.GetNodeFromVector(mousePosition).type == selectedUnit.unitInfo.placeNodeType)
                        {
                            selectedUnit.transform.parent.position = mapGrid.GetNodeFromVector(mousePosition).myPos;
                        }
                        else
                        {
                            selectedUnit.transform.parent.position = mousePosition;
                        }
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
                            if (unitCard.isBan)
                            {
                                return;
                            }

                            if (unitCard.unit.cost > battleCoin)
                            {
                                Debug.Log("돈이부족해소환할수없습니다!");
                                return;
                            }

                            selectedUnitCard = unitCard;
                            selectedUnit = Instantiate(selectedUnitCard.unit.entityPrefab, GameSystem.Instance.battleMap.transform).GetComponentInChildren<Unit>();
                            selectedUnit.Init(selectedUnitCard.unit, selectedUnitCard.cardIndex);
                            selectedUnit.isSpawning = true;
                        }
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                    MapGrid mapGrid = GameSystem.Instance.battleMap.mapGrid;

                    if (selectedUnit != null)
                    {
                        Node node = mapGrid.GetNodeFromVector(mousePosition);
                        if (node.type == selectedUnit.unitInfo.placeNodeType 
                            && !node.isUse)
                        {
                            selectedUnit.transform.parent.position = node.myPos;

                            step = 1 - step;
                        }
                        else
                        {
                            Destroy(selectedUnit.transform.parent.gameObject);
                            selectedUnit = null;
                            selectedUnitCard = null;
                        }

                    }
                }
            }
            else
            {
                if (isButtonDown)
                {
                    Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                    Vector3 vector = mousePosition - buttonDownMousePosition; //x,y 중 절대값 더 큰 값 고르고 0이상 이하로 판단

                    Axis axis = Mathf.Abs(vector.x) > Mathf.Abs(vector.y) ? Axis.horizontal : Axis.vertical;

                    Dir dir = new Dir();

                    if (axis == Axis.horizontal)
                    {
                        dir = vector.x > 0 ? Dir.right : Dir.left;
                    }
                    else
                    {
                        dir = vector.y > 0 ? Dir.top : Dir.bottom;
                    }

                    selectedUnit.transform.parent.GetComponent<SpriteRenderer>().flipX = vector.x < 0;

                    selectedUnit.transform.rotation = Quaternion.Euler(0f, 0f, dirRotation[(int)dir]);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                    selectedUnit.ShowRange();

                    buttonDownMousePosition = mousePosition;
                    isButtonDown = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

                    float distance = Vector3.Distance(mousePosition, buttonDownMousePosition);

                    selectedUnit.HideRange();

                    //distance로 취소 체크
                    if(distance > placeDistance)
                    {
                        MapGrid mapGrid = GameSystem.Instance.battleMap.mapGrid;
                        mapGrid.GetNodeFromVector(selectedUnit.transform.position).isUse = true;

                        battleCoin -= selectedUnit.unitInfo.cost;
                        SetNowCost();

                        GameSystem.Instance.placedUnit.Add(selectedUnit);

                        selectedUnitCard.DeactiveUnitCard();
                        selectedUnit.isSpawning = false;
                    }
                    else
                    {
                        Destroy(selectedUnit.transform.parent.gameObject);
                    }

                    selectedUnit = null;
                    selectedUnitCard = null;
                    isButtonDown = false;
                    step = 1 - step;
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
        battleCoin = 0;
        GameSystem.Instance.placedUnit.Clear();
        SetNowCost();
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
                unitCard.ActiveUnitCard();
                unitCards.Add(unitCard);
            }

            unitCard.Set(player.UnitList[i], i);
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

    public void SetNowCost()
    {
        nowCost.text = $"{battleCoin}";
    }
}
