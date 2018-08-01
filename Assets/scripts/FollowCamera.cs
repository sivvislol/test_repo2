using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    public float rightBound;
    public float leftBound;
    public float topBound;
    public float bottomBound;

    private Camera cam;
    
    public GameObject target;
    private Bounds bounds;    
    Vector3 targetPos;
    Vector3 TransformAux;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start() {

        targetPos = transform.position;

        foreach (MeshRenderer spriteBounds in GameObject.Find("Capa de Patrones 1").GetComponentsInChildren<MeshRenderer>()) {
            bounds.Encapsulate(spriteBounds.bounds);
        }

        cam = this.gameObject.GetComponent<Camera>();
        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;       


        TransformAux = new Vector3(0, 0, 0);

        leftBound = bounds.min.x + camHorzExtent;
        rightBound = bounds.max.x - camHorzExtent;
        bottomBound = bounds.min.y + camVertExtent;
        topBound = bounds.max.y - camVertExtent;


    }

    // Update is called once per frame
    void FixedUpdate() {
        if (target && !Player.attacking) {

            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

         //   Vector3 targetDirection = (target.transform.position - posNoZ);   
           
            //TransformAux.position = transform.position;
            if (target.GetComponent<Animator>().GetBool("walk")) {
                smoothTime = 0.5f;
                TransformAux.x = target.transform.position.x;
                TransformAux.y = target.transform.position.y;

                if (Player.mov.x>0) {
                    TransformAux.x = target.transform.position.x + 4;
                }
                if (Player.mov.x < 0) {
                    TransformAux.x = target.transform.position.x - 4;
                }
                if (Player.mov.y > 0) {
                    TransformAux.y = target.transform.position.y + 4;
                }
                if (Player.mov.y<0) {
                    TransformAux.y = target.transform.position.y - 4;
                }
            } else {
                TransformAux.x = target.transform.position.x;
                TransformAux.y = target.transform.position.y;
                smoothTime = 0.6f;
            }
            targetPos = TransformAux;         

            if (targetPos.x != rightBound || targetPos.x != leftBound || targetPos.y != topBound || targetPos.y != bottomBound) {
                 targetPos = new Vector3(targetPos.x, targetPos.y, transform.position.z);
            }            
            //Right
            if (targetPos.x >= rightBound) {
                 targetPos.x = rightBound;
             }
             //Left
             if (targetPos.x <= leftBound) {
                 targetPos.x = leftBound;
             }
             //Top
             if (targetPos.y >= topBound) {
                
                 targetPos.y = topBound;
             }
                //Bottom
            if (targetPos.y <= bottomBound) {
                targetPos.y = bottomBound;
            }
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
