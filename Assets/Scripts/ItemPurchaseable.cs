using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemPurchaseable : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string nama = "Dekorasi";
        public int price = 30;
        public GameObject[] objekList;
        public Button tombol;
        [HideInInspector] public int jumlahDibeli;
        public TextMeshProUGUI priceText;
    }

    public List<ShopItem> items = new List<ShopItem>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in items)
        {
            foreach (var o in item.objekList)
            
                if (o != null) o.SetActive(false);
                Refresh(item);
            
        }
    }

    public void Beli(int index)
    {
        if (index <0 || index >= items.Count) return;

        ShopItem item = items[index];
        if (item.jumlahDibeli >= item.objekList.Length) return;

        if (MoneyManager.Instance != null && MoneyManager.Instance.TrySpendMoney(item.price))
        {
            GameObject target = item.objekList[item.jumlahDibeli];
            if (target != null) target.SetActive(true);

            item.jumlahDibeli++;

            if (item.jumlahDibeli >= item.objekList.Length && item.tombol != null)
                item.tombol.interactable = false;

            Refresh(item);
        }
    }

    private void Refresh(ShopItem item)
    {
        if (item.priceText == null) return;

        if (item.jumlahDibeli >= item.objekList.Length)
            item.priceText.text = "Habis";
        else
            item.priceText.text = $"{item.nama} (${item.price}) {item.jumlahDibeli}/{item.objekList.Length}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
