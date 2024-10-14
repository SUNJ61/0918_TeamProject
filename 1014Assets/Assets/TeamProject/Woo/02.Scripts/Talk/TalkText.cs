using UnityEngine;
using UnityEngine.UI;

public class TalkText : MonoBehaviour
{
    [SerializeField] Text dialogueText;
    private int currentDialogueIndex = 0;
    public bool talkout = false;
    private string[] dialogues = {
        "아 납치당함",
        "저 깜박이는 불빛은 뭐지?",
        "일단 저 방으로 가보자"
    };

    void Start()
    {
        dialogueText = GameObject.Find("PlayCanves").transform.GetChild(2).GetChild(0).GetComponent<Text>();
        dialogueText.text = dialogues[currentDialogueIndex];
    }

    void Update()
    {
        // 마우스 클릭 시 대사 전환
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextDialogue();
        }
    }

    private void ShowNextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
        }
        else
        {
            gameObject.SetActive(false);
            talkout = true;
        }
    }
}
