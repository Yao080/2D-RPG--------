using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //���C�u�����ǉ�
using UnityEngine.SceneManagement; //���C�u�����ǉ�
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{

    //static
    public static GameManager instance;

    //�ϐ��̐錾�iUI�APlayerController�A�X���C�_�[�AUI*2�A�\�����镶�́A�����s�ڂ��A�������蔻��A
    //UI�i�X�e�[�^�X�j�i�[�p�Aweapon�X�N���v�g�A�o���l�A���x���A���x���A�b�v�ɕK�v�Ȍo���l�ʁA
    //LevelUp���ɌĂ�UI�Acanvas�j
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Slider staminaSlider;

    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines; //�\�����镶��

    private int currentLine; //�����s�ڂ�

    private bool justStarted; //�������蔻��

    public GameObject statusPanel; //�X�e�[�^�X

    [SerializeField]
    private Text hpText, stText, atText; //�X�e�[�^�X�ڍ�

    [SerializeField]
    private Weapon weapon; //weapon�X�N���v�g

    private int totalExp, currentLV; //�o���l�A���x��

    [SerializeField, Tooltip("���x���A�b�v�ɕK�v�Ȍo���l")]
    private int[] requiredExp;

    [SerializeField]
    private GameObject levelUpText; //LevelUp���ɌĂ�UI

    [SerializeField]
    private Canvas canvas; //canvas

    //instance�̔���
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("MaxHp"))
        {
            LoadStatuse();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogBox.activeInHierarchy)
        {
            if (Input.GetMouseButtonUp(1))
            {
                SoundManager.instance.PlaySE(4);

                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogLines.Length)
                    {
                        dialogBox.SetActive(false);
                    }
                    else
                    {
                        dialogText.text = dialogLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowStatusPanel();
        }

    }

    //�v���C���[��HP��UI�ɔ��f
    public void UpdateHealthUI()
    {
        hpSlider.maxValue = player.maxHealth;
        hpSlider.value = player.currentHealth;
    }

    //�X�^�~�iUI�̐ݒ�
    public void UpdateStaminaUI()
    {
        staminaSlider.maxValue = player.totalStamina;
        staminaSlider.value = player.currentStamina;
    }
    /// <summary>
    /// �_�C�A���O�{�b�N�X��\������֐�
    /// </summary>
    /// <param name="lines"></param>
    public void ShowDialog(string[] lines)
    {
        dialogLines = lines;

        currentLine = 0;

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);

        justStarted = true;
    }

    /// <summary>
    /// �_�C�A���O�̕\���ؑ֊֐�
    /// /// </summary>
    /// <param name="x"></param>
    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
    }

    public void Load()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �X�e�[�^�XUI�\���p�֐�
    /// </summary>
    public void ShowStatusPanel()
    {
        statusPanel.SetActive(true);

        Time.timeScale = 0f;

        //UI�X�V�p�֐��̌Ăяo��
        StatusUpadate();
    }

    public void CloseStatusPanel()
    {
        statusPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void StatusUpadate()
    {
        hpText.text = "�̗� :" + player.maxHealth;
        stText.text = "�X�^�~�i :" + player.totalStamina;
        atText.text = "�U���� :" + weapon.attackDamage;
    }

    /// <summary>
    /// �o���l���Z�֐�
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        if(requiredExp.Length <= currentLV)
        {
            return;
        }

        totalExp += exp;

        if(totalExp >= requiredExp[currentLV])
        {
            currentLV++;

            player.maxHealth += 5;
            player.totalStamina += 5;
            weapon.attackDamage += 2;

            GameObject levelUp = Instantiate(levelUpText, player.transform.position, Quaternion.identity); //�L�����p�X�𐶐�����
            levelUp.transform.SetParent(player.transform);
            //levelUp.transform.localPosition = player.transform.position + new Vector3(0, 100, 0); 
        }

    }

    /// <summary>
    /// �Z�[�u�p�֐�
    /// </summary>
    public void SaveStatuse()
    {
        PlayerPrefs.SetInt("MaxHp", player.maxHealth);
        PlayerPrefs.SetFloat("MaxSt", player.totalStamina);
        PlayerPrefs.SetInt("weapon.attackDamage", weapon.attackDamage);
        PlayerPrefs.SetInt("Level", currentLV);
        PlayerPrefs.SetInt("Exp", totalExp);
    }

    /// <summary>
    /// ���[�h�p�֐�
    /// </summary>
    public void LoadStatuse()
    {
        player.maxHealth = PlayerPrefs.GetInt("MaxHp");
        player.totalStamina = PlayerPrefs.GetFloat("MaxSt");
        weapon.attackDamage = PlayerPrefs.GetInt("weapon.attackDamage");
        currentLV = PlayerPrefs.GetInt("Level");
        totalExp = PlayerPrefs.GetInt("Exp");
    }
}
