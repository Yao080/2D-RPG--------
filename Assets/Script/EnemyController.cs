using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //�ϐ��̐錾�i���́A�A�j���[�V�����A�������x�A�҂����ԁA�^�C�}�[�A���������A�ړ��͈́A�ǂ������锻��A�ǂ������Ă��锻��A
    //�ǂ������鑬�x�A�C�Â��͈́A�v���C���[�̈ʒu�A�U���Ԋu�A�U���́A�ő�HP�A����HP�A�m�b�N�o�b�N����A������ю��ԁA������ї́A�^�C�}�[�A
    //������ԕ����A�h���b�v�A�C�e���A�h���b�v���A�G�t�F�N�g�A�o���l�j
    private Rigidbody2D rb; //����
    private Animator enemyAnim; //�A�j���[�V����


    [SerializeField]
    private float movespeed, waitTime, walkTime; //�������x�A�҂����ԁA��������

    private float waitCounter, moveCounter; //�^�C�}�[

    private Vector2 moveDir; //��������

    [SerializeField]
    private BoxCollider2D area; //�ړ��͈�

    [SerializeField, Tooltip("�v���C���[��ǂ�������H")]
    private bool chase; //�ǂ������锻��

    private bool isChaseing; //�ǂ������Ă��锻��

    [SerializeField]
    private float chaseSpeed, rangeToChase; //�ǂ������鑬�x�A�C�Â��͈�

    private Transform target; //�v���C���[�̈ʒu

    [SerializeField]
    private float waitAfterHitting; //�U���Ԋu

    [SerializeField]
    private int attackDamage; //�U����

    [SerializeField]
    private float maxHealth; //�ő�HP
    private float currentHealth; //����HP

    private bool isKnockingBack; //�m�b�N�o�b�N������

    [SerializeField]
    private float knockBackTime, knockBackForce; //������ю��ԁA������ї�

    private float knockBackCounter; //�^�C�}�[

    private Vector2 knockDir; //������ԕ���

    [SerializeField]
    private GameObject portion;

    [SerializeField]
    private float healthDropChance;

    [SerializeField]
    private GameObject blood; //���̃G�t�F�N�g

    [SerializeField]
    private int exp; //�o���l

    [SerializeField]
    private Image hpImage; //HP�C���[�W

    private Flash flash; //�_��

    // Start is called before the first frame update
    void Start()
    {
        //�e�R���|�[�l���g�̎擾�A�^�C�}�[�̐ݒ�A�v���C���[�̈ʒu���擾
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();

        waitCounter = waitTime;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        //HP�̐ݒ�
        currentHealth = maxHealth;
        //�֐��̌Ăяo��
        UpdateHealthImage();

        flash = GetComponent<Flash>();
    }

    // Update is called once per frame
    void Update()
    {
        //�m�b�N�o�b�N����
        if(isKnockingBack)
        {
            if(knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;
                rb.velocity = knockDir * knockBackForce;
            }
            else
            {
                rb.velocity = Vector2.zero;

                isKnockingBack = false;
            }

            return;
        }

        if (!isChaseing)
        {
            //�^�C�}�[�����炵�Ă����A�ړ�������߂�
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    moveCounter = walkTime;

                    enemyAnim.SetBool("moving", true);

                    moveDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    moveDir.Normalize();
                }
            }
            else
            {
                moveCounter -= Time.deltaTime;
                rb.velocity = moveDir * movespeed;

                if (moveCounter <= 0)
                {
                    enemyAnim.SetBool("moving", false);

                    waitCounter = waitTime;
                }
            }

            if (chase)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < rangeToChase)
                {
                    isChaseing = true;
                }
            }
        }
        else
        {
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if (waitCounter <= 0)
                {
                    enemyAnim.SetBool("moving", true);
                }
            }
            else
            {
                moveDir = target.transform.position - transform.position;
                moveDir.Normalize();

                rb.velocity = moveDir * chaseSpeed;
            }
            if (Vector3.Distance(transform.position, target.transform.position) > rangeToChase)
            {
                isChaseing = false;

                waitCounter = waitTime;

                enemyAnim.SetBool("moving", false);
            }
        }
       
        //�s���͈͂̎w��
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
          Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1), transform.position.z);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(isChaseing)
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);

                waitCounter = waitAfterHitting;

                enemyAnim.SetBool("moving", false);
            }
        }
    }

    /// <summary>
    /// �m�b�N�o�b�N�p�֐�
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        isKnockingBack = true;
        knockBackCounter = knockBackTime;

        knockDir = transform.position - position;
        knockDir.Normalize();

        enemyAnim.SetBool("moving", false);
    }
    /// <summary>
    /// �_���[�W�p�֐�
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    public void TakeDamage(int damage, Vector3 position)
    {
        currentHealth -= damage;

        UpdateHealthImage(); //HP�o�[�X�V

        flash.PlayFeedback();

        if (currentHealth <= 0)
        {
            Instantiate(blood, transform.position, transform.rotation);

            GameManager.instance.AddExp(exp);

            //�|�[�V�����h���b�v����
            if(Random.Range(0, 100) < healthDropChance && portion != null)
            {
                Instantiate(portion, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }

        KnockBack(position);
    }

    private void UpdateHealthImage()
    {
        hpImage.fillAmount = currentHealth / maxHealth;
    }
}
