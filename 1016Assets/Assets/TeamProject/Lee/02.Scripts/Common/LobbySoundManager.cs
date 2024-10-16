using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LobbySoundManager : MonoBehaviour
{
    //로비 음악(BG)
    //로비 배경 촛불 타는 소리(SFX)

    public static LobbySoundManager instance;

    [SerializeField]private AudioMixer audioMixer;
    private Slider BG_Slider;
    private Slider SFX_Slider;

    private float BG_Volume;
    private float SFX_Volume;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        audioMixer = Resources.Load<AudioMixer>("AudioMixer");

        BG_Slider = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetChild(2).GetComponent<Slider>();
        SFX_Slider = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetChild(3).GetComponent<Slider>();

        BG_Slider.onValueChanged.AddListener(value => SoundManager.instance.BG_Sound = value); // 슬라이더의 값이 변경되면 자동으로 프로퍼티에 적용
        SFX_Slider.onValueChanged.AddListener(value => SoundManager.instance.SFX_Sound = value); // 슬라이더의 값이 변경되면 자동으로 프로퍼티에 적용

        SoundSetting(0, SoundManager.instance.BG_Sound); //씬 시작시 이전씬의 옵션을 그대로 가져온다.
        SoundSetting(1, SoundManager.instance.SFX_Sound); //씬 시작시 이전씬의 옵션을 그대로 가져온다.
    }

    public void SoundSetting(int option, float value) //로비씬의 사운드 관리
    {
        switch (option)
        {
            case 0: //BG 음량 조절
                BG_Volume = value;
                audioMixer.SetFloat("BG_Volume", BG_Volume);

                break;

            case 1: //SFX 음량 조절
                SFX_Volume = value;
                audioMixer.SetFloat("SFX_Volume", SFX_Volume);

                break;
        }
    }

    public void ActiveSound(GameObject target, AudioClip clip, bool loop, int option) //소리가 나는 오브젝트, 소리 클립, 반복 유무, [BG : 0, SFX : 1]
    {
        switch (option)
        {
            case 0: //BG
                GameObject BackGroundObj = new GameObject("backGroundSound"); //오디오소스를 적용할 오브젝트 생성
                AudioSource BG_audioSource = BackGroundObj.AddComponent<AudioSource>(); //오브젝트에 오디오소스 적용
                BackGroundObj.transform.SetParent(target.transform); //소리를 내는 오브젝트에 자식으로 이동.
                BackGroundObj.transform.position = Vector3.zero; //부모의 위치로 이동.

                BG_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BackGround")[0]; //오디오 믹서 찾은 BackGound 그룹중 1번째 그룹을 할당.
                BG_audioSource.clip = clip;
                BG_audioSource.loop = loop;
                BG_audioSource.minDistance = 10.0f;
                BG_audioSource.maxDistance = 30.0f;
                BG_audioSource.volume = 1.0f;
                BG_audioSource.Play();
                break;

            case 1: //SFX
                GameObject SFXObj = new GameObject("SFXSound"); //오디오소스를 적용할 오브젝트 생성
                AudioSource SFX_audioSource = SFXObj.AddComponent<AudioSource>(); //오브젝트에 오디오소스 적용
                SFXObj.transform.SetParent(target.transform); //소리를 내는 오브젝트에 자식으로 이동.
                SFXObj.transform.position = Vector3.zero; //부모의 위치로 이동.

                SFX_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; //오디오 믹서 찾은 BackGound 그룹중 1번째 그룹을 할당.
                SFX_audioSource.clip = clip;
                SFX_audioSource.loop = loop;
                SFX_audioSource.minDistance = 10.0f;
                SFX_audioSource.maxDistance = 30.0f;
                SFX_audioSource.volume = 1.0f;
                SFX_audioSource.Play();

                break;
        }
    }
}
