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
    readonly string CandleTag = "Candle";
    readonly string BattrlyTag = "Battery";
    readonly string GunTag = "Gun";
    readonly string HealPackTag = "HealPack";
    readonly string BulletTag = "Bullet";

    private bool isTake=false;
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
    private bool canReload;
    public bool CanReload
    {
        get { return canReload; }
        set { canReload = value; }
    }

    [SerializeField] private List<GameObject> ItemSlots = new List<GameObject>();
    [SerializeField] private List<Text> ItemTexts; //아이템 UI 텍스트 목록 (0:손전등, 1:배터리, 2:공기총, 3:구급상자, 4:양초)
    [SerializeField] private List<Image> ItemImages; //아이템 UI 이미지 목록 (0:손전등, 1:손전등 배경)

    private Transform Camera_Tr;
    private Transform PlayerUI_Text;
    private Transform PlayerUI_Image;
    private PlayerHealth playerHealth;
    private InventoryUpdate inventoryUpdate;

    [SerializeField] FlashLight flashscript;
    [SerializeField] Gunstate gunscript;
    [SerializeField] SpriteRenderer[] CandleFire;

    void Awake()
    {
        Camera_Tr = transform;
        playerHealth = transform.parent.GetComponent<PlayerHealth>();
        inventoryUpdate = transform.parent.GetComponent<InventoryUpdate>();
        PlayerUI_Text = transform.parent.GetChild(2).GetChild(1).transform;
        PlayerUI_Image = transform.parent.GetChild(2).GetChild(2).transform;
        
        for (int i = 0; i < Camera_Tr.childCount; i++)
        {
            ItemSlots.Add(Camera_Tr.GetChild(i).gameObject);
        }
        ItemSlots.RemoveAt(0);

        for (int i = 0; i < PlayerUI_Text.childCount; i++)
        {
            ItemTexts.Add(PlayerUI_Text.GetChild(i).GetComponent<Text>());
            ItemTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerUI_Image.childCount; i++)
        {
            ItemImages.Add(PlayerUI_Image.GetChild(i).GetComponent<Image>());
            ItemImages[i].gameObject.SetActive(false);
        }

        flashscript = GameObject.Find("Flash").transform.GetChild(0).GetComponent<FlashLight>();
        gunscript = GameObject.Find("Gun").GetComponent<Gunstate>();
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
            if (hit.collider.CompareTag(CandleTag))
            {
                ItemTexts[4].gameObject.SetActive(true);
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
                    ItemTexts[4].gameObject.SetActive(false);
                    GameManager.G_instance.CanndleCounter(1);
                    Pulling_Manger.instance.SetActiveDemonTrue(hit.collider.gameObject.transform);
                }

            }
            else
            {
                ItemTexts[4].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < ItemTexts.Count; i++)
            {
                ItemTexts[i].gameObject.SetActive(false);
            }
        }
    }
    private void TakeItem(RaycastHit hit) //아이템 가져가는 함수
    {
        if (hit.collider.CompareTag(BattrlyTag)&& isTake)
        {
            //배터리 충전
            ItemTexts[1].gameObject.SetActive(true);
            if (IsCatch)
            {
                hit.collider.gameObject.SetActive(false);
                flashscript.CollectBattery();
            }

        }
        else
        {
            ItemTexts[1].gameObject.SetActive(false);
        }

        if (hit.collider.CompareTag(BulletTag) && CanReload)
        {
            //총알 한발 장전
            ItemTexts[5].gameObject.SetActive(true);
            ItemTexts[6].gameObject.SetActive(false);
            if (IsCatch)
            {
                Bullet bullet = hit.collider.GetComponent<Bullet>();
                if (bullet != null)
                    gunscript.AddBullet(bullet.AddBullet);

                hit.collider.gameObject.SetActive(false);
            }
        }
        else if (hit.collider.CompareTag(BulletTag) && !CanReload)
        {
            ItemTexts[5].gameObject.SetActive(false);
            ItemTexts[6].gameObject.SetActive(true);
        }
        else
        {
            ItemTexts[5].gameObject.SetActive(false);
            ItemTexts[6].gameObject.SetActive(false);
        }

        // 플레쉬 가져가기
        if (hit.collider.CompareTag(FlashTag))
        {
            ItemTexts[0].gameObject.SetActive(true);
            item = Item.FlashLight;
            if (IsCatch)
            {
                CatchItem(hit);
                inventoryUpdate.InventorySetup();
            }
        }
        else
        {
            ItemTexts[0].gameObject.SetActive(false);
            item = Item.None;
        }

        //총 가져가기
        if (hit.collider.CompareTag(GunTag))
        {
            ItemTexts[2].gameObject.SetActive(true);
            item = Item.Gun;
            if (IsCatch)
            {
                CatchItem(hit);
                inventoryUpdate.InventorySetup();
            }
        }
        else
        {
            ItemTexts[2].gameObject.SetActive(false);
            item = Item.None;
        }

        //힐팩 가져가기
        if (hit.collider.CompareTag(HealPackTag))
        {
            ItemTexts[3].gameObject.SetActive(true);
            item = Item.HealPack;
            if (IsCatch)
            {
                CatchItem(hit);
                inventoryUpdate.InventorySetup();
            }
        }
        else
        {
            ItemTexts[3].gameObject.SetActive(false);
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
                hitObject_F.localPosition = new Vector3(0,0,0.338f);
                hitObject_F.localRotation = Quaternion.identity;

                ItemImages[0].gameObject.SetActive(true);
                ItemImages[1].gameObject.SetActive(true);
                isTake=true;
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
                hitObject_G.localPosition = new Vector3(0f, 0f, 0.5f);
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
                hitObject_H.localPosition = new Vector3(0.1f, -0.5f, 0.7f);
                hitObject_H.localRotation = Quaternion.identity;

                break;
        }
        
    }
  
}
