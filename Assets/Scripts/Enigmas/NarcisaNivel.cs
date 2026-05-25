using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Ink.Parsed;

public class NarcisaNivel : MonoBehaviour
{
    private int quantVezesColidir;
    private int quantMaxForColidir;

    private HUDManager hudManager;
    public TextMeshProUGUI numberText;
    public TextMeshProUGUI missionText;

    private GameObject narcisa;
    private Animator animatorNarcisa;
    public GameObject colliderFala;
    public GameObject planeTecla;

    private bool canCollideAgain = true;
    public float cooldownEntreColisoes = 2f;

    private Player player;
    private CorrerGritar correrGritar;
    private DialogueManager dialogueManager;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private TextAsset dialogoFinalInk;
    [HideInInspector] public bool dialogoFinalCompleto = false;

    public GameObject painelFinal;

    public Collider mudarMissionCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        correrGritar = FindFirstObjectByType<CorrerGritar>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        quantMaxForColidir = 3;
        quantVezesColidir = 0;
        hudManager = FindAnyObjectByType<HUDManager>();
        narcisa = GameObject.FindWithTag("Narcisa");
        animatorNarcisa = narcisa.GetComponent<Animator>();
        numberText.gameObject.SetActive(false);
        planeTecla.gameObject.SetActive(true);
        colliderFala.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (quantVezesColidir == quantMaxForColidir)
        {
            missionText.text = "Fale com Narcisa";
            numberText.gameObject.SetActive(false);
            InteragirNarcisa();
        }
        if (dialogueManager.dialogueAcabou == true)
        {
            FimDialogo();
        }

        if (DetectCollisionMission())
        {
            missionText.text = "Fale com Narcisa, a criança no palco";
            mudarMissionCollider.gameObject.SetActive(false);
        }
    }

    public void ColidiuNarcisa()
    {
        if (quantVezesColidir < quantMaxForColidir && canCollideAgain && correrGritar.canCorrer == true)
        {
            // Empurra o player na direção oposta à Narcisa
            Vector3 direcaoEmpurro = new Vector3(player.gameObject.transform.position.x - 5f, 0, player.gameObject.transform.position.z - 5f).normalized;
            Rigidbody rb = player.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                float forcaEmpurro = 3f;
                rb.AddForce(direcaoEmpurro * forcaEmpurro, ForceMode.Impulse);
            }

            quantVezesColidir++;
            numberText.text = "(" + quantVezesColidir + "/" + quantMaxForColidir + ")";

            // Inicia cooldown
            StartCoroutine(CooldownColisao());
        }
    }

    private IEnumerator CooldownColisao()
    {
        canCollideAgain = false;
        yield return new WaitForSeconds(cooldownEntreColisoes);
        canCollideAgain = true;
        player.canNarcisa = false;
        player.canColidir = false;
    }

    public void InteragirNarcisa()
    {
        if (quantVezesColidir == quantMaxForColidir)
        {
            colliderFala.gameObject.SetActive(true);
            planeTecla.gameObject.SetActive(true);
            correrGritar.canCorrer = false;
            dialogoFinalCompleto = true;
        }
        else
        {
            missionText.text = "A Narcisa precisa ser parada, entre na frente dela!";
            numberText.gameObject.SetActive(true);
        }
    }

    private void FimDialogo()
    {
        if (correrGritar.canCorrer == false && dialogoFinalCompleto == false)
        {
            correrGritar.canCorrer = true;
            correrGritar.EscolherNovoPonto();
            dialogueManager.dialogueAcabou = false;
            colliderFala.gameObject.SetActive(false);
            planeTecla.gameObject.SetActive(false);
            dialogueTrigger.SetInkJSON(dialogoFinalInk);
            StartCoroutine(CooldownColisao());
        }
        else if(dialogoFinalCompleto == true)
        {
            colliderFala.gameObject.SetActive(false);
            planeTecla.gameObject.SetActive(false);
            missionText.text = "Saia da sala pela porta que entrou!";
            GameObject[] espelhos = GameObject.FindGameObjectsWithTag("Espelhos");
            foreach (GameObject espelho in espelhos)
            {
                espelho.gameObject.SetActive(false);
            }

        }
    }

    bool DetectCollisionMission()
    {
        Collider[] colididos = Physics.OverlapSphere(player.transform.position, 1.5f);

        foreach (Collider colider in colididos)
        {
            if (colider == mudarMissionCollider)
            {
                return true;
            }
        }

        return false;
    }
}
