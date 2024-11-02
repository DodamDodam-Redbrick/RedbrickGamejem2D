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
<<<<<<< HEAD
        switch (optionType)
        {
            case EventOptionType.gold:
                //Player.Instance.ChangeGold(currentEvent.options);
                break;
        }
=======
>>>>>>> parent of 84b5558 (Merge branch 'main' into BattleSetting)
        GameSystem.Instance.FinishGetEvent();
    }

}
