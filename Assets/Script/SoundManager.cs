using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //�V���O���g�����i�f�U�C���p�^�[���̈��B�m��j
    public static SoundManager instance;

    //�ϐ��iSE�i�[�p�̔z��j
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
    /// SE�Đ��p�֐��i0�F�Q�[���I�[�o�[�@1�F�񕜁@2�F��e�@3�F�U���@4�FUI�@5�F�R�C���j
    /// </summary>
    /// <param name="x"></param>
    public void PlaySE(int x)
    {
        se[x].Stop();

        se[x].Play();
    }
}
