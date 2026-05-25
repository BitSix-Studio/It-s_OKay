using TMPro;
using UnityEngine;

public class ShayNivel : MonoBehaviour
{
    private int quantObj;
    private int quantObjCollect;

    public TextMeshProUGUI numberText;
    public TextMeshProUGUI missionText;
    public GameObject colliderFala;
    private HUDManager hudManager;
    public GameObject portaSaida;
    public bool finalizouMissions = false;

    private GameObject shay;
    private Animator animatorShay;

    CanvasRenderer[] objetivos;
    public GameObject planeTecla;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderFala.gameObject.SetActive(false);
        quantObj = 3;
        quantObjCollect = 0;
        hudManager = FindAnyObjectByType<HUDManager>();
        shay = GameObject.FindWithTag("Shay");
        animatorShay = shay.GetComponent<Animator>();
        animatorShay.SetBool("EntregouItens", false);
        objetivos = hudManager.gameObject.GetComponentsInChildren<CanvasRenderer>();
        shay.GetComponent<AparecerTecla>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (quantObjCollect == quantObj && !shay.GetComponent<AparecerTecla>().enabled && finalizouMissions == false)
        {
            shay.GetComponent<AparecerTecla>().enabled = true;
        }
        if (finalizouMissions == true)
        {
            colliderFala.gameObject.SetActive(false);
            shay.GetComponent<AparecerTecla>().enabled = false;
            planeTecla.gameObject.SetActive(false);
        }
    }

    public void ColetarObjetos()
    {
        if (quantObjCollect < quantObj)
        {
            quantObjCollect++;
            numberText.text = "("+quantObjCollect+"/"+quantObj+")";
        }
        if (quantObjCollect == quantObj)
        {
            missionText.text = "Leve os objetos para a Criança";
            numberText.gameObject.SetActive(false);
        }
    }

    public void InteragirShay()
    {
        if (!objetivos[4].gameObject.activeSelf)
        {
            objetivos[1].gameObject.SetActive(false);
        }
        if (quantObjCollect < quantObj)
        {
            //Implementar o diálogo que ainda falta outro item e aumentar a dificuldade do nível a cada item
        }
        else if (quantObjCollect == quantObj)
        {
            animatorShay.SetBool("EntregouItens", true);
            if (colliderFala.gameObject.activeSelf)
            {
                finalizouMissions = true;
                portaSaida.GetComponentInChildren<Renderer>().material.color = Door.FindFirstObjectByType<Door>().doorColor;
            }
            OndaAqua ondaAqua = FindFirstObjectByType<OndaAqua>();
            if (ondaAqua != null) {
                ondaAqua.gameObject.SetActive(false);
            }
            GameObject[] nuvens = GameObject.FindGameObjectsWithTag("Nuvem");
            foreach (GameObject nuvem in nuvens)
            {
                nuvem.gameObject.SetActive(false);
            }

            if (objetivos[4] != null) { objetivos[4].gameObject.SetActive(false); }
            missionText.text = "Fale com Shay, a criança de cabelo vermelho";

            colliderFala.gameObject.SetActive(true);
        }
    }
}
