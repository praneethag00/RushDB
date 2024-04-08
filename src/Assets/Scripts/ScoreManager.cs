using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/*Keeps track of the score and displays it to the user
  Created By:  Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu
 */

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public int score = 0;


    private void Awake()
    {
        //points();
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Points: " + score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
       points();
    }

    public void points()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                int pts = player.GetComponent<PlayerPoints>().playerPoints;
                score = pts;
                scoreText.text = "Points: " + score.ToString();
            }
        }

            
    }
}
