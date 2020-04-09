using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetup : MonoBehaviour {

    Slider connectedSlider;
    InputField connectedField;
    SliderController controller;

	//Add Listerners to connected slider and field
	void Start () {

        connectedSlider = GetComponent<Slider>();
        connectedField = GetComponentInChildren<InputField>();
        controller = GetComponentInParent<SliderController>();

        SliderID id;

		switch(tag)
        {
            case Tags.xSlider:
                id = SliderID.X;
                break;

            case Tags.ySlider:
                id = SliderID.Y;
                break;

            case Tags.spawnSlider:
                id = SliderID.Spawn;
                break;

            case Tags.waterSlider:
                id = SliderID.Water;
                break;

            case Tags.waterSizeSlider:
                id = SliderID.WaterSize;
                break;

            case Tags.treeSlider:
                id = SliderID.Tree;
                break;

            case Tags.treeSizeSlider:
                id = SliderID.TreeSize;
                break;

            case Tags.rockSlider:
                id = SliderID.Rock;
                break;

            case Tags.rockSizeSlider:
                id = SliderID.RockSize;
                break;

            case Tags.ironSlider:
                id = SliderID.Iron;
                break;

            case Tags.ironSizeSlider:
                id = SliderID.IronSize;
                break;

            default:
                return;
        }

        connectedSlider.onValueChanged.AddListener(delegate { controller.SliderValueChanged(connectedSlider, id); });
        connectedField.onValueChanged.AddListener(delegate { controller.FieldValueChanged(connectedField, id); });

        controller.SliderValueChanged(connectedSlider, id);
    }
}
