using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Transform cardCanvas;

    private Vector3 card1pos = new Vector3(-200f, -236.5f, 0f);
    private Vector3 card2pos = new Vector3(-100f, -236.5f, 0f);
    private Vector3 card3pos = new Vector3(0f, -236.5f, 0f);
    private Vector3 card4pos = new Vector3(100f, -236.5f, 0f);
    private Vector3 card5pos = new Vector3(200f, -236.5f, 0f);

    private void Start()
    {
        cardCanvas = GameObject.Find("CardCanvas").GetComponent<Transform>();

        GameObject card1;
        card1 = Instantiate(Resources.Load("Card1") as GameObject);
        card1.transform.SetParent(cardCanvas);
        card1.transform.localPosition = card1pos;
    }
    public void DropCard()
    {

    }
    public void UseCard()
    {

    }
}
