using UnityEngine;
using UnityEngine.UI;

public class TalkText : MonoBehaviour
{
    [SerializeField] Text dialogueText;
    private int currentDialogueIndex = 0;
    public bool talkout = false;
    private string[] dialogues = {
        "�� ��ġ����",
        "�� �����̴� �Һ��� ����?",
        "�ϴ� �� ������ ������"
    };

    void Start()
    {
        dialogueText = GameObject.Find("PlayCanves").transform.GetChild(2).GetChild(0).GetComponent<Text>();
        dialogueText.text = dialogues[currentDialogueIndex];
    }

    void Update()
    {
        // ���콺 Ŭ�� �� ��� ��ȯ
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
