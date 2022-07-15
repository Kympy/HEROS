using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBase : MonoBehaviour
{
    public int aCnt = 1; //공격 가능 횟수
    public int attack = 0; //하수인 공격력
    public int hp = 0; //하수인 현재 체력
    public int maxHp = 0; //하수인 최대 체력
    public int posNum = 0; //하수인 위치 플레이어123, 적456

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (attack < 0)
        {
            attack = 0;
        }
        if (hp > maxHp)
        {
            hp = maxHp;
        }

        if (hp <= 0)
        {
            animator.SetBool("IsDead", true);
            Destroy(this.gameObject);
            //MinionCount();

        }
    }

    void MinionCount()
    {
        bool check = false;

        if (GameManager.nowTurn == 2 && CardBase.pcnt > 0 && check == false)
        {
            check = true;
            CardBase.pcnt--;
            Debug.Log("pcnt--: " + CardBase.pcnt);
        }

        else if (GameManager.nowTurn == 1 && CardBase.ecnt > 0 && check == false)
        {
            check = true;
            CardBase.ecnt--;
            Debug.Log("ecnt--: " + CardBase.ecnt);
        }

    }


}
