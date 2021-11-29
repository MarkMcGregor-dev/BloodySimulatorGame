using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartrateText : MonoBehaviour
{
    private GameObject HostController;

    [SerializeField] private Text heartRate;

    // Start is called before the first frame update
    void Start()
    {
        HostController = GameObject.Find("HostController");
    }

    // Update is called once per frame
    void Update()
    {
        heartRate.text = "" + Mathf.Round(HostController.GetComponent<HostController>().currentHeartRate);
    }
}
