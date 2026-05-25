using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour
{
    private float horizontalInput, verticalInput, speed;
    [HideInInspector] public bool canMove, canCam;
    private Vector3 inputs;
    [HideInInspector] public Rigidbody rigidBody;

    [SerializeField] private Transform camTransform;

    public GameObject freelookCam;

    public Inventory inventory;
    public ShayNivel shayNivel;
    public NarcisaNivel narcisaNivel;
    private SceneInstructionManager scene;
    private bool canShay;
    public bool canNarcisa, canColidir;
    private bool podePegarObjetos;
    InventoryItem item;
    GameObject objItem;

    [Header("Configurações de Pulo")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask chaoLayerMask;
    [SerializeField] private float distanceCheckChao;
    [SerializeField] private Transform pesPlayer;
    private bool isChao;
    [HideInInspector] public bool canJump;

    private Animator animator;
    private AudioSource playerPassos;
    private AudioSource playerPulo;
    public AudioClip puloClip;
    float lastY;

    private void Awake()
    {
        speed = 5;
        scene = FindFirstObjectByType<SceneInstructionManager>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canMove = true;
        canJump = true;
        canCam = true;
        rigidBody = GetComponent<Rigidbody>();
        playerPassos = pesPlayer.gameObject.GetComponent<AudioSource>();
        playerPulo = GetComponent<AudioSource>();
        lastY = transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        JumpPlayer();
        if (Input.GetKeyDown(KeyCode.E) && canShay)
        {
            if (shayNivel != null)
            {
                shayNivel.InteragirShay();
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && podePegarObjetos)
        {
            GettingObjects();
        }
        if (canNarcisa)
        {
            if (narcisaNivel != null && canColidir)
            {
                narcisaNivel.ColidiuNarcisa();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                narcisaNivel.InteragirNarcisa();
            }
        }
        
    }

    void MovePlayer()
    {
        freelookCam.SetActive(canCam);
        animator.SetBool("Correr", false);
        playerPassos.Pause();
        if (canMove)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            inputs = new Vector3(horizontalInput, 0, verticalInput).normalized;
            
            Vector3 cameraForward = camTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = camTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 direction = cameraForward * inputs.z + cameraRight * inputs.x;

            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            if (direction != Vector3.zero)
            {
                animator.SetBool("Correr", true);
                transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * 10f);
                playerPassos.Play();
            }
        }
    }

    void JumpPlayer()
    {
        if (canJump)
        {
            isChao = Physics.CheckSphere(pesPlayer.position, distanceCheckChao, chaoLayerMask);
            if (isChao)
            {
                animator.SetBool("Pular", false);
                playerPulo.Stop();
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl)) && isChao)
            {
                if (puloClip != null)
                {
                    playerPulo.resource = puloClip;
                }   
                rigidBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                animator.SetBool("Pular", true);
                playerPulo.Play();
            }
        }
    }

    void GettingObjects()
    {
        if (objItem)
        {
            item = objItem.GetComponent<Coletaveis>();
        }
        if (item != null && scene.nameSceneAtual == "Fase1-Shay")
        {
            inventory.AddItem(item);
            if (shayNivel != null)
            {
                shayNivel.ColetarObjetos();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coletavel"))
        {
            podePegarObjetos = true;
            objItem = other.gameObject;
        }
        if (other.gameObject.CompareTag("Shay"))
        {
            canShay = true;
        }
        if (other.gameObject.CompareTag("Narcisa"))
        {
            canNarcisa = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Coletavel"))
        {
            podePegarObjetos = false;
            objItem = null;
        }
        if (other.gameObject.CompareTag("Shay"))
        {
            canShay = false;
        }
        if (other.gameObject.CompareTag("Narcisa"))
        {
            canNarcisa = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Narcisa"))
        {
            canNarcisa = true;
            canColidir = true; 
        }
    }
}
