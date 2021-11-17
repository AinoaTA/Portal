using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public List<Companion> m_AllCompanions;
    public List<Companion> m_CompanionsAttached;
    public List<Companion> m_CompanionsSpawned;
    public List<Turret> m_AllTurrets;
    public List<Turret> m_TurretDestroyed;
    public List<RefractionCube> m_AllRefractions;

    public List<Animator> DoorOpens;
    public void AddDoor(Animator Door)
    {
        DoorOpens.Add(Door);
    }

    private void Awake()
    {

        GameController.GetGameController().SetLevelData(this);
    }

    public void ResetDoorOpened()
    {
        for (int i = 0; i < DoorOpens.Count; i++)
        {
            DoorOpens[i].SetTrigger("Close");
        }
    }
    public void ResetLastCheckPoint()
    {
        for (int i = 0; i < m_AllCompanions.Count; i++)
            m_AllCompanions[i].gameObject.SetActive(true);

        for (int i = 0; i < m_AllTurrets.Count; i++)
        {
            m_AllTurrets[i].gameObject.SetActive(true);
            m_AllTurrets[i].ResetTurret();
        }


        ResetTeleportObjects();
    }
    public void ResetTeleportObjects()
    {
        if (m_TurretDestroyed.Count != 0)
            TeleportResetEnemies();
        if (m_CompanionsSpawned.Count != 0)
            CompanionsSpawnedReset();
        if (DoorOpens.Count != 0)
            ResetDoorOpened();

        ResetCompanionsPos();
        ResetRefractions();
        print("Coño");
        GameController.GetGameController().GetPlayer().BluePortal.ResetPortal();
        GameController.GetGameController().GetPlayer().OrangePortal.ResetPortal();

    }



    //Level reset

    public void ResetRefractions()
    {
        for (int i = 0; i < m_AllRefractions.Count; i++)
        {

            m_AllRefractions[i].ResetRefractions();
        }

    }
    public void ResetCompanionsPos()
    {
        for (int i = 0; i < m_AllCompanions.Count; i++)
        {
            m_AllCompanions[i].gameObject.SetActive(true);
            m_AllCompanions[i].ResetCompanion();
        }
    }
    private void CompanionsSpawnedReset()
    {
        for (int i = 0; i < m_CompanionsSpawned.Count; i++)
            m_CompanionsSpawned[i].gameObject.SetActive(false);

        m_CompanionsSpawned.Clear();
    }

    private void TeleportResetEnemies()
    {
        for (int i = 0; i < m_TurretDestroyed.Count; i++)
        {
            m_TurretDestroyed[i].gameObject.SetActive(true);
            m_TurretDestroyed[i].ResetTurret();
        }
           

        m_TurretDestroyed.Clear();
    }
}
