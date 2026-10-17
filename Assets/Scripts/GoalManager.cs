using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Satu goal (misal: ServicePoint, Customer, Uang, dst).
/// Diisi lewat Inspector di list "Goals" milik GoalManager.
/// </summary>
[Serializable]
public class Goal
{
    [Tooltip("Nama unik goal, dipakai saat memanggil TambahProgress. Contoh: ServicePoint")]
    public string id = "ServicePoint";

    public int progressAwal = 0;
    public int target = 4;

    [Header("Teks syarat (bisa diisi lebih dari 1 tempat)")]
    [Tooltip("Semua objek di sini akan disembunyikan otomatis saat goal selesai.")]
    public List<GameObject> textSyarat = new List<GameObject>();

    [Header("Teks progress (opsional)")]
    [Tooltip("Teks TMP yang menampilkan progress, misal 1/4. Boleh lebih dari 1.")]
    public List<TMP_Text> textProgress = new List<TMP_Text>();
    [Tooltip("{0} = progress sekarang, {1} = target")]
    public string formatProgress = "{0}/{1}";

    [Header("Event saat goal selesai (opsional)")]
    public UnityEvent onSelesai;

    [NonSerialized] public int progress;

    public bool Selesai => progress >= target;
}

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance { get; private set; }

    [Header("Daftar Goal (isi 4-5 atau lebih)")]
    public List<Goal> goals = new List<Goal>();

    [Header("UI")]
    public ScriptPanel scriptPanel;
    public GameObject panelBelumBisa;
    public float durasiPanelBelumBisa = 2.5f;

    
    public bool EmailTerbuka
    {
        get
        {
            if (goals.Count == 0) return false;
            foreach (Goal g in goals)
                if (!g.Selesai) return false;
            return true;
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        foreach (Goal g in goals)
        {
            g.progress = g.progressAwal;
            RefreshUI(g);
        }
    }

    
    public void TambahProgress(string id)
    {
        TambahProgress(id, 1);
    }

    public void TambahProgress(string id, int jumlah)
    {
        Goal g = CariGoal(id);
        if (g == null) return;

        bool sebelumnyaSelesai = g.Selesai;
        g.progress += jumlah;
        Debug.Log($"[GoalManager] {g.id}: {g.progress}/{g.target}");

        RefreshUI(g);

        if (!sebelumnyaSelesai && g.Selesai)
            g.onSelesai?.Invoke();
    }

   
    public void TambahServicePoint()
    {
        TambahProgress("ServicePoint");
    }

    public int GetProgress(string id)
    {
        Goal g = CariGoal(id);
        return g != null ? g.progress : 0;
    }

    public bool IsSelesai(string id)
    {
        Goal g = CariGoal(id);
        return g != null && g.Selesai;
    }

    public void BukaEmail()
    {
        if (EmailTerbuka)
        {
            if (panelBelumBisa != null) panelBelumBisa.SetActive(false);
            scriptPanel.EmailAwal();
        }
        else if (panelBelumBisa != null)
        {
            StopAllCoroutines();
            StartCoroutine(SembunyikanPanelOtomatis(durasiPanelBelumBisa));
        }
    }

   

    private Goal CariGoal(string id)
    {
        foreach (Goal g in goals)
            if (g.id == id) return g;

        Debug.LogWarning($"[GoalManager] Goal dengan id '{id}' tidak ditemukan. Cek penulisan id di list Goals.");
        return null;
    }

    private void RefreshUI(Goal g)
    {
        
        string isi = string.Format(g.formatProgress, g.progress, g.target);
        foreach (TMP_Text t in g.textProgress)
            if (t != null) t.text = isi;

        
        if (g.Selesai)
        {
            foreach (GameObject t in g.textSyarat)
                if (t != null) t.SetActive(false);
        }
    }

    private IEnumerator SembunyikanPanelOtomatis(float delay)
    {
        panelBelumBisa.SetActive(true);
        yield return new WaitForSeconds(delay);
        panelBelumBisa.SetActive(false);
    }
}