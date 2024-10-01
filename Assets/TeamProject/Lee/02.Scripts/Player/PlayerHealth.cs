using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : CretureUpdate
{
    private object[] param = new object[2];

    private int MobCount;
    [SerializeField]private int flash_Index;
    public int Flash_Index
    {
        get { return flash_Index; }
        set { flash_Index = value +1; }
    }

    private readonly float Enemy_Damage = 5.0f;
    private readonly float Player_HealPack = 3.0f;

    private readonly string Hitbox_DTag = "Hitbox_D";
    private readonly string Hitbox_HTag = "Hitbox_H";

    private void Awake()
    {
        param[1] = Enemy_Damage;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDamage(object[] param)
    {
        base.OnDamage(param);
        if (dead && MobCount == 0)
        {
            GameManager.G_instance.isGameover = true;

            GameObject demon = (GameObject)param[0];
            FlashLight flash = transform.GetChild(0).GetChild(flash_Index).GetChild(0).GetComponent<FlashLight>();
            demon.SendMessage("KillPlayer");
            flash.SendMessage("KillPlayer");
        }
        else if(dead && MobCount == 1)
        {
            GameManager.G_instance.isGameover = true;
            FlashLight flash = transform.GetChild(0).GetChild(flash_Index).GetChild(0).GetComponent<FlashLight>();
            flash.SendMessage("KillPlayer");
        }
    }

    public override void AddHealth(float AddHealth)
    {
        base.AddHealth(AddHealth);
    }

    public override void Die()
    {
        base.Die();
    }

    private void ShowEffect() //�¾��� �� ����Ʈ���� �Լ�
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Hitbox_DTag))
        {
            MobCount = 0;
            param[0] = other.transform.parent.gameObject;
            OnDamage(param);
            ShowEffect(); //���� ����Ʈ
        }
        else if (other.gameObject.CompareTag(Hitbox_HTag))
        {
            MobCount = 1;
            OnDamage(param);
        }
    }
}
