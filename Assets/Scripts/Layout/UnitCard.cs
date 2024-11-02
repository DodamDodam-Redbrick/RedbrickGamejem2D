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
    TextMeshProUGUI costText;

    [HideInInspector]
    public UnitInfo unit;

    [HideInInspector]
    public bool isBan;

    [HideInInspector]
    public int cardIndex;

    void ChangeEdgeColor()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        SpriteRenderer unitSpriteRender = new SpriteRenderer();

        unitSpriteRender.GetPropertyBlock(mpb);

        Color[] color = { Color.black, Color.gray, Color.yellow };

        int colorIndex = ((int)unit.unitType) % 10;

        mpb.SetFloat("_Outline", 1f);

        mpb.SetColor("_OutlineColor", color[colorIndex]);

        mpb.SetFloat("_OutlineSize", GameSystem.Instance.outlineSize);

        transform.parent.GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
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
}
