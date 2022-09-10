using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experimentalPlayer1 : MonoBehaviour
{
    public Fighter fighter;
    Player1Controls controls;

    void Awake()
    {
        controls = new Player1Controls();

        //controls.PlayerGameplay.A.performed += ctx => fighter.LightKick();
    }

    void OnEnable()
    {
        controls.PlayerGameplay.Enable();

    }

    void OnDisable()
    {
        controls.PlayerGameplay.Disable();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
