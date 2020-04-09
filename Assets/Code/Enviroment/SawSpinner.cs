using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSpinner : MonoBehaviour {

    public float rotateSpeed = 3.0f;
    public bool isWorking = false;

	// Update is called once per frame
	void Update () {
        if(isWorking)
            transform.Rotate(new Vector3(-(rotateSpeed * Time.deltaTime), 0, 0));
	}
}
