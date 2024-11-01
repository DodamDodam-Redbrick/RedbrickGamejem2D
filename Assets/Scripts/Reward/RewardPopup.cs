using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
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

    public void OnClickCard()
    {
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
