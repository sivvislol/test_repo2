using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Animator anim;
    public static Rigidbody2D rbd2;
    public float speed = 4f; 
    public float dashCooldown;

    public static float rightBound;
    public static float leftBound;
    public static float topBound;
    public static float bottomBound;



    public bool canIDash;   
    public static Vector2 mov;
    public static Vector2 LastMove;
    public Vector3 movAux;
    public static bool dashing = false;
    public static bool attacking = false;
   

   
    public VirtualJoystick joystick;

    [SerializeField]
    public float DashingSpeed;

    private Bounds bounds;
    public static float speedDash ;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rbd2 = GetComponent<Rigidbody2D>();
        canIDash = true;
        LastMove.x = 0;
        LastMove.y = -1;
        anim.SetFloat("MovX", LastMove.x);
        anim.SetFloat("MovY", LastMove.y);



        foreach (MeshRenderer spriteBounds in GameObject.Find("Capa de Patrones 1").GetComponentsInChildren<MeshRenderer>()) {
            bounds.Encapsulate(spriteBounds.bounds);
        }

        leftBound = bounds.min.x + 1;
        rightBound = bounds.max.x -1;
        bottomBound = bounds.min.y + 1;
        topBound = bounds.max.y -1;
        Debug.Log("leftBound=" + leftBound);
        Debug.Log("rightBound=" + rightBound);
        Debug.Log("bottomBound=" + bottomBound);
        Debug.Log("topBound=" + topBound);
    }
	
	// Update is called once per frame
	void Update () {
        DashCooldown();
        speedDash = DashingSpeed;
        if ((Input.GetKeyDown(KeyCode.C))) {
           
            Dash();            
        }
        if ((Input.GetKeyDown(KeyCode.V))) {

            Attack();
        }
        if (!dashing && !attacking) {
            movAux = joystick.InputDirection;
            if (movAux != Vector3.zero) {
                mov.x = movAux.x;
                mov.y = movAux.z;
            } else {
                mov = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
          
            if (mov != Vector2.zero) {
                
                if (Mathf.Abs(mov.x) > Mathf.Abs(mov.y) && mov.x >= 0) {
                    LastMove.x = 1;
                    LastMove.y = 0;
                } else if (Mathf.Abs(mov.x) < Mathf.Abs(mov.y) && mov.y >= 0) {
                    LastMove.x = 0;
                    LastMove.y = 1;

                } else if (Mathf.Abs(mov.x) > Mathf.Abs(mov.y) && mov.x <= 0) {
                    LastMove.x = -1;
                    LastMove.y = 0;
                } else if (Mathf.Abs(mov.x) < Mathf.Abs(mov.y) && mov.y <= 0) {
                    LastMove.x = 0;
                    LastMove.y = -1;
                }
                anim.SetFloat("MovX", LastMove.x);
                anim.SetFloat("MovY", LastMove.y);
                anim.SetBool("walk", true);
            } else {


                anim.SetBool("walk", false);


            }
        }

    }
    void FixedUpdate() {
        if (!dashing && !attacking) {
            if (rbd2.position.x >= rightBound && mov.x > 0) {
                mov.x = 0;
            }
            if (rbd2.position.x <= leftBound && mov.x < 0) {
                mov.x = 0;
            }
            if (rbd2.position.y >= topBound && mov.y > 0) {
                mov.y = 0;
            }
            if (rbd2.position.y <= bottomBound && mov.y < 0) {
                mov.y = 0;
            }
            rbd2.MovePosition(rbd2.position + mov * speed * Time.deltaTime);
            //rbd2.AddForce(joystick.InputDirection * speed * Time.deltaTime);
        } 
       
    }
    public void Dash() {
        if (mov != Vector2.zero && canIDash) { 
            Debug.Log("init dash");
            canIDash = false;
            anim.SetTrigger("dash");
            dashing = true;
            dashCooldown = 2;
        }
    }
    public void Attack() {
        anim.SetBool("walk", false);
        anim.SetTrigger("attack");
        attacking = true;



    }
    public void DashCooldown() {
        
        if (dashCooldown > 0) {
            canIDash = false;
            dashCooldown -= Time.deltaTime;
        }

        if (dashCooldown <= 0 && !canIDash) {
            canIDash = true;
        }

    }

}
