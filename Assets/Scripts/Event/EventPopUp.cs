using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUp : MonoBehaviour
{
    [SerializeField] Button optionButton;
    [SerializeField] TextMeshProUGUI optionText;
    List<Reward> rewards = new List<Reward>();

    public void Set(string option, Reward reward)
    {
        rewards.Add(reward);

        optionText.text = option;

        optionButton.onClick.AddListener(() => OnClickOption());
    }

    public void OnClickOption()
    {
        GameSystem.Instance.rewardPanel.ShowPopupPanel(rewards, GameSystem.Instance.FinishGetEvent);

    }

}
