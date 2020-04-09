using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButtonCenter : MonoBehaviour {


    public float alphaHitMinimumThreshold = 0.1f;
    public float movementSpeed = 3;

    public Direction startPosition = Direction.Right;
    public GameObject[] buttons;
    public GameObject[] innerButtons;
    public bool open = false;

    UnityEngine.UI.Button buildButton;

    // Use this for initialization
    void Start()
    {
        //Get button and add a listener
        buildButton = GetComponent<UnityEngine.UI.Button>();
        buildButton.onClick.AddListener(() => ToggleOpen());

        //Makes only visible part of the button clickable
        //since button object is a rect
        GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = alphaHitMinimumThreshold;

        SetButtons(buttons);

        if(innerButtons.Length > 0)
            SetButtons(innerButtons);
    }

    private void SetButtons(GameObject[] radialButtons)
    {
        //Calculate the end position of all connected buttons
        RectTransform rectTrans = GetComponent<RectTransform>();

        float circumference = (Tags.UIButtonWidth + Tags.UIButtonPadding) * (radialButtons.Length * 2.5f);
        float radius = (circumference / Tags.pi) / 2.0f;
        float angle = ((2.0f * Tags.pi) / 4.0f) / (radialButtons.Length - 1);
        float startAngle = ((2.0f * Tags.pi) / 4.0f) * (int)startPosition;

        //Calculate the position of the buttons
        for (float i = 0; i < radialButtons.Length; i++)
        {
            Vector2 endPosition = new Vector2();
            endPosition.x = rectTrans.position.x + (radius * Mathf.Cos((angle * i) + startAngle));
            endPosition.y = rectTrans.position.y + (radius * Mathf.Sin((angle * i) + startAngle));

            //Tell button its end position and movement speed
            radialButtons[(int)i].GetComponent<RadialButtonMenu>().GetEndPosition(endPosition, movementSpeed, alphaHitMinimumThreshold);
            radialButtons[(int)i].GetComponent<RadialButtonMenu>().SetStartPosition(rectTrans.position);
        }
    }

    //TODO: ScreenSize change
    private void Update()
    {

    }

    public void ToggleOpen()
    {
        open = !open;

        foreach (GameObject gObject in buttons)
        {
            gObject.GetComponent<RadialButtonMenu>().OpenMenu(open);
        }

        foreach(GameObject gObject in innerButtons)
        {
            gObject.GetComponent<RadialButtonMenu>().OpenMenu(open);
        }
    }

    private void OnDestroy()
    {
        if (buildButton != null)
            buildButton.onClick.RemoveAllListeners();
    }
}
