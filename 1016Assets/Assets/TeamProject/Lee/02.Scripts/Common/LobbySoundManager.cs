using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LobbySoundManager : MonoBehaviour
{
    //�κ� ����(BG)
    //�κ� ��� �к� Ÿ�� �Ҹ�(SFX)

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

        BG_Slider.onValueChanged.AddListener(value => SoundManager.instance.BG_Sound = value); // �����̴��� ���� ����Ǹ� �ڵ����� ������Ƽ�� ����
        SFX_Slider.onValueChanged.AddListener(value => SoundManager.instance.SFX_Sound = value); // �����̴��� ���� ����Ǹ� �ڵ����� ������Ƽ�� ����

        SoundSetting(0, SoundManager.instance.BG_Sound); //�� ���۽� �������� �ɼ��� �״�� �����´�.
        SoundSetting(1, SoundManager.instance.SFX_Sound); //�� ���۽� �������� �ɼ��� �״�� �����´�.
    }

    public void SoundSetting(int option, float value) //�κ���� ���� ����
    {
        switch (option)
        {
            case 0: //BG ���� ����
                BG_Volume = value;
                audioMixer.SetFloat("BG_Volume", BG_Volume);

                break;

            case 1: //SFX ���� ����
                SFX_Volume = value;
                audioMixer.SetFloat("SFX_Volume", SFX_Volume);

                break;
        }
    }

    public void ActiveSound(GameObject target, AudioClip clip, bool loop, int option) //�Ҹ��� ���� ������Ʈ, �Ҹ� Ŭ��, �ݺ� ����, [BG : 0, SFX : 1]
    {
        switch (option)
        {
            case 0: //BG
                GameObject BackGroundObj = new GameObject("backGroundSound"); //������ҽ��� ������ ������Ʈ ����
                AudioSource BG_audioSource = BackGroundObj.AddComponent<AudioSource>(); //������Ʈ�� ������ҽ� ����
                BackGroundObj.transform.SetParent(target.transform); //�Ҹ��� ���� ������Ʈ�� �ڽ����� �̵�.
                BackGroundObj.transform.position = Vector3.zero; //�θ��� ��ġ�� �̵�.

                BG_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BackGround")[0]; //����� �ͼ� ã�� BackGound �׷��� 1��° �׷��� �Ҵ�.
                BG_audioSource.clip = clip;
                BG_audioSource.loop = loop;
                BG_audioSource.minDistance = 10.0f;
                BG_audioSource.maxDistance = 30.0f;
                BG_audioSource.volume = 1.0f;
                BG_audioSource.Play();
                break;

            case 1: //SFX
                GameObject SFXObj = new GameObject("SFXSound"); //������ҽ��� ������ ������Ʈ ����
                AudioSource SFX_audioSource = SFXObj.AddComponent<AudioSource>(); //������Ʈ�� ������ҽ� ����
                SFXObj.transform.SetParent(target.transform); //�Ҹ��� ���� ������Ʈ�� �ڽ����� �̵�.
                SFXObj.transform.position = Vector3.zero; //�θ��� ��ġ�� �̵�.

                SFX_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; //����� �ͼ� ã�� BackGound �׷��� 1��° �׷��� �Ҵ�.
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
