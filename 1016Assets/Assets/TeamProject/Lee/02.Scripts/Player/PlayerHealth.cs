using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
public class PlayerHealth : CretureUpdate
{
    private object[] param = new object[2];
    private int MobCount;
    [SerializeField] private int flash_Index = 0;
    [SerializeField] private PostProcessVolume processVolume_Demon;
    [SerializeField] private Vignette vignette;
    [SerializeField] private Image DeadImage;
    [SerializeField] private Image Damage_Image;
    [SerializeField] private RectTransform Inventroy_object;

    private readonly float Enemy_Damage = 5.0f;

    private readonly string Hitbox_DTag = "Hitbox_D";
    private readonly string Hitbox_HTag = "Hitbox_H";

    private void Awake()
    {
        param[1] = Enemy_Damage;
        processVolume_Demon = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
        processVolume_Demon.profile.TryGetSettings(out vignette);
        Inventroy_object = GameObject.Find("PlayerUi").transform.GetChild(4).GetComponent<RectTransform>();
        DeadImage = GameObject.Find("PlayCanvas").transform.GetChild(0).GetComponent<Image>();
        Damage_Image = GameObject.Find("PlayCanvas").transform.GetChild(1).GetComponent<Image>();
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
            demon.SendMessage("KillPlayer");
            StartCoroutine(IncreaseVignetteRoundness_Demon());

        }
        else if (dead && MobCount == 1)
        {
            GameManager.G_instance.isGameover = true;

            GameObject bookhead = (GameObject)param[0];
            bookhead.SendMessage("KillPlayer");
            StartCoroutine(IncreaseVignetteRoundness_BookHead());
        }
    }

    public override void AddHealth(float AddHealth)
    {
        base.AddHealth(AddHealth);
        Mathf.Clamp(health, 0, 10);
        ShowEffect();
    }

    public override void Die()
    {
        base.Die();
        MeshRenderer mesh = transform.GetChild(1).GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    private void ShowEffect() //맞았을 때 이펙트구현 함수
    {
        Color DamageEff = Damage_Image.color;
        float MaxAlpha = 0.3f;
        float healthPersent = health / startHealth;
        float alphaValue = 1.0f - healthPersent;

        DamageEff.a = MaxAlpha * alphaValue;

        Damage_Image.color = DamageEff; 
        Damage_Image.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Hitbox_DTag))
        {
            MobCount = 0;
            param[0] = other.transform.parent.gameObject;
            OnDamage(param);
            ShowEffect();
        }
        else if (other.gameObject.CompareTag(Hitbox_HTag))
        {
            MobCount = 1;
            param[0] = other.transform.parent.gameObject;
            OnDamage(param);
            ShowEffect();
        }
    }
    private IEnumerator IncreaseVignetteRoundness_Demon()
    {
        Inventroy_object.gameObject.SetActive(false);

        float targetRoundness = 1.0f;
        float duration = 3f;
        float startRoundness = vignette.roundness.value;
        float elapsed = 0f;
        float startAlpha = DeadImage.color.a;
        float MaxAlpha = 255f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            vignette.roundness.value = Mathf.Lerp(startRoundness, targetRoundness, t);
            yield return null;
        }

        vignette.roundness.value = targetRoundness;
        Color finalColor = DeadImage.color;
        finalColor.a = MaxAlpha; // 최종 알파 값 설정
        DeadImage.color = finalColor;
        Damage_Image.enabled = false;
    }

    private IEnumerator IncreaseVignetteRoundness_BookHead()
    {
        Inventroy_object.gameObject.SetActive(false);

        float targetRoundness = 1.0f; // 최종 값
        float duration = 9.5f; // 증가하는 시간
        float startRoundness = vignette.roundness.value;
        float elapsed = 0f;
        float startAlpha = DeadImage.color.a;
        float MaxAlpha = 255f;
        float targetAlpha = 1f; // 최종 알파 값 설정

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            vignette.roundness.value = Mathf.Lerp(startRoundness, targetRoundness, t);
            Color newColor = DeadImage.color;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            DeadImage.color = newColor;
            yield return null;
        }

        vignette.roundness.value = targetRoundness;
        Color finalColor = DeadImage.color;
        finalColor.a = targetAlpha;
        DeadImage.color = finalColor;

        yield return new WaitForSeconds(0.25f);

        finalColor.a = MaxAlpha;
        DeadImage.color = finalColor;
        Damage_Image.enabled = false;
    }
}