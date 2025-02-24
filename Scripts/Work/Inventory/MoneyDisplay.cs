using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MoneyDisplay : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnCopperCoinsUpdated))]
    private int copperCoins = 0;

    [SyncVar(hook = nameof(OnSilverCoinsUpdated))]
    private int silverCoins = 0;

    [SyncVar(hook = nameof(OnGoldCoinsUpdated))]
    private int goldCoins = 0;

    // UI елементи
    public Text copperCoinsText;
    public Text silverCoinsText;
    public Text goldCoinsText;

    void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false); // Вимикаємо UI для інших гравців
            return;
        }

        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        copperCoinsText.text = copperCoins.ToString();
        silverCoinsText.text = silverCoins.ToString();
        goldCoinsText.text = goldCoins.ToString();
    }

    public void AddCoins(string coinType, int amount)
    {
        if (isLocalPlayer)
        {
            CmdAddCoins(coinType, amount);
        }
    }

    public bool DeductCoins(int copperCost, int silverCost, int goldCost)
    {
        if (isLocalPlayer)
        {
            CmdDeductCoins(copperCost, silverCost, goldCost);
            return true;
        }
        return false;
    }

    private void OnCopperCoinsUpdated(int oldValue, int newValue) => copperCoinsText.text = newValue.ToString();
    private void OnSilverCoinsUpdated(int oldValue, int newValue) => silverCoinsText.text = newValue.ToString();
    private void OnGoldCoinsUpdated(int oldValue, int newValue) => goldCoinsText.text = newValue.ToString();

    [Command]
    public void CmdAddCoins(string coinType, int amount)
    {
        if (!isServer) return;

        switch (coinType)
        {
            case "Copper":
                copperCoins += amount;
                break;
            case "Silver":
                silverCoins += amount;
                break;
            case "Gold":
                goldCoins += amount;
                break;
            default:
                Debug.LogWarning($"Невідомий тип монет: {coinType}");
                return;
        }
    }

    [Command]
    public void CmdDeductCoins(int copperCost, int silverCost, int goldCost)
    {
        int totalCopper = copperCoins + silverCoins * 100 + goldCoins * 10000;
        int totalCost = copperCost + silverCost * 100 + goldCost * 10000;

        if (totalCopper >= totalCost)
        {
            totalCopper -= totalCost;

            goldCoins = totalCopper / 10000;
            totalCopper %= 10000;
            silverCoins = totalCopper / 100;
            copperCoins = totalCopper % 100;
        }
    }

    public int GetTotalMoney()
    {
        return copperCoins + silverCoins * 100 + goldCoins * 10000;
    }
}
