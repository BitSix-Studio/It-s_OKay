using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerSpawnOnReturn : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FindFirstObjectByType<Player>() != null);
        Player player = FindFirstObjectByType<Player>();

        string currentScene = SceneManager.GetActiveScene().name;

        // Se voltando para uma cena com posição salva
        if (GameStateManager.Instance.HasSavedPosition(currentScene))
        {
            player.gameObject.transform.position = GameStateManager.Instance.GetSavedPosition(currentScene);
            Debug.Log("Jogador reposicionado ao voltar para " + currentScene);
        }
        else
        {
            Debug.Log("Sem posição salva para esta cena.");
        }
    }
}
