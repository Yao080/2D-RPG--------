using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class items : MonoBehaviour
{
    //�ϐ��̐錾�i�A�C�e���񕜗ʁA�A�C�e����������܂ł̎��ԁA�擾�ł���悤�ɂȂ�܂ł̎��ԁj
    public int healthItemRecoveryValue;

    [SerializeField]
    private float lifeTime;

    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        //���Ԍo�߂ō폜
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //�擾�ł���悤�ɂȂ�܂ł̎���
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }
}
