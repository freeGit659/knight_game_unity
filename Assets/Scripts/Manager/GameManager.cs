using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    public SmallEnemyManager[] smallEnemyManager;
    public PlayerController playerController;
    public GameObject key;
    public GameObject skill;
    public GameObject skillIcon;
    public GameObject health;
    public Transform damagePop;
    public CameraManager cameraManager;
    public BossManager bossManager;
    public int playerDamageReceiver;
    public int EnemyDamageReceiver;
    public bool playerAttacked;
    public int numKey;
    public int numSkill;
    public int cout;
    public Vector3 keyPosition;



    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        cameraManager.SetCamera(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (smallEnemyManager[0] != null)
        {
            if (smallEnemyManager[0].healhtPoint <= 0 && !skill.activeSelf)
            {
                keyPosition = new Vector3(smallEnemyManager[0].transform.position.x - 2f, 0f, smallEnemyManager[0].transform.position.z);
                skill.SetActive(true);
            }
        }
        if(smallEnemyManager[1] != null)
        {
            if (smallEnemyManager[1].healhtPoint <= 0 && !health.activeSelf)
            {
                keyPosition = new Vector3(smallEnemyManager[1].transform.position.x - 2f, 0f, smallEnemyManager[1].transform.position.z);
                health.SetActive(true);
            }
        }
        if (smallEnemyManager[2] != null)
        {
            if (smallEnemyManager[2].healhtPoint <= 0 && !key.activeSelf)
            {
                keyPosition = new Vector3(smallEnemyManager[2].transform.position.x - 2f, 0f, smallEnemyManager[2].transform.position.z);
                key.SetActive(true);
            }
        }
        if (bossManager.healhtPoint <= 0) uIManager.GameWin();
    }

    public void ReceiveDamage()
    {
        for (int i = 0; i < smallEnemyManager.Length; i++)
        {
            if (smallEnemyManager[i].isAttack)
            {
                playerAttacked = true;
                playerDamageReceiver = smallEnemyManager[i].normalDamage;
                playerController.DamgeReceived();
                StartCoroutine(playerController.SetAnimator("Hurt", 0.5f));
                break;
            }
            else
            {
                playerAttacked = false;
                playerDamageReceiver = 0;
            }
        }
        for (int j = 0; j < smallEnemyManager.Length; j++)
        {
            if (smallEnemyManager[j].isWasAttack) EnemyDamageReceiver = playerController.NormalDamage;
            else EnemyDamageReceiver = 0;
        }
    }
    public void TakedKey()
    {
        numKey = 1;
    }
    public void TakedSkill()
    {
        numSkill = 1;
        skillIcon.SetActive(true);
    }
    public void TakedHealth(int value, float time)
    {
        playerController.takedHealth(value, time);
    }
    public void SetCamera(int part)
    {
        cameraManager.SetCamera(part);
    }
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
