using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public Camera cam;
    public float z;
    public float fov;

    // Start is called before the first frame update
    void Start()
    {
        
        fov = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        z = Vector3.Distance(Player1.transform.position,Player2.transform.position);
        float campos = (Player1.transform.position.x +Player2.transform.position.x)/2;
        float camposy = (Player1.transform.position.y +Player2.transform.position.y)/2;
        float camposz = Mathf.Abs((z/2)/Mathf.Tan(fov));
        if(camposz < 10){
            camposz = 10;
        }
        if(camposy <6.16f){
            camposy = 6.16f;
        }
        this.transform.position = new Vector3(campos,camposy,camposz);
    }
}
