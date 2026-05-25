using Unity.VisualScripting;
using UnityEngine;

public class OndaAqua : MonoBehaviour
{
    private Vector3 tamanhoMaxOnda;
    private float maxOndaX = 60f, maxOndaZ = 60f;

    private Vector3 scalePadraoObj;

    private Vector3 tamanhoCrescimento;
    private float crescimentoEmX = 10f, crescimentoEmZ = 10f;

    private Player player;

    public bool canArrastar;

    private AudioSource ondaSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tamanhoMaxOnda = new Vector3(maxOndaX, 1f, maxOndaZ);
        tamanhoCrescimento = new Vector3(crescimentoEmX, 0f, crescimentoEmZ);
        scalePadraoObj = transform.localScale;

        player = FindFirstObjectByType<Player>();
        ondaSound = GetComponent<AudioSource>();
        ondaSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        ExpandWave();
        if (canArrastar)
        {
            ArrastarPlayer();
        }
    }

    void ExpandWave()
    {
        if (transform.localScale.x < tamanhoMaxOnda.x)
        {
            transform.localScale += tamanhoCrescimento * Time.deltaTime;

            transform.localScale = Vector3.Min(transform.localScale, tamanhoMaxOnda);
            if (transform.localScale.x == tamanhoMaxOnda.x)
            {
                ondaSound.Stop();
            }
        }
        else
        {
            ondaSound.Play();
            transform.localScale = scalePadraoObj;
        }
    }

    void ArrastarPlayer()
    {
        if (transform.localScale.x < tamanhoMaxOnda.x)
        {
            player.transform.position += new Vector3(3f, 0, 3f) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.canMove = false;
            player.canJump = false;
            canArrastar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.canMove = true;
            player.canJump = true;
            canArrastar = false;
        }
    }
}
