using System.Collections.Generic;
using UnityEngine;

public class Pulling_Manger : MonoBehaviour
{
    public static Pulling_Manger instance; // Singleton ������ ���� static���� ����
    [SerializeField] CamerRay cameraRay;
    [SerializeField] GameObject spwaneDemon;
    [SerializeField] GameObject candlePrefad; // Prefab�� ���
    [SerializeField] List<Transform> Candlepos;
    [SerializeField] List<Transform> DemonSpwane;

    [Header("������Ʈ ����")]
    int Maxpool = 6; // �ִ� �к� ����
    [SerializeField] List<GameObject> demonPool; // ���� Ǯ
    private int counter = 0; // ���� �к� ����

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        cameraRay = GameObject.Find("CameraPivot").GetComponent<CamerRay>();
        candlePrefad = Resources.Load<GameObject>("Candle") as GameObject;
        spwaneDemon = Resources.Load<GameObject>("Demon_M") as GameObject;

        // CandlePosition���� ��ġ ��������
        var CandlePos = GameObject.Find("CandlePosition");
        if (CandlePos != null)
        {
            Candlepos.Clear(); // ���� ����Ʈ �ʱ�ȭ

            // CandlePosition�� �ڽ� Ʈ�������� Candlepos ����Ʈ�� �߰�
            foreach (Transform child in CandlePos.transform)
            {
                Candlepos.Add(child);

                // �� �ڽ��� �ڽĵ��� ��������
                foreach (Transform grandChild in child)
                {
                    DemonSpwane.Add(grandChild); // �Ǵ� �ٸ� ����Ʈ�� �߰�
                }
            }
        }

        InitializeDemonPool();
        CreateCandel();
    }

    void InitializeDemonPool()
    {
        demonPool = new List<GameObject>();
        CreateDemon(); // ������ �����ϰ� ����Ʈ�� �߰�
      
    }

    void CreateDemon()
    {
        
        var demon = Instantiate(spwaneDemon);
        demon.SetActive(false); // ��Ȱ��ȭ ���·� ����
        demonPool.Add(demon); // ����Ʈ�� �߰�
    }

    public void SetActiveDemonTrue(Transform candlepos)
    {
        if (GameManager.G_instance.CandleCounter == 2)
        {
            if (candlepos != null)
            {
                float closestDemonSpawnDistance = Mathf.Infinity;
                Transform closestDemonSpawnPos = null;

                foreach (var spawnPoint in DemonSpwane)
                {
                    float distance = Vector3.Distance(candlepos.position, spawnPoint.position);
                    if (distance < closestDemonSpawnDistance)
                    {
                        closestDemonSpawnDistance = distance;
                        closestDemonSpawnPos = spawnPoint;
                    }
                }

                // ���� ����� DemonSpwane ��ġ�� ���� ��ȯ
                if (closestDemonSpawnPos != null)
                {
                    GameObject demon = GetInactiveDemon(); // ��Ȱ��ȭ�� ���� ��������
                    if (demon != null)
                    {
                        demon.transform.position = closestDemonSpawnPos.position;
                        demon.transform.rotation = Quaternion.identity;
                        demon.SetActive(true); // ���� Ȱ��ȭ
                    }
                }
            }
        }
    }

    GameObject GetInactiveDemon()
    {
        foreach (var demon in demonPool)
        {
            if (!demon.activeInHierarchy) // ��Ȱ��ȭ�� ������Ʈ ã��
            {
                return demon; // ��Ȱ��ȭ�� ���� ��ȯ
            }
        }
        return null; // ��� ������ Ȱ��ȭ ������ ��� null ��ȯ
    }

    void CreateCandel() // ���ʸ� ��ġ�� ����
    {
        GameObject candleGroup = new GameObject("CandleGroup");
        int count = Mathf.Min(Maxpool, Candlepos.Count); // �� ����� ��ġ���迭�� ���
        for (int i = 0; i < count; i++)
        {
            var candleobj = Instantiate(candlePrefad, candleGroup.transform);
            candleobj.transform.position = Candlepos[i].position;
            candleobj.transform.rotation = Quaternion.identity;
            candleobj.name = $"Candle" + (i + 1); // �ε��� + 1
            candleobj.SetActive(false); // ��Ȱ��ȭ ���·� ����
        }
    }

    public void SetActiveTrueCandel()
    {
        GameObject CandleGroupChild = GameObject.Find("CandleGroup");

        foreach (Transform child in CandleGroupChild.transform)
        {
            child.gameObject.SetActive(true); // ��� �к��� Ȱ��ȭ
        }
    }
}
