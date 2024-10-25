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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(RewardIndex index)
    {
        rewardImage.sprite = Resources.Load<Sprite>(Data.RewardData[index].imagePath);
        rewardDescription.text = Data.RewardData[index].description;
    }
}
