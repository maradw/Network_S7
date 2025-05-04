// PlayerSpawner.cs
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private CharacterStats[] _characterDatabase; // Misma referencia que en CharacterSelector

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        int characterIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["characterIndex"];
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        // Instanciar jugador
        GameObject player = PhotonNetwork.Instantiate(
        "Player", // Nombre EXACTO del prefab
        spawnPoint.position,
        spawnPoint.rotation
    );

        // Configurar skin y nombre
        photonView.RPC("RPC_SetupPlayer", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID, characterIndex);
    }

    [PunRPC]
    private void RPC_SetupPlayer(int viewId, int characterIndex)
    {
        GameObject player = PhotonView.Find(viewId).gameObject;
        CharacterStats stats = _characterDatabase[characterIndex];

        // Aquí configura el modelo 3D, materiales, etc. según tu implementación
        player.GetComponentInChildren<TextMeshPro>().text = PhotonNetwork.LocalPlayer.NickName;
        // Ejemplo: player.GetComponent<MeshRenderer>().material = stats.characterMaterial;
    }
}