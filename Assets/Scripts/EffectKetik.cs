using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EffectKetik : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI dialogText;
    public GameObject tombolLanjut;

    [TextArea(2, 5)]
    public string[] kalimat;

    public float delayPerHuruf = 0.04f;
    public bool mulaiOtomatis = true;

    [Header("Kontrol Waktu (jam & hari)")]
    [Tooltip("Kalau aktif, jam & hari berhenti selama dialog, dan lanjut saat kalimat TERAKHIR muncul.")]
    public bool bekukanWaktuSebelumKalimatTerakhir = true;
    public UnityEvent onWaktuBerhenti;
    public UnityEvent onWaktuLanjut;

    
    public static bool WaktuBerhenti { get; private set; }

    private int index = 0;
    private bool sedangMengetik = false;
    private bool menahanWaktu = false;
    private Coroutine routine;

    void Start()
    {
        if (mulaiOtomatis) Mulai();
    }

    private void OnDisable()
    {
       
        LepasWaktu();
    }

    public void Mulai()
    {
        if (kalimat == null || kalimat.Length == 0)
        {
            Debug.LogWarning("[EffectKetik] Array kalimat kosong.");
            return;
        }

        index = 0;
        panel.SetActive(true);
        TampilkanKalimat();
    }

    private void TampilkanKalimat()
    {
        AturWaktu();

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Ketik(kalimat[index]));
    }

    private IEnumerator Ketik(string isi)
    {
        sedangMengetik = true;
        if (tombolLanjut != null) tombolLanjut.SetActive(false);

        dialogText.text = isi;
        dialogText.maxVisibleCharacters = 0;
        dialogText.ForceMeshUpdate();

        int total = dialogText.textInfo.characterCount;

        for (int i = 0; i < total; i++)
        {
            dialogText.maxVisibleCharacters = i + 1; 
            yield return new WaitForSecondsRealtime(delayPerHuruf);
        }

        sedangMengetik = false;
        if (tombolLanjut != null) tombolLanjut.SetActive(true);
        routine = null;
    }

    public void Lanjut()
    {
        if (sedangMengetik)
        {
            if (routine != null) StopCoroutine(routine);
            dialogText.maxVisibleCharacters = int.MaxValue;
            sedangMengetik = false;
            routine = null;
            if (tombolLanjut != null) tombolLanjut.SetActive(true);
            return;
        }

        index++;
        if (index >= kalimat.Length)
        {
            panel.SetActive(false);
            LepasWaktu();
            return;
        }
        TampilkanKalimat();
    }

  
    private void AturWaktu()
    {
        if (!bekukanWaktuSebelumKalimatTerakhir)
        {
            LepasWaktu();
            return;
        }

        bool kalimatTerakhir = index >= kalimat.Length - 1;
        if (kalimatTerakhir) LepasWaktu();
        else TahanWaktu();
    }

    private void TahanWaktu()
    {
        if (menahanWaktu) return;
        menahanWaktu = true;
        WaktuBerhenti = true;
        onWaktuBerhenti?.Invoke();
    }

    private void LepasWaktu()
    {
        if (!menahanWaktu) return;
        menahanWaktu = false;
        WaktuBerhenti = false;
        onWaktuLanjut?.Invoke();
    }
}