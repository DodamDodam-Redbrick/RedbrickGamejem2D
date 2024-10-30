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

        //1. �����Ϳ� �ڱ� ������ �߰������ְ�
        //2. �θ� �Ǵ� ������ ���̾ƿ� ����
        //3. ���� ���� ����
    }
}
