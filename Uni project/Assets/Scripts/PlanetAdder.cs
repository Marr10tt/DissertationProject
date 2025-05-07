using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlanetAdder : MonoBehaviour
{
    public GameObject planetPrefab;
    NBodyScript nBodyScript;

    //input fields
    public TMP_InputField startX;
    public TMP_InputField startY;
    public TMP_InputField startZ;
    public TMP_InputField startForceInput;

    //body variables
    //sample variables - to be changed
    Vector3 startDir = new Vector3(0,0,0);
    float startForce = 0f;

    public List<GameObject> allBodies;

    void Start()
    {
        nBodyScript = planetPrefab.GetComponent<NBodyScript>();
        GetAllBodies();
    }

    void Update()
    {
        //on click, add planet
        if(Input.GetMouseButtonDown(0)){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AddPlanet(mousePos);
        }
    }

    /// <summary>
    /// intantiate a planet on based on given inputs at the mouse position.
    /// </summary>
    /// <param name="mousePos">the current mouse position as a vector 3</param>
    void AddPlanet(Vector3 mousePos){
        //set mouse position lower
        mousePos.y = mousePos.y-50;
        //gather list of other bodies
        nBodyScript.GetBodies();

        //instantiate planet
        Instantiate(planetPrefab, mousePos, transform.rotation);

        GetAllBodies();

        for (int i = 0; i<allBodies.Count; i++){
            NBodyScript tempScript = allBodies.ElementAt(i).GetComponent<NBodyScript>();
            tempScript.GetBodies();
        }
        Debug.Log("click");
    }

    void GetAllBodies(){
        allBodies.Clear();

        GameObject[] tempBodies = GameObject.FindGameObjectsWithTag("Bodies");
        //add bodies to a list rather than array in order to access .Remove()
        for (int y = 0; y < tempBodies.Length; y++){
            allBodies.Add(tempBodies[y]);
        }
    }
    
    /// <summary>
    /// set values for initial force direction and initial force based on user input before adding body
    /// </summary>
    void UpdateValues(){
        nBodyScript.initialForceDirection.x = float.Parse(startX.text);
        nBodyScript.initialForceDirection.y = float.Parse(startY.text);
        nBodyScript.initialForceDirection.z = float.Parse(startZ.text);
        nBodyScript.initialForce = float.Parse(startForceInput.text);
    }


}
