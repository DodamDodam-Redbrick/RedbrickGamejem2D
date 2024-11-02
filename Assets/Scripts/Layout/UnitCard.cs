using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    Image unitImage;

    [SerializeField]
    TextMeshProUGUI costText;

    [HideInInspector]
    public Unit unit;

    public void Set(Unit unit)
    {
        this.unit = unit;

        unitImage.sprite = unit.unitInfo.thumbnail;
        costText.text = $"{unit.unitInfo.cost}";
    }

    public void DeactiveUnitCard()
    {

    }
    //카드 비활성화
}
