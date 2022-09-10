using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yes : MonoBehaviour
{
    public Transform ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.collider.gameObject.layer == 7){
            Debug.Log("omg");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
