using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class PLayerWallet : MonoBehaviour
{
     private Dictionary<CurrencyItemSO, int> currencyInventory = new Dictionary<CurrencyItemSO, int>();

    public void AddCurrency(CurrencyItemSO currency, int amount)
    {
        if (currencyInventory.ContainsKey(currency))
        {
            currencyInventory[currency] += amount;
        }
        else
        {
            currencyInventory[currency] = amount;
        }

        Debug.Log("Added " + amount + " " + currency.Name);
    }

    public bool SpendCurrency(CurrencyItemSO currency, int amount)
    {
        if (currencyInventory.ContainsKey(currency) && currencyInventory[currency] >= amount)
        {
            currencyInventory[currency] -= amount;
            Debug.Log("Spent " + amount + " " + currency.Name);
            return true;
        }
        Debug.Log("Not enough currency!");
        return false;
    }

    public int GetCurrencyAmount(CurrencyItemSO currency)
    {
        return currencyInventory.ContainsKey(currency) ? currencyInventory[currency] : 0;
    }
}
