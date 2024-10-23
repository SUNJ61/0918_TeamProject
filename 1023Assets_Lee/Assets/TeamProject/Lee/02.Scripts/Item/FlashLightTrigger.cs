using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> Mobs = new List<GameObject>();

    public void OffFlashlight() //플래쉬 라이트 끌 때 마다 호출
    {
        foreach (GameObject obj in Mobs)
            obj.SendMessage("OffFlashLight", SendMessageOptions.DontRequireReceiver);

        Mobs.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon") && !Mobs.Contains(other.gameObject)) //태그가 데몬이고 리스트에 존재하지 않는 오브젝트라면
            Mobs.Add(other.gameObject); // 리스트에 오브젝트 추가
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Demon") && Mobs.Contains(other.gameObject))
            Mobs.Remove(other.gameObject); // 리스트에서 오브젝트 제거
    }
}
