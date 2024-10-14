using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;

    public delegate void LevelUpHandler(); //�� �к��� ������ �þ�� �� �߻��� �̺�Ʈ ���
    public event LevelUpHandler SpeedUp;

    public GameObject LastOffCandle;
    [SerializeField] Text Candle_text;
   
    private int candleCounter;
    public int CandleCounter
    {
        get { return candleCounter; }
        set
        {
            candleCounter += value;
            DifficultyLevelUp();
        }
    }
    public bool isGameover = false;
    void Awake()
    {
        if (G_instance == null)
            G_instance = this;
        else if (G_instance != this)
            Destroy(G_instance);
        DontDestroyOnLoad(G_instance);



        Candle_text = GameObject.Find("PlayerUi").transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>();
    }


    public void CanndleCounter(int counter)
    {
        CandleCounter = counter;
        Candle_text.text = $"{CandleCounter.ToString()}/6";
        print(CandleCounter);
    }

    private void DifficultyLevelUp()
    {
        switch(candleCounter) //�к��� ���� ������ ���� �̺�Ʈ �߻�
        {
            case 1:

                break;

            case 2:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);

                break;

            case 3:
                SpeedUp.Invoke(); //����� ���ǵ�� �̺�Ʈ

                break;


            case 4:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);

                break;


            case 5:
                SpeedUp.Invoke(); //����� ���ǵ�� �̺�Ʈ

                break;

            case 6:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);
                SpawnManager.instance.SetActiveBookHead_Final();

                break;

        }
    }
}
