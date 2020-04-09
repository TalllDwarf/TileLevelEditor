using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeTileController : MonoBehaviour {

    Text valueText;
    Slider sideSlider;

    float value;

    MapEditor mapE;

	// Use this for initialization
	void Start () {
        mapE = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapEditor>();

		foreach(Text ui in GetComponentsInChildren<Text>())
        {
            if(ui.text == "1")
            {
                valueText = ui;
                break;
            }
        }

        sideSlider = GetComponentInChildren<Slider>();
        sideSlider.onValueChanged.AddListener(delegate { valueText.text = "" + sideSlider.value; value = sideSlider.value; mapE.SetBrushSize(); });

        valueText.text = sideSlider.value + "";
        value = sideSlider.value;
    }
	
	public float CurrentValue
    {
        get
        {
            return value;
        }
    }
}
