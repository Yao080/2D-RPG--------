using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //•Ï”iUŒ‚—Íj
    [SerializeField]
    public int attackDamage; //UŒ‚—Í

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Õ“Ë”»’è
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //UŒ‚—p‚ÌŠÖ”‚ğŒÄ‚Ño‚·
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);
            SoundManager.instance.PlaySE(3);
        }
    }
}
