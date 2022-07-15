using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : CardBase
{
    public CanvasGroup interact;
    public Button TOB;

    // 체력바
    public Image PhpUI_B;
    public Image EhpUI_B;
    public Text PhpUI_T;
    public Text EhpUI_T;

    public const int PT = 1; //플레이어 턴
    public const int ET = 2; //상대 턴
    public static int nowTurn = PT; //현재 턴

    public int turnCount = 0; //턴 수를 셈, 필요없으면 미사용

    public static int ph = 100; //플레이어 체력
    public static int eh = 100; //적 체력

    public static int useCount = 1; //카드썼는지 확인
    //public int minionNum2 = 0; //클릭한 미니언 배열 번호

    public Transform[] card = null; //카드 배치, size 6으로 하고 1부터 사용
    public static List<Transform> ecard = new List<Transform>(); //카드 배치, size 6으로 하고 1부터 사용

    public static Transform[] pMinion; // 1부터 사용 ~3
    public static Transform[] eMinion; // 1부터 사용

    public Transform tmp = null; //DrawCard 등에서 필요한 경우 사용

    // 플레이어 별 카드 생성
    private Transform cardCanvas;
    private Transform eCardPoint;

    private Vector3[] cardPos = new Vector3[5];

    private float time = 0f;
    private Text gameTips;

    // 이펙트
    public GameObject buffeffect;
    public GameObject debuffeffect;
    public GameObject healeffect;
    public GameObject attackeffect;


    void Start()
    {
        PhpUI_B = GameObject.Find("HPbar").GetComponent<Image>();
        EhpUI_B = GameObject.Find("HPbar2").GetComponent<Image>();
        PhpUI_T = GameObject.Find("HPtext").GetComponent<Text>();
        EhpUI_T = GameObject.Find("HPtext2").GetComponent<Text>();

        pMinion = GameObject.Find("pMinionPoint").GetComponentsInChildren<Transform>();
        eMinion = GameObject.Find("eMinionPoint").GetComponentsInChildren<Transform>();

        gameTips = GameObject.Find("Tips").GetComponent<Text>();
        interact = GameObject.Find("CardCanvas").GetComponent<CanvasGroup>();
        TOB = GameObject.Find("TurnOverB").GetComponent<Button>();

        TOB.interactable = false;
        interact.interactable = true;
        interact.blocksRaycasts = true;

        SpawnCard();
        SpawnAICard();

        card = GameObject.Find("Cards").GetComponentsInChildren<Transform>();


        PlayerTurn();
    }

    void Update()
    {
        //ecard = GameObject.Find("eCards").GetComponentsInChildren<Transform>();
        // 체력바 관리
        HPBar();

        if (nowTurn == PT && useCount == 0)
        {
            interact.interactable = false;
            interact.blocksRaycasts = false;
            TOB.interactable = false;
            //Debug.Log("Block Success");

            time += Time.deltaTime;
            if (time > 3f)
            {
                EnemyTurn();
                time = 0f;
                useCount = 1;
            }
        }

        if (ph <= 0 || eh <= 0)
        {
            GameEnd();
        }
    }

    public void SpawnCard() // 카드 초기 생성
    {
        // 카드가 놓여질 캔버스
        cardCanvas = GameObject.Find("Cards").GetComponent<Transform>();

        // 카드의 초기위치 지정
        cardPos[0] = new Vector3(-200f, -236.5f, 0f);
        cardPos[1] = new Vector3(-100f, -236.5f, 0f);
        cardPos[2] = new Vector3(0f, -236.5f, 0f);
        cardPos[3] = new Vector3(100f, -236.5f, 0f);
        cardPos[4] = new Vector3(200f, -236.5f, 0f);

        // 임시 오브젝트 배열에 캐릭터별 카드 덱을 로드한다
        Object[] temp;

        if (ButtonControl.player1 == 1) // 현재 플레이어의 번호가 x 라면~~
        {
            temp = Resources.LoadAll("AmosCards");
        }
        else if (ButtonControl.player1 == 2)
        {
            temp = Resources.LoadAll("BessieCards");
        }
        else
        {
            temp = Resources.LoadAll("ColinCards");
        }

        // 중복 허용 카드를 소환
        int k;
        for (k = 0; k < 5; k++)
        {
            int i;
            i = Random.Range(0, temp.Length);
            GameObject card = Instantiate(temp[i] as GameObject);
            card.transform.SetParent(cardCanvas);
            card.transform.localPosition = cardPos[k];
        }

    }

   

    public void SpawnAICard()
    {
        // 임시 오브젝트 배열에 캐릭터별 카드 덱을 로드한다
        Object[] temp;
        eCardPoint = GameObject.Find("eCards").GetComponent<Transform>();

        if (ButtonControl.player2 == 1) // 현재 플레이어의 번호가 x 라면~~
        {
            temp = Resources.LoadAll("AmosCards");
        }
        else if (ButtonControl.player2 == 2)
        {
            temp = Resources.LoadAll("BessieCards");
        }
        else
        {
            temp = Resources.LoadAll("ColinCards");
        }

        // 중복 허용 카드를 소환
        int k;
        for (k = 1; k < 6; k++)
        {
            int i;
            i = Random.Range(0, temp.Length);
            GameObject card = Instantiate(temp[i] as GameObject);
            card.transform.SetParent(eCardPoint);
            ecard.Add(card.transform);
        }

    }
    public void PlayerTurn()
    {
        interact = GameObject.Find("CardCanvas").GetComponent<CanvasGroup>();
        TOB = GameObject.Find("TurnOverB").GetComponent<Button>();
        gameTips = GameObject.Find("Tips").GetComponent<Text>();

        interact.interactable = true;
        interact.blocksRaycasts = true;
        TOB.interactable = true;

        gameTips.text = "당신 차례입니다.";
        useCount = 1;
        turnCount += 1;
        nowTurn = PT;
        MinionAttack();
    }
    public void EnemyTurn()
    {
        gameTips = GameObject.Find("Tips").GetComponent<Text>();
        gameTips.text = "적의 차례입니다.";
        turnCount += 1;
        nowTurn = ET;
        MinionAttackAI();
        //GameObject.Find("EventSystem").GetComponent<TestAI>().executeai();
        //AI실행하는 
        GameObject.Find("EventSystem").GetComponent<AI_BASE>().ExecuteAI();
    }


    public void HPBar()
    {
        PhpUI_B.fillAmount = ph / 100f;
        EhpUI_B.fillAmount = eh / 100f;
        PhpUI_T.text = string.Format("{0} / 100", ph);
        EhpUI_T.text = string.Format("{0} / 100", eh);
    }


    void GameEnd()
    {
        if (ph > 0 && eh <= 0) // 게임승리
        {
            GameButton gameButton = GameObject.Find("PauseButton").GetComponent<GameButton>();
            gameButton.EndGame(true);
        }
        else if (ph <= 0 && eh > 0) // 게임패배
        {
            GameButton gameButton = GameObject.Find("PauseButton").GetComponent<GameButton>();
            gameButton.EndGame(false);
        }
        else if (ph <= 0 && eh <= 0)
        {
            //무승부
        }
    }


    void MinionAttack()
    {
        for (int i = 1; i < 4; i++)
        {
            if (pMinion[i].childCount > 0) //하수인 존재 여부와 남은 공격횟수 판단
            {
                pMinion[i].GetComponentInChildren<MinionBase>().aCnt = 1;
                for (int j = 1; j < 4; j++)
                {
                    if (GameObject.Find("EventSystem").GetComponent<Weighting2>().eMinionCount() == 0 && pMinion[i].GetComponentInChildren<MinionBase>().aCnt > 0) //적 하수인이 없다면 영웅 공격
                    {
                        eh -= pMinion[i].GetComponentInChildren<MinionBase>().attack;
                        pMinion[i].GetComponentInChildren<MinionBase>().aCnt = 0;
                        Debug.Log("영웅 피격");
                        Debug.Log("내 체력 : " + eh);
                    }
                    if (eMinion[j].childCount > 0 && pMinion[i].GetComponentInChildren<MinionBase>().aCnt > 0) //자리에 적 하수인이 존재하고 내 하수인이 공격 가능한지 판단
                    {
                        Debug.Log(Mathf.Clamp((pMinion[i].GetComponentInChildren<MinionBase>().hp - eMinion[j].GetComponentInChildren<MinionBase>().attack), 0, 100) * (pMinion[i].GetComponentInChildren<MinionBase>().attack) - Mathf.Clamp((eMinion[j].GetComponentInChildren<MinionBase>().hp - pMinion[i].GetComponentInChildren<MinionBase>().attack), 0, 100) * (eMinion[j].GetComponentInChildren<MinionBase>().attack));
                        //싸워서 이득이면 공격
                        if (Mathf.Clamp((pMinion[i].GetComponentInChildren<MinionBase>().hp - eMinion[j].GetComponentInChildren<MinionBase>().attack), 0, 100) * (pMinion[i].GetComponentInChildren<MinionBase>().attack) >= Mathf.Clamp((eMinion[j].GetComponentInChildren<MinionBase>().hp - pMinion[i].GetComponentInChildren<MinionBase>().attack), 0, 100) * (eMinion[j].GetComponentInChildren<MinionBase>().attack))
                        {
                            eMinion[j].GetComponentInChildren<MinionBase>().hp -= pMinion[i].GetComponentInChildren<MinionBase>().attack;
                            pMinion[i].GetComponentInChildren<MinionBase>().hp -= eMinion[j].GetComponentInChildren<MinionBase>().attack;
                            Animator anim = pMinion[i].GetComponentInChildren<Animator>();
                            anim.SetTrigger("IsAttack");
                            Debug.Log("아군 미니언" + eMinion[j].GetComponentInChildren<MinionBase>().hp);
                            Debug.Log("적 미니언" + pMinion[i].GetComponentInChildren<MinionBase>().hp);
                            pMinion[i].GetComponentInChildren<MinionBase>().aCnt = 0;
                            Debug.Log("싸움");
                        }
                    }
                }
            }
        }
    }

    void MinionAttackAI()
    {
        for (int i = 1; i < 4; i++)
        {
            if (eMinion[i].childCount > 0) //하수인 존재 여부와 남은 공격횟수 판단
            {
                eMinion[i].GetComponentInChildren<MinionBase>().aCnt = 1;
                for (int j = 1; j < 4; j++)
                {
                    if (GameObject.Find("EventSystem").GetComponent<Weighting2>().pMinionCount() == 0 && eMinion[i].GetComponentInChildren<MinionBase>().aCnt > 0) //적 하수인이 없다면 영웅 공격
                    {
                        ph -= eMinion[i].GetComponentInChildren<MinionBase>().attack;
                        eMinion[i].GetComponentInChildren<MinionBase>().aCnt = 0;
                        Debug.Log("영웅 피격");
                        Debug.Log("내 체력 : " + ph);
                    }
                    if (pMinion[j].childCount > 0 && eMinion[i].GetComponentInChildren<MinionBase>().aCnt > 0) //자리에 적 하수인이 존재하고 내 하수인이 공격 가능한지 판단
                    {
                        Debug.Log(Mathf.Clamp((eMinion[i].GetComponentInChildren<MinionBase>().hp - pMinion[j].GetComponentInChildren<MinionBase>().attack), 0, 100) * (eMinion[i].GetComponentInChildren<MinionBase>().attack) - Mathf.Clamp((pMinion[j].GetComponentInChildren<MinionBase>().hp - eMinion[i].GetComponentInChildren<MinionBase>().attack), 0, 100) * (pMinion[j].GetComponentInChildren<MinionBase>().attack));
                        //싸워서 이득이면 공격
                        if (Mathf.Clamp((eMinion[i].GetComponentInChildren<MinionBase>().hp - pMinion[j].GetComponentInChildren<MinionBase>().attack), 0, 100) * (eMinion[i].GetComponentInChildren<MinionBase>().attack) >= Mathf.Clamp((pMinion[j].GetComponentInChildren<MinionBase>().hp - eMinion[i].GetComponentInChildren<MinionBase>().attack), 0, 100) * (pMinion[j].GetComponentInChildren<MinionBase>().attack))
                        {
                            pMinion[j].GetComponentInChildren<MinionBase>().hp -= eMinion[i].GetComponentInChildren<MinionBase>().attack;
                            eMinion[i].GetComponentInChildren<MinionBase>().hp -= pMinion[j].GetComponentInChildren<MinionBase>().attack;
                            Animator anim = eMinion[i].GetComponentInChildren<Animator>();
                            anim.SetTrigger("IsAttack");
                            Debug.Log("아군 미니언" + pMinion[j].GetComponentInChildren<MinionBase>().hp);
                            Debug.Log("적 미니언" + eMinion[i].GetComponentInChildren<MinionBase>().hp);
                            eMinion[i].GetComponentInChildren<MinionBase>().aCnt = 0;
                            Debug.Log("싸움");
                        }
                    }
                }
            }
        }
    }

}