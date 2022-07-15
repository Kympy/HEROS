using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    public GameObject Amos;
    public GameObject Bessie;
    public GameObject Colin;
    public GameObject E_Amos;
    public GameObject E_Bessie;
    public GameObject E_Colin;

    public Transform spawnPointA; // 플레이어 스폰 위치
    public Transform spawnPointB; // 적 스폰 위치

    private Text name1;
    private Text name2;

    private void Start()
    {
        name1 = GameObject.Find("Player1Name").GetComponent<Text>();
        name2 = GameObject.Find("Player2Name").GetComponent<Text>();

        if (ButtonControl.player1 == 1) // 선택했던 캐릭터의 코드
        {
            Instantiate(Amos, spawnPointA); // 생성 부분
            name1.text = "Amos"; // 이름 표시
        }
        else if(ButtonControl.player1 == 2)
        {
            Instantiate(Bessie, spawnPointA);
            name1.text = "Bessie";
        }
        else
        {
            Instantiate(Colin, spawnPointA);
            name1.text = "Colin";
        }

        if(ButtonControl.player2 == 1)
        {
            Instantiate(E_Amos, spawnPointB);
            name2.text = "Amos";
        }
        else if(ButtonControl.player2 == 2)
        {
            Instantiate(E_Bessie, spawnPointB);
            name2.text = "Bessie";
        }
        else
        {
            Instantiate(E_Colin, spawnPointB);
            name2.text = "Colin";
        }
    }
}
