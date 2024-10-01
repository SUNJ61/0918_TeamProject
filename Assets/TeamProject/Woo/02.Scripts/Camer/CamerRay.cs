using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamerRay : MonoBehaviour
{
    private enum Item { None = 0, FlashLight, Gun, HealPack };
    private Item item = Item.None; //�������� �ٶ󺸸� �ش� ���������� ������Ʈ, ������ none���� �ʱ�ȭ

    float raysize = 3.5f;

    readonly string FlashTag = "Flash";
    readonly string BattrlyTag = "Battery";
    readonly string GunTag = "Gun";
    readonly string HealPackTag = "HealPack";

    public bool isTake;

    private bool isAction;
    public bool IsAction
    {
        get { return isAction; }
        set { isAction = value; }
    }
    private bool isCatch;
    public bool IsCatch
    {
        get { return isCatch; }
        set { isCatch = value; }
    }

    [SerializeField] private List<GameObject> ItemSlots = new List<GameObject>();
    private PlayerHealth playerHealth;

    Transform Camera_Tr;
    [SerializeField] Text flashText;
    [SerializeField] Transform flashGameObject;
    [SerializeField] FlashLight flashscript;
    [SerializeField] Image Flashbattery;
    [SerializeField] Image flash_Battery_BG;
    [SerializeField] SpriteRenderer[] CandleFire;
    [SerializeField] Text candleText;
    [SerializeField] Text BattrlyText;

    void Start()
    {
        isTake = false; //���� ����, �÷��̾ �ش� �������� �տ� ��� true�� �ٲ�� ������ ����

        Camera_Tr = transform;
        for (int i = 0; i < Camera_Tr.childCount; i++)
        {
            ItemSlots.Add(Camera_Tr.GetChild(i).gameObject);
        }
        ItemSlots.RemoveAt(0);

        playerHealth = transform.parent.GetComponent<PlayerHealth>();

        flashGameObject = GameObject.Find("Flash").transform;

        flashText = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(0).GetComponent<Text>();
        flashText.gameObject.SetActive(false);
        candleText = GameObject.Find("PlayerUi").transform.GetChild(1).GetComponent<Text>();
        candleText.gameObject.SetActive(false);
        Flashbattery = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(1).GetComponent<Image>();
        Flashbattery.gameObject.SetActive(false);
        flash_Battery_BG = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        flash_Battery_BG.gameObject.SetActive(false);

        BattrlyText = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(3).GetComponent<Text>();
        flashscript = GameObject.Find("Flash").transform.GetChild(0).GetComponent<FlashLight>();
    }

    void Update()
    {
        Ray ray = new Ray(Camera_Tr.position, Camera_Tr.forward);
        Debug.DrawRay(Camera_Tr.position, Camera_Tr.forward * raysize, Color.yellow);

        SeeTheObjectText();
    }

    private void SeeTheObjectText()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera_Tr.position, Camera_Tr.forward, out hit, raysize))
        {
            TakeItem(hit);

            // �к� ����
            if (hit.collider.CompareTag("Candle"))
            {

                candleText.gameObject.SetActive(true);
                if (IsAction)
                {
                    SpriteRenderer[] candleChild = hit.collider.GetComponentsInChildren<SpriteRenderer>();
                    Collider collider = hit.collider.GetComponent<BoxCollider>();
                    foreach (var f in candleChild)
                    {
                        f.enabled = false;
                       
                        if (collider != null)
                            collider.enabled = false;

                    }
                    candleText.gameObject.SetActive(false);
                }

            }
            else
            {
                candleText.gameObject.SetActive(false);
            }
        }
    }
    private void TakeItem(RaycastHit hit) //������ �������� �Լ�
    {
        if (hit.collider.CompareTag(BattrlyTag) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Battery"))
        {
            //���͸� ����
            BattrlyText.gameObject.SetActive(true);
            if (IsCatch)
            {
                Battety battery = hit.collider.GetComponent<Battety>();
                if (battery != null)
                {
                    flashscript.CollectBattery(battery);
                }
                hit.collider.gameObject.SetActive(false);
            }
        }
        else
        {
            BattrlyText.gameObject.SetActive(false);
        }

        // �÷��� ��������
        if (hit.collider.CompareTag(FlashTag))
        {
            flashText.gameObject.SetActive(true);
            item = Item.FlashLight;
            if (IsCatch)
            {
                CatchItem(hit);
            }
        }
        else
        {
            flashText.gameObject.SetActive(false);
            item = Item.None;
        }

        //�� ��������
        if (hit.collider.CompareTag(GunTag))
        {
            item = Item.Gun;
            if (IsCatch)
            {
                CatchItem(hit);
            }
        }
        else
        {
            item = Item.None;
        }

        //���� ��������
        if (hit.collider.CompareTag(HealPackTag))
        {
            item = Item.HealPack;
            if (IsCatch)
            {
                CatchItem(hit);
            }
        }
        else
        {
            item = Item.None;
        }
    }

    // ������Ʈ ��� �Լ�
    private void CatchItem(RaycastHit hit)
    {
        switch(item)
        {

            case Item.FlashLight:
                Transform hitObject_F = hit.collider.transform;
                for(int i = 0; i < ItemSlots.Count -1; i++)
                {
                    if (ItemSlots[i].transform.childCount == 0)
                    {
                        hitObject_F.SetParent(ItemSlots[i].transform);
                        playerHealth.Flash_Index = i;
                        break;
                    }
                }
                hitObject_F.localPosition = Vector3.zero; //��ġ ���� �ʿ�
                hitObject_F.localRotation = Quaternion.identity;

                isTake = true;
                Flashbattery.gameObject.SetActive(true);
                flash_Battery_BG.gameObject.SetActive(true);
                break;

            case Item.Gun:
                Transform hitObject_G = hit.collider.transform;
                for (int i = 0; i < ItemSlots.Count - 1; i++)
                {
                    if (ItemSlots[i].transform.childCount == 0)
                    {
                        hitObject_G.SetParent(ItemSlots[i].transform);
                        break;
                    }
                }
                hitObject_G.localPosition = Vector3.zero; //��ġ ���� �ʿ�
                hitObject_G.localRotation = Quaternion.identity;

                break;

            case Item.HealPack:
                Transform hitObject_H = hit.collider.transform;
                for (int i = 0; i < ItemSlots.Count - 1; i++)
                {
                    if (ItemSlots[i].transform.childCount == 0)
                    {
                        hitObject_H.SetParent(ItemSlots[i].transform);
                        break;
                    }
                }
                hitObject_H.localPosition = Vector3.zero; //��ġ ���� �ʿ�
                hitObject_H.localRotation = Quaternion.identity;

                break;
        }
        
    }
  
}
