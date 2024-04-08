using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(wait());


    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3.5f);
        // GameObject.Find("GameSetup").GetComponent<GameSetup>().DisconnectPlayer(true);
    }
}
