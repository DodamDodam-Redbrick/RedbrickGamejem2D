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

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        //디버깅
        UnitInfo unit = (UnitInfo)DataManager.entityData[EntityType.sword];
 
        reward = new Reward(DataManager.imageData[ImageIndex.unit_sword_thumbnail], "hi", RewardType.unit, unit);

        Set(reward);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Reward reward)
    {
        this.reward = reward;

        rewardImage.sprite = reward.thumbnail;
        rewardDescription.text = reward.description;
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
