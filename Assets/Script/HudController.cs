using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public GameObject m_GameOver;
    public GameObject Pause;
    public GameObject Winner;

    public AudioSource Buttons;
    private void Start()
    {
        GameController.GetGameController().SetHudController(this);
        LockCursor();
    }


    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnLockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void GameOverAction()
    {
        UnLockCursor();
        m_GameOver.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinnerAction()
    {
        StartCoroutine(WinnerActionDelay());
    }
    public IEnumerator WinnerActionDelay()
    {
        yield return new WaitForSeconds(3f);
        UnLockCursor();
        Winner.SetActive(true);
        Time.timeScale = 0;
    }

    public void DesactiveGameOver()
    {
        LockCursor();
        m_GameOver.SetActive(false);
        Winner.SetActive(false);
        Time.timeScale = 1;

    }

    public void DesactiveWinnerLevel()
    {
        LockCursor();
        Winner.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause.SetActive(true);
            UnLockCursor();
            Time.timeScale = 0f;
        }

    }

    public void QuitPauseMenu()
    {
        LockCursor();
        Pause.SetActive(false);
        Winner.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
