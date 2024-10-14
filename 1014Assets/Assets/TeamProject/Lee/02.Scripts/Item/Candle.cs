using UnityEngine;

public class Candle : MonoBehaviour,IItem
{
    SpriteRenderer[] CandleFire;
    Collider Candle_Collider;

    private void Awake()
    {
        CandleFire = GetComponentsInChildren<SpriteRenderer>();
        Candle_Collider = GetComponent<Collider>();
    }

    public void CatchItem()
    {
        foreach (var candle in CandleFire)
            candle.enabled = false;

        Candle_Collider.enabled = false;

        GameManager.G_instance.LastOffCandle = gameObject;
        GameManager.G_instance.CanndleCounter(1);
    }

    public void Use() { /* Candle은 인벤토리에 들어가지 않으므로 선언만 해둠. */ }
    public void ItemUIOn()
    {
        InGameUIManager.instance.OnPlayerUI_Text("양초끄기 [G]");
    }
}
