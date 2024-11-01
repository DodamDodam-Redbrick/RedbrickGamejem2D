using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopUp : MonoBehaviour
{
    [SerializeField]
    Image rewardImage;

    [SerializeField]
    TextMeshProUGUI rewardDescription;

    Reward reward;

    public void Set(Reward reward)
    {
        this.reward = reward;
        rewardImage.sprite = reward.thumbnail;
        rewardDescription.text = reward.description;
    }

    public void OnClickBuy()
    {
        Debug.Log($"{reward.shopPrice}");
        if(reward.shopPrice > Player.Instance.gold)
        {
            Debug.Log("Dont Have Money");
            return;
        }
        //1. 데이터에 자기 리워드 추가시켜주고
        switch (reward.rewardType)
        {
            case RewardType.gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit_sword:
                Player.Instance.AddUnit(reward.unit);
                break;
        }

        //2. 부모가 되는 리워드 레이아웃 숨김
        GameSystem.Instance.FinishGetReward();

        //3. 다음 스텝 진행
        //go to minimap
    }
}
