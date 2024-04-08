using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*Script for directing screens after the player goes to the game over screen 
 * Created BY:  Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/

public class LoadNext : MonoBehaviour
{
    public bool win;
    // Start is called before the first frame update
    void Start()
    {
        //get the win value from do not distroy component and check to see if its true
        win = GameObject.Find("GameSetup").GetComponent<GameSetup>().win;
        if (win == true)
        {
            GameObject.FindWithTag("Lose").GetComponent<Text>().gameObject.SetActive(false);
            StartCoroutine(wait());
        }
        else if (win == false)
        {
            GameObject.FindWithTag("Win").GetComponent<Text>().gameObject.SetActive(false);
            StartCoroutine(wait());
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
}
