using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model{
    [CreateAssetMenu]
    public class CurrencyItemSO : ItemSO
    {
         [field: SerializeField]
        public int Value { get; private set; } // The currency value

        public string ActionName => "Exchange";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformExchange(GameObject character, List<ItemParameter> itemState = null)
        {
            PLayerWallet wallet = character.GetComponent<PLayerWallet>();
            if (wallet != null)
            {
                wallet.AddCurrency(this, Value);
                return true;
            }
            return false;
        }
        }
}