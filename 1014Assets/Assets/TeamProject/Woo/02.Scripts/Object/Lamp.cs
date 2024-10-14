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
            light.enabled = !light.enabled; // 라이트 상태 반전
        }

        Invoke("ToggleLights", 0.5f); // 1초 후에 다시 호출
    }
}
