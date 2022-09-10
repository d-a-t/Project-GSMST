using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack2 : MonoBehaviour
{
    public GameObject myself;
    public float delay = .25f;
    public float waittime = 0f;
    public Player1 playerscript;

    // Start is called before the first frame update
    void Start()
    {
        myself.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time>playerscript.p2nextcd){
            myself.SetActive(false);
        }
    }
}
