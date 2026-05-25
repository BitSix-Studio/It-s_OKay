using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]//Cria o botão de interação em cima do jogador
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange; //variavel de verdadeiro ou falso se o jogador está dentro do collider trigger

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false); //vai fazer com que o botão desapareça no começo do jogo
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);//se o player estiver colidindo o botão irá aparecer

            if(Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        } 
        else
        {
            visualCue.SetActive(false);
        }//se o player estiver fora não irá
    }

    public void SetInkJSON(TextAsset novoInkJSON)
    {
        inkJSON = novoInkJSON;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

}
