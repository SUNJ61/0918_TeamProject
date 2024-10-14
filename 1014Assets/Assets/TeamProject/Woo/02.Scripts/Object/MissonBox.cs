using UnityEngine;
using UnityEngine.UI; 

public class MissonBox : MonoBehaviour
{
    private Transform Player_Tr;

    readonly string PlayerTag = "Player";
    [SerializeField] Text Misson_Text;
    [SerializeField] Text MissonNumber_Text;
    [SerializeField] RectTransform Inventroy_object;
    void Start()
    {
        Player_Tr = GameObject.Find(PlayerTag).transform;
        Misson_Text = GameObject.Find("Misson").transform.GetChild(0).GetComponent<Text>();
        Inventroy_object = GameObject.Find("PlayerUi").transform.GetChild(4).GetComponent<RectTransform>();
        MissonNumber_Text = Misson_Text.transform.GetChild(0).GetComponent<Text>();

        MissonNumber_Text.gameObject.SetActive(false);
        Misson_Text.gameObject.SetActive(false);  
        Inventroy_object.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(PlayerTag))
        {
            Inventroy_object.gameObject.SetActive(true);
            MissonNumber_Text.gameObject.SetActive(true);
            Misson_Text.gameObject.SetActive(true);

            SpawnManager.instance.SetActiveTrueCandel();
            SpawnManager.instance.SetActiveBookHead();
            SpawnManager.instance.SetActiveTrueItem();

            Destroy(gameObject);
        }
       
    }
}
