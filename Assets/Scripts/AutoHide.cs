using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoHide : MonoBehaviour
{
    public float autoHide = 3f;
    private Coroutine hideCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);

        }
        hideCoroutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(autoHide);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine (hideCoroutine);  
            hideCoroutine = null;
        }
    }
}
