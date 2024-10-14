using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private List<Transform> BookHead_SpawnPoint;
    [SerializeField] private List<Transform> CandlePos;
    [SerializeField] private List<Transform> ItemPos;
    [SerializeField] private List<Transform> Gunpos;
    [SerializeField] private List<int> CandleRandomIdx;
    [SerializeField] private List<int> ItemRandomIdx;


    private Transform Player_Tr;

    private Vector3 Fin_BookHeadSpawnPos = Vector3.zero;

    private readonly string RespawnObj = "Respawn";
    private readonly string CandlePosObj = "CandlePosition";
    private readonly string ItemPosObj = "ItemCreatePos";
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
        DontDestroyOnLoad(instance);

        Player_Tr = GameObject.FindWithTag("Player").transform;

        ListSetting();
    }

    private void ListSetting()
    {
        GetPoint(BookHead_SpawnPoint, RespawnObj);
        GetPoint(CandlePos, CandlePosObj);
        GetPoint(ItemPos, ItemPosObj);

        for(int i = 0; i < ItemPos.Count; i++)
            ItemRandomIdx.Add(i);
        for(int i = 0; i < CandlePos.Count; i++)
            CandleRandomIdx.Add(i);
    }
    private void GetPoint(List<Transform> PosList, string GetPosName) //Pos����Ʈ ��������.
    {
        var spawn = GameObject.Find(GetPosName).transform;
        if (spawn != null)
        {
            foreach (Transform pos in spawn)
                PosList.Add(pos);
        }
    }

    public void FarRespawnSetup(List<Transform> SpawnPoint, GameObject RespawnObj, Transform Standard) //�� ������ ��ġ ã�� ����
    {
        float Respawn_Dist = (SpawnPoint[0].position - Standard.position).magnitude;
        Vector3 Respawn_Pos = SpawnPoint[0].position;
        foreach (Transform point in SpawnPoint)
        {
            float Dist = (point.position - Standard.position).magnitude;
            if (Dist > Respawn_Dist)
                Respawn_Pos = point.position;
        }

        RespawnObj.transform.position = Respawn_Pos;
    }

    public void SetActiveDemonTrue(GameObject candlepos) //�к��� ������ ���� �Լ� ȣ��
    {
        if (candlepos != null)
        {
            GameObject demon = Pulling_Manger.instance.GetObject(0);
            Transform pos = candlepos.transform.parent.GetChild(0).transform;
            demon.transform.position = pos.position;
            demon.SetActive(true);
        }
    }

    public void SetActiveBookHead_Final() //�к� 6�� ������ ȣ��
    {
        GameObject bookhead = Pulling_Manger.instance.GetObject(1);
        bookhead.transform.position = Fin_BookHeadSpawnPos;
        bookhead.SetActive(true);
    }
    public void SetActiveBookHead() //�̼ǽ��ۿ� ȣ��
    {
        GameObject bookhead = Pulling_Manger.instance.GetObject(1);
        if (bookhead != null)
            StartCoroutine(RespawnWait(10f, BookHead_SpawnPoint, bookhead, Player_Tr));
    }

    public void BookHeadRespawn(GameObject RespawnObj) //����尡 �÷��̾�� �׾��� �� ȣ��
    {
        StartCoroutine(RespawnWait(10f, BookHead_SpawnPoint, RespawnObj, Player_Tr));
    }

    IEnumerator RespawnWait(float Delay, List<Transform> SpawnPoint, GameObject RespawnObj, Transform Standard)
    {
        yield return new WaitForSeconds(Delay);
        FarRespawnSetup(SpawnPoint, RespawnObj, Standard);
        RespawnObj.SetActive(true);
    }

    public void SetActiveTrueCandel() //�̼ǽ��۽� ȣ��
    {
        
        foreach (GameObject candle in Pulling_Manger.instance.Data[2].Pool_List)
        {
            int idx = GetRandomIdx(CandleRandomIdx);
            candle.transform.parent = CandlePos[idx];
            candle.transform.position = CandlePos[idx].position;
            candle.SetActive(true);
        }

        GameObject candleGroup = GameObject.Find(Pulling_Manger.instance.Data[2].GroupName);
        Destroy(candleGroup);
    }
    public void SetActiveTrueItem()
    {
        foreach (GameObject item in Pulling_Manger.instance.Data[3].Pool_List) //������ ����Ʈ�� 3 ~ 7�� ���� ���� 
        {
            int idx = GetRandomIdx(ItemRandomIdx);
            item.transform.parent = ItemPos[idx];
            item.transform.position = ItemPos[idx].position;
            item.SetActive(true);
        }
        for (int i = 3; i < 7; i++)
        {
            GameObject itemGroup = GameObject.Find(Pulling_Manger.instance.Data[i].GroupName);
            Destroy(itemGroup);
        }
    }
    private int GetRandomIdx(List<int> RandomIdx) // 0~29 ������ ������ 15�� �̱�.
    {
        int i = Random.Range(0, RandomIdx.Count);
        int idx = RandomIdx[i];
        RandomIdx.RemoveAt(i);
        return idx;
    }
}
