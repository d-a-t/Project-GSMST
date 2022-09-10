using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : MonoBehaviour
{
    // Start is called before the first frame update
    Player1Controls controls;
    public GameObject player;
    public GameObject player2;
    public CharacterController2D p1c;
    public CharacterController2D p2c;
    public Collider2D[] p1attacks;
    public Collider2D[] p2attacks;
    public Rigidbody2D hit;
    public healthbar healthbar;
    public healthbarp2 healthbarp2;
    public GameObject attA;
    public GameObject attB;
    public GameObject attC;
    public GameObject attS;
    public GameObject attA2;
    public GameObject attB2;
    public GameObject attC2;
    public GameObject attS2;

    public int maxHealth = 100;
    public int p1hp;
    public int p2hp;
    public float p1cd = .5f;
    public float p2cd = .5f;
    public float p1nextcd = 0;
    public float p2nextcd = 0;

    bool upk = false;
    bool downk = false;
    bool leftk = false;
    bool rightk = false;
    bool ak = false;
    bool bk = false;
    bool ck = false;
    bool sk = false;
    bool startk = false;
    bool selectk = false;

    bool upk2 = false;
    bool downk2 = false;
    bool leftk2 = false;
    bool rightk2 = false;
    bool ak2 = false;
    bool bk2 = false;
    bool ck2 = false;
    bool sk2 = false;
    bool startk2 = false;
    bool selectk2 = false;

    public float mve = 0; 
    public float mve2 = 0;

    bool jump1 = false;
    bool jump2 = false;

    void Awake()
    {
        controls = new Player1Controls();

        controls.PlayerGameplay.Up.performed += ctx => Up();
        controls.PlayerGameplay.Down.performed += ctx => Down();
        controls.PlayerGameplay.Left.performed += ctx => Left();
        controls.PlayerGameplay.Right.performed += ctx => Right();
        controls.PlayerGameplay.A.performed += ctx => A();
        controls.PlayerGameplay.B.performed += ctx => B();
        controls.PlayerGameplay.C.performed += ctx => C();
        controls.PlayerGameplay.S.performed += ctx => S();
        controls.PlayerGameplay.Start.performed += ctx => bStart();
        controls.PlayerGameplay.Select.performed += ctx => Select();

        controls.PlayerGameplay.Up1.performed += ctx => Up2();
        controls.PlayerGameplay.Down1.performed += ctx => Down2();
        controls.PlayerGameplay.Left1.performed += ctx => Left2();
        controls.PlayerGameplay.Right1.performed += ctx => Right2();
        controls.PlayerGameplay.A1.performed += ctx => A2();
        controls.PlayerGameplay.B1.performed += ctx => B2();
        controls.PlayerGameplay.C1.performed += ctx => C2();
        controls.PlayerGameplay.S1.performed += ctx => S2();
        controls.PlayerGameplay.Start1.performed += ctx => bStart2();
        controls.PlayerGameplay.Select1.performed += ctx => Select2();

        controls.PlayerGameplay.Up.canceled += ctx => Upc();
        controls.PlayerGameplay.Down.canceled += ctx => Downc();
        controls.PlayerGameplay.Left.canceled += ctx => Leftc();
        controls.PlayerGameplay.Right.canceled += ctx => Rightc();


        controls.PlayerGameplay.Up1.canceled += ctx => Up2c();
        controls.PlayerGameplay.Down1.canceled += ctx => Down2c();
        controls.PlayerGameplay.Left1.canceled += ctx => Left2c();
        controls.PlayerGameplay.Right1.canceled += ctx => Right2c();

	}

    // public void Start() {
    //             //var bruh = InputController.Controls.Player;
    //     KeyStroke poop = new KeyStroke(new List<(KeyCode, float)> {(KeyCode.W, .25F),(KeyCode.A, .25F), (KeyCode.D, .25F)} );
    //    // BasicInputStroke<object> poop2 = new BasicInputStroke<object>(new List<BasicInput<object>> {(  InputController.Keyboard[KeyCode.Q] as BasicInput<object>, .25F),(bruh.Button2, .25F)} );

    //     InputController.Keyboard[KeyCode.E].Connect( (bool val)  => {
    //         Debug.Log("Bruh12341243");
    //         return true;
    //     });

	// 	InputController.CustomInputs[InputController.Keyboard[KeyCode.Q]].Coannect((object val) => {
    //         Debug.Log("bruh");
	// 		if (InputController.Keyboard[KeyCode.Q].Value) {
	// 			Debug.Log("Bruh12");
	// 		}
	// 		return true;
	// 	});
	// }

    private void Attack1(Collider2D col, GameObject gplayer)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.extents, col.transform.rotation.z, LayerMask.GetMask("hits"));
        foreach(Collider2D c in cols){                
            if(c.transform.parent.parent == gplayer.transform)
            continue;
            Rigidbody2D hitfx = Instantiate(hit,(c.transform.position+col.transform.position)/2,c.transform.rotation);
            if(gplayer.name == "P1"){
                p2hp -=20;
                healthbarp2.SetHealth(p2hp);
            }
            else if(gplayer.name == "P2"){
                p1hp -=20;
                healthbar.SetHealth(p1hp);
            }
            Debug.Log(c.name);
        }
    }
 
    void OnEnable()
    {
        controls.PlayerGameplay.Enable();

    }

    void OnDisable()
    {
        controls.PlayerGameplay.Disable();

    }

    void Up()
    {
        jump1 = true;
    }

    void Down()
    {
        downk = true;
    }

    void Left()
    {
        leftk = true;
        mve = 1;
    }

    void Right()
    {
        rightk = true;
        mve = -1;
    }

    void Upc()
    {
        jump1 = false;
    }

    void Downc()
    {
        downk = false;
    }

    void Leftc()
    {
        leftk = false;
        mve = 0;
    }

    void Rightc()
    {
        rightk = false;
        mve = 0;
    }

    void A()
    {
        if (Time.time > p1nextcd){
            attA.SetActive(true);
            Attack1(p1attacks[0],player);
            p1nextcd = Time.time + p1cd;
        }
    }

    void B()
    {
        if (Time.time > p1nextcd){
            attB.SetActive(true);
            Attack1(p1attacks[1],player);
            p1nextcd = Time.time + p1cd;
        }
    }

    void C()
    {
        if (Time.time > p1nextcd){
            attC.SetActive(true);
            Attack1(p1attacks[2],player);
            p1nextcd = Time.time + p1cd;
        }
    }

    void S()
    {
        if (Time.time > p1nextcd){
            attS.SetActive(true);
            Attack1(p1attacks[3],player);
            p1nextcd = Time.time + p1cd;
        }
    }

    void Select()
    {
        Debug.Log("Select");
    }

    void bStart()
    {
        Debug.Log("Start");
    }



    void Up2()
    {
        jump2 = true;
    }

    void Down2()
    {
        downk2 = true;
    }

    void Left2()
    {
        leftk2 = true;
        mve2 = 1;
    }

    void Right2()
    {
        rightk2 = true;
        mve2 = -1;
    }

    void Up2c()
    {
        jump2 = false;
    }

    void Down2c()
    {
        downk2 = false;
    }

    void Left2c()
    {
        leftk2 = false;
        mve2 = 0;
    }

    void Right2c()
    {
        rightk2 = false;
        mve2 = 0;
    }

    void A2()
    {
        if (Time.time > p2nextcd){
            attA2.SetActive(true);
            Attack1(p2attacks[0], player2);
            p2nextcd = Time.time + p2cd;
        }
    }

    void B2()
    {
        if (Time.time > p2nextcd){
            attB2.SetActive(true);
            Attack1(p2attacks[1], player2);
            p2nextcd = Time.time + p2cd;
        }
    }

    void C2()
    {
        if (Time.time > p2nextcd){
            attC2.SetActive(true);
            Attack1(p2attacks[2], player2);
            p2nextcd = Time.time + p2cd;
        }
    }

    void S2()
    {
        if (Time.time > p2nextcd){
            attS2.SetActive(true);
            Attack1(p2attacks[3], player2);
            p2nextcd = Time.time + p2cd;
        }
    }

    void Select2()
    {
        Debug.Log("Select2");
    }

    void bStart2()
    {
        Debug.Log("Start2");
    }

    void Start()
    {
        p1hp = maxHealth;
        p2hp = maxHealth;
    }

    void FixedUpdate ()
    {
        p1c.Move(mve, false, jump1);
        p2c.Move(mve2, false, jump2);
        
    }

    
}
