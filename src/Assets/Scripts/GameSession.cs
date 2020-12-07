using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSession : MonoBehaviour
{
    [SerializeField]
	    int playerCurLifes = 3;
	    [SerializeField]
    GameObject msgCanvasObj;
    private void Awake()
    {
        int gameSessionCount = FindObjectsOfType<GameSession>().Length;
        if (gameSessionCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            msgCanvasObj.SetActive(false);
        }
    }
    public void PlayerDeath()
    {
        if (playerCurLifes > 0)
        {
            playerCurLifes--;
            LevelRestart();
        }
        else
        {
            TriggerDeathScreen();
        }
    }

    private void LevelRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void TriggerDeathScreen()
    {
        msgCanvasObj.SetActive(true);
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene("Lv 1");
        Destroy(gameObject);
    }
}
