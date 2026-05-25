using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string doorID;
    public string sceneToLoad, nextDoorID;
    public Color changedColor = Color.red;
    public string newTagOnReturn = "PortaFechada"; // Tag a aplicar ao retornar

    private bool canEnter = false;
    private bool canExit = false;

    private Renderer rend;
    private ShayNivel shayNivel;
    private NarcisaNivel narcisaNivel;
    private SceneInstructionManager scene;

    public Color doorColor;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        shayNivel = FindFirstObjectByType<ShayNivel>();
        narcisaNivel = FindFirstObjectByType<NarcisaNivel>();
        scene = FindFirstObjectByType<SceneInstructionManager>();

        // Restaura a cor da porta, se já tiver sido usada
        if (GameStateManager.Instance.HasPortaCor(doorID))
        {
            rend.material.color = GameStateManager.Instance.GetPortaCor(doorID, rend.material.color);
        }

        if (GameStateManager.Instance.entranceDoorID == "Porta_Corredor")
        {
            if (GameStateManager.Instance.previousSceneName == "Fase1-Shay")
            {
                gameObject.tag = newTagOnReturn;
                Debug.Log($"[Door] TAG da porta {doorID} alterada para {newTagOnReturn} (entrada reconhecida)");
                if (nextDoorID == "Fase2-Narcisa")
                {
                    GameObject doorNarcisa = GameObject.Find("portaNarcisa");
                    doorNarcisa.gameObject.tag = "EntradaFase";
                    doorNarcisa.GetComponentInChildren<Renderer>().material.color = doorColor;
                }
            }
            
        }
        else
        {
            Debug.Log($"[Door] Porta {doorID} não é a usada para entrar. Tag mantida: {gameObject.tag}");
        }
    }

    void Update()
    {
        if ((canEnter || canExit) && Input.GetKeyDown(KeyCode.F))
        {
            Player player = FindFirstObjectByType<Player>();
            if (player == null) return;

            string currentScene = SceneManager.GetActiveScene().name;

            // Salva posição do jogador e ID da porta usada
            GameStateManager.Instance.SavePlayerPosition(currentScene, player.transform.position);
            GameStateManager.Instance.previousSceneName = currentScene;
            GameStateManager.Instance.entranceDoorID = doorID;

            // Salva a cor da porta
            rend.material.color = changedColor;
            GameStateManager.Instance.SetPortaCor(doorID, changedColor);

            // Troca de cena
            if (sceneToLoad != "" && sceneToLoad != null) 
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else if (scene != null && scene.nameSceneAtual == "Fase2-Narcisa")
            {
                narcisaNivel.painelFinal.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("EntradaFase"))
            {
                canEnter = true;
            }
            if (this.gameObject.CompareTag("SaidaFase"))
            {
                if (shayNivel != null)
                {
                    if (shayNivel.finalizouMissions == true)
                    {
                        canExit = true;
                    }
                }
                else if (narcisaNivel != null)
                {
                    if (narcisaNivel.dialogoFinalCompleto == true)
                    {
                        canExit = true;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("SaidaFase"))
            {
                canExit = false;
            }
            if (this.gameObject.CompareTag("EntradaFase"))
            {
                canEnter = false;
            }
        }
    }
}