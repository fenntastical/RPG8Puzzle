using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using TMPro;
using System;
using UnityEngine.SceneManagement;

//
// Author: Fenn Edmonds
// Purpose: Controls the tutorial level
//

public class TutorialMgr : MonoBehaviour
{
    public int selectedTile = 10;
    // public List<GameObject> mainTiles;
    public List<TileMgr> Tiles;
    float tileMove = 22.5f;
    float boardPos;

    public int[] boardState;
    string goalState = "123456780";

    bool selected = false;


    public PuzzleSolver solver;

    float boundxMax = 30f, boundxMin = -30f, boundyMax = 30f, boundyMin = -30f;

    int turns;
    public TextMeshProUGUI turnsTxt;

    public GameObject Cutin;

    bool gameDone = false;
    Animator cutinAni;
    bool cutinDone = false;
    [HideInInspector]
    public bool monsterDefeated = false;
    bool aniFinish = false;

    [HideInInspector]
    public bool firstDialogue = false;

    public Dialogue dialogue;
    bool inprogressStart = false;

    public GameObject infoText;

    // Start is called before the first frame update
    void Start()
    {
        turns = 0;
        gameDone = false;
        turnsTxt.text = turns.ToString();
        // RandomizePuzzle();
        SetupBoard();
        cutinAni = Cutin.GetComponent<Animator>();
        Cutin.SetActive(false);
        infoText.SetActive(false);
        // dialogue.StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (inprogressStart == false)
            dialogue.StartDialogue();
        inprogressStart = true;
        if (firstDialogue)
        { infoText.SetActive(true);}
        if (Input.GetMouseButtonDown(0) && firstDialogue == true)
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit.collider != null)
                {
                    TileMgr tile = hit.collider.GetComponent<TileMgr>();
                    if (tile != null)
                    {
                        selectedTile = tile.tileNumber;
                        foreach (TileMgr tileChange in Tiles)
                        {
                            tileChange.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        tile.GetComponent<SpriteRenderer>().color = Color.red;
                        Debug.Log("Hit " + tile.tileNumber);
                        selected = true;
                    }
                    if (tile == null || tile.tileNumber == 0)
                    {
                        foreach (TileMgr tileChange in Tiles)
                        {
                            tileChange.GetComponent<SpriteRenderer>().color = Color.white;
                        }
                        selectedTile = 10;
                        selected = false;
                    }
                }
            }

        if (selected)
        {
            if (Input.GetKeyDown("w"))
                MoveTile("Up");
            if (Input.GetKeyDown("d"))
                MoveTile("Right");
            if (Input.GetKeyDown("s"))
                MoveTile("Down");
            if (Input.GetKeyDown("a"))
                MoveTile("Left");

            CheckWin();
        }

        if (gameDone && cutinDone == false)
        {
            AnimatorStateInfo aInfo = cutinAni.GetCurrentAnimatorStateInfo(0);
            float NTime = aInfo.normalizedTime;

            if (NTime > 1.0f)
            {
                Cutin.SetActive(false);
                cutinDone = true;
            }

            if (cutinDone)
            {
                firstDialogue = false;
                infoText.SetActive(false);
                // string[] newD = new string
                // Array.Clear(dialogue.lines, 0, dialogue.lines.Length);
                // dialogue.lines[0] = "Nice Job! Now good luck on your own!";
                // dialogue.lines[1] = "Feel free to use hints, but beware it may cause your damage to lessen.";
                dialogue.dialogue2 = true;
                dialogue.StartDialogue2();
            }

        }

