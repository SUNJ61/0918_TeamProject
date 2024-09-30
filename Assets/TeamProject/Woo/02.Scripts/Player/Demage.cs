using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Demage : MonoBehaviour
{
    int Player_hp;
    int Player_maxhp = 100;
    Transform Player_tr;
    [SerializeField]GameManager gameManager;
    [SerializeField]PlayerrMove Player_move; // �÷��̾� �̵� ��ũ��Ʈ
    CamerRotate Player_rotate; //  �÷��̾� ȸ����ũ��Ʈ
    [SerializeField] CapsuleCollider flash_Collider;
    [SerializeField] FlashLight FlashLight;

  

    void Start()
    {
        FlashLight = GameObject.Find("Flash").transform.GetChild(0).GetComponent<FlashLight>();
        Player_hp = Player_maxhp;
        Player_tr = transform;
        Player_move = GetComponent<PlayerrMove>();
        Player_rotate = GameObject.Find("CamraPos").GetComponent<CamerRotate>();
        flash_Collider = GameObject.Find("Spotlight_cookie1").GetComponent <CapsuleCollider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Hitbox"))
        {
            Player_hp -= 50;
            print(Player_hp);
            Debug.Log("�浹");


            if (Player_hp <= 0)
            {
                GameManager.G_instance.isGameover = true;
                Player_move.enabled = false;
                Player_rotate.enabled = false;

                other.transform.parent.SendMessage("KillPlayer", SendMessageOptions.DontRequireReceiver);
                FlashLight.SendMessage("KillPlayer");
                

            }

        }

    }

    
    
}
