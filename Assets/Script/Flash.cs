using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    //�ϐ��̐錾�i�摜�̃R���|�[�l���g�i�[�AAnimator�R���|�[�l���g�i�[�A�s�����ԁA�����ԁj
    private SpriteRenderer spriteRenderer;
    
    private Animator animator;

    [SerializeField]
    private float invisibleTime;

    [SerializeField]
    private float visibleTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// �_�ŗp�֐�
    /// </summary>
    public void PlayFeedback()
    {
        StartCoroutine("FlashCoroutine");
    }
    /// <summary>
    /// ���ۂɓ_�ł�����֐��i�R���[�`���j
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashCoroutine()
    {
        for(int i = 0; i < 3; i++) 
        {
            animator.enabled = false;

            Color spriteColor = spriteRenderer.color;
            spriteColor.a = 0;
            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(invisibleTime);

            animator.enabled = true;
            spriteColor.a = 1;
            spriteRenderer.color = spriteColor;

            yield return new WaitForSeconds(visibleTime);
        }

        yield break;
    }
}
