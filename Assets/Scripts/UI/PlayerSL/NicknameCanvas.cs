using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class NicknameCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject mainCanvases;
    [SerializeField] private CharacterSelector characterSelector;

    private void Awake()
    {
        confirmButton.onClick.AddListener(OnConfirmNickname);
        mainCanvases.SetActive(false);
    }

    private void OnConfirmNickname()
    {
        if (!string.IsNullOrEmpty(nicknameInputField.text))
        {
            string nick = nicknameInputField.text;
            MasterManager.GameSettings.SetNickname(nicknameInputField.text);
            MasterManager.GameSettings.SetCharacterIndex(characterSelector.GetSelectedCharacterIndex());

            mainCanvases.SetActive(true);
            gameObject.SetActive(false);

            var props = new ExitGames.Client.Photon.Hashtable();
            props.Add("Nickname", nick);
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            FindObjectOfType<PhotonConnectionTest>().ConnectToPhoton();
        }
    }
}
