using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [SerializeField] private List<Image> ItemImages; //������ UI �̹��� ��� (0:������, 1:������ ���)
    [SerializeField] private List<RectTransform> ItemSlots_UI = new List<RectTransform>(); //������ ���� UI ����Ʈ.

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

    public void OnItemIcon(GameObject obj, int idx) //�κ��丮�� ������ �������� �Ծ��� �� ȣ��
    {
        RectTransform Tr = obj.GetComponent<RectTransform>();
        obj.transform.SetParent(ItemSlots_UI[idx].transform);
        Tr.anchoredPosition = Vector2.zero;
        obj.SetActive(true);
    }

    public void OffItemIcon(GameObject obj, string GroupName) //�κ��丮���� ����ϰų� �������� �ٴڿ� ���� �� ȣ��
    {
        RectTransform Tr = obj.GetComponent<RectTransform>();
        GameObject UIgroup = GameObject.Find(GroupName);
        obj.transform.SetParent(UIgroup.transform);
        Tr.anchoredPosition = new Vector2(0f, -30f);
        obj.SetActive(false);
    }
}
