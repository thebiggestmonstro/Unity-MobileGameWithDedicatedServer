using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Login : UI_Base
{
    enum GameObjects_Btn
    {
        Btn_Connect,
        Btn_Send
    }

    private void Start()
    {
        Bind<GameObject>(typeof(GameObjects_Btn));

        GetObject((int)GameObjects_Btn.Btn_Connect).GetOrAddComponent<Button>().onClick.AddListener(Connect);
        GetObject((int)GameObjects_Btn.Btn_Send).GetOrAddComponent<Button>().onClick.AddListener(Send);
    }

    private void Connect()
    {
        NetworkClient.Instance.Connect();
    }

    private void Send()
    {
        NetworkClient.Instance.SendServer("Hello From Client!!");
    }
}