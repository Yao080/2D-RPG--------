using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //変数の宣言（剛体、アニメーション、動く速度、待ち時間、タイマー、動く方向、移動範囲、追いかける判定、追いかけている判定、
    //追いかける速度、気づく範囲、プレイヤーの位置、攻撃間隔、攻撃力、最大HP、現在HP、ノックバック判定、吹き飛び時間、吹き飛び力、タイマー、
    //吹き飛ぶ方向、ドロップアイテム、ドロップ率、エフェクト、経験値）
    private Rigidbody2D rb; //剛体
    private Animator enemyAnim; //アニメーション


    [SerializeField]
    private float movespeed, waitTime, walkTime; //動く速度、待ち時間、動く時間

    private float waitCounter, moveCounter; //タイマー

    private Vector2 moveDir; //動く方向

    [SerializeField]
    private BoxCollider2D area; //移動範囲

    [SerializeField, Tooltip("プレイヤーを追いかける？")]
    private bool chase; //追いかける判定

    private bool isChaseing; //追いかけている判定

    [SerializeField]
    private float chaseSpeed, rangeToChase; //追いかける速度、気づく範囲

    private Transform target; //プレイヤーの位置

    [SerializeField]
    private float waitAfterHitting; //攻撃間隔

    [SerializeField]
    private int attackDamage; //攻撃力

    [SerializeField]
    private float maxHealth; //最大HP
    private float currentHealth; //現在HP

    private bool isKnockingBack; //ノックバック中判定

    [SerializeField]
    private float knockBackTime, knockBackForce; //吹き飛び時間、吹き飛び力

    private float knockBackCounter; //タイマー

    private Vector2 knockDir; //吹き飛ぶ方向

    [SerializeField]
    private GameObject portion;

    [SerializeField]
    private float healthDropChance;

    [SerializeField]
    private GameObject blood; //血のエフェクト

    [SerializeField]
    private int exp; //経験値

    [SerializeField]
    private Image hpImage; //HPイメージ

    private Flash flash; //点滅

    // Start is called before the first frame update
    void Start()
    {
        //各コンポーネントの取得、タイマーの設定、プレイヤーの位置を取得
        rb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();

        waitCounter = waitTime;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        //HPの設定
        currentHealth = maxHealth;
        //関数の呼び出し
        UpdateHealthImage();

        flash = GetComponent<Flash>();
    }

    // Update is called once per frame
    void Update()
    {
        //ノックバック判定
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
            //タイマーを減らしていき、移動先を決める
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
       
        //行動範囲の指定
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
    /// ノックバック用関数
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
    /// ダメージ用関数
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    public void TakeDamage(int damage, Vector3 position)
    {
        currentHealth -= damage;

        UpdateHealthImage(); //HPバー更新

        flash.PlayFeedback();

        if (currentHealth <= 0)
        {
            Instantiate(blood, transform.position, transform.rotation);

            GameManager.instance.AddExp(exp);

            //ポーションドロップ判定
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
