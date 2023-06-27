using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;
    SmallEnemyManager smallEnemyManager;
    BossManager bossManager;
    Animator playerAnimator;
    private float timeIdel;
    // Start is called before the first frame update
    void Start()
    {
        timeIdel = 0.5f;
    }

    // Update is called once per frame
    public void Idel(bool status)
    {
        playerAnimator.SetBool("Idel", status);
    }
    public void Attack(bool status)
    {
        playerAnimator.SetBool("Attack", status);
    }
    public void Walk(bool status)
    {
        playerAnimator.SetBool("Walk", status);
    }
    public void Hurt(bool status)
    {
        playerAnimator.SetBool("Hurt", status);
    }
}
