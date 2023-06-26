using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //ƒ‰ƒCƒuƒ‰ƒŠ’Ç‰Á

public class Title : MonoBehaviour
{
    //‘JˆÚ—pŠÖ”
    public void GameStart()
    {
        SceneManager.LoadScene("Main");
        SoundManager.instance.PlaySE(4);
    }
}
