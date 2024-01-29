using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public Package package;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            package.Save("Bag");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            package.Load("Bag");
        }
    }
}
