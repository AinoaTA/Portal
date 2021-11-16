using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public Teleport m_Activated;
    static TeleportController m_TeleportController;
    public List<Teleport> m_Teleports;
    private void Start()
    {
        m_TeleportController = this;
    }

    static public TeleportController GetTeleportController()
    {
        return m_TeleportController;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ButtonLastCheckPoint();
        }
    }
    public Vector3 SpawnToLastTeleport()
    {
        
        //si no se ha pasado por encima de un checkpoint le llevará a la posición de inicio.
        if (m_Activated != null)
        {
            GameController.GetGameController().GetLevelData().ResetTeleportObjects();
            return m_Activated.m_ToSpawn.position;
        }
        return GameController.GetGameController().GetPlayer().m_InitialPosPlayer.position;
    }

    //del menu
    public void ButtonLastCheckPoint()
    {
        GameController.GetGameController().GetPlayer().m_CharacterController.enabled = false;
        GameController.GetGameController().GetPlayer().transform.position = SpawnToLastTeleport();
        GameController.GetGameController().GetPlayer().m_CharacterController.enabled = true;

       
        
        GameController.GetGameController().GetHudController().QuitPauseMenu();
        GameController.GetGameController().GetHudController().DesactiveGameOver();
    }

    public void ResetTeleport()
    {
        m_Activated = null;
        for (int i = 0; i < m_Teleports.Count; i++)
        {
            m_Teleports[i].ResetTeleport();
        }
    }
}
