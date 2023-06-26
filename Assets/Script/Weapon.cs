using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //�ϐ��i�U���́j
    [SerializeField]
    public int attackDamage; //�U����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //�Փ˔���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //�U���p�̊֐����Ăяo��
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);
            SoundManager.instance.PlaySE(3);
        }
    }
}
