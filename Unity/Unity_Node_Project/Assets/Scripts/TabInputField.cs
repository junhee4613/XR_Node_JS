using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    public InputField[] inputField;                   //TabŰ�� �̵��� InputField �迭

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();

            if (currentInputField != null)
            {
                int currentIndex = System.Array.IndexOf(inputField, currentInputField);
                int nextIndex = (currentIndex + 1) % inputField.Length; //���� Input Field �ε��� ���

                //���� Input���� ��Ŀ�� �̵�
                inputField[nextIndex].Select();
                inputField[nextIndex].ActivateInputField();
            } 
        }
    }
}
