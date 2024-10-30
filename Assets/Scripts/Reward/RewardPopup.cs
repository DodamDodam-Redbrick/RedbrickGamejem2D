using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    Image rewardImage;

    TextMeshProUGUI rewardDescription;

    Reward reward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Reward reward)
    {
        this.reward = reward;

        rewardImage.sprite = Resources.Load<Sprite>(DataManager.RewardData[reward.rewardType].imagePath);
        rewardDescription.text = DataManager.RewardData[reward.rewardType].description;
    }

    public void OnClickCard()
    {
        switch (reward.rewardType)
        {
            case RewardType.gold:
                Player.Instance.ChangeGold(reward.gold);
                break;
            case RewardType.unit:
                Player.Instance.AddUnit(reward.unit);
                break;
        }

        //1. 데이터에 자기 리워드 추가시켜주고
        //2. 부모가 되는 리워드 레이아웃 삭제
        //3. 다음 스텝 진행
    }
}
