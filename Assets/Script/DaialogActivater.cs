using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaialogActivater : MonoBehaviour
{
    //�ϐ��̐錾�i��b���́A�_�C�A���O�̕\������A�Z�[�u�|�C���g����j

    [SerializeField, Header("��b����"), Multiline(3)]
    private string[] lines;

    private bool canActivater;

    [SerializeField]
    private bool savePoint; //�Z�[�u�|�C���g����

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�_�C�A���O�\���p�R�[�h
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
    /// �Փ˔���i�Ԃ��������j
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
    /// �Փ˔���i���ꂽ���j
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
