using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    //�ϐ��̐錾�i�\�����ԁj
    private float lifeTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
