using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //シングルトン化（デザインパターンの一種。確約）
    public static SoundManager instance;

    //変数（SE格納用の配列）
    public AudioSource[] se;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this) 
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// SE再生用関数（0：ゲームオーバー　1：回復　2：被弾　3：攻撃　4：UI　5：コイン）
    /// </summary>
    /// <param name="x"></param>
    public void PlaySE(int x)
    {
        se[x].Stop();

        se[x].Play();
    }
}
