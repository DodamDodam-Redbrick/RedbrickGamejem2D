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

    RewardPanel rewardPanel;

    Reward reward;

    public void Set(Reward reward, RewardPanel panel)
    {
        rewardPanel = panel;

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
            case RewardType.unit:
                Player.Instance.AddUnit(reward.unit);
                break;
        }

        //2. �θ� �Ǵ� ������ ���̾ƿ� ����
        //�ϴ� Destroy�ϴµ� ��Ȱ������ ������õ
        //transform.parent.gameObject.SetActive(false);
        rewardPanel.HidePopupPanel();

        //3. ���� ���� ����
        //go to minimap
    }
}
