using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionHP : MonoBehaviour
{
    private MinionBase minionBase;
    private Slider hpBar;
    private int valHP;
    private float nowhp;
    private float maxhp;
    void Start()
    {
        nowhp = this.transform.parent.parent.GetComponent<MinionBase>().hp;
        maxhp = this.transform.parent.parent.GetComponent<MinionBase>().maxHp;
        hpBar = this.GetComponent<Slider>();
        Debug.Log("현재체력 : " + nowhp);
    }

    void Update()
    {
        nowhp = this.transform.parent.parent.GetComponent<MinionBase>().hp;
        maxhp = this.transform.parent.parent.GetComponent<MinionBase>().maxHp;
        hpBar.value = nowhp / maxhp;
    }
}
