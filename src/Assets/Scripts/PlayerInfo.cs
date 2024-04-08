using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*saves player charcter info can be used later to change characters
  Created By:  Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu
 */

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    public int mySelectedCharacter;

    public GameObject[] allCharacters;

    private void OnEnable()
    {
        if(PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else
        {
            if(PlayerInfo.PI != this)
            {
                Destroy(PlayerInfo.PI.gameObject);
                PlayerInfo.PI = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        mySelectedCharacter = 0;
        PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
    }

    
}
