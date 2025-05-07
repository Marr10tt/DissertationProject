using System.Collections.Generic;
using UnityEngine;

public class NBodyScript : MonoBehaviour
{
    [Header ("attributes")]
    Rigidbody rb;
    public GameObject self;
    public List<GameObject> otherBodies;

    [Header ("Forces")]
    float bigG = Mathf.Pow(6.67F, 10^-11);
    float attractionForce;
    public float initialForce;
    public Vector3 initialForceDirection = new Vector3(0,0,0);

    private void Start(){
        rb = self.GetComponent<Rigidbody>();
        rb.AddForce(initialForceDirection * initialForce);
        GetBodies();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < otherBodies.Count; i++){
            PhysicsCalculation(otherBodies[i]);
            rb.AddForce((otherBodies[i].transform.position - self.transform.position) * attractionForce);
        }
    }

    public void GetBodies(){
        Debug.Log("Retrieving all bodies...");

        //wipe existing list
        otherBodies.Clear();
        
        //collect all bodies
        GameObject[] tempBodies = GameObject.FindGameObjectsWithTag("Bodies");
        //add bodies to a list rather than array in order to access .Remove()
        for (int y = 0; y < tempBodies.Length; y++){
            otherBodies.Add(tempBodies[y]);
        }
        //remove the self from the list, will reduce unneccesary physics calls by n where n is the number of bodies
        otherBodies.Remove(self);
        Debug.Log("Retrieved all bodies...");
    }

    void PhysicsCalculation(GameObject otherBody){
        //calculate distance and mass in order to apply the attraction force
        float distance = Vector3.Distance(self.transform.position, otherBody.transform.position);
        float masses = (bigG * rb.mass * otherBody.GetComponent<Rigidbody>().mass);
        attractionForce = Mathf.Pow((masses/distance), 2.0F);

    }
}