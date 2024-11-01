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
        //1. �����Ϳ� �ڱ� ������ �߰������ְ�
        switch (reward.rewardType)
        {   
            case RewardType.gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit_sword:
                Player.Instance.AddUnit(reward.unit);
                break;
        }

        //2. �θ� �Ǵ� ������ ���̾ƿ� ����
        GameSystem.Instance.FinishGetReward();

        //3. ���� ���� ����
        //go to minimap
    }
}
