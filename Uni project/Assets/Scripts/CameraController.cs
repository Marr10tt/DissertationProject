using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody rb;
    bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isMoving = false;
        if(Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f){
            rb.linearVelocity = transform.right * Input.GetAxisRaw("Horizontal") * speed;
            isMoving = true;
        }

        if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f){
            rb.linearVelocity = transform.forward * Input.GetAxisRaw("Vertical") * speed;
            isMoving = true;
        }

        if(Input.GetKey(KeyCode.Space)){
            rb.linearVelocity = transform.up * 1 * speed;
            isMoving = true;
        }

        if(Input.GetKey(KeyCode.LeftControl)){
            rb.linearVelocity = transform.up * -1 * speed;
            isMoving = true;
        }
        if(isMoving == false){
            rb.linearVelocity = new Vector3(0,0,0);
        }
    }
}
