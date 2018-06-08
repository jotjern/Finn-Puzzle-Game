using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngameUI : MonoBehaviour {

    public TilemapManager tmm;
    public TextMeshProUGUI timerText;
    public CatTimer catTimer;
    public static bool timerOn = false;

    public TMPro.TextMeshProUGUI text;
    public Toggle enableTimer;

    void Start () {
        enableTimer.isOn = timerOn;
    }
	
	// Update is called once per frame
	void Update () {
        text.text = tmm.GetTilemap().GetPressedRedButtonCount() + "/" + tmm.GetTilemap().buttonsToWin;
        catTimer.enabled = enableTimer.isOn;
        timerOn = enableTimer.isOn;
        timerText.gameObject.SetActive(enableTimer.isOn);
	}
}
