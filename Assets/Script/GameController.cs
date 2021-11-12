using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //public Transform m_DestroyObjects;
    static GameController m_GameController;

    //LevelData m_LevelData;
    PlayerController m_player;

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

    public PlayerController GetPlayer()
    {
        return m_player;
    }
    public void ResetLevel()
    {
        //for (int i = 0; i < m_DestroyObjects.childCount; i++)
        //{
        //    Transform l_Transform = m_DestroyObjects.GetChild(i);
        //    l_Transform.gameObject.SetActive(false);
        //}


        //m_LevelData.ResetLastCheckPoint();
        //m_LevelData.ResetDecansLevel();
        //TeleportController.GetTeleportController().ResetTeleport();
        //HudController.GetHudController().QuitPauseMenu();
        //HudController.GetHudController().DesactiveGameOver();

        //ResetStats();
        //if (Gate != null)
        //    Gate.ResetGate();
        //if (m_LevelData.m_DoorKey != null)
        //    m_LevelData.m_DoorKey.ResetKeyDoor();
    }

    //public void ResetStats()
    //{
    //    m_player.ResetPlayerPos();
    //    m_player.GetComponent<Shoot>().ResetsShootStates();
    //    m_player.GetComponent<HealthSystemPlayer>().ResetStates();
    //}
  


    //public void SetLevelata(LevelData _levelData)
    //{
    //    m_LevelData = _levelData;
    //}
    //public LevelData GetLevelData()
    //{
    //    return m_LevelData;
    //}

    

    //public void CheckPointPlayerStats(float cLife, float cShield, int cBullet, int cBulletHold)
    //{
    //    m_player.GetComponent<HealthSystemPlayer>().m_ShieldLifeTime = cShield;
    //    m_player.GetComponent<HealthSystemPlayer>().currentLife = cLife;
    //    m_player.GetComponent<Shoot>().UpdateTextUI(cBullet, cBulletHold, m_player.GetComponent<Shoot>().CurrentValues.BulletForCharger);
    //    m_player.GetComponent<Shoot>().CurrentValues.CurrentBulletHold = cBulletHold;
    //    m_player.GetComponent<Shoot>().CurrentValues.CurrentBullets = cBullet;
    //}
}
