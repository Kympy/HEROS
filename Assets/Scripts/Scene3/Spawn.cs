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

    public Transform spawnPointA;
    public Transform spawnPointB;

    private Text name1;
    private Text name2;

    private void Start()
    {
        name1 = GameObject.Find("Player1Name").GetComponent<Text>();
        name2 = GameObject.Find("Player2Name").GetComponent<Text>();

        if (ButtonControl.player1 == 1)
        {
            Instantiate(Amos, spawnPointA);
            name1.text = "Amos";
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
