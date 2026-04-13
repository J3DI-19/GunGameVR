using UnityEngine;
using TMPro;

public class VRKeyboardInput : MonoBehaviour
{
    public TMP_InputField inputField;

    TouchScreenKeyboard keyboard;

    void Update()
    {
        if (keyboard != null)
        {
            inputField.text = keyboard.text;

            if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                keyboard = null;
            }
        }
    }

    public void OpenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open(
            inputField.text,
            TouchScreenKeyboardType.Default,
            false,
            false,
            false,
            false,
            "Enter Name"
        );
    }
}