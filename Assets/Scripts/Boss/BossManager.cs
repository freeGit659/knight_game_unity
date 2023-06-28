using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HPBar hpBar;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform damagePop;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject spellSpawner;
    [SerializeField] private GameObject spellBox;
    [SerializeField] private AudioClip spellSound;
    private bool isFacingLeft;
    public bool isWasAttack;
    public bool isAttack;
    public bool isInPatrol;
    private bool isSpawning = false;
    private bool isPoint;
    public float PointA;
    public float PointB;
    private int speed;
    private int maxHealthPoint;
    public int healhtPoint;
    private int maxManaPoint;
    private int manaPoint;
    public int normalDamage;
    public int spellDamage;
    private float timeAttacked;
    private float timeSkill;
    public int damageInput;
    private float normalAttackSpeed;
    public float Posx;
    public float DistanceToPlayerX;
    public float DistanceToPlayerY;
    public int numSpell;
    float DistancePlayertoA;
    float DistancePlayertoB;
    float DistanceAtoB;
    private enum Action
    {
        Patrol = 0,
        Follow = 1,
        Attack = 2,
        Spell = 3
    }
    Action action = Action.Patrol;

    public int patrolDestination;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        //hpBar = GetComponent<HPBar>();
        maxHealthPoint = 2000;
        healhtPoint = maxHealthPoint;
        maxManaPoint = 100;
        manaPoint = 0;
        normalDamage = 20;
        spellDamage = 100;
        speed = 1;
        timeAttacked = 0.5f;
        timeSkill = 4f;
        normalAttackSpeed = 2f;
        isWasAttack = false;
        UpdateBar();
    }

    //Update is called once per frame
    void Update()
    {
        Posx = transform.position.x;
        PointA = patrolPoints[0].position.x;
        PointB = patrolPoints[1].position.x;
        UpdateBar();
        Distance();
        switch (action)
        {
            case Action.Patrol:
                Patrol();
                if (IsPlayerInPatrol()) action = Action.Follow;
                if (isPlayerInRangeAttack()) action = Action.Attack;
                if (manaPoint >= 100) action = Action.Spell;
                break;
            case Action.Follow:
                Follow(); 
                if (isPlayerInRangeAttack()) action = Action.Attack;
                if (!IsPlayerInPatrol()) action = Action.Patrol;
                if (manaPoint >= 100) action = Action.Spell;
                break;
            case Action.Attack:
                NormalAttack();
                if (!isPlayerInRangeAttack()) action = Action.Follow;
                if (manaPoint >= 100) action = Action.Spell;
                break;
            case Action.Spell:
                Skill();
                break;
        }
        IsDead();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AttackArea"))
        {
            StartCoroutine(SetAnimator("Hurt", 0.3f));
            healhtPoint -= playerController.NormalDamage;
            SetManaPoint();
            playerController.SetManaPoint();
            Vector3 damageInputVector = new Vector3(transform.position.x, transform.position.y + 1f,transform.position.z);
            txtDamageInput damageText =  Instantiate(damagePop, damageInputVector, Quaternion.identity).GetComponent<txtDamageInput>();
            damageText.SetText(playerController.NormalDamage);
        }
        if (collision.gameObject.CompareTag("PlayerSkillBox"))
        {
            StartCoroutine(SetAnimator("Hurt", 0.3f));
            Vector3 damageInputVector = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            healhtPoint -= playerController.NormalDamage * 5;
            txtDamageInput damageText = Instantiate(damagePop, damageInputVector, Quaternion.identity).GetComponent<txtDamageInput>();
            damageText.SetText(playerController.NormalDamage * 5);
        }
        if (collision.gameObject.CompareTag("Point")) isPoint = true;
    }
    protected virtual void Follow()
    {
        Vector3 scale = transform.localScale;
        animator.SetBool("Walk", true);
        if (player.transform.position.x > transform.position.x) //player in right enemy
        {
            patrolDestination = 0;
            scale.x = Mathf.Abs(scale.x);
            isFacingLeft = false;
            transform.Translate(speed * Time.deltaTime, y: 0, z: 0);
        }
        else if (player.transform.position.x < transform.position.x) // player in left enemy
        {
            patrolDestination = 1;
            scale.x = Mathf.Abs(scale.x) * -1;
            isFacingLeft = true;
            transform.Translate(speed * Time.deltaTime * -1, y: 0, z: 0);
        }
        transform.localScale = scale;
    }
    public virtual void NormalAttack()
    {
        transform.Translate(x: 0, y: 0, z: 0);
        animator.SetBool("Walk", false);
        if (normalAttackSpeed >= 2f)
        {
            StartCoroutine(SetAnimator("Attack", 1f));
            SetManaPoint();
        }
        normalAttackSpeed -= Time.deltaTime;
        if (normalAttackSpeed <= 1.5f && isAttack == false)
        {
            isAttack = true;
            gameManager.ReceiveDamage("Normal");
        }
        if (normalAttackSpeed <= 0f)
        {
            isAttack = false;
            normalAttackSpeed = 2f;
        }
    }
    public void Skill()
    {
        animator.SetBool("Idel", true);
        if (timeSkill >= 4f)
        {
            animator.SetBool("Cast", true);
            SoundManager.instance.PlaySound(spellSound);
            numSpell+= 2;
            bool isSpawning = false;
            manaPoint = 0;
        }
        timeSkill -= Time.deltaTime;
        if (timeSkill <= 2f && !isSpawning)
        {
            animator.SetBool("Spell", true);
            animator.SetBool("Cast", false);
            isSpawning = true;
            spellSpawner.SetActive(true);
        }
        if (timeSkill <=0.6f) spellBox.SetActive(true);
        if (timeSkill <= 0)
        {
            action = Action.Patrol;
            animator.SetBool("Spell", false);
            spellSpawner.SetActive(false);
            spellBox.SetActive(false);  
            isSpawning= false;
            timeSkill = 4f;
        }
    }
    protected virtual void Distance()
    {
        DistanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
        DistanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
    }
    protected virtual bool isPlayerInRangeAttack()
    {
        if (DistanceToPlayerX <= 3f) return true;
        else return false;
    }
    public void Patrol()
    {
        animator.SetBool("Walk", true);
        Vector3 scale = transform.localScale;
        if (patrolDestination == 0) //move to right
        {
            transform.Translate(speed * Time.deltaTime * (isFacingLeft ? -1 : 1), y: 0, z: 0);
            if (Mathf.Abs(transform.position.x - patrolPoints[1].position.x) <= 5f)
            {
                scale.x *= -1;
                transform.localScale = scale;
                patrolDestination = 1;
                isFacingLeft = true;
            }
        }

        if (patrolDestination == 1) //move to left
        {
            transform.Translate(speed * Time.deltaTime * (isFacingLeft ? -1 : 1), y: 0, z: 0);
            if (Mathf.Abs(transform.position.x - patrolPoints[0].position.x) <= 5f)
            {
                scale.x *= -1;
                transform.localScale = scale;
                patrolDestination = 0;
                isFacingLeft = false;
            }
        }
    }
    public bool IsPlayerInPatrol()
    {
        DistancePlayertoA = Mathf.Abs(player.transform.position.x - patrolPoints[0].position.x);
        DistancePlayertoB = Mathf.Abs(player.transform.position.x - patrolPoints[1].position.x);
        DistanceAtoB = Mathf.Abs(patrolPoints[0].transform.position.x - patrolPoints[1].position.x);
        if (DistancePlayertoA + DistancePlayertoB - DistanceAtoB <= 1f) return true;
        else return false;
    }
    public void SetManaPoint()
    {
        manaPoint += 10;
    }
    public void UpdateBar()
    {
        hpBar.UpdateBar("HP", healhtPoint, maxHealthPoint);
        hpBar.UpdateBar("MP", manaPoint, maxManaPoint);
    }
    protected virtual void IsDead()
    {
        if (healhtPoint <= 0)
        {
            animator.SetBool("Die", true);
            Destroy(gameObject,0.5f);
        }
    }
    public IEnumerator SetAnimator(string Parameter, float time)
    {
        animator.SetBool(Parameter, true);
        yield return new WaitForSeconds(time);
        animator.SetBool(Parameter, false);
    }
}
