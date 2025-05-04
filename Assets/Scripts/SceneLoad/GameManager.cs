using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
       public static GameManager Instance;
       
      private void Awake()
      {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
     
          Instance = this;

          DontDestroyOnLoad(this.gameObject);

        PhotonNetwork.AutomaticallySyncScene = true;
    }
         
     
       
    public void LoadScene(string sceneName)
    {
        if (PhotonNetwork.IsMasterClient)

            PhotonNetwork.LoadLevel(sceneName);
    }

    private void Start()
    {
        StartCoroutine (LoadSceneWithLoadingScreen());
    }
    private IEnumerator LoadSceneWithLoadingScreen()
    {
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        // Carga la pantalla de carga de forma aditiva
        //AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync("mapa", LoadSceneMode.Additive);
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingSceneName)); // Establece la escena de carga como activa
        // Desactiva la escena actual
        //yield return loadLoadingScene;

        yield return null; // Espera un frame
        yield return new WaitForSeconds(5f); // Espera visible de pantalla de carga

        // Cargar la escena del juego sincronizada con Photon (reemplaza todas las demás)
        PhotonNetwork.LoadLevel("mapa");
    }
    public override void OnEnable()
    {
        base.OnEnable();
       // SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void CallLoadScene()
    {
        if (SceneManager.GetActiveScene().name == "UI")
        {
           // StartCoroutine(LoadInitialSceneAsync());
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
       // SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*   public void LoadSceneWithScreenLoad(string nameScene)
       {
           StartCoroutine(LoadWithLoadingScreen(nameScene));
           //
       }*/

    //boton
    public void LoadSceneWithScreenLoad(string targetScene)
    {
        StartCoroutine(LoadWithPhotonLoadingScreen(targetScene));
    }

    private IEnumerator LoadWithPhotonLoadingScreen(string targetScene)
    {
        UnloadUIScene();
        // Cargar la escena de carga de forma aditiva
        AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync("LoadScene", LoadSceneMode.Additive);
        
        yield return loadLoadingScene;

        // Esperar un frame para asegurarse de que la escena se cargó
        yield return null;
        yield return new WaitForSeconds(5f);

        // Solo el MasterClient debe ejecutar la carga sincronizada de la escena destino
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(targetScene); // Esto sincroniza la carga entre todos los jugadores
        }
    }
    public void UnloadUIScene()
    {
        //
        SceneManager.UnloadSceneAsync(0);
        Scene gameScene = SceneManager.GetSceneByName("UI");
        if (gameScene.IsValid())
        {
            GameObject[] rootObjects = gameScene.GetRootGameObjects();
            for (int i = 0; i < rootObjects.Length; i++)
            {
                rootObjects[i].SetActive(false);
            }
        }

    }
        /* private IEnumerator LoadWithLoadingScreen(string targetScene)
         {

             AsyncOperation loadLoading = SceneManager.LoadSceneAsync("LoadScene", LoadSceneMode.Additive);
             yield return loadLoading;

             yield return null;


             AsyncOperation loadTarget = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            // AsyncOperation loadTarget2 = PhotonNetwork.LoadLevel(targetScene);
             loadTarget.allowSceneActivation = false;

             float progress = 0f;
             while (progress < 0.9f)
             {
                 progress = Mathf.Clamp01(loadTarget.progress / 0.9f);

                 yield return null;
             }

             yield return new WaitForSeconds(0.5f);


             loadTarget.allowSceneActivation = true;


             yield return new WaitForSeconds(1f);
             SceneManager.UnloadSceneAsync("LoadScene");
         }
     */

        /*public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Single);

        }
        [Header("Carga de Escena")]
        [SerializeField] private Image loadingBarFill;

        private IEnumerator LoadInitialSceneAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync("Main Game");
            operation.allowSceneActivation = false;

            float targetProgress = 0f;

            while (!operation.isDone)
            {
                float actualProgress = Mathf.Clamp01(operation.progress / 0.9f);

                targetProgress = Mathf.MoveTowards(targetProgress, actualProgress, Time.deltaTime * 0.5f);

                if (loadingBarFill != null)
                    loadingBarFill.fillAmount = targetProgress;

                if (targetProgress >= 0.9f)
                {
                    yield return new WaitForSeconds(1f);

                    while (loadingBarFill.fillAmount < 1f)
                    {
                        loadingBarFill.fillAmount += Time.deltaTime;
                        yield return null;
                    }

                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }*/

        //lamar ese

        // PhotonNetwork.LoadLevel(sceneName);
    }
