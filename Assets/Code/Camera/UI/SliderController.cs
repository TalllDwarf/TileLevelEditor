using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SliderID
{
    X,
    Y,
    Spawn,
    Tree,
    TreeSize,
    Water,
    WaterSize,
    Rock,
    RockSize,
    Iron,
    IronSize
}

public class SliderController : MonoBehaviour
{

    //Collection of all the sliders and input field for the map generator
    Slider xSlider;
    Slider ySlider;

    InputField xField;
    InputField yField;

    Slider treeSlider;
    InputField treeField;

    Slider treeSizeSlider;
    InputField treeSizeField;

    Slider waterSlider;
    InputField waterField;

    Slider waterSizeSlider;
    InputField waterSizeField;

    Slider rockSlider;
    InputField rockField;

    Slider rockSizeSlider;
    InputField rockSizeField;

    Slider ironSlider;
    InputField ironField;

    Slider ironSizeSlider;
    InputField ironSizeField;

    Slider spawnSlider;
    InputField spawnField;

    int oldX = 0, oldY = 0;

    int currentX = 0, currentY = 0, amountOfTrees = 0, treeSpawnSize = 0, amountOfWater = 0, waterSpawnSize = 0;
    int amountOfRock = 0, rockSpawnSize = 0, amountOfIron = 0, ironSpawnSize = 0, spawnSize = 0;

    public SpawnObject GetCurrentSpawnObject()
    {
        return new SpawnObject(currentX, currentY, spawnSize, amountOfWater, waterSpawnSize, amountOfTrees, treeSpawnSize, amountOfRock, rockSpawnSize, amountOfIron, ironSpawnSize);
    }

    public int CurrentX
    {
        get
        {
            return currentX;
        }

        set
        {
            if (value > 0)
            {
                currentX = value;
                xSlider.value = value;
                xField.text = value + "";
            }
        }
    }

    public int CurrentY
    {
        get
        {
            return currentY;
        }

        set
        {
            if (value > 0)
            {
                currentY = value;
                ySlider.value = value;
                yField.text = value + "";
            }
        }
    }

    public int AmountOfTrees
    {
        get
        {
            return amountOfTrees;
        }

        set
        {
            if (value != 0)
            {
                amountOfTrees = value;
                treeSlider.value = value;
                treeField.text = value + "";
            }
        }
    }

    public int TreeSpawnSize
    {
        get
        {
            return treeSpawnSize;
        }

        set
        {
            if (value != 0)
            {
                treeSpawnSize = value;
                treeSizeSlider.value = value;
                treeSizeField.text = value + "";
            }
        }
    }

    public int AmountOfWater
    {
        get
        {
            return amountOfWater;
        }

        set
        {
            if (value != 0)
            {
                amountOfWater = value;
                waterSlider.value = value;
                waterField.text = value + "";
            }
        }
    }

    public int WaterSpawnSize
    {
        get
        {
            return waterSpawnSize;
        }

        set
        {
            if (value != 0)
            {
                waterSpawnSize = value;
                waterSizeSlider.value = value;
                waterSizeField.text = value + "";
            }
        }
    }

    public int AmountOfRock
    {
        get
        {
            return amountOfRock;
        }

        set
        {
            if (value != 0)
            {
                amountOfRock = value;
                rockSlider.value = value;
                rockField.text = value + "";
            }
        }
    }

    public int RockSpawnSize
    {
        get
        {
            return rockSpawnSize;
        }

        set
        {
            if (value != 0)
            {
                rockSpawnSize = value;
                rockSizeSlider.value = value;
                rockSizeField.text = value + "";
            }
        }
    }

    public int AmountOfIron
    {
        get
        {
            return amountOfIron;
        }

        set
        {
            if (value != 0)
            {
                amountOfIron = value;
                ironSlider.value = value;
                ironField.text = value + "";
            }
        }
    }

    public int IronSpawnSize
    {
        get
        {
            return ironSpawnSize;
        }

        set
        {
            if (value != 0)
            {
                ironSpawnSize = value;
                ironSizeSlider.value = value;
                ironSizeField.text = value + "";
            }
        }
    }

