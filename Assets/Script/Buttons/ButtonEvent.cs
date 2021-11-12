using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
    public UnityEvent m_Event;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Companion") && GameController.GetGameController().GetPlayer().CanAttach())
            m_Event.Invoke();
        
            
    }
}
