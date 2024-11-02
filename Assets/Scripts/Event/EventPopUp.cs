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

    EventOptionType optionType;
    Event currentEvent;
    public void Set(string option, EventOptionType optionType, Event currentEvent)
    {
        optionText.text = option;
        this.optionType = optionType;
        this.currentEvent = currentEvent;
        optionButton.onClick.AddListener(() => OnClickOption(this.optionType));
    }

    public void OnClickOption(EventOptionType optionType)
    {
        switch (optionType)
        {
            case EventOptionType.gold:
                //Player.Instance.ChangeGold(currentEvent.options);
                break;
        }
        GameSystem.Instance.FinishGetEvent();
      

    }

}
