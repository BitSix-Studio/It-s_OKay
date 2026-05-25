using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject pausePanel;
    public GameObject comoJogarPanel;

    private bool jogoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (comoJogarPanel.activeSelf)
            {
                // Se estiver na tela de instrução, voltar pro pause
                VoltarParaPause();
            }
            else
            {
                if (jogoPausado)
                    ContinuarJogo();
                else
                    PausarJogo();
            }
        }
    }

    public void PausarJogo()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        jogoPausado = true;
    }

    public void ContinuarJogo()
    {
        pausePanel.SetActive(false);
        comoJogarPanel.SetActive(false);
        Time.timeScale = 1f;
        jogoPausado = false;
    }

    public void AbrirComoJogar()
    {
        comoJogarPanel.SetActive(true);
    }

    public void VoltarParaPause()
    {
        comoJogarPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // Altere para o nome da sua cena de menu
    }
}