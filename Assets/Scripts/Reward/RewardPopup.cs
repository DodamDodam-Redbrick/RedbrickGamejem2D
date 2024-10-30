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
        //�����
        UnitInfo unit = (UnitInfo)DataManager.entityData[EntityType.sword];
 
        reward = new Reward(DataManager.imageData[ImageIndex.unit_sword_thumbnail], "hi", RewardType.unit, unit);

        Set(reward);
#endif
    }

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
            case RewardType.unit:
                Player.Instance.AddUnit(reward.unit);
                break;
        }

        //2. �θ� �Ǵ� ������ ���̾ƿ� ����
        //�ϴ� Destroy�ϴµ� ��Ȱ������ ������õ
        //transform.parent.gameObject.SetActive(false);
        Destroy(transform.parent);

        //3. ���� ���� ����
        //go to minimap
    }
}
