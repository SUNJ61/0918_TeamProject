using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [SerializeField] private List<Image> ItemImages; //아이템 UI 이미지 목록 (0:손전등, 1:손전등 배경)
    [SerializeField] private List<RectTransform> ItemSlots_UI = new List<RectTransform>(); //아이템 슬롯 UI 리스트.

    private Transform PlayerUI_TextObj;
    private Transform PlayerUI_Image;
    private RectTransform SlotsUI_Tr;
    private RectTransform TalkBox;
    private RectTransform S_Option_Bg;

    private Text PlayerUI_Text;
    private Text Misson_Text;
    private Text Timer_Text;
    private Text Talk_Text;

    private readonly float TalkPading = 20.0f;

    private readonly string PlayerUI_Obj = "PlayerUi";

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);

        PlayerUI_TextObj = GameObject.Find(PlayerUI_Obj).transform.GetChild(1).GetChild(0);       
        PlayerUI_Image = GameObject.Find(PlayerUI_Obj).transform.GetChild(2).transform;
        SlotsUI_Tr = GameObject.Find(PlayerUI_Obj).transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();
        S_Option_Bg = GameObject.Find("PlayCanvas").transform.GetChild(5).GetComponent<RectTransform>();

        TalkBox = GameObject.Find("Talk").transform.GetComponent<RectTransform>();

        PlayerUI_Text = PlayerUI_TextObj.GetComponent<Text>();
        Misson_Text = GameObject.Find("Misson").transform.GetChild(0).GetComponent<Text>();
        Timer_Text = GameObject.Find("Timer").transform.GetChild(0).GetComponent<Text>();
        Talk_Text = TalkBox.transform.GetChild(0).GetComponent<Text>();

        Misson_Text.gameObject.SetActive(false);
        Timer_Text.gameObject.SetActive(false);
        S_Option_Bg.gameObject.SetActive(false);

        for (int i = 0; i < PlayerUI_Image.childCount; i++)
        {
            ItemImages.Add(PlayerUI_Image.GetChild(i).GetComponent<Image>());
            ItemImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < SlotsUI_Tr.childCount; i++)
        {
            ItemSlots_UI.Add(SlotsUI_Tr.GetChild(i).GetComponent<RectTransform>());
        }
    }

    public void SetPlayerUI_Text(string txt)
    {
        PlayerUI_Text.text = txt;
        PlayerUI_TextObj.gameObject.SetActive(true);
    }

    public void ActivePlayerUI_Text(bool state)
    {
        PlayerUI_TextObj.gameObject.SetActive(state);
    }

    public void OnPlayerUI_Img(int idx)
    {
        ItemImages[idx].gameObject.SetActive(true);
    }

    public void OffPlayerUI_Img()
    {
        for (int i = 0; i < ItemImages.Count; i++)
        {
            ItemImages[i].gameObject.SetActive(false);
        }
    }

    public void OnOption_S_On()
    {
        S_Option_Bg.gameObject.SetActive(true);
    }

    public void OnOption_S_Off()
    {
        S_Option_Bg.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
       Application.Quit();
#endif
    }

    public void OnItemIcon(GameObject obj, int idx) //인벤토리에 들어오는 아이템을 먹었을 때 호출
    {
        RectTransform Tr = obj.GetComponent<RectTransform>();
        obj.transform.SetParent(ItemSlots_UI[idx].transform);
        Tr.anchoredPosition = Vector2.zero;
        obj.SetActive(true);
    }

    public void OffItemIcon(GameObject obj, string GroupName) //인벤토리에서 사용하거나 아이템을 바닥에 버릴 때 호출
    {
        RectTransform Tr = obj.GetComponent<RectTransform>();
        GameObject UIgroup = GameObject.Find(GroupName);
        obj.transform.SetParent(UIgroup.transform);
        Tr.anchoredPosition = new Vector2(0f, -30f);
        obj.SetActive(false);
    }

    public void OnMisson(string txt)
    {
        Misson_Text.text = txt;
    }

    public void SetTimer(float time)
    {
        Timer_Text.text = $"{time}";
    }

    public void OnTimer(bool state)
    {
        Timer_Text.gameObject.SetActive(state);
    }

    public void SetTalk(string txt)
    {
        Talk_Text.text= txt;

        float txtWidth = Talk_Text.preferredWidth;
        TalkBox.sizeDelta = new Vector2(txtWidth + TalkPading, TalkBox.sizeDelta.y);
    }

    public void AutoSetTalk(string[] texts)
    {
        StartCoroutine(AutoTalk(texts));
    }

    IEnumerator AutoTalk(string[] texts)
    {
        int idx = 0;
        while(texts.Length > idx)
        {
            OnTalk(true);
            Talk_Text.text = texts[idx];

            float txtWidth = Talk_Text.preferredWidth;
            TalkBox.sizeDelta = new Vector2(txtWidth + TalkPading, TalkBox.sizeDelta.y);

            idx++;
            yield return new WaitForSeconds(3.0f);
        }
        OnTalk(false);
    }

    public void OnTalk(bool state)
    {
        TalkBox.gameObject.SetActive(state);
    }
}
