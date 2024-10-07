using System.Collections.Generic;
using UnityEngine;

public class Pulling_Manger : MonoBehaviour
{
    public static Pulling_Manger instance; // Singleton 패턴을 위해 static으로 변경
    [SerializeField] CamerRay cameraRay;
    [SerializeField] GameObject spwaneDemon;
    [SerializeField] GameObject candlePrefad; // Prefab을 사용
    [SerializeField] List<Transform> Candlepos;
    [SerializeField] List<Transform> DemonSpwane;

    [Header("오브젝트 갯수")]
    int Maxpool = 6; // 최대 촛불 개수
    [SerializeField] List<GameObject> demonPool; // 데몬 풀
    private int counter = 0; // 현재 촛불 개수

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

        // CandlePosition에서 위치 가져오기
        var CandlePos = GameObject.Find("CandlePosition");
        if (CandlePos != null)
        {
            Candlepos.Clear(); // 기존 리스트 초기화

            // CandlePosition의 자식 트랜스폼을 Candlepos 리스트에 추가
            foreach (Transform child in CandlePos.transform)
            {
                Candlepos.Add(child);

                // 각 자식의 자식들을 가져오기
                foreach (Transform grandChild in child)
                {
                    DemonSpwane.Add(grandChild); // 또는 다른 리스트에 추가
                }
            }
        }

        InitializeDemonPool();
        CreateCandel();
    }

    void InitializeDemonPool()
    {
        demonPool = new List<GameObject>();
        CreateDemon(); // 데몬을 생성하고 리스트에 추가
      
    }

    void CreateDemon()
    {
        
        var demon = Instantiate(spwaneDemon);
        demon.SetActive(false); // 비활성화 상태로 생성
        demonPool.Add(demon); // 리스트에 추가
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

                // 가장 가까운 DemonSpwane 위치에 데몬 소환
                if (closestDemonSpawnPos != null)
                {
                    GameObject demon = GetInactiveDemon(); // 비활성화된 데몬 가져오기
                    if (demon != null)
                    {
                        demon.transform.position = closestDemonSpawnPos.position;
                        demon.transform.rotation = Quaternion.identity;
                        demon.SetActive(true); // 데몬 활성화
                    }
                }
            }
        }
    }

    GameObject GetInactiveDemon()
    {
        foreach (var demon in demonPool)
        {
            if (!demon.activeInHierarchy) // 비활성화된 오브젝트 찾기
            {
                return demon; // 비활성화된 데몬 반환
            }
        }
        return null; // 모든 데몬이 활성화 상태일 경우 null 반환
    }

    void CreateCandel() // 양초만 위치만 생성
    {
        GameObject candleGroup = new GameObject("CandleGroup");
        int count = Mathf.Min(Maxpool, Candlepos.Count); // 총 계수와 위치값배열을 등록
        for (int i = 0; i < count; i++)
        {
            var candleobj = Instantiate(candlePrefad, candleGroup.transform);
            candleobj.transform.position = Candlepos[i].position;
            candleobj.transform.rotation = Quaternion.identity;
            candleobj.name = $"Candle" + (i + 1); // 인덱스 + 1
            candleobj.SetActive(false); // 비활성화 상태로 생성
        }
    }

    public void SetActiveTrueCandel()
    {
        GameObject CandleGroupChild = GameObject.Find("CandleGroup");

        foreach (Transform child in CandleGroupChild.transform)
        {
            child.gameObject.SetActive(true); // 모든 촛불을 활성화
        }
    }
}
