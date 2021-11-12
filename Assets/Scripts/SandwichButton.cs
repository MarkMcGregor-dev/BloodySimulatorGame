using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandwichButton : MonoBehaviour
{
    public bool sandwichActivated;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ActivateSandwich);
    }

    public void ActivateSandwich()
    {
        Debug.Log("NOM");
        sandwichActivated = true;

        button.interactable = false;

        Invoke("ActivateSandwichButton", 25.0f);
    }

    private void ActivateSandwichButton()
    {
        button.GetComponent<Button>().interactable = true;
        sandwichActivated = false;
    }
}
