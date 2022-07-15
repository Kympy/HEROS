using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private Animator animator;
    private Animator animator2;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            animator = GameObject.Find("MaleA").GetComponent<Animator>();
            animator2 = GameObject.Find("MaleB").GetComponent<Animator>();
            animator.SetBool("IsAttack", true); // 한명은 공격모션
            animator2.SetBool("IsBlock", true); // 한명은 방어모션
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // 플레이 선택화면으로 넘어가기
    }
    public void ExitGame()
    {
        Application.Quit(); // 게임 종료
    }

}
