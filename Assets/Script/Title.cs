using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //ライブラリ追加

public class Title : MonoBehaviour
{
    //遷移用関数
    public void GameStart()
    {
        SceneManager.LoadScene("Main");
        SoundManager.instance.PlaySE(4);
    }
}
