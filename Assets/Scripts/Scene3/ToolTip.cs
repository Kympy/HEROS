using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "PlayerMonster" || hit.transform.gameObject.tag == "EnemyMonster" 
                    || hit.transform.gameObject.tag == "Card" || hit.transform.gameObject.tag == "Enemy")
                {
                    GameObject tooltip = hit.transform.GetChild(0).GetChild(0).gameObject;
                    tooltip.SetActive(true);
                    Vector2 mouseInput = Input.mousePosition;
                    tooltip.transform.position = mouseInput + new Vector2(-65f, 0f);
                }
            }
        }
    }
}
