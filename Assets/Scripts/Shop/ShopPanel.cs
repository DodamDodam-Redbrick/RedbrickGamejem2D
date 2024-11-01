using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] GameObject shopPopUpPrefab;

    [SerializeField] TextMeshProUGUI goldText;
    public void ShowShopPanel()
    {
        gameObject.SetActive(true);
    }

    void UpdateGoldText()
    {
        goldText.text = $"{Player.Instance.gold}";
    }
}
