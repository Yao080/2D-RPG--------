using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    //変数の宣言（画像のコンポーネント格納、Animatorコンポーネント格納、不可視時間、可視時間）
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
    /// 点滅用関数
    /// </summary>
    public void PlayFeedback()
    {
        StartCoroutine("FlashCoroutine");
    }
    /// <summary>
    /// 実際に点滅させる関数（コルーチン）
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
