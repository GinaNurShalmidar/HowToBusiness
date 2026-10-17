using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairChanger : MonoBehaviour
{
    [System.Serializable]
    public class HairStyle
    {
        public string nama = "Rambut";
        public GameObject model;
    }

    public HairStyle[] styles;
    public int startIndex = 0;
    public bool randomStart = false;

    private int currentIndex = -1;

    private void Awake()
    {
        if (styles == null || styles.Length == 0) return;

        int first = randomStart ? Random.Range(0, styles.Length) : startIndex;
        ShowOnly(first);
    }

    public void ChangeRandomHair()
    {
        if (styles == null || styles.Length < 2) return;

        int next;
        do
        {
            next = Random.Range(0, styles.Length);

        } while(next == currentIndex);

        ShowOnly(next);
    }

    private void ShowOnly(int index)
    {
        for (int i = 0; i < styles.Length; i++)
        {
            if (styles[i].model != null)
                styles[i].model.SetActive(i == index);
        }
        currentIndex = index;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
