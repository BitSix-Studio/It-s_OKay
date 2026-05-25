using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInstructionManager : MonoBehaviour
{
    Scene cenaAtual;
    public string nameSceneAtual;
    void Start()
    {
        // Obtém a cena atual
        cenaAtual = SceneManager.GetActiveScene();
        nameSceneAtual = cenaAtual.name;
    }
}
