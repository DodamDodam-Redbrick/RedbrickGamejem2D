using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPanel : MonoBehaviour
{
    [SerializeField] GameObject eventPopUpPrefab;
    [SerializeField] Transform eventLayOut;
    public void ShowEventPanel(Event showEvent)
    {
        gameObject.SetActive(true);

        foreach (Transform child in eventLayOut)
        {
            Destroy(child.gameObject);
        }

        GameObject eventPopUpObj = Instantiate(eventPopUpPrefab, eventLayOut);
        EventPopUp eventPopUp = eventPopUpObj.GetComponent<EventPopUp>();
        eventPopUp.Set(showEvent);
    }
}   
