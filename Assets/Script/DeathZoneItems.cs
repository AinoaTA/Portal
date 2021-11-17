using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Companion"))
        {
            other.gameObject.SetActive(false);
           
        }else if (other.CompareTag("Turret"))
        {
            other.GetComponent<IDeath>().Death();
        }
    }
}
