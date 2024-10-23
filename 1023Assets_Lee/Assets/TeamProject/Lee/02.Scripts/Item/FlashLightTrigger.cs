using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightTrigger : MonoBehaviour
{
    [SerializeField] List<GameObject> Mobs = new List<GameObject>();

    public void OffFlashlight() //�÷��� ����Ʈ �� �� ���� ȣ��
    {
        foreach (GameObject obj in Mobs)
            obj.SendMessage("OffFlashLight", SendMessageOptions.DontRequireReceiver);

        Mobs.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Demon") && !Mobs.Contains(other.gameObject)) //�±װ� �����̰� ����Ʈ�� �������� �ʴ� ������Ʈ���
            Mobs.Add(other.gameObject); // ����Ʈ�� ������Ʈ �߰�
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Demon") && Mobs.Contains(other.gameObject))
            Mobs.Remove(other.gameObject); // ����Ʈ���� ������Ʈ ����
    }
}
