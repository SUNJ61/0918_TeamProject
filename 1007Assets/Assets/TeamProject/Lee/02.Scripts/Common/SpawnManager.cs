using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager S_instance;

    private Transform Player_Tr;

    private Vector3 Respawn_Tr;
    private float Respawn_Dist;


    private void Awake()
    {
        if (S_instance == null)
            S_instance = this;
        else if (S_instance != this)
            Destroy(S_instance);
        DontDestroyOnLoad(S_instance);

        Player_Tr = GameObject.FindWithTag("Player").transform;
    }

    public void BookHeadRespawn(List<Transform> SpawnPoint, GameObject RespawnObj)
    {
        StartCoroutine(RespawnWait(10f, SpawnPoint, RespawnObj));
    }

    IEnumerator RespawnWait(float Delay, List<Transform> SpawnPoint, GameObject RespawnObj)
    {   
        yield return new WaitForSeconds(Delay);
        RespawnSetup(SpawnPoint, RespawnObj);
        RespawnObj.SetActive(true);
    }

    public void RespawnSetup(List<Transform> SpawnPoint, GameObject RespawnObj)
    {
        Respawn_Dist = (SpawnPoint[0].position - Player_Tr.position).magnitude;
        Respawn_Tr = SpawnPoint[0].position;
        foreach (Transform point in SpawnPoint)
        {
            float Dist = (point.position - Player_Tr.position).magnitude;
            if (Dist > Respawn_Dist)
            {
                Respawn_Tr = point.position;
            }
        }

        RespawnObj.transform.position = Respawn_Tr;
    }
}
