using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.MaterialProperty;

public class ShopPopUp : MonoBehaviour
{
    [SerializeField]
    Image rewardImage;

    [SerializeField]
    TextMeshProUGUI rewardDescription;

    [SerializeField]
    TextMeshProUGUI priceText;

    Reward reward;

    public void Set(Reward reward)
    {
        this.reward = reward;
        rewardImage.sprite = reward.thumbnail;
        rewardDescription.text = reward.description;
        priceText.text = $"{reward.shopPrice} G";
    }

    public void OnClickBuy()
    {
        Debug.Log($"{reward.shopPrice}");
        if(reward.shopPrice > Player.Instance.gold)
        {
            Debug.Log("Dont Have Money");
            return;
        }
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

    public void RerollPopUp()
    {
        bool isUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => reward.rewardType == (RewardType)unit);
        List <RewardType> rewardTypes = GameSystem.Instance.shopList;

        if (rewardTypes.Count == 0)
        {
            Debug.LogWarning("No reward types available for reroll.");
            return; // ���� Ÿ���� ������ �Լ��� �����մϴ�.
        }

        RewardType newRewardType;
        bool isNewUnitType;

        do
        {
            // rewards���� �������� ���� Ÿ�� ����
            newRewardType = rewardTypes[Random.Range(0, rewardTypes.Count)];

            // ���ο� ������ ���� Ÿ������ Ȯ��
            isNewUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => newRewardType == (RewardType)unit);

            // ���ǿ� ���� �ߺ� Ȯ��
        }
        while (newRewardType == reward.rewardType ||
            (isUnitType && !isNewUnitType) || // ���� ������ ���� Ÿ���̰� �� ���� ���� Ÿ���� ���
            (!isUnitType && isNewUnitType)); // ���� ������ ���� Ÿ���� �ƴϰ� �� ���� ���� Ÿ���� �ƴ� ���

        Reward rerollReward = new Reward(
            DataManager.rewardData[newRewardType].thumbnail, DataManager.rewardData[newRewardType].description, newRewardType, DataManager.rewardData[newRewardType].shopPrice);

        Set(rerollReward);
    }
}
