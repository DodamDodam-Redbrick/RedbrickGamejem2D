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
    Reward reward;

    public void Set(string option, Reward reward)
    {
        this.reward = reward;

        optionText.text = option;

        optionButton.onClick.AddListener(() => OnClickOption());
    }

    public void OnClickOption()
    {
        reward = GameSystem.Instance.CopyUnitType(reward);
        List<Reward> rewards = new List<Reward>();
        rewards.Add(reward);
        GameSystem.Instance.rewardPanel.ShowPopupPanel(rewards, GameSystem.Instance.FinishGetEvent);

    }

}
