using UnityEngine;
using TMPro;

public class TwoBodyAdder : MonoBehaviour
{
    public GameObject planetPrefab;

    TwoBodyScript twoBodyScript;

    //input fields
    public TMP_InputField startX;
    public TMP_InputField startY;
    public TMP_InputField startZ;
    public TMP_InputField startForceInput;

    //body variables
    public GameObject otherBody;

    Vector3 mousePos = new Vector3(0,0,0);

    void Start()
    {
        twoBodyScript = planetPrefab.GetComponent<TwoBodyScript>();
        twoBodyScript.otherBody = otherBody;
    }

    void Update()
    {
        //on click, add planet
        if(Input.GetMouseButtonDown(0)){
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            AddPlanet();
        }
    }

    void AddPlanet(){
        //set mouse position lower
        mousePos.y = mousePos.y-50;

        //instantiate planet
        Instantiate(planetPrefab, mousePos, transform.rotation);
        Debug.Log("click");
    }

        /// <summary>
    /// set values for initial force direction and initial force based on user input before adding body
    /// </summary>
    public void UpdateValues(){
        twoBodyScript.initialForceDirection.x = float.Parse(startX.text);
        twoBodyScript.initialForceDirection.y = float.Parse(startY.text);
        twoBodyScript.initialForceDirection.z = float.Parse(startZ.text);
        twoBodyScript.initialForce = float.Parse(startForceInput.text);
    }
}
