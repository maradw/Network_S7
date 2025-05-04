using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startButton;
    private string gameSceneName = "mapa"; // Nombre exacto de tu escena de mapa

    private string loadingSceneName = "LoadScene";  // Nombre exacto de tu escena de carga

    private void Start()
    {
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.onClick.AddListener(OnClickStartGame);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    private void OnClickStartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel(loadingSceneName); // Carga la escena del juego sincronizada con Photon
        // Inicia la carga con pantalla
       // StartCoroutine(LoadSceneWithLoadingScreen());
    }

    private IEnumerator LoadSceneWithLoadingScreen()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        // Carga la pantalla de carga de forma aditiva
        AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingSceneName)); // Establece la escena de carga como activa
        // Desactiva la escena actual
        yield return loadLoadingScene;

        yield return null; // Espera un frame
        yield return new WaitForSeconds(5f); // Espera visible de pantalla de carga

        // Cargar la escena del juego sincronizada con Photon (reemplaza todas las demás)
        PhotonNetwork.LoadLevel(gameSceneName);
    }
}
