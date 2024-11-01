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

    List<ShopPopUp> shopPools = new List<ShopPopUp>();

    public void ShowShopPanel(List<Reward> rewards)
    {
        gameObject.SetActive(true);

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
}
