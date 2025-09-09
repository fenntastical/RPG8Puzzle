using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;

    public string[] lines2;
    public float textSpeed;
    public PanelMover textbox;

    private int index;
    public bool inprogress;

    public TutorialMgr tutorialMgr;

    [HideInInspector]
    public bool dialogue2 = false;
    public bool d1 = true;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (d1 == true)
            {
                if (textComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }

            if (dialogue2 == true && d1 == false)
            {
                if (textComponent.text == lines2[index])
                {
                    NextLine2();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines2[index];
                }
            }
        }
    }

    public void StartDialogue()
    {
        //     GameEventsManager.instance.miscEvents.PatronTalked();
        textComponent.text = string.Empty;
        index = 0;
        textbox.isVisible = true;
        inprogress = true;
        //     GameEventsManager.instance.playerEvents.DisablePlayerMovement();
        StartCoroutine(TypeLine());
        //     // for(int i = 0; i < lines.Length; i++)
        //     //     NextLine();
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {

        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            tutorialMgr.firstDialogue = true;
            textbox.isVisible = false;
            inprogress = false;
            d1 = false;
            // dialogue2 = true;
            // tutorialMgr.firstDialogue = true;
            // GameEventsManager.instance.playerEvents.EnablePlayerMovement();
        }
    }
    
    public void StartDialogue2()
    {
    //     GameEventsManager.instance.miscEvents.PatronTalked();
        textComponent.text = string.Empty;
        index = 0;
        textbox.isVisible = true;
        inprogress = true;
    //     GameEventsManager.instance.playerEvents.DisablePlayerMovement();
        StartCoroutine(TypeLine2());
    //     // for(int i = 0; i < lines.Length; i++)
    //     //     NextLine();
    }

    IEnumerator TypeLine2()
    {
        foreach (char c in lines2[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine2()
    {

        if (index < lines2.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine2());
        }
        else
        {
            // tutorialMgr.firstDialogue = true;
            textbox.isVisible = false;
            inprogress = false;
            SceneManager.LoadScene(3);
            // tutorialMgr.firstDialogue = true;
            // GameEventsManager.instance.playerEvents.EnablePlayerMovement();
        }
    }
    
}