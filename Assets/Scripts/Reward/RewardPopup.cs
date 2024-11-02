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
        Debug.Log($"{reward.rewardType}");

        //1. �����Ϳ� �ڱ� ������ �߰������ְ�
        switch (reward.rewardType)
        {   
            case RewardType.reward_gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit_sword_1:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_sword_2:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_sword_3:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_wizard_1:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_wizard_2:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_wizard_3:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_archer_1:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_archer_2:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_archer_3:
                Player.Instance.AddUnit(reward.unit);
                break;

            case RewardType.unit_soldier_1:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_soldier_2:
                Player.Instance.AddUnit(reward.unit);
                break;
            case RewardType.unit_soldier_3:
                Player.Instance.AddUnit(reward.unit);
                break;

        }

        //2. �θ� �Ǵ� ������ ���̾ƿ� ����
        GameSystem.Instance.FinishGetReward(endAction);

        //3. ���� ���� ����
        //go to minimap
    }
}
