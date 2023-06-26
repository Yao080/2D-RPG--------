using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    //変数宣言（移動スピード、アニメーション、剛体、攻撃モーション、現在HP、最大HP、吹き飛び判定、吹き飛ぶ方向、吹き飛び時間、
    //吹き飛ぶ力、タイマー、無敵時間、スタミナ量、スタミナ回復速度、現在のスタミナ、ダッシュの速度、長さ、スタミナ消費量、タイマー、
    //移動時にかける用の変数）
    [SerializeField, Tooltip("移動スピード")]
    private int moveSpeed; //移動スピード

    [SerializeField]
    private Animator playerAnim; //プレイヤー

    public Rigidbody2D rb; //剛体

    [SerializeField]
    private Animator weaponAnim; //攻撃モーション

    [System.NonSerialized]
    public int currentHealth; //現在HP
    public int maxHealth; //最大HP

    private bool isknockingback; //吹き飛び判定ｓ
    private Vector2 knockDir; //吹き飛ぶ方向

    [SerializeField]
    private float knockbackTime, knockackForce; //吹き飛び時間、吹き飛ぶ力
    private float knockbackCounter; //タイマー

    [SerializeField]
    private float invincibilityTime; //無敵時間
    private float invincibilityCounter; //タイマー

    public float totalStamina, recoverySpeed; //スタミナ量、回復速度

    [System.NonSerialized]
    public float currentStamina; //現在スタミナ量

    [SerializeField]
    private float dashSpeed, dashLength, dashCost; //ダッシュ速度、長さ、消費量

    private float dashCounter, activeMoveSpeed; //タイマー、移動時にかける用の変数

    private Flash flash;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; //HP

        GameManager.instance.UpdateHealthUI();

        activeMoveSpeed = moveSpeed; //移動スピード設定

        //スタミナ＆スタミナUIの設定
        currentStamina = totalStamina;
        GameManager.instance.UpdateStaminaUI();

        flash = GetComponent<Flash>();

    }

    // Update is called once per frame
    void Update()
    {
        //メニューを開いているときは、動けないようにする

        if (GameManager.instance.statusPanel.activeInHierarchy)
        {
            return;
        }

        //無敵時間の判定と無敵時のコード取得
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }

        if(isknockingback)
        {
            knockbackCounter -= Time.deltaTime;
            rb.velocity = knockDir * knockackForce;
            if(knockbackCounter<= 0)
            {
                isknockingback = false;
            }
            else
            {
                return;
            }
        }

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * activeMoveSpeed;

        //方向とアニメーションを連動
        //プレイヤーの速度が0じゃないとき
        //攻撃モーション
        if(rb.velocity != Vector2.zero)
        {
            playerAnim.enabled = true;

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                //右を向いているとき
                if(Input.GetAxisRaw("Horizontal") > 0)
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);

                    weaponAnim.SetFloat("X", 1f);
                    weaponAnim.SetFloat("Y", 0);
                }
                //左を向いているとき
                else
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);

                    weaponAnim.SetFloat("X", -1f);
                    weaponAnim.SetFloat("Y", 0);
                }
            }
            //前を向いているとき
            else if(Input.GetAxisRaw("Vertical") > 0)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1);

                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", 1);
            }
            //後ろを向いているとき
            else
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", -1);

                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", -1);
            }
        }
        else
        {
            playerAnim.enabled = false;
        }

        //攻撃モーション
        if(Input.GetMouseButtonDown(0))
        {
            weaponAnim.SetTrigger("Attack");
        }

        //スタミナ
        if(dashCounter <= 0)
        {
            if(Input.GetKeyDown(KeyCode.Space) && currentStamina > dashCost)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;

                currentStamina -= dashCost;

                GameManager.instance.UpdateStaminaUI();
            }
        }
        else
        {
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina + recoverySpeed * Time.deltaTime, 0, totalStamina);
        GameManager.instance.UpdateStaminaUI();

    }

    /// <summary>
    /// 吹き飛ばし用関数
    /// </summary>
    /// <param name="position"></param>
    public void KnockBack(Vector3 position)
    {
        knockbackCounter = knockbackTime;
        isknockingback = true;

        knockDir = transform.position - position;

        knockDir.Normalize();
    }
    /// <summary>
    /// ダメージ用関数
    /// </summary>
    /// <param name="damage"></param>
    public  void DamagePlayer(int damage)
    {
        if(invincibilityCounter <= 0)
        {
         flash.PlayFeedback();

         currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

          invincibilityCounter = invincibilityTime;

          SoundManager.instance.PlaySE(3);

            if (currentHealth == 0)
          {
              gameObject.SetActive(false);
              SoundManager.instance.PlaySE(0);
              GameManager.instance.Load();
          }
        }
        GameManager.instance.UpdateHealthUI();
    }
    //衝突判定（ポーション）
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "portion" && maxHealth != currentHealth && collision.GetComponent<items>().waitTime <= 0)
        {
            items items = collision.GetComponent<items>();

            SoundManager.instance.PlaySE(1);

            currentHealth = Mathf.Clamp(currentHealth + items.healthItemRecoveryValue, 0, maxHealth);

            GameManager.instance.UpdateHealthUI();
            Destroy(collision.gameObject);
        }
    }
}
