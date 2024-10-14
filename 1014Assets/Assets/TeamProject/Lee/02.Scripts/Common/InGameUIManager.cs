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
    private Text PlayerUI_Text;
    private Transform PlayerUI_Image;
    private RectTransform SlotsUI_Tr;

    private readonly string PlayerUI_Obj = "PlayerUi";

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
        DontDestroyOnLoad(instance);

        PlayerUI_TextObj = GameObject.Find(PlayerUI_Obj).transform.GetChild(1).GetChild(0);
        PlayerUI_Text = PlayerUI_TextObj.GetComponent<Text>();
        PlayerUI_Image = GameObject.Find(PlayerUI_Obj).transform.GetChild(2).transform;
        SlotsUI_Tr = GameObject.Find(PlayerUI_Obj).transform.GetChild(4).GetChild(0).GetComponent<RectTransform>();

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

    public void OnPlayerUI_Text(string txt)
    {
        PlayerUI_Text.text = txt;
        PlayerUI_TextObj.gameObject.SetActive(true);
    }

    public void OffPlayerUI_Text()
    {
        PlayerUI_TextObj.gameObject.SetActive(false);
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
}
