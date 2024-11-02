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
    public UnitInfo unit;

    public void Set(UnitInfo unit)
    {
        this.unit = unit;

        unitImage.sprite = unit.thumbnail;
        costText.text = $"{unit.cost}";
    }

    public void DeactiveUnitCard()
    {

    }
    //카드 비활성화
}
