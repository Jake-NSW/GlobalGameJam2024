using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Hello");

        yield return new WaitForSeconds(1f);
        Debug.Log("World");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
