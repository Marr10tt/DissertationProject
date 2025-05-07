using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UniversalNBody : MonoBehaviour
{

    public List<GameObject> AllBodies;

    float attractionForce;
    float bigG = Mathf.Pow(6.67F, 10^-11);
    public GameObject self;
    Rigidbody rb;

    void Start()
    {
        GetBodies();
    }

    void FixedUpdate(){
        for (int i = 0; i < AllBodies.Count; i++){
            self = AllBodies.ElementAt(i).GameObject();
            rb = self.GetComponent<Rigidbody>();
            for (int x = 0; x < AllBodies.Count; x++){
                if(AllBodies[x] != self){
                    PhysicsCalculation(AllBodies[x]);
                    rb.AddForce((AllBodies[x].transform.position - self.transform.position) * attractionForce);
                }
            }
        }
    }
    public void GetBodies(){
        Debug.Log("Retrieving all bodies...");

        //wipe existing list
        AllBodies.Clear();
        
        //collect all bodies
        GameObject[] tempBodies = GameObject.FindGameObjectsWithTag("Bodies");
        //add bodies to a list rather than array in order to access .Remove()
        for (int y = 0; y < tempBodies.Length; y++){
            AllBodies.Add(tempBodies[y]);
        }
        //remove the self from the list, will reduce unneccesary physics calls by n where n is the number of bodies
        AllBodies.Remove(self);
        Debug.Log("Retrieved all bodies...");
    }

    public void PhysicsCalculation(GameObject otherBody){
        //calculate distance and mass in order to apply the attraction force
        float distance = Vector3.Distance(self.transform.position, otherBody.transform.position);
        float masses = (bigG * rb.mass * otherBody.GetComponent<Rigidbody>().mass);
        attractionForce = Mathf.Pow((masses/distance), 2.0F);
    }
}
