using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

/*player script instantiate the player at random spawn points
 * Created BY: Praneetha Gobburi
 * msu-pgobburi1@mcneese.edu*/
public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawn = Random.Range( GameSetup.GS.spawnPoints.Length-1, 0);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
                GameSetup.GS.spawnPoints[spawn].position, GameSetup.GS.spawnPoints[spawn].rotation, 0);
            //PickupControls.playerfound = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
