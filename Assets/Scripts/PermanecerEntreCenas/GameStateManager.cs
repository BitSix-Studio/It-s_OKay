using UnityEngine;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    // Guarda posição por nome de cena
    private Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();
    private Dictionary<string, Color> portaCores = new Dictionary<string, Color>();

    public string entranceDoorID;        // ID da porta usada
    public string previousSceneName; // Para saber de onde veio

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Para a posição do Player
    public void SavePlayerPosition(string sceneName, Vector3 position)
    {
        savedPositions[sceneName] = position;
    }

    public bool HasSavedPosition(string sceneName)
    {
        return savedPositions.ContainsKey(sceneName);
    }

    public Vector3 GetSavedPosition(string sceneName)
    {
        return savedPositions[sceneName];
    }

    //Para a cor da porta
    public void SetPortaCor(string doorID, Color cor)
    {
        portaCores[doorID] = cor;
    }

    public bool HasPortaCor(string doorID)
    {
        return portaCores.ContainsKey(doorID);
    }

    public Color GetPortaCor(string doorID, Color corPadrao)
    {
        return portaCores.TryGetValue(doorID, out Color cor) ? cor : corPadrao;
    }
}
