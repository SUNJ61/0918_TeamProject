using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamerRay : MonoBehaviour
{
    float raysize = 3.5f;
    readonly string FlashTag = "Flash";
    readonly string BattrlyTag = "Battery";
    public bool isTake;
    Transform Player_handpos;
    Transform Camera_tr;
    [SerializeField] Text flashText;
    [SerializeField] Transform flashGameObject;
    [SerializeField] FlashLight flashscript;
    [SerializeField] Image Flashbattery;
    [SerializeField] Image flash_Battery_BG;
    [SerializeField] SpriteRenderer[] CandleFire;
    [SerializeField] Text candleText;
    [SerializeField] Text BattrlyText;
    [SerializeField] FlashLight FlashLight;
     public bool OnMisson;
    void Start()
    {
        isTake = true;
        OnMisson = false;
        Camera_tr = transform;
        Player_handpos = GameObject.Find("HandPos").transform;
        flashText = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(0).GetComponent<Text>();
        candleText = GameObject.Find("PlayerUi").transform.GetChild(1).GetComponent<Text>();
        candleText.gameObject.SetActive(false);
        flashGameObject = GameObject.Find("Flash").transform;
        flashText.gameObject.SetActive(false);
        flashscript = GameObject.Find("flashlight").GetComponent<FlashLight>();
        flashscript.enabled = false;
        Flashbattery = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(1).GetComponent<Image>();
        Flashbattery.gameObject.SetActive(false);
        
        flash_Battery_BG = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        flash_Battery_BG.gameObject.SetActive(false);
        BattrlyText = GameObject.Find("PlayerUi").transform.GetChild(2).GetChild(3).GetComponent<Text>();
        FlashLight = GameObject.Find("Flash").transform.GetChild(0).GetComponent<FlashLight>();
        
    }

    void Update()
    {
        Ray ray = new Ray(Camera_tr.position, Camera_tr.forward);
        Debug.DrawRay(Camera_tr.position, Camera_tr.forward * raysize, Color.yellow);

        RaycastHit hit;
        hit = SeeTheObjectText();
    }

    



    // 오브젝트 텍스쳐 보이는 함수
    // 아니 리소스에서 아이템을 가져와야하는데 이걸 그냥 찾는걸로 하면 오류가 생기고 안하면 

    private RaycastHit SeeTheObjectText()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera_tr.position, Camera_tr.forward, out hit, raysize))
        {
           
            if (hit.collider.CompareTag(BattrlyTag) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Battery"))
            {
                //배터리 충전
                BattrlyText.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isTake = false;
                    Battety battery = hit.collider.GetComponent<Battety>();
                    if (battery != null)
                    {
                        flashscript.CollectBattery(battery);
                        isTake = true;
                        OnMisson = true;    
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit = CatchFlash(hit);
                    flashscript.enabled = true;
                    OnMisson = true;

                }
            }
           
            else
            {
                
                flashText.gameObject.SetActive(false);
            }
            // 촛불 끄기
            if (hit.collider.CompareTag("Candle"))
            {

                candleText.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.G))
                {
                    SpriteRenderer[] candleChild = hit.collider.GetComponentsInChildren<SpriteRenderer>();
                    Collider collider = hit.collider.GetComponent<BoxCollider>();
                    foreach (var f in candleChild)
                    {
                        f.enabled = false;
                       
                        if (collider != null)
                            collider.enabled = false;

                    }
                    //GameManager.G_instance.CanndleCounter(1);
                    candleText.gameObject.SetActive(false);
                }

            }
            else
            {
                candleText.gameObject.SetActive(false);

            }
        }
       

        return hit;
    }
    // 오브젝트 잡는 함수
    private RaycastHit CatchFlash(RaycastHit hit)
    {
        
        Transform hitObject = hit.collider.transform;
        hitObject.SetParent(Player_handpos);
        hitObject.localPosition = Vector3.zero;
        hitObject.localRotation = Quaternion.identity;
        Flashbattery.gameObject.SetActive(true );
        flash_Battery_BG.gameObject.SetActive(true );
        //flash.enabled = true;
        return hit;
    }
  
}