    public int SpawnSize
    {
        get
        {
            return spawnSize;
        }

        set
        {
            if (value != 0)
            {
                spawnSize = value;
                spawnSlider.value = value;
                spawnField.text = value + "";
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        //Row count and Column count for the map
        xSlider = GameObject.FindGameObjectWithTag(Tags.xSlider).GetComponent<Slider>();
        ySlider = GameObject.FindGameObjectWithTag(Tags.ySlider).GetComponent<Slider>();

        xField = GameObject.FindGameObjectWithTag(Tags.xSlider).GetComponentInChildren<InputField>();
        yField = GameObject.FindGameObjectWithTag(Tags.ySlider).GetComponentInChildren<InputField>();

        //Set our current values to the default value
        CurrentX = (int)xSlider.value;
        CurrentY = (int)ySlider.value;

        //Tree count
        treeSlider = GameObject.FindGameObjectWithTag(Tags.treeSlider).GetComponent<Slider>();
        treeField = GameObject.FindGameObjectWithTag(Tags.treeSlider).GetComponentInChildren<InputField>();

        AmountOfTrees = (int)treeSlider.value;

        treeSizeSlider = GameObject.FindGameObjectWithTag(Tags.treeSizeSlider).GetComponent<Slider>();
        treeSizeField = GameObject.FindGameObjectWithTag(Tags.treeSizeSlider).GetComponentInChildren<InputField>();

        TreeSpawnSize = (int)treeSizeSlider.value;

        //Water count 
        waterSlider = GameObject.FindGameObjectWithTag(Tags.waterSlider).GetComponent<Slider>();
        waterField = GameObject.FindGameObjectWithTag(Tags.waterSlider).GetComponentInChildren<InputField>();

        AmountOfWater = (int)waterSlider.value;

        waterSizeSlider = GameObject.FindGameObjectWithTag(Tags.waterSizeSlider).GetComponent<Slider>();
        waterSizeField = GameObject.FindGameObjectWithTag(Tags.waterSizeSlider).GetComponentInChildren<InputField>();

        WaterSpawnSize = (int)waterSizeSlider.value;

        //Rock count
        rockSlider = GameObject.FindGameObjectWithTag(Tags.rockSlider).GetComponent<Slider>();
        rockField = GameObject.FindGameObjectWithTag(Tags.rockSlider).GetComponentInChildren<InputField>();

        AmountOfRock = (int)rockSlider.value;

        rockSizeSlider = GameObject.FindGameObjectWithTag(Tags.rockSizeSlider).GetComponent<Slider>();
        rockSizeField = GameObject.FindGameObjectWithTag(Tags.rockSizeSlider).GetComponentInChildren<InputField>();

        RockSpawnSize = (int)rockSizeSlider.value;

        //Iron Count
        ironSlider = GameObject.FindGameObjectWithTag(Tags.ironSlider).GetComponent<Slider>();
        ironField = GameObject.FindGameObjectWithTag(Tags.ironSlider).GetComponentInChildren<InputField>();

        AmountOfIron = (int)ironSlider.value;

        ironSizeSlider = GameObject.FindGameObjectWithTag(Tags.ironSizeSlider).GetComponent<Slider>();
        ironSizeField = GameObject.FindGameObjectWithTag(Tags.ironSizeSlider).GetComponentInChildren<InputField>();

        IronSpawnSize = (int)ironSizeSlider.value;

        //Spawn size
        spawnSlider = GameObject.FindGameObjectWithTag(Tags.spawnSlider).GetComponent<Slider>();
        spawnField = GameObject.FindGameObjectWithTag(Tags.spawnSlider).GetComponentInChildren<InputField>();

        SpawnSize = (int)spawnSlider.value;
    }

    public void SliderValueChanged(Slider changedSlider, SliderID id)
    {
        UpdateValue((int)changedSlider.value, id);
    }

    public void FieldValueChanged(InputField inField, SliderID id)
    {
        if (!String.IsNullOrEmpty(inField.text))
        {
            int value = Int32.Parse(inField.text);
            UpdateValue(value, id);
        }
    }

    private void UpdateValue(int value, SliderID id)
    {
        switch (id)
        {
            case SliderID.X:
                CurrentX = value;
                UpdateMaxValues();
                break;
            case SliderID.Y:
                CurrentY = value;
                UpdateMaxValues();
                break;
            case SliderID.Spawn:
                SpawnSize = value;
                break;
            case SliderID.Tree:
                AmountOfTrees = value;
                break;
            case SliderID.TreeSize:
                TreeSpawnSize = value;
                break;
            case SliderID.Water:
                AmountOfWater = value;
                break;
            case SliderID.WaterSize:
                WaterSpawnSize = value;
                break;
            case SliderID.Rock:
                AmountOfRock = value;
                break;
            case SliderID.RockSize:
                RockSpawnSize = value;
                break;
            case SliderID.Iron:
                AmountOfIron = value;
                break;
            case SliderID.IronSize:
                IronSpawnSize = value;
                break;
            default:
                break;
        }
    }

    private void UpdateMaxValues()
    {
        int newMax = 0;
        if ((currentX / 50) != (oldX / 50) || (currentY / 50) != (oldY / 50))
        {
            newMax += 15 * (currentX / 50);
            newMax += 15 * (currentY / 50);
        }

        if (newMax != 0)        
            UpdateMaxValues(newMax);

        oldX = currentX;
        oldY = CurrentY;
    }

    //Updates the max value of resources depending on the size of the map
    private void UpdateMaxValues(int newMax)
    {
        ironSlider.maxValue = newMax;
        rockSlider.maxValue = newMax;
        treeSlider.maxValue = newMax;
        waterSlider.maxValue = newMax;
    }
}
