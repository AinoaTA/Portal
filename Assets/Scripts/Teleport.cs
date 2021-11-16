using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform m_ToSpawn;
    public bool CheckPoint;
    public Material m_On;
    public Material m_Off;
    public GameObject m_TeleportRenderer;
    public ParticleSystem m_ActiveParticles;
    public AudioSource m_AudioSource;


    //public float HealthSaved;
    //public float ShieldSaved;
    //public int CurrentBulletSaved;
    //public int CurrentBulletHold;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !CheckPoint)
        {
            CheckPoint = true;

            GameController.GetGameController().GetLevelData().m_CompanionsSpawned.Clear();
            GameController.GetGameController().GetLevelData().m_TurretDestroyed.Clear();
            GameController.GetGameController().GetLevelData().m_CompanionsAttached.Clear();

            m_ActiveParticles.gameObject.SetActive(true);
            m_ActiveParticles.Play();
            m_TeleportRenderer.GetComponent<MeshRenderer>().material = m_On;
            TeleportController.GetTeleportController().m_Activated=this;

            m_AudioSource.Play();
        }      
    }

    public void ResetTeleport()
    {
        CheckPoint = false;
        m_TeleportRenderer.GetComponent<MeshRenderer>().material = m_Off;
    }
}
