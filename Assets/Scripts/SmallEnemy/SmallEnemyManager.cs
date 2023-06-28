using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyManager : MonoBehaviour
{
    public Animator enemyAnimator;
    public HPBar hpBar;
    public GameObject player;
    public PlayerController playerController;
    public Transform[] patrolPoints;
    public Transform damagePop;
    public GameManager gameManager;
    private bool isFacingLeft;
    public bool isWasAttack;
    public bool isAttack;
    private bool isPoint;
    public float PointA;
    public float PointB;
    private int speed;
    private int maxHealthPoint;
    public int healhtPoint;
    public int normalDamage;
    private float timeAttacked;
    public int damageInput;
    private float normalAttackSpeed;
    public float playerPosx;
    public float DistanceToPlayerX;
    public float DistanceToPlayerY;
 
    public int patrolDestination;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAnimator = GetComponent<Animator>();
        maxHealthPoint = 300;
        healhtPoint = maxHealthPoint;
        normalDamage = 10;
        speed = 1;
        timeAttacked = 0.5f;
        normalAttackSpeed = 2f;
        isWasAttack = false;
        hpBar.UpdateBar("HP", healhtPoint, maxHealthPoint);
        enemyAnimator.SetBool("WasAttacked", false);
    }

    //Update is called once per frame
    void Update()
    {
        playerPosx = transform.position.x;
        PointA = patrolPoints[0].position.x;
        PointB = patrolPoints[1].position.x;
        hpBar.UpdateBar("HP",healhtPoint, maxHealthPoint);
        Swarm();
        IsDead();
        AttackedAnimator();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.CompareTag("AttackArea"))
            {
                isWasAttack = true;
                playerController.SetManaPoint();
                Vector3 damageInputVector = new Vector3(transform.position.x, transform.position.y + 1f,transform.position.z);
                healhtPoint -= playerController.NormalDamage;
                txtDamageInput damageText =  Instantiate(damagePop, damageInputVector, Quaternion.identity).GetComponent<txtDamageInput>();
                damageText.SetText(playerController.NormalDamage);
            }
            if (collision.gameObject.CompareTag("Point"))
            {
                isPoint = true;
            }
            if (collision.gameObject.CompareTag("PlayerSkillBox"))
            {
                Vector3 damageInputVector = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                healhtPoint -= playerController.NormalDamage * 5;
                txtDamageInput damageText = Instantiate(damagePop, damageInputVector, Quaternion.identity).GetComponent<txtDamageInput>();
                damageText.SetText(playerController.NormalDamage*5);
            }

    }

    protected virtual void Swarm()
    {
        Vector3 scale = transform.localScale;
        if (isWasAttack) return;
        if (!isWasAttack && !IsPlayerInPatrol())
        {
            Patrol();
            enemyAnimator.SetBool("Walk", true);
        }
        else
        {
            if (player.transform.position.x > transform.position.x //player in right enemy
                && !CheckinRangeAttack()
                )
            {
                patrolDestination = 0;
                enemyAnimator.SetBool("Attack", false);
                enemyAnimator.SetBool("Walk", true);
                scale.x = Mathf.Abs(scale.x);
                isFacingLeft = false;
                transform.Translate(speed * Time.deltaTime, y: 0, z: 0);
            }
            else if (player.transform.position.x < transform.position.x // player in left enemy
                && !CheckinRangeAttack()
                )
            {
                patrolDestination = 1;
                enemyAnimator.SetBool("Attack", false);
                enemyAnimator.SetBool("Walk", true);
                scale.x = Mathf.Abs(scale.x) * -1;
                isFacingLeft= true;
                transform.Translate(speed * Time.deltaTime * -1, y: 0, z: 0);
            }
            if (CheckinRangeAttack())
            {
                transform.Translate(x: 0, y: 0, z: 0);
                enemyAnimator.SetBool("Walk", false);
                Attack(true);
            }
            else Attack(false);

            transform.localScale = scale;
        }
    }
    protected virtual void AttackedAnimator()
    {
        if (isWasAttack && timeAttacked > 0)
        {
            enemyAnimator.SetBool("WasAttacked", true);
            timeAttacked -= Time.deltaTime;
        }
        if (timeAttacked <= 0)
        {
            enemyAnimator.SetBool("WasAttacked", false);
            isWasAttack = false;
            timeAttacked = 0.5f;
        }
    }
    protected virtual void Distance()
    {
        DistanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
        DistanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
    }
    protected virtual bool CheckinRangeAttack()
    {
        Distance();
        if (DistanceToPlayerX <= 1.5f && DistanceToPlayerY <= 1.5f) return true;
        else return false;
    }
    public virtual void Attack(bool attack)
    {
        if (attack)
        {
            enemyAnimator.SetBool("Attack", true);
            normalAttackSpeed -= Time.deltaTime;
            if (normalAttackSpeed <= 0)
            {
                isAttack = true;
                gameManager.ReceiveDamage();
                enemyAnimator.SetBool("Attack", false);
                normalAttackSpeed = 2f;
            }
            else isAttack= false;
        }
        else isAttack = false;
    }
    public void Patrol()
    {
        enemyAnimator.SetBool("Attack", false);
        Vector3 scale = transform.localScale;
        if (patrolDestination == 0) //move to right
        {
            transform.Translate(speed * Time.deltaTime * (isFacingLeft ? -1 : 1), y: 0, z: 0);
            if (Mathf.Abs(transform.position.x - patrolPoints[1].position.x) <= 0.1f)
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
            if (Mathf.Abs(transform.position.x - patrolPoints[0].position.x) <= 0.1f)
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
        float DistancePlayertoA = Mathf.Abs(player.transform.position.x - patrolPoints[0].position.x);
        float DistancePlayertoB = Mathf.Abs(player.transform.position.x - patrolPoints[1].position.x);
        float DistanceAtoB = Mathf.Abs(patrolPoints[0].transform.position.x - patrolPoints[1].position.x);
        if (DistancePlayertoA + DistancePlayertoB == DistanceAtoB) return true;
        else return false;
    }
    protected virtual void IsDead()
    {
        if (healhtPoint <= 0)
        {
            enemyAnimator.SetBool("Die", true);
            Destroy(gameObject,0.5f);
        }
    }
}
