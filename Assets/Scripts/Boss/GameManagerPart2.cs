using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPart2 : MonoBehaviour
{
    public BossManager bossManager;
    public PlayerController playerController;
    public Transform damagePop;
    public int playerDamageReceiver;
    public int enemyDamageReceiver;
    public bool playerAttacked;
    public bool enemyAttacked;
    public float timeAnimation;
    public int cout;



    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame

    public void ReceiveDamage(string name)
    {
        if (name == "Normal") playerDamageReceiver = bossManager.normalDamage;
        if (name == "Spell") playerDamageReceiver = bossManager.spellDamage;
        playerController.SetManaPoint();
        playerController.DamgeReceived();
        StartCoroutine(playerController.SetAnimator("Hurt", 0.5f));
        cout++;
    }     
}
