using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MoveScene : MonoBehaviour
{

    public GameObject panelJualBeli;
    public GameObject panelJualMotor;
    public GameObject panelBelomBisa;
    public GameObject panelBerhasilJual;
    public GameObject panelTabungan;
    public float displayDuration = 3f;
    public GameObject panelLapak3;
    public GameObject panelLoadingLapak;
    public GameObject panelBeliLapak;
    public Button buttonTabungan;

    public AudioClip sfxKlik;
    [Range(0f, 1f)] public float volumeSfx = 1f;

    public float delayPindahScene = 0.25f;

    private AudioSource audioSource;
    private bool sedangPindahScene = false;
    private Coroutine hideCoroutine;
    // Start is called before the first frame update


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    void Start()
    {
       if (buttonTabungan != null)
        {
            buttonTabungan.interactable = false;
        }

       
    }

    public void PlayKlik()
    {
        if (sfxKlik == null || audioSource == null) return;
        audioSource.PlayOneShot(sfxKlik, volumeSfx);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PindahSceneDenganSuara(string namaScene)
    {
        if (sedangPindahScene) yield break; 
        sedangPindahScene = true;

        PlayKlik();
        yield return new WaitForSecondsRealtime(delayPindahScene);
        SceneManager.LoadScene(namaScene);
    }

    public void SceneBerencana(string sceneName)
    {
        StartCoroutine(PindahSceneDenganSuara("ScenePerencanaan"));
    }

    public void SceneUtama(string sceneName)
    {
        StartCoroutine(PindahSceneDenganSuara("SceneUtama"));
    }
    
    public void PanelJualBeli()
    {
        PlayKlik();
        panelJualBeli.SetActive(true);
    }
    public void ClosePanelJualBeli()
    {
        PlayKlik();
        panelJualBeli.SetActive(false);
    }

    public void JualMotorPanel()
    {
        PlayKlik();
        panelJualMotor.SetActive(true);

        if (buttonTabungan != null)
        {
            buttonTabungan.interactable = true;
        }
    }

    public void CloseJualMotorPanel()
    {
        PlayKlik();
        panelJualMotor.SetActive(false);
    }

    public void BerhasilJual()
    {
        panelBerhasilJual.SetActive(true);  
    }

    public void TabunganPanel() {
        PlayKlik();
        panelTabungan.SetActive(true);
    }

    public void InTabunganPanel()
    {
        PlayKlik();
        panelTabungan.SetActive(false);
    }
   

    public void BelomBisa()
    {
        PlayKlik();

        if (panelBelomBisa != null)
        {
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            hideCoroutine = StartCoroutine(ShowAndHideRoutine());


        }
    }

    public void CloseBerhasilBeliLapak()
    {
        StartCoroutine(PindahSceneDenganSuara("SceneUtama"));
    }


    private IEnumerator ShowAndHideRoutine()
    {
        panelBelomBisa.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        panelBelomBisa.SetActive(false);
        hideCoroutine = null;
    }

    public void pAnelLapak3()
    {
        PlayKlik();
        panelLapak3.SetActive(true);
    }

    public void pAnelLapak3Close()
    {
        PlayKlik();
        panelLoadingLapak.SetActive(false);
    }

    public void panelMasukIntro()
    {
        PlayKlik();
        panelLoadingLapak.SetActive(true);
    }

    public void BerhasilBeliLapak()
    {
        PlayKlik();
        panelBeliLapak.SetActive(true);
    }

}
