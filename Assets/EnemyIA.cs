using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {

    private Vector2 mov;
    Animator anim;
    Rigidbody2D rbd2;
    private float speed;
    private bool ArrivePoint;
    public Vector2[] vecArray;
    public Vector2 Destination;
    public bool Patrolling;
    public bool Chasing;
    private float Comparacion;
    public float Distancia_Perseguir;
    public static Vector2 LastMove;
    public int NextPoint;
    private bool Orden;
    private Vector2 PosAux;
    private bool hit;
    private int Health;
    GameObject HealthBar;
    private bool oCollide;
    public Transform target;//set target from inspector instead of looking in Update
    private float force = 20f;
    private bool Die;
    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        rbd2 = GetComponent<Rigidbody2D>();
        speed = 2;
        Die = false;
        hit = false;
        NextPoint = 0;
        oCollide = false;
        Health = 100;
        LastMove.x = 0;
        LastMove.y = -1;
        Patrolling = true;
        Chasing = false;
        anim.SetFloat("movX", LastMove.x);
        anim.SetFloat("movY", LastMove.y);
        ArrivePoint = true;
        Orden = true;
        mov = Vector2.zero;
        vecArray = new Vector2[]
            {
               new Vector2( rbd2.position.x,  rbd2.position.y),
               new Vector2( rbd2.position.x+8,  rbd2.position.y),
                new Vector2( rbd2.position.x+8,  rbd2.position.y-8),
                new Vector2( rbd2.position.x,  rbd2.position.y-8)
            };


        Destination = vecArray[0];



    }
    public Vector2 SiguientePunto() {
        mov = Vector2.zero;

        if (NextPoint < 0 || NextPoint == 0) {
            Orden = true;
            NextPoint = 0;
        }
        /* VUELTA ATRAS */
        /*
        if (Orden) {
            NextPoint++;
        } else {
            NextPoint--;
        }
        */
        /* EN CIRCULOS*/
        if (Orden) {
            NextPoint++;
        } else {
            NextPoint = 0;
            Orden = true;
        }
        if (NextPoint == vecArray.Length) {
            Orden = false;
            NextPoint--;
        }

        return vecArray[NextPoint];
    }
    void Movimiento_Enemigo() {

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
            } else if (Mathf.Abs(mov.x) == Mathf.Abs(mov.y) && mov.y < 0) {
                LastMove.x = 0;
                LastMove.y = -1;

            } else if (Mathf.Abs(mov.x) == Mathf.Abs(mov.y) && mov.y > 0) {
                LastMove.x = 0;
                LastMove.y = 1;
            }

            anim.SetFloat("movX", LastMove.x);
            anim.SetFloat("movY", LastMove.y);

            anim.SetBool("walk", true);
        } else {
            anim.SetBool("walk", false);
        }


        if (Patrolling) {
            if (System.Math.Round(Destination.x) > System.Math.Round(rbd2.position.x)) {
                mov.x = 1;
            } else if (System.Math.Round(Destination.x) < System.Math.Round(rbd2.position.x)) {
                mov.x = -1;
            } else {
                mov.x = 0;
            }
            if (System.Math.Round(Destination.y, 2) > System.Math.Round(rbd2.position.y, 2)) {
                mov.y = 1;
            } else if (System.Math.Round(Destination.y) < System.Math.Round(rbd2.position.y)) {
                mov.y = -1;
            } else {
                mov.y = 0;
            }
        }
        if (Chasing) {
            if (!oCollide) {
                float DistanciaX = target.position.x - rbd2.position.x;
                float DistanciaY = target.position.y - rbd2.position.y;

                if (Mathf.Abs(DistanciaX) > Mathf.Abs(DistanciaY) && DistanciaX > 0) {
                    mov.x = (DistanciaX < 0 ? -1 : 1);
                    mov.y = 0;
                } else if (Mathf.Abs(DistanciaX) < Mathf.Abs(DistanciaY) && DistanciaX > 0) {
                    mov.x = 0;
                    mov.y = (DistanciaY < 0 ? -1 : 1);
                } else if (Mathf.Abs(DistanciaX) > Mathf.Abs(DistanciaY) && DistanciaX < 0) {
                    mov.x = (DistanciaX < 0 ? -1 : 1);
                    mov.y = 0;
                } else if (Mathf.Abs(DistanciaX) < Mathf.Abs(DistanciaY) && DistanciaX < 0) {
                    mov.x = 0;
                    mov.y = (DistanciaY < 0 ? -1 : 1);
                } else if (DistanciaX == 0) {
                    mov.x = 0;
                    mov.y = (DistanciaY < 0 ? -1 : 1);
                } else if (DistanciaY == 0) {
                    mov.x = (DistanciaX < 0 ? -1 : 1);
                    mov.y = 0;
                }
                rbd2.position = Vector3.MoveTowards(rbd2.position, target.position, speed * Time.deltaTime);

            } else {

                rbd2.velocity = Vector3.zero;
            }

        } else {
            rbd2.MovePosition(rbd2.position + mov * speed * Time.deltaTime);
        }

        if (mov == Vector2.zero && !Chasing) {
            ArrivePoint = true;
        }


    }
    // Update is called once per frame
    void FixedUpdate() {
        if (!Die) { 
            if (ArrivePoint) {
                Destination = SiguientePunto();
                ArrivePoint = false;
            }

            Calcular_Distancia();
            Movimiento_Enemigo();
        } else {
            rbd2.velocity = Vector3.zero;
            anim.SetBool("die", true);
        }
    }
    void Calcular_Distancia() {

        Comparacion = Vector3.Distance(target.position, rbd2.position);

        if (Comparacion > Distancia_Perseguir) {
            Chasing = false;
            Patrolling = true;

        } else {
            Patrolling = false;
            Chasing = true;

        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (Player.attacking && !hit) {
            oCollide = false;
            var rel = rbd2.position - Player.rbd2.position;
            rel.Normalize();
            hit = true;
            rbd2.AddForce(rel * force, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        oCollide = true;

    }
    private void OnCollisionExit2D(Collision2D collision) {
        oCollide = false;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (hit) {
            GetDamage();
            StartCoroutine(GolpeAux());
            
        }
    }
    IEnumerator GolpeAux() {

        yield return new WaitForSeconds(0.25f);
        rbd2.velocity = Vector3.zero;
        hit = false;
    }
    private void GetDamage() {
     
        Health -= 25;
        Debug.Log(Health);
        HealthBar = this.gameObject.FindObjectInChilds("HealthResting");
        float ResultBar = Health / 100f;
        if (ResultBar<0) {
            ResultBar = 0;
        }
        if (Health<=0) {
            Die = true;
        }
        // rotater.transform.localScale.x = rotater.transform.localScale.x - 0.5f;
        HealthBar.gameObject.transform.localScale = new Vector3(ResultBar, HealthBar.gameObject.transform.localScale.y, HealthBar.gameObject.transform.localScale.z);
      
    }


}
public static class Extensores {

    public static Vector2 Redondear(this Vector2 vector2, int decimalPlaces = 0) {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++) {
            multiplier *= 10f;
        }
        return new Vector2(
            Mathf.Round(vector2.x * multiplier) / multiplier,
            Mathf.Round(vector2.y * multiplier) / multiplier
           );
    }

    public static GameObject FindObjectInChilds(this GameObject gameObject, string gameObjectName) {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        foreach (Transform item in children) {
            if (item.name == gameObjectName) {
                return item.gameObject;
            }
        }

        return null;
    }
}