        if (cutinDone && firstDialogue)
        {
            SceneManager.LoadScene(2);
        }

    }

    void SetupBoard()
    {
        boundxMax += boardPos;
        boundxMin += boardPos;
        boundyMax += boardPos;
        boundyMin += boardPos;

        float[] setupPosX = { boundxMin, boardPos, boundxMax, boundxMin, boardPos, boundxMax, boundxMin, boardPos, boundxMax, };
        float[] setupPosY = { boundyMax, boundyMax, boundyMax, boardPos, boardPos, boardPos, boundyMin, boundyMin, boundyMin };

        // foreach (var tile in boardState)
        // {
        for (int i = 0; i < boardState.Length; i++)
        {
            for (int j = 0; j < Tiles.Count; j++)
            {
                if (boardState[i] == Tiles[j].tileNumber)
                {
                    Vector3 pos = Tiles[j].transform.position;
                    pos.x = setupPosX[i];
                    pos.y = setupPosY[i];
                    Tiles[j].transform.position = pos;
                }

            }
        }
    }

    void CheckWin()
    {
        string result = string.Join("", boardState);
        Debug.Log("Result: " + result);
        if (result == goalState && gameDone == false)
        {
            Debug.Log("You WIN!!!");
            Cutin.SetActive(true);
            gameDone = true;
            // Array.Clear(dialogue.lines, 0, dialogue.lines.Length); 
            // dialogue.lines[0] = "Nice Job! Now good luck on your own!";
            // dialogue.lines[1] = "Feel free to use hints, but beware it may cause your damage to lessen.";
        }
    }
    void MoveTile(string direction)
    {
        int foundIndex = 0;
        int foundZero = 0;
        int indexCheck = 0;

        switch (direction)
        {
            case "Up":
                // int foundIndex;
                for (int i = 0; i < 9; i++)
                {
                    if (boardState[i] == selectedTile)
                        foundIndex = i;
                }

                indexCheck = foundIndex - 3;
                if (indexCheck < 0)
                    break;


                // Debug.Log(boardState[foundIndex - 3]);
                if (boardState[foundIndex - 3] == 0)
                {

                    Vector3 pos = Tiles[selectedTile].transform.position;
                    pos.y += 30;
                    StartCoroutine(LerpPosition(Tiles[selectedTile], pos, .5f));
                    // Tiles[selectedTile].transform.position = pos;

                    for (int j = 0; j < Tiles.Count; j++)
                    {
                        if (Tiles[j].tileNumber == 0)
                        {
                            Vector3 zpos = Tiles[j].transform.position;
                            pos.y -= 30;
                            Tiles[j].transform.position = pos;

                        }

                    }

                    int zeroHold = foundIndex - 3;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
                    turns += 1;
                    turnsTxt.text = turns.ToString();
                }
                // }
                break;
            case "Down":
                // int foundIndex;
                for (int i = 0; i < 9; i++)
                {
                    if (boardState[i] == selectedTile)
                        foundIndex = i;

                }

                indexCheck = foundIndex + 3;
                if (indexCheck > 8)
                    break;

                // Debug.Log(boardState[foundIndex + 3]);
                // Debug.Log("Down");
                if (boardState[foundIndex + 3] == 0)
                {

                    Vector3 pos = Tiles[selectedTile].transform.position;
                    pos.y -= 30;
                    // Tiles[selectedTile].transform.position = pos;
                    StartCoroutine(LerpPosition(Tiles[selectedTile], pos, .5f));

                    for (int j = 0; j < Tiles.Count; j++)
                    {
                        if (Tiles[j].tileNumber == 0)
                        {
                            Vector3 zpos = Tiles[j].transform.position;
                            pos.y += 30;
                            Tiles[j].transform.position = pos;
                        }

                    }

                    int zeroHold = foundIndex + 3;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
                    turns += 1;
                    turnsTxt.text = turns.ToString();
                }
                break;
            case "Right":
                for (int i = 0; i < 9; i++)
                {
                    if (boardState[i] == selectedTile)
                        foundIndex = i;
                }
                for (int k = 0; k < 9; k++)
                {
                    if (boardState[k] == 0)
                        foundZero = k;
                }

                indexCheck = foundIndex + 1;
                if (indexCheck > 8)
                    break;

                // Debug.Log(boardState[foundIndex + 1]);
                // Debug.Log(foundZero);
                if (boardState[foundIndex + 1] == 0 && foundZero != 3 && foundZero != 6)
                {

                    Vector3 pos = Tiles[selectedTile].transform.position;
                    pos.x += 30;
                    // Tiles[selectedTile].transform.position = pos;
                    StartCoroutine(LerpPosition(Tiles[selectedTile], pos, .5f));

                    for (int j = 0; j < Tiles.Count; j++)
                    {
                        if (Tiles[j].tileNumber == 0)
                        {
                            Vector3 zpos = Tiles[j].transform.position;
                            pos.x -= 30;
                            Tiles[j].transform.position = pos;
                        }

                    }

                    int zeroHold = foundIndex + 1;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
                    turns += 1;
                    turnsTxt.text = turns.ToString();
                }
                break;
            case "Left":
                for (int i = 0; i < 9; i++)
                {
                    if (boardState[i] == selectedTile)
                        foundIndex = i;
                }
                for (int k = 0; k < 9; k++)
                {
                    if (boardState[k] == 0)
                        foundZero = k;
                }

                indexCheck = foundIndex - 1;
                if (indexCheck < 0)
                    break;

                // Debug.Log(boardState[foundIndex - 1]);
                // Debug.Log(foundZero);
                if (boardState[foundIndex - 1] == 0 && foundZero != 2 && foundZero != 5)
                {

                    Vector3 pos = Tiles[selectedTile].transform.position;
                    pos.x -= 30;
                    // Tiles[selectedTile ].transform.position = pos;
                    StartCoroutine(LerpPosition(Tiles[selectedTile], pos, .5f));

                    for (int j = 0; j < Tiles.Count; j++)
                    {
                        if (Tiles[j].tileNumber == 0)
                        {
                            Vector3 zpos = Tiles[j].transform.position;
                            pos.x += 30;
                            Tiles[j].transform.position = pos;
                        }

                    }

                    int zeroHold = foundIndex - 1;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
                    turns += 1;
                    turnsTxt.text = turns.ToString();
                }
                break;
        }
    }


    IEnumerator LerpPosition(TileMgr tile, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = tile.transform.position;

        while (time < duration)
        {
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        tile.transform.position = targetPosition;
    }

}
