using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject RewardPopupPrefab;

    [SerializeField]
    GameObject RewardLayout;

    List<GameObject> rewardPools = new List<GameObject>();

    public void ShowPopupPanel(List<Reward> rewards)
    {
        gameObject.SetActive(true);

        foreach (Reward reward in rewards)
        {
            GameObject rewardPopup = GetUnUseRewardPool();
            //ÀçÈ°¿ëÀ¸·Î ¹Ù²Ü ¿¹Á¤
            if (rewardPopup != null)
            {
                rewardPopup.SetActive(true);
            }
            else
            {
                rewardPopup = Instantiate(RewardPopupPrefab, RewardLayout.transform);
                rewardPools.Add(rewardPopup);
            }

            rewardPopup.GetComponent<RewardPopup>().Set(reward, this);
        }
    }
    
    public void HidePopupPanel()
    {
        foreach(GameObject rewardPopup in rewardPools) //±ò²ûÇÏ°Ô ²ø¶§ ÀüºÎ ²¨¹ö¸²
        {
            rewardPopup.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    GameObject GetUnUseRewardPool()
    {
        foreach (GameObject reward in rewardPools)
        {
            if(reward.activeInHierarchy == false)
            {
                return reward;
            }
        }

        return null;
    }
}
