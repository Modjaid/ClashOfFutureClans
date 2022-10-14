using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomName : MonoBehaviour
{
    [SerializeField] private List<string> randomNames;

    public string SelectedName { get; private set; }

    void Start()
    {
        SelectedName = randomNames[Random.Range(0, randomNames.Count)];
        GetComponent<TextMeshProUGUI>().text = SelectedName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
