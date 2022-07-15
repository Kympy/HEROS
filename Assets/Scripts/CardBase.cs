using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBase : MonoBehaviour
{
    public static int pcnt = 0;
    public static int ecnt = 0;

    // 버프
    public int m_AttackBuff = 1; // 하수인 개별 공격력 버프1

    // 디버프
    public int m_Debuff = 1; // 하수인 3턴 디버프
    public int h_Debuff = 1; // 영웅 5턴 디버프

    // 회복
    public int h_Heal = 10; // 영웅 회복

    // 데미지
    public int m_Damage = 5; //하수인 하나에게 데미지
    public int h_Damage = 5;

    // 광역공격
    public int m_ADamage = 5; //하수인 전체 데미지

    // 소환
    public int minionNum = 0; // 하수인 인덱스
    public GameObject tempMinion; // 소환할 하수인

    // 팁
    private Text tips;

    // 카드 캔버스
    private Transform canvas;
    private Transform canvasAI;

    private Transform clickPos;

    void Start()
    {
        tips = GameObject.Find("Tips").GetComponent<Text>();
        canvas = GameObject.Find("Cards").GetComponent<Transform>();
    }


    public void CardEffect(Transform card)
    {
        if (card.GetComponentInChildren<CardBase>().m_AttackBuff > 0)
        {
            Buff1();
            Debug.Log("버프");
        }
        else if (card.GetComponentInChildren<CardBase>().m_Debuff > 0)
        {
            Debuff1();
            Debug.Log("디버프");
        }
        else if (card.GetComponentInChildren<CardBase>().minionNum > 0)
        {
            Debug.Log("소환");
            Summon(card.GetComponentInChildren<CardBase>().minionNum);
        }
        else if (card.GetComponentInChildren<CardBase>().m_ADamage > 0)
        {
            Debug.Log("Attack1 공격");
            Attack1();
        }
        else if (card.GetComponentInChildren<CardBase>().h_Heal > 0)
        {
            Debug.Log("힐");
            Heal1();
        }
        else
        {
            Debug.Log("턴 넘김");
            GameManager.useCount = 1;
        }
    }

    public void Buff1()
    {
        tips = GameObject.Find("Tips").GetComponent<Text>();
        Debug.Log("Buff1 Clicked");


        if (GameManager.nowTurn == 1) // 플레이어 턴
        {
            if (GameObject.Find("EventSystem").GetComponent<Weighting2>().pMinionCount() == 0) // 대상이 없다면
            {
                Debug.Log("Minion is Null");
                tips.text = ("버프를 적용할 Minion이 존재하지 않습니다.");
                GameManager.useCount = 1;
            }

            else
            {
                tips.text = "아군 하수인의 공격력이 상승합니다.";

                if (GameManager.pMinion[1].childCount == 1)
                {
                    GameManager.pMinion[1].GetComponentInChildren<MinionBase>().attack += m_AttackBuff; // 공격력 상승
                }
                if (GameManager.pMinion[2].childCount == 1)
                {
                    GameManager.pMinion[2].GetComponentInChildren<MinionBase>().attack += m_AttackBuff;
                }
                if (GameManager.pMinion[3].childCount == 1)
                {
                    GameManager.pMinion[3].GetComponentInChildren<MinionBase>().attack += m_AttackBuff;
                }
                GameManager.useCount = 0;
                GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().buffeffect, new Vector3(-20, 0, -4), Quaternion.identity);
                Destroy(instance, 2.0f);

                DestroyCard();
            }

        }

        else if (GameManager.nowTurn == 2) // 적 턴
        {
            if (GameObject.Find("EventSystem").GetComponent<Weighting2>().eMinionCount() == 0)
            {
                Debug.Log("Minion is Null");
                tips.text = ("버프를 적용할 Minion이 존재하지 않습니다.");
                GameManager.useCount = 0;
            }

            else
            {
                tips.text = "적군 하수인의 공격력이 상승합니다.";

                if (GameManager.eMinion[1].childCount == 1)
                {
                    GameManager.eMinion[1].GetComponentInChildren<MinionBase>().attack += m_AttackBuff;
                }
                if (GameManager.eMinion[2].childCount == 1)
                {
                    GameManager.eMinion[2].GetComponentInChildren<MinionBase>().attack += m_AttackBuff;
                }
                if (GameManager.eMinion[3].childCount == 1)
                {
                    GameManager.eMinion[3].GetComponentInChildren<MinionBase>().attack += m_AttackBuff;
                }
                AI_BASE.card_cnt = 0;
                GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().buffeffect, new Vector3(0, 0, -4), Quaternion.identity);
                Destroy(instance, 2.0f);

                DestroyEnemyCard();
            }

        }
    }

    // 디버프 1
    public void Debuff1()
    {
        if (GameManager.nowTurn == 1) // 플레이어 턴
        {
            if (GameObject.Find("EventSystem").GetComponent<Weighting2>().eMinionCount() == 0)
            {
                Debug.Log("Minion is Null");
                tips.text = ("디버프를 적용할 Minion이 존재하지 않습니다.");
                GameManager.useCount = 1;
            }

            else
            {
                tips.text = "적군 하수인의 공격력이 감소합니다.";

                if (GameManager.eMinion[1].childCount == 1)
                {
                    GameManager.eMinion[1].GetComponentInChildren<MinionBase>().attack -= m_Debuff;
                    Debug.Log("디버프 성공 1");
                }
                if (GameManager.eMinion[2].childCount == 1)
                {
                    GameManager.eMinion[2].GetComponentInChildren<MinionBase>().attack -= m_Debuff;
                    Debug.Log("디버프 성공 2");
                }
                if (GameManager.eMinion[3].childCount == 1)
                {
                    GameManager.eMinion[3].GetComponentInChildren<MinionBase>().attack -= m_Debuff;
                    Debug.Log("디버프 성공 3");
                }
                GameManager.useCount = 0;
                GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().debuffeffect, new Vector3(0, 0, -4), Quaternion.identity);
                Destroy(instance, 2.0f);
                DestroyCard();
            }

        }

        else if (GameManager.nowTurn == 2) // 적 턴
        {
            if (GameObject.Find("EventSystem").GetComponent<Weighting2>().pMinionCount() == 0)
            {
                Debug.Log("Minion is Null");
                tips.text = ("디버프를 적용할 Minion이 존재하지 않습니다.");
                GameManager.useCount = 0;
            }

            else
            {
                tips.text = "아군 하수인의 공격력이 감소합니다.";

                if (GameManager.pMinion[1].childCount == 1)
                {
                    GameManager.pMinion[1].GetComponentInChildren<MinionBase>().attack -= m_Debuff;
                }
                if (GameManager.pMinion[2].childCount == 1)
                {
                    GameManager.pMinion[2].GetComponentInChildren<MinionBase>().attack -= m_Debuff;
                }
                if (GameManager.pMinion[3].childCount == 1)
                {
                    GameManager.pMinion[3].GetComponentInChildren<MinionBase>().attack -= m_Debuff;
                }
                AI_BASE.card_cnt = 0;
                GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().debuffeffect, new Vector3(-20, 0, -4), Quaternion.identity);
                Destroy(instance, 2.0f);
                DestroyEnemyCard();
            }

        }
    }


    // 영웅 체력 회복
    public void Heal1()
    {
        if (GameManager.nowTurn == 1) // 플레이어가 사용하면 플레이어 회복
        {
            if (GameManager.ph == 100)
            {
                Debug.Log("Player's HP is Full");
                tips.text = ("플레이어의 체력이 가득 차있습니다.");
                GameManager.useCount = 1;
            }

            else
            {
                GameManager.ph += h_Heal;
                if (GameManager.ph > 100)
                    GameManager.ph = 100;
                tips.text = "영웅이 회복되었습니다.";
                GameManager.useCount = 0;
                GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().healeffect, new Vector3(-20, 0, -4), Quaternion.identity);
                Destroy(instance, 2.0f);
                DestroyCard();
            }

        }
        else if (GameManager.nowTurn == 2) // 적이 사용하면 적 회복
        {
            if (GameManager.eh == 100)
            {
                Debug.Log("Enemy's HP is Full");
                tips.text = ("AI의 체력이 가득 차있습니다.");
                GameManager.useCount = 0;
            }

            else
            {
                GameManager.eh += h_Heal;
                if (GameManager.eh > 100)
                    GameManager.eh = 100;
                AI_BASE.card_cnt = 0;
                GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().healeffect, new Vector3(0, 0, -4), Quaternion.identity);
                Destroy(instance, 2.0f);
                DestroyEnemyCard();
            }
        }
    }

    // 하수인 단일 공격
    public void Attack1()
    {
        Debug.Log("Attack1 Clicked");
        if (GameManager.nowTurn == 1) // 플레이어 턴
        {
            if (GameManager.eMinion[1].childCount == 1)
            {
                GameManager.eMinion[1].GetComponentInChildren<MinionBase>().hp -= 5;
            }
            if (GameManager.eMinion[2].childCount == 1)
            {
                GameManager.eMinion[2].GetComponentInChildren<MinionBase>().hp -= 5;
            }
            if (GameManager.eMinion[3].childCount == 1)
            {
                GameManager.eMinion[3].GetComponentInChildren<MinionBase>().hp -= 5;
            }
            GameManager.eh -= 5;

            GameManager.useCount = 0;
            GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().attackeffect, new Vector3(0, 0, -4), Quaternion.identity);
            Destroy(instance, 2.0f);
            DestroyCard();
        }

        else if (GameManager.nowTurn == 2) // 적 턴
        {
            if (GameManager.pMinion[1].childCount == 1)
            {
                GameManager.pMinion[1].GetComponentInChildren<MinionBase>().hp -= 5;
            }
            if (GameManager.pMinion[2].childCount == 1)
            {
                GameManager.pMinion[2].GetComponentInChildren<MinionBase>().hp -= 5;
            }
            if (GameManager.pMinion[3].childCount == 1)
            {
                GameManager.pMinion[3].GetComponentInChildren<MinionBase>().hp -= 5;
            }
            GameManager.ph -= 5;

            AI_BASE.card_cnt = 0;
            GameObject instance = Instantiate(GameObject.Find("EventSystem").GetComponent<GameManager>().attackeffect, new Vector3(-20, 0, -4), Quaternion.identity);
            Destroy(instance, 2.0f);
            DestroyEnemyCard();
        }
    }


    // 하수인 소환
    public void Summon(int num)
    {
        tips = GameObject.Find("Tips").GetComponent<Text>(); // 팁 가져오기
        Debug.Log("Summon Clicked");

        int temp1 = 0;
        int temp2 = 0;

        for (int q = 1; q < 4; q++) // 최대 3 마리 소환가능
        {
            if (GameManager.eMinion[q].childCount != 0) // 적 소환수가 하나라도 있다면
                temp1++;

            if (GameManager.pMinion[q].childCount != 0) // 플레이어 소환수가 하나라도 있다면
                temp2++;
        }

        ecnt = temp1; // 적 소환수 갯수
        pcnt = temp2; // 플레이어 소환수 갯수

        Debug.Log("Top pcnt: " + pcnt + " / ecnt: " + ecnt);

        switch (num) // 소환 카드로부터 num 을 받아 알맞은 소환수를 생성
        {
            case 1:
                {
                    tempMinion = Resources.Load("MinionPrefab/Minion1") as GameObject;
                    Debug.Log("Minion1 Created");
                    break;
                }
            case 2:
                {
                    tempMinion = Resources.Load("MinionPrefab/Minion2") as GameObject;
                    Debug.Log("Minion2 Created");
                    break;
                }
            case 3:
                {
                    tempMinion = Resources.Load("MinionPrefab/Minion3") as GameObject;
                    Debug.Log("Minion3 Created");
                    break;
                }
            default:
                {
                    tempMinion = null;
                    break;
                }
        }

        if (GameManager.nowTurn == 1) // 플레이어 턴
        {
            new Vector3(-15f, 0.5f, -2f);

            if (pcnt == 3) // 소환수가 3명이라면 소환 불가
            {
                Debug.Log("Minion is Full");
                tips.text = ("Minion이 가득 찼습니다");
                GameManager.useCount = 1;
            }

            else // 소환 가능하다면
            {
                for (int i = 1; i < 4; i++)
                {
                    if (GameManager.pMinion[i].childCount == 0) // 빈자리를 찾아 소환
                    {
                        GameObject temp = Instantiate(tempMinion, GameManager.pMinion[i].position, Quaternion.Euler(0f, 90f, 0f));
                        temp.transform.parent = GameManager.pMinion[i];
                        temp.gameObject.tag = "pMinion";
                        tips.text = temp.name + "하수인이 소환되었습니다.";
                        GameManager.useCount = 0;
                        break;
                    }
                }

                DestroyCard();        // 사용한 카드 삭제
            }
        }
        else if (GameManager.nowTurn == 2) // 적 턴이라면
        {
            if (ecnt == 3)
            {
                Debug.Log("Enemy Minion is Full");
                tips.text = ("적의 Minion이 가득 찼습니다");
                GameManager.useCount = 0;
            }

            else
            {
                for (int i = 1; i < 4; i++)
                {
                    if (GameManager.eMinion[i].childCount == 0)
                    {
                        GameObject temp = Instantiate(tempMinion, GameManager.eMinion[i].position, Quaternion.Euler(0f, -90f, 0f));
                        temp.transform.parent = GameManager.eMinion[i];
                        temp.gameObject.tag = "eMinion";
                        tips.text = temp.name + "적 하수인이 소환되었습니다.";
                        GameManager.useCount = 1;
                        break;
                    }

                }
            }
            DestroyEnemyCard(); // 적 카드 삭제
        }


        Debug.Log("under pcnt: " + pcnt + " / ecnt: " + ecnt);

    }


    public void DestroyCard()
    {
        clickPos = this.transform;

        Object[] newCard;

        if (GameManager.nowTurn == 1)
        {
            if (ButtonControl.player1 == 1) // 현재 플레이어의 번호가 x 라면~~
            {
                newCard = Resources.LoadAll("AmosCards");
            }
            else if (ButtonControl.player1 == 2)
            {
                newCard = Resources.LoadAll("BessieCards");
            }
            else
            {
                newCard = Resources.LoadAll("ColinCards");
            }

            int i = Random.Range(0, newCard.Length);
            GameObject card = Instantiate(newCard[i] as GameObject);
            card.transform.SetParent(canvas);
            card.transform.localPosition = clickPos.localPosition;
        }

        Destroy(this.transform.gameObject);
    }


    public void DestroyEnemyCard()
    {
        GameObject enemyCard;
        Debug.Log("파괴할 적 카드 : " + GameManager.ecard[Weighting2.bestcard].name);
        enemyCard = GameObject.Find("eCards").transform.GetChild(Weighting2.bestcard).gameObject;


        Object[] newCard;


        if (GameManager.nowTurn == 2)
        {
            if (ButtonControl.player2 == 1) // 현재 플레이어의 번호가 x 라면~~
            {
                newCard = Resources.LoadAll("AmosCards");
            }
            else if (ButtonControl.player2 == 2)
            {
                newCard = Resources.LoadAll("BessieCards");
            }
            else
            {
                newCard = Resources.LoadAll("ColinCards");
            }
            int i;
            i = Random.Range(0, newCard.Length);
            GameObject card = Instantiate(newCard[i] as GameObject);

            Debug.Log("베스트카드는 : " + Weighting2.bestcard + " + 1 번째 카드");
            GameManager.ecard.RemoveAt(Weighting2.bestcard);

            GameManager.ecard.Add(card.transform);
            
            Debug.Log("적이 뽑은 카드 이름은 " + card.name);
            Debug.Log("생성 후 카드목록 : " + "1번카드 : " +  GameManager.ecard[0] + " 2번카드 : " + GameManager.ecard[1] + "\n" + " 3번카드 : " + GameManager.ecard[2] + " 4번카드 : " + GameManager.ecard[3] + " 5번카드 : " + GameManager.ecard[4]);
            canvasAI = GameObject.Find("eCards").GetComponent<Transform>();
            card.transform.SetParent(canvasAI);
            card.transform.localPosition = new Vector3(-200.8435f, -225.5195f, 74.25744f);

            Destroy(enemyCard);
        }
    }
}