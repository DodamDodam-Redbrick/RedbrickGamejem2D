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
    Image ban;

    [SerializeField]
    TextMeshProUGUI costText;

    [HideInInspector]
    public UnitInfo unit;

    [HideInInspector]
    public bool isBan;

    public void Set(UnitInfo unit)
    {
        this.unit = unit;

        unitImage.sprite = unit.thumbnail;
        costText.text = $"{unit.cost}";
    }

    public void ActiveUnitCard()
    {
        ban.gameObject.SetActive(false);
        isBan = false;
    }

    public void DeactiveUnitCard()
    {
        ban.gameObject.SetActive(true);
        isBan = true;
    }
    //카드 비활성화
}
