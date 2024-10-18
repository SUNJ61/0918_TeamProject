using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField] bool OptionCilck;
    [SerializeField] Image Optionimage;

    [SerializeField] RectTransform VideoOption;
    [SerializeField] RectTransform AudioOption;
    void Awake()
    {
        Optionimage = GameObject.Find("Ui").transform.GetChild(2).GetComponent<Image>();
        VideoOption = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetComponent<RectTransform>();
        AudioOption = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetComponent<RectTransform>();

        Optionimage.gameObject.SetActive(false);
        VideoOption.gameObject.SetActive(false);
        AudioOption.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManger.S_instance.NextGameScene();
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
       Application.Quit();
#endif
    }
    public void Optionopen()
    {
        Optionimage.gameObject.SetActive(true);
    }
    public void OptionClose()
    {
        Optionimage.gameObject.SetActive(false);
    }
    public void SoundMeauOpen()
    {
        AudioOption.gameObject.SetActive(true);
        VideoOption.gameObject.SetActive(false);
    }
    public void VideoMeauOpen()
    {
        AudioOption.gameObject.SetActive(false);
        VideoOption.gameObject.SetActive(true);
    }
}
