using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] Light[] AllLight;

    void Start()
    {
        AllLight = gameObject.GetComponentsInChildren<Light>();
        ToggleLights();
    }

    void ToggleLights()
    {
        foreach (var light in AllLight)
        {
            light.enabled = !light.enabled; // ����Ʈ ���� ����
        }

        Invoke("ToggleLights", 0.5f); // 1�� �Ŀ� �ٽ� ȣ��
    }
}
