using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    [SerializeField]
    Image rewardImage;

    [SerializeField]
    TextMeshProUGUI rewardDescription;

    Reward reward;

    UnityAction endAction;

    public void Set(Reward reward, UnityAction endAction = null)
    {
        this.reward = reward;
        this.endAction = endAction;

        rewardImage.sprite = reward.thumbnail;
        rewardDescription.text = reward.description;
    }

    public void OnClickCard()
    {
        //1. 데이터에 자기 리워드 추가시켜주고
        switch (reward.rewardType)
        {   
            case RewardType.reward_gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit_sword:
                UnitInfo unit = reward.unit;
                Player.Instance.AddUnit(unit);
                break;
        }

        //2. 부모가 되는 리워드 레이아웃 숨김
        GameSystem.Instance.FinishGetReward(endAction);

        //3. 다음 스텝 진행
        //go to minimap
    }
}
