using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        priceText.text = $"{reward.shopPrice}";
    }

    public void OnClickBuy()
    {
        if (reward.shopPrice > Player.Instance.gold)
        {
            Debug.Log("Dont Have Money");
            return;
        }
        //1. 데이터에 자기 리워드 추가시켜주고
        switch (reward.rewardType)
        {
            case RewardType.reward_gold:
                break;
            case RewardType.unit_sword:
                UnitInfo unit = reward.unit;
                Player.Instance.AddUnit(unit);
                break;

            case RewardType.shop_potion_one:
                Player.Instance.AddItem();
                break;


        }
        Player.Instance.ChangeGold(-reward.shopPrice);
        GameSystem.Instance.shopPanel.UpdateGold();
        //3. 다음 스텝 진행
        //go to minimap
    }

    public void RerollPopUp()
    {
        bool isUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => reward.rewardType == (RewardType)unit);
        List<RewardType> rewardTypes = GameSystem.Instance.shopList;

        if (rewardTypes.Count == 0)
        {
            Debug.LogWarning("No reward types available for reroll.");
            return; // 보상 타입이 없으면 함수를 종료합니다.
        }

        RewardType newRewardType;
        bool isNewUnitType;

        do
        {
            // rewards에서 랜덤으로 보상 타입 선택
            newRewardType = rewardTypes[Random.Range(0, rewardTypes.Count)];

            // 새로운 보상이 유닛 타입인지 확인
            isNewUnitType = System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>().Any(unit => newRewardType == (RewardType)unit);

            // 조건에 따라 중복 확인
        }
        while (newRewardType == reward.rewardType ||
            (isUnitType && !isNewUnitType) || // 기존 보상이 유닛 타입이고 새 보상도 유닛 타입인 경우
            (!isUnitType && isNewUnitType)); // 기존 보상이 유닛 타입이 아니고 새 보상도 유닛 타입이 아닌 경우

        Reward rerollReward = new Reward(
            DataManager.Instance.rewardData[newRewardType].thumbnail, DataManager.Instance.rewardData[newRewardType].description, newRewardType, DataManager.Instance.rewardData[newRewardType].shopPrice);

        Set(rerollReward);
    }
}