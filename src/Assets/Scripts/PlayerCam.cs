using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*Camera control scipt  - user moves the moues the camera follows
 * Created By: Michael Boudreaux, Praneetha Gobburi
 * 
 */

public class PlayerCam : MonoBehaviour
{
    public float dragX;
    public float dragY;

    public Transform parent;

    private PhotonView myPhotonView;

    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        //cursor is locked in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        //cursor is invisible
        //Cursor.visible = false
        myPhotonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPhotonView.IsMine)
        {
            float xCursor = Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragX;
            float yCursor = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            yRotation += xCursor;

            //set the postion of the camera
            parent.Rotate(Vector3.up, xCursor);
            parent.Rotate(Vector3.right, xRotation);
        }
    }
       
}
