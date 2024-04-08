using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] DodgeBalls;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Ball());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Ball()
    {
        yield return new WaitForSeconds(3);

        foreach (GameObject ball in DodgeBalls)
        {
            Instantiate(ball, Vector3.zero, Quaternion.identity);
        }
    }
}
