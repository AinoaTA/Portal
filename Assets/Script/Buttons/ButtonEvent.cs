using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
    public UnityEvent m_Event;
    public UnityEvent m_ExitEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Companion") && GameController.GetGameController().GetPlayer().m_ThrewCompanion)
        {
            print("a");
            m_Event.Invoke();

        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        m_ExitEvent?.Invoke();
    }

}
