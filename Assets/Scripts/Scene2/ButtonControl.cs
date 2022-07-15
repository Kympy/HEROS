using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour // 캐릭터 선택화면 버튼 컨트롤
{
    public static int player1 = 0; // 플레이어의 캐릭터 ID
    public static int player2 = 0; // 적의 캐릭터 ID
    public static bool isCustom = false; // 커스텀모드로 할것인지 아닌지

    public GameObject playerA; // Amos
    public GameObject playerB; // Bessie
    public GameObject playerC; // Colin
    
    public Button customButton; // 커스텀모드 전환 버튼

    private Text explanation; // 캐릭터 설명 텍스트
    private Text playerID; // 캐릭터 이름 표시 텍스트

    public void Start()
    {
        explanation = GameObject.Find("Explanation").GetComponent<Text>();
        playerID = GameObject.Find("PlayerID").GetComponent<Text>();
        customButton.interactable = false; // 커스텀 모드 활성화를 위해서는 내 캐릭터를 선택해야한다.
    }

    public void SelectAmos() // 1번캐릭터 Amos 선택시 호출
    {
        if (isCustom == false) // 랜덤 적 생성 모드일 경우
        {
            player1 = 1; // Amos 대입
            player2 = (int)Random.Range(1.0f, 4.0f); // 적 랜덤 대입
            customButton.interactable = true;
            playerID.text = "You : Amos\nPlayer : Random";
        }
        else // 커스텀 모드일 경우
        {
            player2 = 1;
            if (player1 == 1)
            {
                playerID.text = "You : Amos\nPlayer : Amos";
            }
            else if (player1 == 2)
            {
                playerID.text = "You : Bessie\nPlayer : Amos";
            }
            else
            {
                playerID.text = "You : Colin\nPlayer : Amos";
            }
        }
        playerA.SetActive(true); // 캐릭터 모델을 보여주는 변수
        playerB.SetActive(false);
        playerC.SetActive(false);

        explanation.text = "Amos 는 강한 전사로 방어적인 성향을 가지고 있으며 받는 데미지를 감소시키거나 체력을" +
            " 회복하는 등의 방어 카드를 보유하고 있다. 공격력이 뛰어나지 않지만 우수한 생존력으로 상대 플레이어를 지치게" +
            " 만들 수 있다.";
    }
    public void SelectBessie() // 2번째 캐릭터 Bessie 선택
    {
        if (isCustom == false)
        {
            player1 = 2;
            player2 = (int)Random.Range(1.0f, 4.0f);
            customButton.interactable = true;
            playerID.text = "You : Bessie\nPlayer : Random";
        }
        else
        {
            player2 = 2;
            if (player1 == 1)
            {
                playerID.text = "You : Amos\nPlayer : Bessie";
            }
            else if (player1 == 2)
            {
                playerID.text = "You : Bessie\nPlayer : Bessie";
            }
            else
            {
                playerID.text = "You : Colin\nPlayer : Bessie";
            }
        }
        playerA.SetActive(false);
        playerB.SetActive(true);
        playerC.SetActive(false);

        explanation.text = "Bessie 는 공격적인 성향을 가지고 있으며 플레이어를 직접 공격하거나 광역 공격을 하는 다양한 공격 카드를 " +
            "보유하고 있다. 지속적인 공격으로 빠른 승리를 할 수 있는 대신 방어에는 취약하다는 단점을 가지고 있다." +
            " 따라서 게임이 길어질수록 불리해질 수 있다.";
    }
    public void SelectColin() // 3번째 캐릭터 Colin 선택
    {
        if (isCustom == false)
        {
            player1 = 3;
            player2 = (int)Random.Range(1.0f, 4.0f);
            customButton.interactable = true;
            playerID.text = "You : Colin\nPlayer : Random";
        }
        else
        {
            player2 = 3;
            if (player1 == 1)
            {
                playerID.text = "You : Amos\nPlayer : Colin";
            }
            else if (player1 == 2)
            {
                playerID.text = "You : Bessie\nPlayer : Colin";
            }
            else
            {
                playerID.text = "You : Colin\nPlayer : Colin";
            }
        }
        playerA.SetActive(false);
        playerB.SetActive(false);
        playerC.SetActive(true);

        explanation.text = "Colin 은 밸런스형 캐릭터로 소환수를 다루는 것이 특징이다. 공격이나 방어가 우수하지는 않지만 " +
            "플레이어를 보호하는 방패병을 소환하여 게임을 안정적으로 이끌어 간다.";
    }
    public void StartGame() // 게임 시작
    {
        if (player1 != 0 && player2 != 0) // 캐릭터가 정상적으로 선택되었다면
        {
            SceneManager.LoadScene(2);
        }
        else explanation.text = "게임을 플레이 하기 위해 최소 1명의 캐릭터를 선택하세요.";
    }
    public void CustomMode() // 커스텀 선택 모드
    {
        isCustom = true;
        Debug.Log(isCustom);
    }
    public void ResetCustom() // 선택 초기화
    {
        player1 = 0;
        player2 = 0;
        isCustom = false;
        customButton.interactable = false;
        playerID.text = "You : \nPlayer : ";
    }
}
