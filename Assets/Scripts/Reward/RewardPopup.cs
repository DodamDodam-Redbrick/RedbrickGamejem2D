using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
    Image rewardImage;

    TextMeshProUGUI rewardDescription;

    RewardType rewardType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(RewardType index)
    {
        rewardImage.sprite = Resources.Load<Sprite>(DataManager.RewardData[index].imagePath);
        rewardDescription.text = DataManager.RewardData[index].description;
    }
}
