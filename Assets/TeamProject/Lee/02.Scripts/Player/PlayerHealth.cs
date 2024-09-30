using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : CretureUpdate
{
    private FlashLight FlashLight;

    private object[] param = new object[2];

    private int MobCount;

    private readonly float Enemy_Damage = 5.0f;
    private readonly float Player_HealPack = 3.0f;

    private readonly string Hitbox_DTag = "Hitbox_D";
    private readonly string Hitbox_HTag = "Hitbox_H";

    private void Awake()
    {
        param[1] = Enemy_Damage;
        FlashLight = GameObject.Find("Flash").transform.GetChild(0).GetComponent<FlashLight>();
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
            demon.SendMessage("KillPlayer", SendMessageOptions.DontRequireReceiver);

            FlashLight flashLight;

            FlashLight.SendMessage("KillPlayer");
        }
        else if(dead && MobCount == 1)
        {
            GameManager.G_instance.isGameover = true;

            FlashLight.SendMessage("KillPlayer");
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

    private void ShowEffect() //맞았을 때 이펙트구현 함수
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Hitbox_DTag))
        {
            MobCount = 0;
            param[0] = other.transform.parent.gameObject;
            OnDamage(param);
            ShowEffect(); //블러드 이펙트
        }
        else if (other.gameObject.CompareTag(Hitbox_HTag))
        {
            MobCount = 1;
            OnDamage(param);
        }
    }
}
