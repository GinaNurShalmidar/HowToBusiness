using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class KetikEffect : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI dialogText;
    public GameObject tombolLanjut;

    [TextArea(2, 5)]
    public string[] kalimat;

    public float delayPerHuruf = 0.04f;
    public bool mulaiOtomatis = true;

    private int index = 0;
    private bool sedangMengetik = false;
    private Coroutine routine;

    // Start is called before the first frame update
    void Start()
    {
        if (mulaiOtomatis) Mulai();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Mulai()
    {
        index = 0;
        panel.SetActive(true);
        TampilkanKalimat();
    }

    private void TampilkanKalimat()
    {
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
            dialogText.maxVisibleCharacters += i;
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
            return;
        }
        TampilkanKalimat();
    }
}
