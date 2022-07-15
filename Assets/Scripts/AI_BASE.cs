using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_BASE : GameManager
{
    public static int card_cnt = 0;

    void Start()
    {
        interact = GameObject.Find("CardCanvas").GetComponent<CanvasGroup>();
        TOB = GameObject.Find("TurnOverB").GetComponent<Button>();
    }
    void Update()
    {
        if (nowTurn == ET && useCount == 0)
        {
            interact.interactable = false;
            interact.blocksRaycasts = false;
        }
    }

    public void ExecuteAI()
    {
        Debug.Log("Start");

        GameObject.Find("EventSystem").GetComponent<Weighting2>().CardUse();
        PlayerTurn();
    }
}