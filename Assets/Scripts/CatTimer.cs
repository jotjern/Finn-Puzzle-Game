using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatTimer : MonoBehaviour {

    public CatController cat;
    public TMPro.TextMeshProUGUI timer;

    private float timeleft = 3;
    private bool disabled = false;

    // Use this for initialization
    void Start () {
        cat.active = false;
        disabled = false;
        timeleft = 3;
        timer.text = string.Format(".2f", timeleft);
    }
    
    // Update is called once per frame
    void Update () {
        timeleft -= Time.deltaTime;
        
        if (timeleft < 0f && !disabled)
        {
            disabled = true;
            cat.active = true;
        }
        if (timeleft < 0f)
        {
            timer.text = string.Format("{0:0.00}", -timeleft);
        } else
        {
            timer.text = string.Format("{0:0.00}", timeleft);
        }
    }
}
