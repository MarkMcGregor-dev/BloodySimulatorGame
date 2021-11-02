using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyText : MonoBehaviour
{
    private GameObject HostController;

    [SerializeField] private Text energy;

    // Start is called before the first frame update
    void Start()
    {
        HostController = GameObject.Find("HostController");
    }

    // Update is called once per frame
    void Update()
    {
        energy.text = "Energy: " + Mathf.Round(HostController.GetComponent<HostController>().currentEnergy * 100f) / 100f;
    }
}
