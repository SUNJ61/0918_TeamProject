using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour, IItem
{
    MeshCollider DoorCol;
    Image endingimg;
    Text endingText;
    [SerializeField] GameObject PlayerUi;
    [SerializeField] GameObject PlayCanvas;
    string fulltext = "���� ���� Ż�⿡ �����߰�\n �ڵ��� ������ �ǹ��� ����� �־���...";
    string playertag = "Player";
    void Start()
    {
        DoorCol = GetComponent<MeshCollider>();
        DoorCol.enabled = false;
        endingimg = GameObject.Find("PlayCanvas").transform.GetChild(6).GetComponent<Image>();
        endingText = endingimg.transform.GetChild(0).GetComponent<Text>();
        endingimg.enabled = false;
        endingText.enabled = false;
        PlayerUi = GameObject.Find("PlayerUi").gameObject;
        PlayCanvas = GameObject.Find("PlayCanvas").gameObject;
    }

    public void OpenCololider()
    {
        DoorCol.enabled = true;
    }
          
     
    IEnumerator EndingimgStart()
    {
        endingimg.enabled = true;
        
        float duration = 5f; 
        float elapsedTime = 0f; 
        Color startColor = endingimg.color; 
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); 
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; 
            endingimg.color = Color.Lerp(startColor, targetColor, elapsedTime / duration); 
            yield return null; 
        }
        endingimg.color = targetColor;


        endingText.enabled = true;
        for (int i = 0; i <= fulltext.Length; i++)
        {
            endingText.text = fulltext.Substring(0, i);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Lobby");
    }

    public void CatchItem()
    {
        GameManager.G_instance.isGameover = true;
        InGameUIManager.instance.ActivePlayerUI_Text(false);
        for (int i = 0; i < 5; i++)
        {
            PlayCanvas.transform.GetChild(i).gameObject.SetActive(false);
        }
        PlayerUi.gameObject.SetActive(false);
        InGameUIManager.instance.OnOption_S_Off();
        StartCoroutine(EndingimgStart());
        DoorCol.enabled = false;
    }

    public void Use()
    {
        //�κ��丮���� ����ϴ� �����۾ƴ� ���� x
    }

    public void ItemUIOn()
    {
        InGameUIManager.instance.SetPlayerUI_Text("Ż���ϱ� [G]");
    }
}
