using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class EventPanel : MonoBehaviour
{
    [SerializeField] GameObject eventPopUpPrefab;
    [SerializeField] Transform eventLayOut;

    [SerializeField] TextMeshProUGUI mainText;

    List<EventPopUp> eventPools = new List<EventPopUp>();
    public void ShowEventPanel(Event showEvent)
    {
        gameObject.SetActive(true);
        mainText.text = showEvent.mainEvent;
        foreach (string option in showEvent.options)
        {
            EventPopUp eventPopUp = GetUnUseEventPool();
            if (eventPopUp != null)
            {
                eventPopUp.gameObject.SetActive(true);
            }
            else
            {
                eventPopUp = Instantiate(eventPopUpPrefab, eventLayOut).GetComponent<EventPopUp>();
                eventPools.Add(eventPopUp);
            }
            eventPopUp.Set(option);
        }
    }

    EventPopUp GetUnUseEventPool()
    {
        foreach (EventPopUp eventPopUp in eventPools)
        {
            if (eventPopUp.gameObject.activeInHierarchy == false)
            {
                return eventPopUp;
            }
        }
        return null;
    }

    public void HidePopUpPanel()
    {
        foreach(EventPopUp eventPopUp in eventPools)
        {
            eventPopUp.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);

    }
    
}   
