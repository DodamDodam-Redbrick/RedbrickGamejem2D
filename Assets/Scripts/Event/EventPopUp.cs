using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPopUp : MonoBehaviour
{
    [SerializeField] Button optionOneButton;
    [SerializeField] Button optionTwoButton;

    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI optionOneText;
    [SerializeField] TextMeshProUGUI optionTwoText;

    Event currentEvent;
    public void Set(Event Event)
    {
        currentEvent = Event;

        mainText.text = currentEvent.mainEvent;
        optionOneText.text = currentEvent.optionOne;
        optionTwoText.text = currentEvent.optionTwo;
    }

    public void OnClickOptionOne()
    {

    }

    public void OnClickOptionTwo()
    {

    }
}
