using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //���C�u�����ǉ�

public class Title : MonoBehaviour
{
    //�J�ڗp�֐�
    public void GameStart()
    {
        SceneManager.LoadScene("Main");
        SoundManager.instance.PlaySE(4);
    }
}
