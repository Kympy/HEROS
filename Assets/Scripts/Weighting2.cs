using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weighting2 : AI_BASE //카드 종류 변경으로 쓸데없이 꼬인 코드가 좀 있습니다.
{
    public float a1 = 1; //영웅 힐
    public float a2 = 1; //광역 공격
    public float a3 = 1; //광역 버프
    public float a4 = 1; //광역 디버프
    public float a5 = 1; //소환

    public static int bestcard = 0;
    public int pMinionCount() //상대 필드(플레이어 필드)의 미니언 수
    {
        int num = 0;
        for (int i = 1; i < 4; i++)
        {
            if (pMinion[i].childCount != 0)
            {
                num++;
            }
        }
        return num;
    }
    public int eMinionCount() //내 필드(AI 필드)의 미니언 수
    {
        int num = 0;
        for (int i = 1; i < 4; i++)
        {
            if (eMinion[i].childCount != 0)
            {
                num++;
            }
        }
        return num;
    }
    void Gene()
    {
        float[,] gene = new float[10, 5];
        float[] jnum = new float[10];
        float[] tmp1 = new float[5];
        float tmp2;
        //초기 유전자 10개 생성
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                gene[i, j] = Random.Range(0.5f, 1.5f);
            }
        }
        //10세대 까지 반복
        for (int max = 1; max < 11; max++)
        {
            //적합도 측정
            for (int i = 0; i < 10; i++)
            {
                jnum[i] = Judge(gene[i, 0], gene[i, 1], gene[i, 2], gene[i, 3], gene[i, 4]);
            }
            //최상위 유전자 4개 고르기
            for (int i = 0; i < 10; i++)
            {
                //적합도 1위 유전자 찾기
                if (jnum[i] >= Mathf.Max(jnum[0], jnum[1], jnum[2], jnum[3], jnum[4], jnum[5], jnum[6], jnum[7], jnum[8], jnum[9]))
                {
                    for (int j = 0; j < 5; j++)
                    {
                        //1순위와 자리 교대 뒤 적합도 판단 대상에서 빠짐(적합도 0)
                        tmp1[j] = gene[0, j];
                        gene[0, j] = gene[i, j];
                        gene[i, j] = tmp1[j];
                        tmp2 = jnum[0];
                        jnum[0] = 0;
                        jnum[i] = tmp2;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                //적합도 2위 유전자 찾기
                if (jnum[i] >= Mathf.Max(jnum[0], jnum[1], jnum[2], jnum[3], jnum[4], jnum[5], jnum[6], jnum[7], jnum[8], jnum[9]))
                {
                    for (int j = 0; j < 5; j++)
                    {
                        //2순위와 자리 교대 뒤 적합도 판단 대상에서 빠짐(적합도 0)
                        tmp1[j] = gene[1, j];
                        gene[1, j] = gene[i, j];
                        gene[i, j] = tmp1[j];
                        tmp2 = jnum[1];
                        jnum[1] = 0;
                        jnum[i] = tmp2;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (jnum[i] >= Mathf.Max(jnum[0], jnum[1], jnum[2], jnum[3], jnum[4], jnum[5], jnum[6], jnum[7], jnum[8], jnum[9]))
                {
                    for (int j = 0; j < 5; j++)
                    {
                        //3순위와 자리 교대 뒤 적합도 판단 대상에서 빠짐(적합도 0)
                        tmp1[j] = gene[2, j];
                        gene[2, j] = gene[i, j];
                        gene[i, j] = tmp1[j];
                        tmp2 = jnum[2];
                        jnum[2] = 0;
                        jnum[i] = tmp2;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (jnum[i] >= Mathf.Max(jnum[0], jnum[1], jnum[2], jnum[3], jnum[4], jnum[5], jnum[6], jnum[7], jnum[8], jnum[9]))
                {
                    for (int j = 0; j < 5; j++)
                    {
                        //4순위와 자리 교대
                        tmp1[j] = gene[3, j];
                        gene[3, j] = gene[i, j];
                        gene[i, j] = tmp1[j];
                    }
                }
            }
            //하위 유전자 자리에 상위 유전자의 자식 생성하기
            for (int i = 4; i < 10; i++)
            {
                //부모 둘 중복 없이 선택
                int[] pg = new int[2];
                pg[0] = Random.Range(0, 3);
                pg[1] = Random.Range(0, 3);
                while (pg[0] == pg[1])
                {
                    pg[1] = Random.Range(0, 3);
                }
                for (int j = 0; j < 5; j++)
                {
                    gene[i, j] = gene[Random.Range(0, 2), j]; //두 부모에게 랜덤으로 하나씩 물려받기
                    int mutant = Random.Range(0, 5); //20%확률로
                    if (mutant == 0)
                    {
                        gene[i, j] = Random.Range(0.5f, 1.5f);//돌연변이 발생
                    }
                }
            }
            max++; //다음 세대로 이동
        }
        //최상위 유전자 찾아서 추출
        for (int i = 0; i < 10; i++)
        {
            jnum[i] = Judge(gene[i, 0], gene[i, 1], gene[i, 2], gene[i, 3], gene[i, 4]);
            if (jnum[i] >= Mathf.Max(jnum[0], jnum[1], jnum[2], jnum[3], jnum[4], jnum[5], jnum[6], jnum[7], jnum[8], jnum[9]))
            {
                for (int j = 0; j < 5; j++)
                {
                    //1순위와 자리 교대
                    tmp1[j] = gene[0, j];
                    gene[0, j] = gene[i, j];
                    gene[i, j] = tmp1[j];
                }
            }
        }
        //1순위 유전자가 가진 알파값 배정
        a1 = gene[0, 0];
        a2 = gene[0, 1];
        a3 = gene[0, 2];
        a4 = gene[0, 3];
        a5 = gene[0, 4];
    }

    float Judge(float num1, float num2, float num3, float num4, float num5) //적합도 계산
    {
        float judge;
        judge = (100 - eh) / 10 * num1 + (100 - ph) / 10 * num2 + eMinionCount() * num3 + pMinionCount() * num4 + NowCtrlnum() * num5;
        return judge;
    }

    public void CardUse() //가중치 판단 후 카드 사용
    {
        Gene();
        float best = 0;
        if (true) //쓸모없어진 조건문
        {
            for (int i = 0; i < 5; i++)
            {
                Debug.Log("힐 가중치 : " + AfterHeroHeal(ecard[i]));
                //Debug.Log("영웅 공격 가중치 : " + AfterHeroAttack(ecard[i]));
                Debug.Log("소환 가중치 : " + AfterSummon(ecard[i]));
                Debug.Log("광역 가중치 : " + AfterAOE(ecard[i]));
                Debug.Log("버프 가중치 : " + AfterBuff(ecard[i]));
                if (ecard[i].gameObject.GetComponent<CardBase>().h_Heal > 0) // 회복카드라면
                {
                    if (AfterHeroHeal(ecard[i]) >= best)
                    {
                        best = AfterHeroHeal(ecard[i]);
                        bestcard = i;
                    }
                }
                if (ecard[i].GetComponentInChildren<CardBase>().minionNum != 0) //소환 카드라면
                {
                    if (AfterSummon(ecard[i]) >= best) //현재 최고 가중치 카드라면
                    {
                        best = AfterSummon(ecard[i]); //잠재적인 사용 후보로 등록
                        bestcard = i;
                    }
                }
                if (ecard[i].GetComponent<CardBase>().m_ADamage > 0) //광역 공격 카드라면
                {
                    if (AfterAOE(ecard[i]) >= best)
                    {
                        best = AfterAOE(ecard[i]);
                        bestcard = i;
                    }
                }
                if (ecard[i].GetComponent<CardBase>().m_AttackBuff > 0 || ecard[i].GetComponent<CardBase>().m_AttackBuff < 0) //버프나 디버프 카드라면
                {
                    if (AfterBuff(ecard[i]) >= best)
                    {
                        best = AfterBuff(ecard[i]); //개발상의 편의를 위해 함수 내에서 버프와 디버프를 구분
                        bestcard = i;
                    }
                }
            }
            if (bestcard != 0) //최적의 카드를 찾았는지 확인
            {
                Debug.Log("베스트 카드 : " + bestcard);
                if ((eMinionCount() >= 3 && ecard[bestcard].GetComponent<CardBase>().minionNum != 0)) //소환카드인데 미니언이 다 찬 경우는 아닌지 확인
                {
                    card_cnt -= 1;//그냥 넘김
                }
                else
                {
                    CardEffect(ecard[bestcard]); //최적의 카드 사용
                    card_cnt -= 1;
                }
            }
            else
            {
                card_cnt -= 1; //못찾으면 턴 넘김
            }
        }

    }
    
    float NowCtrlnum() //현재 장악도 계산
    {
        float ctrlNum = 0;
        float efNum = 0;
        float pfNum = 0;
        for (int i = 0; i < 3; i++)
        {
            if (eMinion[i + 1].childCount != 0) //각 자리에 미니언이 존재 한다면 그 자리의 미니언의 능력치를 장악도에 반영
            {
                efNum += eMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * eMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp;
            }
        }
        if (efNum == 0)
        {
            efNum = 1; //미니언이 없다면 1로 고정
        }
        for (int i = 0; i < 3; i++)
        {
            if (pMinion[i + 1].childCount != 0) //각 자리에 미니언이 존재 한다면 그 자리의 미니언의 능력치를 장악도에 반영
            {
                pfNum += pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp;
            }
        }
        if (efNum == 0)
        {
            pfNum = 1; //미니언이 없다면 1로 고정
        }
        ctrlNum = efNum / pfNum;

        return ctrlNum;
    }
    float AfterHeroHeal(Transform card) //회복 가중치 계산
    {
        int heal = card.GetComponent<CardBase>().h_Heal;
        return ((eh + heal) / eh) * (100 - eh) * NowCtrlnum() * a1; //(사용 후 체력/현재 체력)*(100-현재 체력) * (장악도) * 알파1
    }

    float AfterSummon(Transform card) //소환 가중치 계산
    {
        if (card.GetComponentInChildren<CardBase>().minionNum == 0 || eMinionCount() >= 3) //소환 카드가 아니거나 소환수가 꽉 차있으면 가중치0
            return 0;
        Debug.Log("소환수 번호" + card.GetComponentInChildren<CardBase>().minionNum);
        float ctrlNum = 0;
        float efNum = 0;
        float pfNum = 0;
        if (eMinion[0 + 1].childCount != 0 && eMinion[1 + 1].childCount != 0 && eMinion[2 + 1].childCount != 0) //내 하수인이 하나라도 있다면 내 필드쪽 장악도를 정상적으로 계산
        {
            ctrlNum = NowCtrlnum();
        }
        for (int i = 0; i < 3; i++) //내 하수인들의 능력치를 장악도 계산에 반영
        {
            if (eMinion[i + 1].childCount != 0)
            {
                efNum += eMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * eMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp;
            }
        }
        efNum += card.GetComponentInChildren<MinionBase>().attack * card.GetComponentInChildren<MinionBase>().hp;
        if (efNum == 0) //내 하수인이 없다면 내 필드쪽 장악도를 1로 계산
        {
            efNum = 1;
        }
        for (int i = 0; i < 3; i++)
        {
            if (pMinion[i + 1].childCount != 0) //플레이어 하수인들의 능력치를 장악도 계산에 반영
            {
                pfNum += pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp;
            }
        }
        if (pfNum == 0) //플레이어 하수인이 없다면 내 필드쪽 장악도를 1로 계산
        {
            pfNum = 1;
        }
        ctrlNum = efNum / pfNum;

        return ctrlNum / NowCtrlnum() * a5;
    }
    float AfterAOE(Transform card) //광역공격 가중치 계산
    {
        int mdmg;
        int hdmg;
        float ctrlNum = 0;
        float efNum = 0;
        float pfNum = 0;
        for (int i = 0; i < 3; i++)
        {
            if (eMinion[i + 1].childCount != 0) //카드 사용 후 내 필드쪽 장악도 계산
            {
                efNum += eMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * eMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp;
            }
        }
        if (efNum == 0) //내 하수인이 없다면 내 필드쪽 장악도를 1로 고정
        {
            efNum = 1;
        }
        mdmg = card.GetComponent<CardBase>().m_ADamage;
        hdmg = card.GetComponent<CardBase>().h_Damage;

        for (int i = 0; i < 3; i++)
        {
            if (pMinion[i + 1].childCount != 0 && pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp - mdmg > 0) //카드 사용 후 플레이어 필드쪽 장악도 계산
            {
                pfNum += pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * (pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp - mdmg);
            }
        }
        if (pfNum == 0) //카드 사용 후 플레이어 필드쪽 장악도 계산
        {
            pfNum = 1;
        }
        ctrlNum = efNum / pfNum;
        return ctrlNum / NowCtrlnum() * a2 + ((ph - hdmg) / ph) * (100 - ph) * NowCtrlnum() * a2;//(사용 후 적 체력/현재 적 체력)*(100-현재 적 체력) * (장악도) * 알파2
    }
    float AfterBuff(Transform card)
    {
        int atk = card.GetComponent<CardBase>().m_AttackBuff;

        float ctrlNum = 0;
        float efNum = 0;
        float pfNum = 0;

        if (atk > 0 && eMinionCount() == 0)
        {
            return 0;
        }
        if (atk < 0 && pMinionCount() == 0)
        {
            return 0;
        }
        if (pMinion[0 + 1].childCount != 0 && pMinion[1 + 1].childCount != 0 && pMinion[2 + 1].childCount != 0)
        {
            return 0; //내 하수인이 없다면 가중치 0
        }
        //각 자리에 하수인이 있는지 판단하고 없다면 가중치 미반영
        int ema1;
        if (eMinion[0 + 1].childCount > 0)
        {
            ema1 = eMinion[0 + 1].GetComponentInChildren<MinionBase>().attack;
        }
        else
        {
            ema1 = 0;
        }
        int ema2;
        if (eMinion[1 + 1].childCount > 0)
        {
            ema2 = eMinion[1 + 1].GetComponentInChildren<MinionBase>().attack;
        }
        else
        {
            ema2 = 0;
        }
        int ema3;
        if (eMinion[2 + 1].childCount > 0)
        {
            ema3 = eMinion[2 + 1].GetComponentInChildren<MinionBase>().attack;
        }
        else
        {
            ema3 = 0;
        }
        int emh1;
        if (eMinion[0 + 1].childCount > 0)
        {
            emh1 = eMinion[0 + 1].GetComponentInChildren<MinionBase>().hp;
        }
        else
        {
            emh1 = 0;
        }
        int emh2;
        if (eMinion[1 + 1].childCount > 0)
        {
            emh2 = eMinion[1 + 1].GetComponentInChildren<MinionBase>().hp;
        }
        else
        {
            emh2 = 0;
        }
        int emh3;
        if (eMinion[2 + 1].childCount > 0)
        {
            emh3 = eMinion[2 + 1].GetComponentInChildren<MinionBase>().hp;
        }
        else
        {
            emh3 = 0;
        }
        int pma1;
        if (pMinion[0 + 1].childCount > 0)
        {
            pma1 = pMinion[0 + 1].GetComponentInChildren<MinionBase>().attack;
        }
        else
        {
            pma1 = 0;
        }
        int pma2;
        if (pMinion[1 + 1].childCount > 0)
        {
            pma2 = pMinion[1 + 1].GetComponentInChildren<MinionBase>().attack;
        }
        else
        {
            pma2 = 0;
        }
        int pma3;
        if (pMinion[2 + 1].childCount > 0)
        {
            pma3 = pMinion[2 + 1].GetComponentInChildren<MinionBase>().attack;
        }
        else
        {
            pma3 = 0;
        }
        int pmh1;
        if (pMinion[0 + 1].childCount > 0)
        {
            pmh1 = pMinion[0 + 1].GetComponentInChildren<MinionBase>().hp;
        }
        else
        {
            pmh1 = 0;
        }
        int pmh2;
        if (pMinion[1 + 1].childCount > 0)
        {
            pmh2 = pMinion[1 + 1].GetComponentInChildren<MinionBase>().hp;
        }
        else
        {
            pmh2 = 0;
        }
        int pmh3;
        if (pMinion[2 + 1].childCount > 0)
        {
            pmh3 = pMinion[2 + 1].GetComponentInChildren<MinionBase>().hp;
        }
        else
        {
            pmh3 = 0;
        }

        if (atk > 0) //아군 버프카드일 경우
        {
            efNum = Mathf.Clamp((ema1 + atk), 0, 100) * emh1 + Mathf.Clamp((ema2 + atk), 0, 100) * emh2 + efNum + Mathf.Clamp((ema3 + atk), 0, 100) * emh3;
        }
        else if (atk < 0) //적 디버프 카드일 경우
        {
            pfNum = Mathf.Clamp((pma1 + atk), 0, 100) * pmh1 + Mathf.Clamp((pma2 + atk), 0, 100) * pmh2 + efNum + Mathf.Clamp((pma3 + atk), 0, 100) * pmh3;
        }

        if (efNum == 0) //내 필드쪽 가중치가 0이면 1로 변경
        {
            efNum = 1;
        }
        if (pfNum == 0) //아직까지 상대 필드 가중치가 0이면 아군 버프카드 이므로 적 능력치를 그대로 장악도에 반영
        {
            for (int i = 0; i < 3; i++)
            {
                if (pMinion[i + 1].childCount != 0)
                {
                    pfNum += pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().attack * pMinion[i + 1].GetChild(0).GetComponent<MinionBase>().hp;
                }
            }
        }
        if (pfNum == 0) //그래도 상대 필드쪽 가중치가 0이면 1로 변경
        {
            pfNum = 1;
        }
        ctrlNum = efNum / pfNum;

        if (atk > 0) //버프카드 가중치
            return ctrlNum / NowCtrlnum() * a3;
        else if (atk < 0) //디버프카드 가중치
            return ctrlNum / NowCtrlnum() * a4;
        else
            return 0;
    }

}