using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //ライブラリ追加
using UnityEngine.SceneManagement; //ライブラリ追加
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{

    //static
    public static GameManager instance;

    //変数の宣言（UI、PlayerController、スライダー、UI*2、表示する文章、今何行目か、文字送り判定、
    //UI（ステータス）格納用、weaponスクリプト、経験値、レベル、レベルアップに必要な経験値量、
    //LevelUp字に呼ぶUI、canvas）
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private Slider staminaSlider;

    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines; //表示する文章

    private int currentLine; //今何行目か

    private bool justStarted; //文字送り判定

    public GameObject statusPanel; //ステータス

    [SerializeField]
    private Text hpText, stText, atText; //ステータス詳細

    [SerializeField]
    private Weapon weapon; //weaponスクリプト

    private int totalExp, currentLV; //経験値、レベル

    [SerializeField, Tooltip("レベルアップに必要な経験値")]
    private int[] requiredExp;

    [SerializeField]
    private GameObject levelUpText; //LevelUp時に呼ぶUI

    [SerializeField]
    private Canvas canvas; //canvas

    //instanceの判定
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

    //プレイヤーのHPをUIに反映
    public void UpdateHealthUI()
    {
        hpSlider.maxValue = player.maxHealth;
        hpSlider.value = player.currentHealth;
    }

    //スタミナUIの設定
    public void UpdateStaminaUI()
    {
        staminaSlider.maxValue = player.totalStamina;
        staminaSlider.value = player.currentStamina;
    }
    /// <summary>
    /// ダイアログボックスを表示する関数
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
    /// ダイアログの表示切替関数
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
    /// ステータスUI表示用関数
    /// </summary>
    public void ShowStatusPanel()
    {
        statusPanel.SetActive(true);

        Time.timeScale = 0f;

        //UI更新用関数の呼び出し
        StatusUpadate();
    }

    public void CloseStatusPanel()
    {
        statusPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void StatusUpadate()
    {
        hpText.text = "体力 :" + player.maxHealth;
        stText.text = "スタミナ :" + player.totalStamina;
        atText.text = "攻撃力 :" + weapon.attackDamage;
    }

    /// <summary>
    /// 経験値加算関数
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

            GameObject levelUp = Instantiate(levelUpText, player.transform.position, Quaternion.identity); //キャンパスを生成する
            levelUp.transform.SetParent(player.transform);
            //levelUp.transform.localPosition = player.transform.position + new Vector3(0, 100, 0); 
        }

    }

    /// <summary>
    /// セーブ用関数
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
    /// ロード用関数
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
