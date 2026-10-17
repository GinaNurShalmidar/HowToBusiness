using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptPanel : MonoBehaviour
{
    public GameObject hpPanel;
    public GameObject PanelTabungan;
    public GameObject JualBeliBarang;
    public GameObject GoalsPanel;
    public GameObject panelEmail;
    public GameObject panelEmailSalah;
    public GameObject panelEmailBenar;
    public GameObject panelEmailTemplate;
    public GameObject panelLoading;
    public GameObject panelBelomBisa;
    private Coroutine hideCoroutine;
    public float displayDuration = 1f;
    public GameObject QuestPanel;
    public Slider loadingSlider;
    public float duration = 1.5f;
    private Coroutine routine;
    public TextMeshProUGUI persenText;


    public void LoadThenOpen(GameObject targetPanel)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(LoadRoutine(targetPanel)); 
    }

    private IEnumerator LoadRoutine(GameObject targetPanel)
    {
        panelLoading.SetActive(true);
        loadingSlider.value = 0f;

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            loadingSlider.value = Mathf.Clamp01(timer/duration);

            if (persenText != null)
            {
                persenText.text = Mathf.RoundToInt(loadingSlider.value * 100f) + "%";
            }

            yield return null;
        }

        panelLoading.SetActive(false);
        if (targetPanel != null) targetPanel.SetActive(true);
        routine = null;
    }
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BelomBisa()
    {
        if (panelBelomBisa != null)
        {
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            hideCoroutine = StartCoroutine(ShowAndHideRoutine());
        }


    }

    private IEnumerator ShowAndHideRoutine()
    {
        panelBelomBisa.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        panelBelomBisa.SetActive(false);
        hideCoroutine = null;
    }

    public void OpenHp()
    {
        hpPanel.SetActive(true);
    }

    public void closeHp()
    {
        hpPanel.SetActive(false);
    }
    
    public void TabunganOpen()
    {
        if (PanelTabungan != null)
        {
            PanelTabungan.SetActive(true);

            if (MoneyManager.Instance !=null)
            {
                MoneyManager.Instance.UpdateMoneyUI();
            }
        }

    }

    public void TabunganClose()
    {
        PanelTabungan.SetActive(false);
    }

    public void OpenJualBeli()
    {
        JualBeliBarang.SetActive(true);

        if (MoneyManager.Instance !=null)
        {
            MoneyManager.Instance.UpdateMoneyUI();
        }

    }

    public void CloseJualBeli()
    {
        JualBeliBarang.SetActive(false);
    }

    public void EmailAwal()
    {
        panelEmail.SetActive(true);
    }

    public void emailSalah()
    {
        panelEmailSalah.SetActive(true);
    }

    public void emailBenar()
    {
        panelEmailBenar.SetActive(true);
    }

    public void TemplateEmail()
    {
        panelEmailTemplate.SetActive(true);
    }

    public void PanelGoals()
    {
        GoalsPanel.SetActive(true);
    }

    public void CloseGoals()
    {
        GoalsPanel.SetActive(false);
    }

    public void openQuest()
    {
        QuestPanel.SetActive(true);
    }

    public void CloseQuest()
    {
        QuestPanel.SetActive(false );
    }
}
