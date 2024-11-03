using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    [SerializeField]
    Image unitImage;

    [SerializeField]
    Image ban;

    [SerializeField]
    GameObject offUnitPanel;

    [SerializeField]
    TextMeshProUGUI costText;

    [HideInInspector]
    public UnitInfo unit;

    [HideInInspector]
    public bool isBan;

    [HideInInspector]
    public int cardIndex;

    void ChangeEdgeColor()
    {
        Material mat = unitImage.material;

        Color[] color = { Color.black, Color.gray, Color.yellow };

        int colorIndex = ((int)unit.unitType) % 10;

        mat.SetFloat("_Outline", 1f);

        mat.SetColor("_OutlineColor", color[colorIndex]);

        mat.SetFloat("_OutlineSize", GameSystem.Instance.outlineSize);
    }


    public void Set(UnitInfo unit, int cardIndex)
    {
        this.unit = unit;
        this.cardIndex = cardIndex;

        ChangeEdgeColor();

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

    public void ShowOffUnitPanel()
    {
        offUnitPanel.SetActive(true);
    }

    public void FinishOffUnitPanel()
    {
        offUnitPanel.SetActive(false);
    }
}
