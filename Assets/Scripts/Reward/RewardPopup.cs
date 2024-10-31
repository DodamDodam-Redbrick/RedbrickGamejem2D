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
        //1. 데이터에 자기 리워드 추가시켜주고
        switch (reward.rewardType)
        {
            case RewardType.gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit:
                Player.Instance.AddUnit(reward.unit);
                break;
        }

        //2. 부모가 되는 리워드 레이아웃 삭제
        //일단 Destroy하는데 재활용으로 변경추천
        //transform.parent.gameObject.SetActive(false);
        rewardPanel.HidePopupPanel();

        //3. 다음 스텝 진행
        //go to minimap
    }
}
