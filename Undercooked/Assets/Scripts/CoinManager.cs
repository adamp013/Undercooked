using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshPro textMesh;
    private float coins = 0f;

    void Start()
    {
        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
        }
        UpdateText();
    }

    public void AddCoins(float amount)
    {
        coins += amount;
        UpdateText();
    }

    private void UpdateText()
    {
        textMesh.text = $"{coins:0} coins";
    }
}
