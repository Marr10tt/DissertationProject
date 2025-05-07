using UnityEngine;

public class FixedDistance : MonoBehaviour
{
    [Header ("attributes")]
    public GameObject centralBody;
    public float speed = 20f;

    void Update(){
        transform.RotateAround(centralBody.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
