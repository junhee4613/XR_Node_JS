using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    public InputField[] inputField;                   //Tab키로 이동할 InputField 배열

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();

            if (currentInputField != null)
            {
                int currentIndex = System.Array.IndexOf(inputField, currentInputField);
                int nextIndex = (currentIndex + 1) % inputField.Length; //다음 Input Field 인덱스 계산

                //다음 Input리딩 포커스 이동
                inputField[nextIndex].Select();
                inputField[nextIndex].ActivateInputField();
            } 
        }
    }
}
