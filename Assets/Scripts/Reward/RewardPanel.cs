using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject RewardPopupPrefab;

    [SerializeField]
    GameObject RewardLayout;

    List<RewardPopup> rewardPools = new List<RewardPopup>();

    public void ShowPopupPanel(List<Reward> rewards, UnityAction endAction = null)
    {
        Debug.Log("����2");
        gameObject.SetActive(true);

        foreach (Reward reward in rewards)
        {
            RewardPopup rewardPopup = GetUnUseRewardPool();
            //��Ȱ������ �ٲ� ����
            if (rewardPopup != null)
            {
                rewardPopup.gameObject.SetActive(true);
            }
            else
            {
                rewardPopup = Instantiate(RewardPopupPrefab, RewardLayout.transform).GetComponent<RewardPopup>();
                rewardPools.Add(rewardPopup);
            }

            rewardPopup.Set(reward, endAction);
        }
    }
    
    public void HidePopupPanel()
    {
        foreach(RewardPopup rewardPopup in rewardPools) //����ϰ� ���� ���� ������
        {
            rewardPopup.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    RewardPopup GetUnUseRewardPool()
    {
        foreach (RewardPopup reward in rewardPools)
        {
            if(reward.gameObject.activeInHierarchy == false)
            {
                return reward;
            }
        }

        return null;
    }
}
