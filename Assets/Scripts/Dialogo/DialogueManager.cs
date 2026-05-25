using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    public bool dialogueAcabou;
    public bool possuiEscolhas;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    public Player player; //Chama o script do player

    private static DialogueManager instance; //criou o manager de dialogo

    private const string SPEAKER_TAG = "speaker"; //Cria uma variavel string para o nome que vai aparecer em baixo

    private const string PORTRAIT_TAG = "portrait"; //Cria uma variavel string para o quadro do personagem

    private const string LAYOUT_LAG = "layout"; //Cria uma variavel string para o lado que o quadro irá aparecer
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Tem mais do que um"); //procura no inicio do jogo se tem mais de uma instancia e avisa o jogador, tbm n entendi
        }
        instance = this;//chama o manager de dialogo no momento que o jogo começa
    }

    public static DialogueManager GetInstance()
    {
        return instance;//retorna uma instancia.
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        possuiEscolhas = false;
        dialoguePanel.SetActive(false);
        dialogueAcabou = false;

        //Pega o animador de Layout
        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        //pega todos os textos das escolhas
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        //se o dialogo não estiver acontecendo vai voltar normalmente
        if (!dialogueIsPlaying)
        {
            return;
        }

        //para dar continuidade ao dialogo ao interagir
        if (Input.GetKeyDown(KeyCode.Space) && possuiEscolhas == false)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);//dá inicio ao dialogo

        //reseta o retrato, o layout e a caixa de nome
        displayNameText.text = "Professora";
        portraitAnimator.Play("default");

        player.canMove = false;
        player.canJump = false;
        player.canCam = false;

        ContinueStory();    
        
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f); 
        //o bug do salto após dialogo é resolvido, esse codigo faz com tenha que esperar 0.2 segundos para poder usar o botão de interação novamente

        dialogueIsPlaying = false;
        dialogueAcabou = true;
        dialoguePanel.SetActive(false);//faz com que o painel do dialogo saia da tela do jogador
        dialogueText.text = "";

        player.canMove = true;
        player.canJump = true;
        player.canCam = true;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Coisa o texto do dialogo atual
            dialogueText.text = currentStory.Continue(); //verifica se o dialogo tem continuação 
            //mostra o dialogo(se tiver algum)
            DisplayChoices();

            // Atualiza possuiEscolhas com base nas escolhas ativas
            possuiEscolhas = currentStory.currentChoices.Count > 0;

            HandleTags(currentStory.currentTags); //responsavel pelas Tags
        }
        else
        {
            StartCoroutine(ExitDialogueMode());//caso não tem continuação, sairá do dialogo
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        //loopa por cada Tag e cuida delas de acordo com suas funções
        foreach(string tag in currentTags)
        {
            //analiza a tag
            string[] splitTag = tag.Split(':'); //retorna uma array
            if (splitTag.Length != 2)
            {
                Debug.LogError("A tag não pode ser analizada adequadamente: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey) //fica responsavel pela tag
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_LAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("A tag chegou a nós porém não pode ser implementada: " + tag);
                    break;
            }
        }
    }
    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //Dá a certeza que o botão de escolha suporta o tamanho da frase
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("São escolhas demais para um painel tão pequeno. Numero de escolhas dados: " + currentChoices.Count);
        }

        int index = 0;

        //permite e inicia as escolhas dependendo de quantas escolhas tem no dialogo
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;   
        }

        //Procura por escolhas que sobraram que a UI suporta e faz com que elas fiquem escondidas
        for(int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        //O event system pede pra gente limpar,e esperar (?)
        //Por pelo menos um frame depois de selecionarmos um objeto
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex) //confirma para o programa que vc escolheu umas das escolhas para prosseguir o dialogo
    {
        possuiEscolhas = false;
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
