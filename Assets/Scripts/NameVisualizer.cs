using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameVisualizer : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        if (PlayerPrefs.HasKey("Name"))
            GetComponent<TextMeshProUGUI>().text = RegistrationSystem.GetName();
        else
        {
            GetComponent<TextMeshProUGUI>().text = "Hammer";
        }
    }
}
