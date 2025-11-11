using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login : UI_Base
{
    Dictionary<string, int> enumNumbers = new Dictionary<string, int>();

    private string _userName = string.Empty;
    private string _password = string.Empty;

    [SerializeField]
    int maxUserNameLength = 10;
    [SerializeField]
    int maxPasswordLength = 10;

    public enum GameObjects_Btn
    {
        Btn_Login,
        Btn_Quit,
    }

    public enum GameObjects_Input
    {
        Input_UserName,
        Input_Password,
    }

    public enum GameObjects_Text
    {
        Txt_InputUserNameError,
        Txt_InputPasswordError,
        Txt_LoginError,
    }

    private void Awake()
    {
        GenerateEnumsSerialNumber<UI_Login>(enumNumbers);

        Bind<GameObject>(typeof(GameObjects_Btn));
        Bind<GameObject>(typeof(GameObjects_Input));
        Bind<GameObject>(typeof(GameObjects_Text));
    }

    private void Start()
    {
        GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_InputUserNameError)]).SetActive(false);
        GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_InputPasswordError)]).SetActive(false);
        GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_LoginError)]).SetActive(false);

        GetObject(enumNumbers[GetEnumFullName(GameObjects_Btn.Btn_Login)]).GetOrAddComponent<Button>().onClick.AddListener(Login);

        GetObject(enumNumbers[GetEnumFullName(GameObjects_Input.Input_UserName)]).GetOrAddComponent<TMP_InputField>().onValueChanged.AddListener(UpdateUserName);
        GetObject(enumNumbers[GetEnumFullName(GameObjects_Input.Input_Password)]).GetOrAddComponent<TMP_InputField>().onValueChanged.AddListener(UpdatePassword);
    }

    private void Login()
    {
    }

    private void UpdateUserName(string value)
    {
        _userName = value;
        ValidateAndUpdateUI();
    }

    private void UpdatePassword(string value) 
    {
        _password = value;
        ValidateAndUpdateUI();
    }

    private void ValidateAndUpdateUI()
    {
        var userNameRegex = Regex.Match(_userName, "^[a-zA-Z0-9]+$");

        var interactable =  
            !string.IsNullOrWhiteSpace(_userName) && 
            !(string.IsNullOrWhiteSpace(_password)) && 
            (_userName.Length <= maxUserNameLength && _password.Length <= maxPasswordLength) &&
            userNameRegex.Success;

        EnableLoggingButton(interactable);

        if (_userName != null)
        {
            var userNameTooLong = _userName.Length > maxUserNameLength || !userNameRegex.Success;
            GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_InputUserNameError)]).SetActive(userNameTooLong);
        }

        if (_password != null)
        {
            var passwordTooLong = _password.Length > maxPasswordLength;
            GetObject(enumNumbers[GetEnumFullName(GameObjects_Text.Txt_InputPasswordError)]).SetActive(passwordTooLong);
        }
    }

    private void EnableLoggingButton(bool interactable)
    {
        GameObject loginButton = GetObject(enumNumbers[GetEnumFullName(GameObjects_Btn.Btn_Login)]);

        loginButton.GetOrAddComponent<Button>().interactable = interactable;
        var color = loginButton.GetOrAddComponent<Button>().interactable ? Color.white : Color.grey;
        loginButton.GetComponentInChildren<TextMeshProUGUI>().color = color;
    }
}