using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamerRay : MonoBehaviour
{
    private enum Item { None = 0, FlashLight, Gun, HealPack };
    private Item item = Item.None; //아이템을 바라보면 해당 아이템으로 업데이트, 먹으면 none으로 초기화

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
        isTake = false; //임의 생성, 플레이어가 해당 아이템을 손에 쥐면 true로 바뀌도록 설정할 예정

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

            // 촛불 끄기
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
    private void TakeItem(RaycastHit hit) //아이템 가져가는 함수
    {
        if (hit.collider.CompareTag(BattrlyTag) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Battery"))
        {
            //배터리 충전
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

        // 플레쉬 가져가기
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

        //총 가져가기
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

        //힐팩 가져가기
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

    // 오브젝트 잡는 함수
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
                hitObject_F.localPosition = Vector3.zero; //위치 변경 필요
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
                hitObject_G.localPosition = Vector3.zero; //위치 변경 필요
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
                hitObject_H.localPosition = Vector3.zero; //위치 변경 필요
                hitObject_H.localRotation = Quaternion.identity;

                break;
        }
        
    }
  
}
