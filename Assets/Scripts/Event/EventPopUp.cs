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


    public void Set(string option)
    {
        optionText.text = option;

        optionButton.onClick.AddListener(() => OnClickOption());
    }

    public void OnClickOption()
    {
        //switch (optionType)
        //{
        //    case EventOptionType.gold:
        //        //Player.Instance.ChangeGold(currentEvent.options);
        //        break;
        //}
        GameSystem.Instance.FinishGetEvent();
    }

}
