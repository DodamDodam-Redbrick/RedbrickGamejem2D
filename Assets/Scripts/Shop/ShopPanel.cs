using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject ShopPopupPrefab;

    [SerializeField]
    GameObject RewardLayout;

    [SerializeField]
    TextMeshProUGUI goldText;
    [SerializeField]
    TextMeshProUGUI rerollText;

    [SerializeField]
    private int maxRerollCount = 3; // ÃÖ´ë ¸®·Ñ È½¼ö

    private int currentRerollCount = 0; // ÇöÀç ¸®·Ñ È½¼ö

    List<ShopPopUp> shopPools = new List<ShopPopUp>();

    List<RewardType> currentRewards = new List<RewardType>();

    public void ShowShopPanel(List<Reward> rewards)
    {

        gameObject.SetActive(true);
        UpdateRerollCount();
        UpdateGold();
        foreach (Reward reward in rewards)
        {

            ShopPopUp shopPopup = GetUnUseRewardPool();
            //ÀçÈ°¿ëÀ¸·Î ¹Ù²Ü ¿¹Á¤
            if (shopPopup != null)
            {
                shopPopup.gameObject.SetActive(true);
            }
            else
            {
                shopPopup = Instantiate(ShopPopupPrefab, RewardLayout.transform).GetComponent<ShopPopUp>();
                shopPools.Add(shopPopup);
            }

            shopPopup.Set(reward);
        }
        Debug.Log($"{shopPools.Count}");
    }

    public void HidePopupPanel()
    {
        foreach (ShopPopUp rewardPopup in shopPools) //±ò²ûÇÏ°Ô ²ø¶§ ÀüºÎ ²¨¹ö¸²
        {
            rewardPopup.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    ShopPopUp GetUnUseRewardPool()
    {
        foreach (ShopPopUp reward in shopPools)
        {
            if (reward.gameObject.activeInHierarchy == false)
            {
                return reward;
            }
        }

        return null;
    }

    public void UpdateGold()
    {
        GameSystem.Instance.PlaySound(GameSystem.Instance.buyClip);
        goldText.text = $"{Player.Instance.gold}";
    }

    public void OnClickRerollButton()
    {
        if (currentRerollCount < maxRerollCount)
        {
            currentRerollCount++;
            UpdateRerollCount();
            foreach (ShopPopUp shops in shopPools)
            {
                shops.RerollPopUp();
            }
        }
        else
        {
            Debug.Log("No more rerolls available.");

        }
    }

    private void UpdateRerollCount()
    {
        rerollText.text = $"Rerolls: {maxRerollCount - currentRerollCount}";
    }

    public List<RewardType> GetCurrentRewards()
    {
        return currentRewards;
    }
}