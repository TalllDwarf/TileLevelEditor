using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{

    //Position and speed
    public bool canPlayerMove = false;
    public Vector3 moveToPosition;
    public float speed = 2.0f;

    public Vector3 minPos = new Vector3(0, 2, -3.5f);
    public Vector3 maxPos;

    private bool moveing = false;

    //Rotation
    private Quaternion minRot = Quaternion.Euler(6.0f, -360.0f, 0.0f);
    private Quaternion maxRot = Quaternion.Euler(70.0f, 360f, 0.0f);

    private float cameraYPosition = 0.5f;

    private Vector3 mousePos;
    private EventSystem eventS;

    private Coord mapSize;
    private Camera cam;

    // Use this for initialization
    void Start()
    {
        //Get main camera
        cam = GetComponent<Camera>();
        mousePos = Input.mousePosition;

        //Get evemt system
        eventS = GetComponentInChildren<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveing)
        {
            Vector3 movePos = moveToPosition - transform.position;

            if (movePos.magnitude <= 0.5f)
            {
                transform.position = moveToPosition;
                canPlayerMove = true;
                moveing = false;
            }
            else
            {
                movePos.Normalize();
                movePos *= speed;
                transform.position += (movePos * Time.deltaTime);
            }
        }
        //can we move and is the mouse over a gui
        else if (canPlayerMove)
        {
            if (mousePos != Input.mousePosition && !eventS.IsPointerOverGameObject())
            {
                //Is our mouse over a tile
                RaycastHit? hit = GetMouseHit();

                if (hit != null)
                {
                    if (hit.Value.collider.tag == Tags.Floor)
                    {
                        hit.Value.collider.gameObject.GetComponent<TileController>().HitByRay();
                    }
                }
            }

            //Move camera 
            float moveX = (Input.GetAxis(Tags.Horizontal) * speed) * Time.deltaTime;
            moveX = Mathf.Clamp(transform.position.x + moveX, minPos.x, maxPos.x);

            float moveZ = (Input.GetAxis(Tags.Vertical) * speed) * Time.deltaTime;
            moveZ = Mathf.Clamp(transform.position.z + moveZ, minPos.z, maxPos.z);

            cameraYPosition = Mathf.Clamp(cameraYPosition + (Input.GetAxis(Tags.MouseWheel) / 5), 0.0f, 1.0f);

            float moveY = Mathf.Lerp(minPos.y, maxPos.y, cameraYPosition);
            transform.rotation = Quaternion.Lerp(minRot, maxRot, cameraYPosition);

            transform.position = new Vector3(moveX, moveY, moveZ);

        }
    }

    public bool IsMouseOverUI()
    {
        return eventS.IsPointerOverGameObject();
    }

    //Returns a tile that the mouse it over
    public RaycastHit? GetMouseHit()
    {
        //Check players mouse position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

        RaycastHit hit;

        //Check if we are hovering over a tile
        if (Physics.Raycast(ray, out hit))
        {
            //If the thing we hit is a tile
            if (hit.collider.GetComponent<TileController>() != null)
                return hit;
            else
                return null;
        }

        return null;
    }

    public void UpdateMaxMapSize(Coord newMapSize)
    {
        mapSize = newMapSize;
        maxPos = new Vector3((mapSize.x * 2) - 5, 18, (mapSize.y * 2) - 5);
    }

    //Updates the camera position
    public void JumpToPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void MoveToPosition(Vector3 newPosition)
    {
        moveToPosition = newPosition;
        canPlayerMove = false;
        moveing = true;
    }
}
