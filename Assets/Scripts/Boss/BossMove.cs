using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public Animator enemyAnimator;
    public HPBar hpBar;
    public GameObject player;
    public PlayerController playerController;
    public Transform[] patrolPoints;
    public Transform damagePop;
    public GameManagerPart2 gameManager;
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
        hpBar = GetComponent<HPBar>();
        speed = 1;
    }

    //Update is called once per frame
    void Update()
    {
        playerPosx = transform.position.x;
        PointA = patrolPoints[0].position.x;
        PointB = patrolPoints[1].position.x;
        Swarm();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Point")) isPoint = true;
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
    }
    protected virtual void Distance()
    {
        DistanceToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
        DistanceToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
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
}
