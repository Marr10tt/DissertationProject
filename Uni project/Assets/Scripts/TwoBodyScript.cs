using UnityEngine;

public class TwoBodyScript : MonoBehaviour
{
    [Header ("attributes")]
    Rigidbody rb;
    public GameObject self;
    public GameObject otherBody;

    [Header ("Forces")]
    float bigG = Mathf.Pow(6.67F, 10^-11);
    float attractionForce;
    public float initialForce;
    public Vector3 initialForceDirection = new Vector3(0,0,0);

    private void Start(){
        rb = self.GetComponent<Rigidbody>();
        rb.AddForce(initialForceDirection * initialForce);
    }

    private void FixedUpdate(){
        ForceCalculation();
        //apply the calculated force
        rb.AddForce((otherBody.transform.position - self.transform.position) * attractionForce);
    }

    public void ForceCalculation (){
        // find distance
        float distance = Vector3.Distance(self.transform.position, otherBody.transform.position);
        float masses = (bigG * rb.mass * otherBody.GetComponent<Rigidbody>().mass);
        attractionForce = Mathf.Pow((masses/distance), 2.0F);
    }
}
