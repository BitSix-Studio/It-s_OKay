using UnityEngine;
using UnityEngine.AI;

public class CorrerGritar : MonoBehaviour
{
    [SerializeField] private NavMeshAgent AI;
    [SerializeField] private float velocidade;
    [SerializeField] private Transform[] pontosMovimento;
    Transform pontoAtual;
    [SerializeField] private float distancia;
    int ultimoIndex = -1;

    [Header("Animações")]
    private Animator anim;

    public bool canCorrer;

    private void Start()
    {
        canCorrer = false;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canCorrer)
        {
            anim.SetBool("Correr", true);
            distancia = Vector3.Distance(transform.position, pontoAtual.position);

            if (distancia < 2f)
            {
                EscolherNovoPonto();
            }

            AI.speed = velocidade;
            transform.position = new Vector3(AI.transform.position.x, transform.position.y, AI.transform.position.z);
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else
        {
            anim.SetBool("Correr", false);
        }
    }

    public void EscolherNovoPonto()
    {
        int novoIndex;
        do
        {
            novoIndex = Random.Range(0, pontosMovimento.Length);
        } while (novoIndex == ultimoIndex && pontosMovimento.Length > 1);

        ultimoIndex = novoIndex;
        pontoAtual = pontosMovimento[novoIndex];
        AI.SetDestination(pontoAtual.position);
    }

}
