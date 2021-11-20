using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //public Vector3 m_InitialPosPlayer;
    static GameController m_GameController;

    //LevelData m_LevelData;
    PlayerController m_player;
    LevelData m_LevelData;
    HudController m_Hud;
    private void Awake()
    {
        if (m_GameController == null)
        {
            m_GameController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    static public GameController GetGameController()
    {
        return m_GameController;
    }
    public void SetPlayer(PlayerController fpsPlayer)
    {
        m_player = fpsPlayer;
    }
    public void SetLevelData(LevelData levelData)
    {
        m_LevelData = levelData;
    }

    public void SetHudController(HudController hud)
    {
        m_Hud = hud;
    }
    public PlayerController GetPlayer()
    {
        return m_player;
    }

    public LevelData GetLevelData() => m_LevelData;
    public HudController GetHudController() => m_Hud;
    public void ResetLevel()
    {
        m_LevelData.ResetLastCheckPoint();
        TeleportController.GetTeleportController().ResetTeleport();
        m_LevelData.ResetCompanionsPos();
        m_LevelData.ResetDoorOpened();

        GetGameController().GetHudController().QuitPauseMenu();
        GetGameController().GetHudController().DesactiveGameOver();
        GetGameController().GetHudController().DesactiveWinnerLevel();

        GetGameController().GetPlayer().BluePortal.ResetPortal();
        GetGameController().GetPlayer().OrangePortal.ResetPortal();

        GetGameController().GetPlayer().Detach(0);
        ResetStats();
    }

    public void ResetStats()
    {
        m_player.ResetPlayer();
    }

}
