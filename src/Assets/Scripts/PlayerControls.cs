using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
/*Player scripts for controlling movement and animation of player
 * also when the player gets hit by ball
 * Created BY: Michael Boudreaux, Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/


public class PlayerControls : MonoBehaviour
{
    public float rotateSpeed;
    public float jumpSpeed;

    public Text healthText;
    public Text healthText2;


    public float jumpHspeed;
    public float jumpdelay;
    private bool ground;
    private bool falling;
    private bool hit;
    public static bool quit;
    private float health = 100;
    private float minHealth = 0;
    private float MaxHealth = 100;

    private CharacterController characterController;
    private Animator animator;
    public Camera cam;
    private PhotonView myPhotonView;


    private bool jumping;
    private float stepoffset;
    private float? lastGroundedTime;
    private float? jumpButtonTime;
    public float ySpeed;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("in player controls1");
        //cam = Camera.main;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        //check to see if the camera that you are view is yours by photon view
        hit = false;
        myPhotonView = GetComponent<PhotonView>();
        if (!myPhotonView.IsMine)
        {
            Debug.Log("is not my cam");
            //Destroy(cam);
            cam.enabled = false;
        }
        else
        {
            Debug.Log("is my cam");
            GameObject[] DBballs = GameObject.FindGameObjectsWithTag("ball");
            foreach (GameObject ball in DBballs)
            {
                ball.GetComponent<PickupControls>().enabled = true;
            }

        }

        healthText = GameObject.Find("ScoreText").GetComponent<Text>();
        //healthText2 = GameObject.Find("OnPlayerText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText2.text = health.ToString();
        if (myPhotonView.IsMine)
        {
            healthText.text = "Health: " + health.ToString();

            //get the horizontal input and vertical input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //allow the players to be move in the horizontal input and vertical but in up and down
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            //clamp for animation
            float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude) / 2;

            //if player presses leftshift increase their speed to sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputMagnitude *= 2;
            }
            animator.SetFloat("speed", inputMagnitude, 0.25f, Time.deltaTime);
            moveDirection.Normalize();

            ySpeed += Physics.gravity.y * Time.deltaTime;
            //grounded and phyiscs gravity to check for jump
            if (characterController.isGrounded)
            {
                lastGroundedTime = Time.time;
            }
            // if player presses space they jump
            if (Input.GetButtonDown("Jump"))
            {
                jumpButtonTime = Time.time;
            }
            //if the are in the air drecrese their y value to make them fall down
            if (Time.time - lastGroundedTime <= jumpdelay)
            {
                characterController.stepOffset = stepoffset;
                ySpeed = -0.5f;
                animator.SetBool("isGrounded", true);
                ground = true;
                animator.SetBool("jump", false);
                jumping = false;
                animator.SetBool("falling", false);
                falling = false;

                if (Time.time - jumpButtonTime <= jumpdelay)
                {
                    ySpeed = jumpSpeed;
                    animator.SetBool("jump", true);
                    jumping = true;
                    jumpButtonTime = null;
                    lastGroundedTime = null;
                }
            }
            else
            {
                characterController.stepOffset = 0;
                animator.SetBool("isGrounded", false);
                ground = false;
                if ((jumping && ySpeed < 0) || ySpeed < -2)
                {
                    animator.SetBool("falling", true);
                }
            }
            //if they are moving
            if (moveDirection != Vector3.zero)
            {
                animator.SetBool("isMoving", true);

                moveDirection = transform.TransformDirection(moveDirection);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (ground == false)
            {
                Vector3 velocity = moveDirection * inputMagnitude * jumpHspeed;
                velocity.y = ySpeed;

                characterController.Move(velocity * Time.deltaTime);

            }
            //God mode resets the health to 100
            if (Input.GetKeyDown(KeyCode.G))
            {
                health = 100;
            }
            //Health is below game over distroy
            if (health == minHealth || health <= minHealth)
            {
                quit = false;
                gameover(quit, false);
            }
            //User quit to menu
            if (Input.GetKeyDown(KeyCode.M))
            {
                quit = true;
                gameover(quit, false);

            }
        }
    }

    // disconnect player form the game room
    public static void gameover(bool quit, bool win)
    {
        GameObject.Find("GameSetup").GetComponent<GameSetup>().DisconnectPlayer(quit, win);
    }

    private void OnAnimatorMove()
    {
        if (ground)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }
    
    //send out you changs to all other clients
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //mine(local the one that belongs to the cam)
            stream.SendNext(health);

        }
        else if (stream.IsReading)
        {
            health = (float)stream.ReceiveNext();
        }
    }

    //sends you damage to other clients and updates it
    [PunRPC]
    void Damage()
    {
        health -= 20;
    }

    //sends you extra points to other clients and updates it
    [PunRPC]
    void ExtraPoints()
    {
        health += 10;
    }


    public void OnTriggerEnter(Collider other)
    {
        //if the player gets hit by a ball
        if (other.CompareTag("ball"))
        {
            //if the player get hits by thier own ball stop
            PhotonView parentBeforePV = other.GetComponent<PickupControls>().parentfrombefore;
            if (parentBeforePV.ViewID == myPhotonView.ViewID)
                return;
            //send that you got hit to other client
            myPhotonView.RPC("Damage", RpcTarget.AllBuffered);

        }
    }

}
