using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectShuriken : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    private RectTransform shuriken;
    private int currentPosition;

    private void Awake()
    {
        shuriken = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        currentPosition = 0;
        ChangePosition(0);
    }
    private void Update()
    {
   
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);

        //Press E or enter to interact with button
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        AssignPosition();
    }
    private void AssignPosition()
    {
        shuriken.position = new Vector3(shuriken.position.x, buttons[currentPosition].position.y);
    }
    private void Interact()
    {
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
