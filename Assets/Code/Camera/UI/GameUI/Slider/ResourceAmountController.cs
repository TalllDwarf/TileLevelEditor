using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceAmountController : MonoBehaviour {

    public Text sizeText;
    public Slider sizeSlider;

    public Text maxText;
    public Slider maxSlider;

    public Toggle regenToggle;

    public Text regenSpeedText;
    public Slider regenSpeedSlider;

    public Text regenAmountTet;
    public Slider regenAmountSlider;

    private MapEditor mapE;

	// Use this for initialization
	void Start () {
        mapE = GameObject.FindGameObjectWithTag(Tags.MapController).GetComponent<MapEditor>();

        sizeSlider.onValueChanged.AddListener(delegate { sizeText.text = sizeSlider.value + ""; mapE.SetBrushSize(); });
        maxSlider.onValueChanged.AddListener(delegate { maxText.text = maxSlider.value + ""; });
        regenSpeedSlider.onValueChanged.AddListener(delegate { regenSpeedText.text = regenSpeedSlider.value + ""; });
        regenAmountSlider.onValueChanged.AddListener(delegate { regenAmountTet.text = regenAmountSlider.value + ""; });
    }
	
	
}
