using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MoneyManager : MonoBehaviour
{
   public static MoneyManager Instance { get; private set; }

    public int StartingMoney = 50;
    public int currentMoney;

    public TextMeshProUGUI[] moneyText;

    public AudioClip sfxDapatUang;
    [Range(0f, 1f)] public float volumeSfx = 1f;

    private AudioSource audioSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        currentMoney = StartingMoney;
        UpdateMoneyUI();
    }

    public bool TrySpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log($"[MoneyManager] -${amount} digunakan! Sisa uang: ${currentMoney}");
            
            UpdateMoneyUI();
            return true;
        }

        Debug.LogWarning("[MoneyManager] uang tidak cukup!");
        return false;
    }

    public void AddMoney(int amount) 
    
    { 
    currentMoney += amount;
        Debug.Log($"[MoneyManager] +${amount} didapatkan! Total uang: ${currentMoney}");
        UpdateMoneyUI();
        PutarSfx(sfxDapatUang);
    }

    private void PutarSfx(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        
        audioSource.PlayOneShot(clip, volumeSfx);
    }
    public void UpdateMoneyUI()
    {
        if (moneyText != null && moneyText.Length > 0)
        {
            foreach (TextMeshProUGUI text in moneyText)
            {
                if (text != null)
                {
                    text.text = "$ " + currentMoney;
                }
            }
        }
        else
        {
            Debug.LogWarning("[MoneyManager] Slot 'Money Text' masih kosong");
        }

    }

    
    
}
