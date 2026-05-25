using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject panelPrincipal;
    public GameObject panelSobre;
    public GameObject panelComoJogar;

    public string nomeDaCenaDoJogo = ""; // Defina a primeira cena do jogo aqui
    private bool comoJogarAberto = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(comoJogarAberto)
                VoltarMenu();
        }
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    public void AbrirSobre()
    {
        panelPrincipal.SetActive(false);
        panelSobre.SetActive(true);
    }

    public void AbrirComoJogar()
    {
        panelPrincipal.SetActive(false);
        panelComoJogar.SetActive(true);
        comoJogarAberto=true;
    }

    public void VoltarMenu()
    {
        panelSobre.SetActive(false);
        panelComoJogar.SetActive(false);
        panelPrincipal.SetActive(true);
        comoJogarAberto=false;
    }

    public void SairJogo()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo."); // Só funciona fora do editor
    }
}