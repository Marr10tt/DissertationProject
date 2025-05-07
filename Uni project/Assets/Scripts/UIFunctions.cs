using UnityEngine;
using TMPro;
public class UIFunctions : MonoBehaviour
{

    public TMP_InputField timeScaleValue;

    public void SetTimescale(){
        Time.timeScale = float.Parse(timeScaleValue.text);
    }

}
