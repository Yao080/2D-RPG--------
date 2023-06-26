using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaialogActivater : MonoBehaviour
{
    //変数の宣言（会話文章、ダイアログの表示判定、セーブポイント判定）

    [SerializeField, Header("会話文章"), Multiline(3)]
    private string[] lines;

    private bool canActivater;

    [SerializeField]
    private bool savePoint; //セーブポイント判定

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ダイアログ表示用コード
        if(Input.GetMouseButtonDown(1) && canActivater && !GameManager.instance.dialogBox.activeInHierarchy)
        {
            GameManager.instance.ShowDialog(lines);

            if(savePoint) 
            {
              GameManager.instance.SaveStatuse();
            }
        }
    }

    /// <summary>
    /// 衝突判定（ぶつかった時）
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canActivater = true;
        }
    }

    /// <summary>
    /// 衝突判定（離れた時）
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canActivater = false;

            GameManager.instance.ShowDialogChange(canActivater);
        }
    }
}
