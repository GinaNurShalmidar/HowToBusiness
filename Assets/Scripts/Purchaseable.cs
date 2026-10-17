using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Purchaseable : MonoBehaviour
{
    //nested class keknya
    [System.Serializable]
    public class ServicePointUpgrade
    {
        public string name = "Kasir baru";
        public int price = 50;
        public GameObject servicePointObject;
        public Waypoint waypointScript;
        public float newMaxSpawnTime = 5f;
        public float newMinSpawnTime = 3f;
    }
    //sampe sini

   public List<ServicePointUpgrade> upgradesList = new List<ServicePointUpgrade>();
    //variabel
    public Waypoint distributionWaypoint;
    public Waypoint exitWaypoint;

    public TextMeshProUGUI priceText;
    public Button buyButton;

    public WaypointSpawner spawner;

    private int currentUpgradeIndex = 0;

    
    //sampe sini
    private void Start()
    {
        UpdateUI();
    }

    public void BuyNextUpgrade()
    {
        if (currentUpgradeIndex >= upgradesList.Count) return; //pengaman biar ga lewat dari list nya

        ServicePointUpgrade currentUpgrade = upgradesList[currentUpgradeIndex]; //ngambil 1 indeks berurutan buat ngejualnya
        if (MoneyManager.Instance != null && MoneyManager.Instance.TrySpendMoney(currentUpgrade.price)) //manggil try spend money
        {
            if (currentUpgrade.servicePointObject != null)
                currentUpgrade.servicePointObject.SetActive(true);

            if (currentUpgrade.waypointScript != null && exitWaypoint != null)
                currentUpgrade.waypointScript.next = exitWaypoint;

            if (distributionWaypoint != null && currentUpgrade.waypointScript != null)
            {
                if (!distributionWaypoint.nextWaypoints.Contains(currentUpgrade.waypointScript))
                    distributionWaypoint.nextWaypoints.Add(currentUpgrade.waypointScript);
            }

            if (spawner != null)
            {
                spawner.minSpawn = currentUpgrade.newMinSpawnTime;
                spawner.maxSpawn = currentUpgrade.newMaxSpawnTime;
                Debug.Log($"Nambah banyak : {spawner.minSpawn} - {spawner.maxSpawn}");
                spawner.StartSpawning();
            }

            if (GoalManager.Instance != null)
            {
                GoalManager.Instance.TambahServicePoint();
            }

            Debug.Log($"[Purchaseable] Berhasil buka {currentUpgrade.name}!");

            currentUpgradeIndex++;   
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (currentUpgradeIndex < upgradesList.Count)
        {
            ServicePointUpgrade nextUpgrade = upgradesList[currentUpgradeIndex];
            if (priceText != null)
            {
                priceText.text = $" (${nextUpgrade.price})";
            }
        }
        else
        {
            if (priceText != null) priceText.text = "MAXED";
            if (buyButton != null) buyButton.interactable = false;
            gameObject.SetActive(false);
        }
    }

   
}
