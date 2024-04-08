using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private PhotonView PV;
    private CharacterController cc;
    public float speed;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //allow the players to be move in the horizontal input and vertical but in up and down
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            //clamp for animation
            float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude) / 2;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputMagnitude *= 2;
            }
            Vector3 velocity = moveDirection * inputMagnitude;
            cc.Move(velocity * Time.deltaTime);
            //Movement();
            //BasicRotation();
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cc.Move(transform.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cc.Move(-transform.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cc.Move(-transform.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cc.Move(transform.right * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed += 7;
        }

    }

    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0,mouseX,0));
    }
}
