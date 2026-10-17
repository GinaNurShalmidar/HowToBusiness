using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DayManager : MonoBehaviour
{
    public static DayManager instance {  get; private set; }

    public int currentDay = 1;
    public float startHour = 6f;
    public float endHour = 22f;
    public float secondsPerGameHour = 30f;
    public bool isPaused = false;

    public TextMeshProUGUI[] dayText;
    public TextMeshProUGUI[] clockText;

    public GameObject panelGantiHari;
    public TextMeshProUGUI textHariSelesai;


    public float CurrentHour {  get; private set; }
    public static event Action<int> OnDayChanged;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentHour = startHour;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || EffectKetik.WaktuBerhenti) return;
        CurrentHour += Time.deltaTime / secondsPerGameHour;


        if (CurrentHour >= endHour)
        {
            NextDay();
            return;
        }
        UpdateUI();
        
    }

    public void NextDay()
    {
        currentDay++;
        CurrentHour = startHour;
        Debug.Log($"[DayManager] Hari ke-{currentDay} dimulai");

        OnDayChanged?.Invoke(currentDay );
        UpdateUI();

        TampilkanUIGantiHari();
    }

    private void TampilkanUIGantiHari()
    {
        isPaused = true;

        if (textHariSelesai != null) {
            textHariSelesai.text = $"Hari {currentDay - 1} Selesai!";
        }

        if (panelGantiHari != null) { 
            panelGantiHari.SetActive(true);
        }
    }

    public void LanjutHari()
    {
        if (panelGantiHari != null)
        {
            panelGantiHari.SetActive(false);
        }

        isPaused = false;
    }

    public void UpdateUI()
    {
        int hour = Mathf.FloorToInt(CurrentHour);
        int minute = Mathf.FloorToInt((CurrentHour - hour) * 60f);

        foreach (var t in dayText)
        {
            if (t != null) t.text = "Hari " + currentDay;
        }

        foreach (var t in clockText)
        {
            if (t != null) t.text = $"{hour:00}:{minute:00}";

        }

    }
}
