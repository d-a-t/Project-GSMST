using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class campoint : MonoBehaviour
{
    public GameObject p1;
    public GameObject p2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = (p1.transform.position.x + p2.transform.position.x)/2;
        float y = (p1.transform.position.y + p2.transform.position.y);
        float z = (p1.transform.position.z + p2.transform.position.z)/2;
        transform.position = new Vector3(x,y,z);
        
    }
}
