using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//
// Author: Fenn Edmonds autumne@unr.edu
// Purpose: Controls the DaggerBoss
//

public class DaggerBossMgr : MonoBehaviour
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

    public GameObject doorCut;
    Animator doorOpen;
    bool cutinDone = false;

    public GameObject slash;
    Animator slashAni;
    bool slashDone = false;

    bool doorDone = false;
    [HideInInspector]
    public bool monsterDefeated = false;
    bool aniFinish = false;

    public List<GameObject> monsterList;
    public GameObject currentMonster;

    public GameObject hintBox;
    public TextMeshProUGUI hintText;

    public int winCon = 0;
    int winCounter = 0;

    bool hintUsed = false;
    bool solveUsed = false;

    public PlayerHealth playerHealth;

    public float startTime;

    bool canPress = true;

    public bool endless = false;

    public SolvesNHintsDagger SnHHolder;

    bool autoSolve = false;

    public Button solveB;
    public Button hintB;

    // Start is called before the first frame update
    void Start()
    {
        turns = 0;
        winCounter = 0;
        gameDone = false;
        turnsTxt.text = turns.ToString();
        RandomizePuzzle();
        string result = string.Join("", boardState);
        if (result == goalState)
        { RandomizePuzzle(); }
        SetupBoard();
        cutinAni = Cutin.GetComponent<Animator>();
        doorOpen = doorCut.GetComponent<Animator>();
        slashAni = slash.GetComponent<Animator>();
        Cutin.SetActive(false);
        hintBox.SetActive(false);
        hintUsed = false;
        solveUsed = false;
        doorCut.SetActive(true);
        startTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        startTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(startTime / 60);
        int seconds = Mathf.FloorToInt(startTime % 60);
        turnsTxt.text = string.Format("{0:00}: {1:00}", minutes, seconds);
        AnimatorStateInfo aInfo2 = doorOpen.GetCurrentAnimatorStateInfo(0);
        float NTime2 = aInfo2.normalizedTime;

        if (autoSolve == false)
        {
            int newseconds = Mathf.FloorToInt(startTime % 60);
            if (newseconds == 30 || (minutes > 0 && newseconds == 0))
                slash.SetActive(true);
        }

        if (NTime2 > 1.0f)
        {
            doorCut.SetActive(false);
            startTime = 0;
        }

        AnimatorStateInfo aInfo3 = slashAni.GetCurrentAnimatorStateInfo(0);
        float NTime3 = aInfo3.normalizedTime;

        if (NTime3 > 1.0f)
        {
            slash.SetActive(false);
            slashDone = true;
        }

        if (slashDone)
        {
            playerHealth.DealDamage(1);
            slashDone = false;
        }

        if (Input.GetMouseButtonDown(0))
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
                    // Debug.Log("Hit " + tile.tileNumber);
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

        if (selected && canPress == true)
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

        if (gameDone)
        {
            hintBox.SetActive(false);
            AnimatorStateInfo aInfo = cutinAni.GetCurrentAnimatorStateInfo(0);
            float NTime = aInfo.normalizedTime;

            if (NTime > 1.0f)
            {
                Cutin.SetActive(false);
                cutinDone = true;
            }

            if (cutinDone)
            {

                if (monsterDefeated == false)
                {
                    RandomizePuzzle();
                    string result = string.Join("", boardState);
                    if (result == goalState)
                    { RandomizePuzzle(); }
                    SetupBoard();
                    turns = 0;
                    // turnsTxt.text = turns.ToString();
                    cutinDone = false;
                    gameDone = false;
                    hintUsed = false;
                    solveUsed = false;
                    startTime = 0;
                }

                if (monsterDefeated == true)
                {
                    SnHHolder.UpdateStores();

                    if (endless)
                    {
                        SceneManager.LoadScene(11);
                    }
                    else
                        SceneManager.LoadScene(5);
                }
            }

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

        // SpawnMonster();
        // }
    }

    public void SpawnMonster()
    {
        int randomM = Random.Range(0, 3);
        switch (randomM)
        {
            case 0:
                currentMonster = Instantiate(monsterList[0], new Vector3(106.2f, -35f, 0), transform.rotation);
                break;

            case 1:
                currentMonster = Instantiate(monsterList[1], new Vector3(106.2f, -3.1f, 0), transform.rotation);
                break;

            case 2:
                currentMonster = Instantiate(monsterList[2], new Vector3(106.2f, -8.7f, 0), transform.rotation);
                break;
        }
        // currentMonster = Instantiate(monsterList[randomM], new Vector3(106.2f, -35f, 0), transform.rotation);
    }

    public void SolvePuzzle()
    {
        autoSolve = true;
        solveB.enabled = false;
        hintB.enabled = false;
        int[,] CurrentPuzzle = {
            { boardState[0], boardState[1], boardState[2] },
            { boardState[3], boardState[4], boardState[5] },
            { boardState[6], boardState[7], boardState[8] }
        };

        int findingZero = 0;
        for (int k = 0; k < 9; k++)
        {
            if (boardState[k] == 0)
                findingZero = k;
        }

        int x = findingZero / 3;   // row
        int y = findingZero % 3;   // col
        Debug.Log("Zero at (row=" + x + ", col=" + y + ")");
        List<string> solutionPath = solver.SolvePuzzleBFS(CurrentPuzzle, x, y);

        if (solutionPath != null)
        {
            Debug.Log("Solution found in " + (solutionPath.Count - 1) + " moves.");
            foreach (string state in solutionPath)
            {
                Debug.Log(state);
                // string formatted = string.Join(",", state.ToCharArray());
                // Debug.Log(formatted);
            }

            AnimateSolve(solutionPath);
            // CheckWin();
        }
        else
        {
            Debug.Log("No solution exists.");
        }

        // AnimateSolve(solutionPath);
    }

    void AnimateSolve(List<string> solution)
    {
        StartCoroutine(AnimateSolveCoroutine(solution));
    }

    IEnumerator AnimateSolveCoroutine(List<string> solution)
    {
        int currentMove = 0;
        bool moveDone = false;

        for (int i = 0; i < solution.Count - 1; i++)
        {
            string currentString = string.Join(",", solution[i].ToCharArray());
            string nextString = string.Join(",", solution[i + 1].ToCharArray());

            int[] currentBoard = currentString.Split(',').Select(int.Parse).ToArray();
            int[] nextBoard = nextString.Split(',').Select(int.Parse).ToArray();

            int cZeroIndex = 0;
            int nZeroIndex = 0;
            for (int j = 0; j < 9; j++)
            {
                if (currentBoard[j] == 0)
                    cZeroIndex = j;
            }

            for (int j = 0; j < 9; j++)
            {
                if (nextBoard[j] == 0)
                    nZeroIndex = j;
            }

            // Debug.Log(cZeroIndex);
            // Debug.Log(nZeroIndex);
            // if
            if (nZeroIndex + 3 < 9)
            {
                if (currentBoard[nZeroIndex + 3] == nextBoard[nZeroIndex])
                {
                    Debug.Log("Down " + currentMove);

                    Vector3 pos = Tiles[0].transform.position;
                    pos.y += 30;
                    Tiles[0].transform.position = pos;

                    int tileToMove = currentBoard[nZeroIndex];

                    Vector3 zpos = Tiles[tileToMove].transform.position;
                    zpos.y -= 30;
                    // Tiles[tileToMove].transform.position = pos;
                    yield return StartCoroutine(LerpPosition(Tiles[tileToMove], zpos, .5f));


                    boardState[cZeroIndex] = boardState[nZeroIndex];
                    boardState[nZeroIndex] = 0;
                }

            }

            if (nZeroIndex - 3 > -1)
            {
                if (currentBoard[nZeroIndex - 3] == nextBoard[nZeroIndex])
                {
                    Debug.Log("UP " + currentMove);
                    Vector3 pos = Tiles[0].transform.position;
                    pos.y -= 30;
                    Tiles[0].transform.position = pos;

                    int tileToMove = currentBoard[nZeroIndex];

                    Vector3 zpos = Tiles[tileToMove].transform.position;
                    zpos.y += 30;
                    // Tiles[tileToMove].transform.position = pos;
                    yield return StartCoroutine(LerpPosition(Tiles[tileToMove], zpos, .5f));


                    boardState[cZeroIndex] = boardState[nZeroIndex];
                    boardState[nZeroIndex] = 0;
                }
            }

            if (nZeroIndex - 1 > -1)
            {
                if (currentBoard[nZeroIndex - 1] == nextBoard[nZeroIndex])
                {
                    Debug.Log("Left " + currentMove);
                    Vector3 pos = Tiles[0].transform.position;
                    pos.x += 30;
                    Tiles[0].transform.position = pos;

                    int tileToMove = currentBoard[nZeroIndex];

                    Vector3 zpos = Tiles[tileToMove].transform.position;
                    zpos.x -= 30;
                    // Tiles[tileToMove].transform.position = pos;
                    yield return StartCoroutine(LerpPosition(Tiles[tileToMove], zpos, .5f));


                    boardState[cZeroIndex] = boardState[nZeroIndex];
                    boardState[nZeroIndex] = 0;
                }
            }

            if (nZeroIndex + 1 < 9)
            {
                if (currentBoard[nZeroIndex + 1] == nextBoard[nZeroIndex])
                {
                    Debug.Log("Right " + currentMove);
                    Vector3 pos = Tiles[0].transform.position;
                    pos.x -= 30;
                    Tiles[0].transform.position = pos;

                    int tileToMove = currentBoard[nZeroIndex];

                    Vector3 zpos = Tiles[tileToMove].transform.position;
                    zpos.x += 30;
                    // Tiles[tileToMove].transform.position = pos;
                    yield return StartCoroutine(LerpPosition(Tiles[tileToMove], zpos, .5f));

                    boardState[cZeroIndex] = boardState[nZeroIndex];
                    boardState[nZeroIndex] = 0;
                }
            }
            currentMove++;

            string word = boardState.Select(i => i.ToString()).Aggregate((i, j) => i + j);
            Debug.Log(word);

            solveUsed = true;
            CheckWin();


        }
        // yield return;
    }

    void CheckWin()
    {
        string result = string.Join("", boardState);
        // Debug.Log("Result: " + result);
        if (result == goalState && gameDone == false)
        {
            Debug.Log("You WIN!!!");
            Cutin.SetActive(true);
            // cutinAni = Cutin.GetComponent<Animator>();
            DaggerHealth mHealth = currentMonster.GetComponent<DaggerHealth>();
            if (hintUsed == false && solveUsed == false)
            {
                mHealth.DealDamage(3);
            }
            if (hintUsed == true && solveUsed == false)
            {
                mHealth.DealDamage(2);
            }
            if (solveUsed == true)
            {
                mHealth.DealDamage(1);
            }
            gameDone = true;
            autoSolve = false;
            solveB.enabled = true;
            hintB.enabled = true;
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
                            zpos.y -= 30;
                            Tiles[j].transform.position = zpos;

                        }

                    }

                    int zeroHold = foundIndex - 3;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
                    // turns += 1;
                    // turnsTxt.text = turns.ToString();
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
                            zpos.y += 30;
                            Tiles[j].transform.position = zpos;
                        }

                    }

                    int zeroHold = foundIndex + 3;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
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
                            zpos.x -= 30;
                            Tiles[j].transform.position = zpos;
                        }

                    }

                    int zeroHold = foundIndex + 1;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
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
                            zpos.x += 30;
                            Tiles[j].transform.position = zpos;
                        }

                    }

                    int zeroHold = foundIndex - 1;
                    boardState[zeroHold] = selectedTile;
                    boardState[foundIndex] = 0;
                }
                break;

        }
    }


    IEnumerator LerpPosition(TileMgr tile, Vector3 targetPosition, float duration)
    {
        canPress = false;
        float time = 0;
        Vector3 startPosition = tile.transform.position;

        while (time < duration)
        {
            tile.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        if (time >= duration)
        {
            canPress = true;
        }
        tile.transform.position = targetPosition;
    }

    void RandomizePuzzle()
    {
        int ScrambleNum = Random.Range(15, 30);
        int foundZ = 0;

        for (int i = 0; i <= ScrambleNum; i++)
        {
            int randomDirection = Random.Range(0, 4);

            switch (randomDirection)
            {
                case 0:
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            if (boardState[k] == 0)
                                foundZ = k;
                        }
                        int indexCheck = 0;
                        indexCheck = foundZ + 3;
                        if (indexCheck > 8)
                        { break; }

                        Vector3 pos = Tiles[0].transform.position;
                        pos.y -= 30;
                        Tiles[0].transform.position = pos;

                        int tileToMove = boardState[foundZ + 3];

                        Vector3 zpos = Tiles[tileToMove].transform.position;
                        zpos.y += 30;
                        Tiles[tileToMove].transform.position = zpos;


                        boardState[foundZ] = boardState[foundZ + 3];
                        boardState[foundZ + 3] = 0;
                        break;
                    }
                // break;

                case 1:
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            if (boardState[k] == 0)
                                foundZ = k;
                        }
                        int indexCheck = 0;
                        indexCheck = foundZ - 3;
                        if (indexCheck < 0)
                            break;

                        Vector3 pos = Tiles[0].transform.position;
                        pos.y += 30;
                        Tiles[0].transform.position = pos;

                        int tileToMove = boardState[foundZ - 3];

                        Vector3 zpos = Tiles[tileToMove].transform.position;
                        zpos.y -= 30;
                        Tiles[tileToMove].transform.position = zpos;


                        boardState[foundZ] = boardState[foundZ - 3];
                        boardState[foundZ - 3] = 0;
                        break;
                    }

                case 2:
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            if (boardState[k] == 0)
                                foundZ = k;
                        }
                        int indexCheck = 0;
                        indexCheck = foundZ - 1;
                        if (indexCheck < 0)
                            break;

                        Vector3 pos = Tiles[0].transform.position;
                        pos.x += 30;
                        Tiles[0].transform.position = pos;

                        int tileToMove = boardState[foundZ - 1];

                        Vector3 zpos = Tiles[tileToMove].transform.position;
                        zpos.x -= 30;
                        Tiles[tileToMove].transform.position = zpos;


                        boardState[foundZ] = boardState[foundZ - 1];
                        boardState[foundZ - 1] = 0;
                        break;
                    }
                case 3:
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            if (boardState[k] == 0)
                                foundZ = k;
                        }
                        int indexCheck = 0;
                        indexCheck = foundZ + 1;
                        if (indexCheck > 8)
                            break;

                        Vector3 pos = Tiles[0].transform.position;
                        pos.x -= 30;
                        Tiles[0].transform.position = pos;

                        int tileToMove = boardState[foundZ + 1];

                        Vector3 zpos = Tiles[tileToMove].transform.position;
                        zpos.x += 30;
                        Tiles[tileToMove].transform.position = zpos;


                        boardState[foundZ] = boardState[foundZ + 1];
                        boardState[foundZ + 1] = 0;
                        break;
                    }
            }
        }
    }

    public void getHint()
    {
        int tileToHint = 0;
        string directionToHint = "";
        int[,] CurrentPuzzle = {
            { boardState[0], boardState[1], boardState[2] },
            { boardState[3], boardState[4], boardState[5] },
            { boardState[6], boardState[7], boardState[8] }
        };

        int findingZero = 0;
        for (int k = 0; k < 9; k++)
        {
            if (boardState[k] == 0)
                findingZero = k;
        }

        int x = findingZero / 3;   // row
        int y = findingZero % 3;   // col
        Debug.Log("Zero at (row=" + x + ", col=" + y + ")");
        List<string> hintSolutionPath = solver.SolvePuzzleBFS(CurrentPuzzle, x, y);

        if (hintSolutionPath != null)
        {
            Debug.Log("Solution found in " + (hintSolutionPath.Count - 1) + " moves.");
        }

        string currentString = string.Join(",", hintSolutionPath[0].ToCharArray());
        string nextString = string.Join(",", hintSolutionPath[1].ToCharArray());

        int[] currentBoard = currentString.Split(',').Select(int.Parse).ToArray();
        int[] nextBoard = nextString.Split(',').Select(int.Parse).ToArray();

        int cZeroIndex = 0;
        int nZeroIndex = 0;
        for (int j = 0; j < 9; j++)
        {
            if (currentBoard[j] == 0)
                cZeroIndex = j;
        }

        for (int j = 0; j < 9; j++)
        {
            if (nextBoard[j] == 0)
                nZeroIndex = j;
        }

        if (nZeroIndex + 3 < 9)
        {
            if (currentBoard[nZeroIndex + 3] == nextBoard[nZeroIndex])
            {
                tileToHint = currentBoard[nZeroIndex];
                directionToHint = "Down";
            }
        }

        if (nZeroIndex - 3 > -1)
        {
            if (currentBoard[nZeroIndex - 3] == nextBoard[nZeroIndex])
            {
                tileToHint = currentBoard[nZeroIndex];
                directionToHint = "Up";
            }
        }

        if (nZeroIndex - 1 > -1)
        {
            if (currentBoard[nZeroIndex - 1] == nextBoard[nZeroIndex])
            {
                tileToHint = currentBoard[nZeroIndex];
                directionToHint = "Left";
            }
        }

        if (nZeroIndex + 1 < 9)
        {
            if (currentBoard[nZeroIndex + 1] == nextBoard[nZeroIndex])
            {
                tileToHint = currentBoard[nZeroIndex];
                directionToHint = "Right";
            }
        }

        hintBox.SetActive(true);
        string tileText = tileToHint.ToString();
        hintText.text = "Move " + tileText + " " + directionToHint;

        hintUsed = true;
    }


}
