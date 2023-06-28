using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Animator animator;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject skillBox;
    [SerializeField] private Transform damagePop;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip skillSound;
    [SerializeField] private HPBar hPBar;
    private bool isFacingRight;
    private bool isStayGround;
    public bool isAreaAttackSmalEnemy;
    public float isPush;
    public bool isAttack;
    public bool isNormalAtteckComplete;
    private bool isEnter;
    public bool isSkill;
    public bool inSkill;
    public float moveSpeed;
    public float skillPower;
    public float skillTime;
    public float skillCoolDown;
    public float jumpForce;
    public float NormalAttackSpeed;
    private float movePos;
    public int NormalDamage;
    public int maxHealthPoint;
    public int healthPoint;
    private int maxManaPoint;
    private int manaPoint;
    public float timeAnimation;
    // Start is called before the first frame update
    void Start()
    {
        maxHealthPoint = 200;
        healthPoint = maxHealthPoint;
        maxManaPoint = 100;
        manaPoint = maxManaPoint;
        NormalAttackSpeed = 0.5f;
        NormalDamage = 20;
        skillPower = 20f;
        skillTime = 0.5f;
        skillCoolDown = 2f;
        isFacingRight = true;
        rigidbody2D= GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inSkill)
        {
            bool isJumpKeyPress = Input.GetKeyDown(KeyCode.Space);
            if (isJumpKeyPress && isStayGround)
            {
                animator.SetBool("Jump", isJumpKeyPress);
                rigidbody2D.AddForce(Vector2.up * jumpForce);
                isStayGround = false;
            }
            movePos = Input.GetAxis("Horizontal");
            rigidbody2D.velocity = new Vector2(movePos * moveSpeed, rigidbody2D.velocity.y);
            flip();
            transform.position = new Vector2(
                Mathf.Clamp(transform.position.x, -8f, 80f),
                Mathf.Clamp(transform.position.y, -6f, 3f));

            animator.SetFloat("move", Mathf.Abs(movePos));
            animator.SetBool("Jump", isJumpKeyPress);
            if (Input.GetKeyDown(KeyCode.Return) && NormalAttackSpeed == 0.5f)
            {
                isAttack = true;
                SoundManager.instance.PlaySound(attackSound);
                NormalAttackSpeed = 0.5f;
                StartCoroutine(SetAnimator("Attack", 0.5f));
            }
        }
        UpdateBar();
        NormaAttack();
        CheckDie();
        if (Input.GetKeyDown(KeyCode.R) && gameManager.numSkill == 1 && !isSkill && manaPoint >= 30)
        {
            StartCoroutine(Skill());
        }
    }
    public void flip()
    {
        if (isFacingRight && movePos < 0 || !isFacingRight && movePos > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x = scale.x * -1;
            transform.localScale = scale;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isStayGround = true;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spell"))
        {
            StartCoroutine(SetAnimator("Hurt", 0.5f));
            gameManager.ReceiveDamage("Spell");
        }
    }
    public void NormaAttack()
    {
        if (isAttack && NormalAttackSpeed > 0)
        {
            NormalAttackSpeed -= Time.deltaTime;
            attackArea.SetActive(true);
        }
        if (NormalAttackSpeed <= 0)
        {
            attackArea.SetActive(false);
            NormalAttackSpeed = 0.5f;
            isAttack = false;
        }
        
    }
    public void DamgeReceived()
    {
            healthPoint -= gameManager.playerDamageReceiver;
            Vector2 damageInputVector = new Vector2(transform.position.x, transform.position.y + 1f);
            txtDamageInput damageText = Instantiate(damagePop, damageInputVector, Quaternion.identity).GetComponent<txtDamageInput>();
            damageText.SetText(gameManager.playerDamageReceiver);
    }
    public void SetManaPoint()
    {
        if(gameManager.numSkill == 1) manaPoint += 10;
        if(manaPoint > maxManaPoint) manaPoint = maxManaPoint;
    }
    public void UpdateBar()
    {
        hPBar.UpdateBar("HP", healthPoint, maxHealthPoint);
        hPBar.UpdateBar("MP", manaPoint, maxManaPoint);
    }
    public void CheckDie()
    {
        if (healthPoint <= 0) 
        {
            animator.SetBool("Die", true);
            uiManager.GameOver();
        }
    }
    public IEnumerator SetAnimator(string Parameter, float time)
    {
            animator.SetBool(Parameter, true);
            yield return new WaitForSeconds(time);
            animator.SetBool(Parameter, false);
    }
    public IEnumerator Skill()
    {
        isSkill= true;
        inSkill= true;
        SoundManager.instance.PlaySound(skillSound);
        manaPoint -= 50;
        animator.SetBool("Skill", true);
        skillBox.SetActive(true);
        rigidbody2D.velocity = new Vector2(skillPower * transform.localScale.x, rigidbody2D.velocity.y);
        tr.emitting = true;
        yield return new WaitForSeconds(skillTime);
        inSkill= false;
        animator.SetBool("Skill", false);
        skillBox.SetActive(false);
        tr.emitting = false;
        yield return new WaitForSeconds(skillCoolDown);
        isSkill = false;
    }
    public void takedHealth(int value, float time)
    {
        StartCoroutine(heathUp(value, time));
    }
    public IEnumerator heathUp(int value, float time)
    {
        if (healthPoint < maxHealthPoint) healthPoint += value;
        yield return new WaitForSeconds(time);
        if (healthPoint > maxHealthPoint) healthPoint = maxHealthPoint;
        StartCoroutine(heathUp(value, time));
    }
}
