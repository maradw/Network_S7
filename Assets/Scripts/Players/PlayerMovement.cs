using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    [Header("Movilidad")]
    public float speed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Control de c�mara")]
    public Transform cameraTransform;        // La c�mara como child del jugador
    public float mouseSensitivity = 2f;      // Sensibilidad
    public float minPitch = -40f;            // L�mite de giro vertical hacia abajo
    public float maxPitch = 75f;

    private CharacterController controller;
    private float pitch = 0f;
    private Vector3 velocity;

    [SerializeField] private TMP_Text nickname;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Shoot()
    {
        photonView.RPC("RPC_Shoot", RpcTarget.All, firePoint.position, firePoint.rotation);
    }
    [PunRPC]
    void RPC_Shoot(Vector3 position, Quaternion rotation)
    {
        Instantiate(bulletPrefab, position, rotation);
    }
    void Start()
    {
        if (!photonView.IsMine)
        {
            // Desactiva la c�mara y el audio listener de los dem�s
            var cam = GetComponentInChildren<Camera>();
            if (cam) cam.gameObject.SetActive(false);
            return;
        }

        // Aseg�rate de que la c�mara local siga a este transform
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (nickname != null)
        {
            nickname.text = photonView.Owner.NickName; // Usa el nombre de Photon
            nickname.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("PlayerMovement: No hay referencia al TMP_Text del nickname.");
        }

        if (photonView.IsMine)
        {
            // Env�a el nombre a todos los jugadores
            photonView.RPC("UpdateNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [PunRPC]
    void UpdateNickname(string name)
    {
        nickname.text = name;
        nickname.gameObject.SetActive(true);
    }

    void Update()
    {
        // if (!photonView.IsMine) return;
        if (photonView.IsMine && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        HandleCameraRotation();
        HandleMovement();
    }

    private void HandleCameraRotation()
    {
        // Lectura del rat�n
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotaci�n horizontal del jugador (al eje Y global)
        transform.Rotate(Vector3.up, mouseX);

        // Rotaci�n vertical de la c�mara (pitch), con clamp
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraTransform.localEulerAngles = Vector3.right * pitch;
    }

    private void HandleMovement()
    {
        // Movimiento relativo a la c�mara
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        Vector3 moveDir = (forward.normalized * v + right.normalized * h);

        if (moveDir.magnitude > 0.1f)
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        // Gravedad y salto
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
