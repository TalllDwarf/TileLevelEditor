using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButtonMenu : MonoBehaviour {

    Vector2 startPosition, endPosition;
    RectTransform screenPosition;

    float movementSpeed;
    float lerpAmount = 0;
    bool open = false;

    protected virtual void Start()
    {
        screenPosition = GetComponent<RectTransform>();
    }

    //Gets a new end position
    public void GetEndPosition(Vector2 newEndPosition, float newMovementSpeed, float alphaHit)
    {
        GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = alphaHit;

        endPosition = newEndPosition;
        movementSpeed = newMovementSpeed;
    }

    //Sets start position;
    public void SetStartPosition(Vector2 newStartPosition)
    {
        startPosition = newStartPosition;
    }

    //Update the buttons position if we should be open or closed
    protected virtual void Update()
    {
        if (open && !Equals(screenPosition.position, endPosition))
        {
            lerpAmount += (movementSpeed * Time.deltaTime);
            lerpAmount = Mathf.Clamp01(lerpAmount);

            screenPosition.position = Vector2.Lerp(startPosition, endPosition, lerpAmount);
        }
        else if (!open && !Equals(screenPosition.position, startPosition))
        {

            lerpAmount += (movementSpeed * Time.deltaTime);
            lerpAmount = Mathf.Clamp01(lerpAmount);

            screenPosition.position = Vector2.Lerp(endPosition, startPosition, lerpAmount);
        }
    }

    public void OpenMenu(bool isOpen)
    {
        //reset if we fully opened or closed
        //if we did not fully open/close move back to where we came from
        if (isOpen != open)
        {
            open = isOpen;

            if (lerpAmount == 1 || lerpAmount == 0)
                lerpAmount = 0;
            else
                lerpAmount = 1.0f - lerpAmount;
        }
    }

    public bool Equals(Vector3 v3, Vector2 v2)
    {
        return (v3.x == v2.x && v3.y == v2.y);
    }

}
