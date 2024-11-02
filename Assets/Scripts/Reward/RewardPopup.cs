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
        //1. �����Ϳ� �ڱ� ������ �߰������ְ�
        switch (reward.rewardType)
        {   
            case RewardType.reward_gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit_sword_1:
                UnitInfo unit = reward.unit;
                Player.Instance.AddUnit(unit);
                break;
        }

        //2. �θ� �Ǵ� ������ ���̾ƿ� ����
        GameSystem.Instance.FinishGetReward(endAction);

        //3. ���� ���� ����
        //go to minimap
    }
}
