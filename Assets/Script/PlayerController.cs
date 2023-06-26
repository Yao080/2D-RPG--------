using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    //�ϐ��錾�i�ړ��X�s�[�h�A�A�j���[�V�����A���́A�U�����[�V�����A����HP�A�ő�HP�A������є���A������ԕ����A������ю��ԁA
    //������ԗ́A�^�C�}�[�A���G���ԁA�X�^�~�i�ʁA�X�^�~�i�񕜑��x�A���݂̃X�^�~�i�A�_�b�V���̑��x�A�����A�X�^�~�i����ʁA�^�C�}�[�A
    //�ړ����ɂ�����p�̕ϐ��j
    [SerializeField, Tooltip("�ړ��X�s�[�h")]
    private int moveSpeed; //�ړ��X�s�[�h

    [SerializeField]
    private Animator playerAnim; //�v���C���[

    public Rigidbody2D rb; //����

    [SerializeField]
    private Animator weaponAnim; //�U�����[�V����

    [System.NonSerialized]
    public int currentHealth; //����HP
    public int maxHealth; //�ő�HP

    private bool isknockingback; //������є��肓
    private Vector2 knockDir; //������ԕ���

    [SerializeField]
    private float knockbackTime, knockackForce; //������ю��ԁA������ԗ�
    private float knockbackCounter; //�^�C�}�[

    [SerializeField]
    private float invincibilityTime; //���G����
    private float invincibilityCounter; //�^�C�}�[

    public float totalStamina, recoverySpeed; //�X�^�~�i�ʁA�񕜑��x

    [System.NonSerialized]
    public float currentStamina; //���݃X�^�~�i��

    [SerializeField]
    private float dashSpeed, dashLength, dashCost; //�_�b�V�����x�A�����A�����

    private float dashCounter, activeMoveSpeed; //�^�C�}�[�A�ړ����ɂ�����p�̕ϐ�

    private Flash flash;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; //HP

        GameManager.instance.UpdateHealthUI();

        activeMoveSpeed = moveSpeed; //�ړ��X�s�[�h�ݒ�

        //�X�^�~�i���X�^�~�iUI�̐ݒ�
        currentStamina = totalStamina;
        GameManager.instance.UpdateStaminaUI();

        flash = GetComponent<Flash>();

    }

    // Update is called once per frame
    void Update()
    {
        //���j���[���J���Ă���Ƃ��́A�����Ȃ��悤�ɂ���

        if (GameManager.instance.statusPanel.activeInHierarchy)
        {
            return;
        }

        //���G���Ԃ̔���Ɩ��G���̃R�[�h�擾
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

        //�����ƃA�j���[�V������A��
        //�v���C���[�̑��x��0����Ȃ��Ƃ�
        //�U�����[�V����
        if(rb.velocity != Vector2.zero)
        {
            playerAnim.enabled = true;

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                //�E�������Ă���Ƃ�
                if(Input.GetAxisRaw("Horizontal") > 0)
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);

                    weaponAnim.SetFloat("X", 1f);
                    weaponAnim.SetFloat("Y", 0);
                }
                //���������Ă���Ƃ�
                else
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);

                    weaponAnim.SetFloat("X", -1f);
                    weaponAnim.SetFloat("Y", 0);
                }
            }
            //�O�������Ă���Ƃ�
            else if(Input.GetAxisRaw("Vertical") > 0)
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1);

                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", 1);
            }
            //���������Ă���Ƃ�
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

        //�U�����[�V����
        if(Input.GetMouseButtonDown(0))
        {
            weaponAnim.SetTrigger("Attack");
        }

        //�X�^�~�i
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
    /// ������΂��p�֐�
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
    /// �_���[�W�p�֐�
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
    //�Փ˔���i�|�[�V�����j
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
