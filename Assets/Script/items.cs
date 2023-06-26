using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour
{
    //変数の宣言（アイテム回復量、アイテムが消えるまでの時間、取得できるようになるまでの時間）
    public int healthItemRecoveryValue;

    [SerializeField]
    private float lifeTime;

    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        //時間経過で削除
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //取得できるようになるまでの時間
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }
}
