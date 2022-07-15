using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    private GameManager GM;

    private CanvasGroup pauseCanvas;
    private Button backButton;
    private CanvasGroup victory;
    private CanvasGroup defeat;
    private Button turnOver;

    private void Start()
    {
        pauseCanvas = GameObject.Find("PauseCanvas").GetComponent<CanvasGroup>();
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        victory = GameObject.Find("Victory").GetComponent<CanvasGroup>();
        defeat = GameObject.Find("Defeat").GetComponent<CanvasGroup>();

        GM = GameObject.Find("EventSystem").GetComponent<GameManager>();
        turnOver = GameObject.Find("TurnOverB").GetComponent<Button>();
    }

    public void TurnOver()
    {
        GM.EnemyTurn();
        GameManager.useCount = 1;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseCanvas.alpha = 1;
        pauseCanvas.interactable = true;
    }

    public void Back()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        pauseCanvas.alpha = 0;
        pauseCanvas.interactable = false;
    }

    public void GoHome()
    {
        // 게임 시간 다시 진행
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        // static 변수 초기화
        CardBase.pcnt = 0;
        CardBase.ecnt = 0;
        GameManager.nowTurn = 1;
        GameManager.ph = 100;
        GameManager.eh = 100;
        GameManager.useCount = 1;
        GameManager.pMinion = null;
        GameManager.eMinion = null;
        GameManager.ecard.Clear();
        Weighting2.bestcard = 0;
        // 홈화면 로드
        SceneManager.LoadScene(0);
    }

    public void EndGame(bool IsWon)
    {
        Time.timeScale = 0f;
        pauseCanvas.alpha = 1;
        pauseCanvas.interactable = true;
        backButton.interactable = false;
        
        if(IsWon == true)
        {
            victory.alpha = 1;
        }
        else if(IsWon == false)
        {
            defeat.alpha = 1;
        }
    }
    public void CloseTooltip()
    {
        transform.parent.parent.gameObject.SetActive(false);
    }
}
