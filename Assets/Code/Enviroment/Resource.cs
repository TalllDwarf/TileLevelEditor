using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public GameObject[] resourceView;
    public ResourceType resourceType;

    public int maxResouse;
    public int resourceLeft;

    public bool doesResourseRegen;
    public float regenTime;
    public int regenAmount;

    GameObject currentResourceObject;
    ResourceView view = ResourceView.Empty;

    float nextRegen;

    private bool isItem = false;

    public void Init(int max, int rLeft, bool regen = false, float regTime = 0f, int regAmount = 0)
    {
        maxResouse = max;

        if(rLeft == 0)
        {
            resourceLeft = Random.Range((max / 5) + 1, max);
        }
        else
            resourceLeft = rLeft;
        doesResourseRegen = regen;
        regenTime = regTime;
        regenAmount = regAmount;
        ExtractResource(0);
    }

    public void IsItem()
    {
        isItem = true;
        maxResouse = 3;
        resourceLeft = 1;
        CheckViewObject();
    }

    public int GetResourceLeft()
    {
        return resourceLeft;
    }

    //Amount to extract
    public int ExtractResource(int extractAmount)
    {
        //Extract resource
        if(resourceLeft > extractAmount)
        {
            resourceLeft -= extractAmount;
        }
        else
        {
            extractAmount = resourceLeft;
            resourceLeft = 0;
        }

        CheckViewObject();
        
        return extractAmount;
    }

    //Increase/decrease max resources
    public void AddMaxResource(int amountToAdd)
    {
        maxResouse = Mathf.Abs(maxResouse - amountToAdd);
    }


    private void CheckViewObject()
    {
        int prefabResource = (maxResouse / 3);

        //Make sure we have the correct object on the tile
        if (resourceLeft > (prefabResource * 2))
        {
            if (view != ResourceView.Full)
            {
                view = ResourceView.Full;
                CreateObject();
            }
        }
        else if (resourceLeft > prefabResource)
        {
            if (view != ResourceView.Moderate)
            {
                view = ResourceView.Moderate;
                CreateObject();
            }
        }
        else
        {
            if (view != ResourceView.Limited)
            {
                view = ResourceView.Limited;
                CreateObject();
            }
        }
    }

    //Create the object on top of the tile
    private void CreateObject()
    {
        if(currentResourceObject != null)
        {
            Destroy(currentResourceObject);
        }
        currentResourceObject = Instantiate(resourceView[(int)view], transform.position, Quaternion.Euler(0,Random.Range(0,359),0), transform);
    }

    // Update is called once per frame
    void Update () {
        if (!isItem)
        {
            //Regen resources
            if (doesResourseRegen && resourceLeft < maxResouse)
            {
                nextRegen -= Time.deltaTime;

                if (nextRegen <= 0)
                {
                    nextRegen = regenTime;
                    ExtractResource(-regenAmount);
                }
            }

            //If we no longer have resources tell our parent to destory us
            if (resourceLeft == 0)
            {
                GetComponentInParent<TileController>().DetroyConnectedObject();
            }
        }
	}
}
