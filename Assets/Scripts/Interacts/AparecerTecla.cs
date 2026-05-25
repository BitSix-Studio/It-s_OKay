using UnityEngine;

public class AparecerTecla : MonoBehaviour
{
    public GameObject planeTecla;

    private bool aparecerTecla;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aparecerTecla = false;
        planeTecla.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (planeTecla != null)
        {
            if (aparecerTecla)
            {
                planeTecla.SetActive(true);
            }
            else
            {
                planeTecla.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            aparecerTecla = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            aparecerTecla = false;
        }
    }
}
