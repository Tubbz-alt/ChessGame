﻿using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardManager : MonoBehaviour {

    public static BoardManager Instance{ set; get; }

    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	private int selectionX = -1;
	private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private Material previousMat;
    public Material selectedMat;

    public int[] EnPassantMove { set; get; }

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public bool isWhiteTurn = true;

    private void Start()
    {
        Instance = this;
        SpawnAllChessmans();
    }

    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8,8];
        EnPassantMove = new int[2] { -1, -1 };

        // spawn the white team

        //king 
        SpawnChessman(0, 3, 0);

        //queen
        SpawnChessman(1, 4, 0);

        //rooks
        SpawnChessman(2, 0, 0);
        SpawnChessman(2, 7, 0);

        //bishops
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);

        //knights
        SpawnChessman(4, 1, 0);
        SpawnChessman(4, 6, 0);

        //pawns
        for(int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1);
        }

        //spawn the black team

        //king 
        SpawnChessman(6, 4, 7);

        //queen
        SpawnChessman(7, 3, 7);

        //rooks
        SpawnChessman(8, 0, 7);
        SpawnChessman(8, 7, 7);

        //bishops
        SpawnChessman(9, 2, 7);
        SpawnChessman(9, 5, 7);

        //knights
        SpawnChessman(10, 1, 7);
        SpawnChessman(10, 6, 7);

        //pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6);
        }
    }
	// Use this for initialization
	private void DrawChessboard () 
	{
		Vector3 widthLine = Vector3.right * 8;
		Vector3 heightLine = Vector3.forward * 8;

		for (int i = 0; i <= 8; i++) 
		{
			Vector3 start = Vector3.forward * i;
			Debug.DrawLine (start, start + widthLine);
			for (int j = 0; j <= 8; j++) 
			{
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
		}

        //draw the selection
        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                           Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                           Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
	}

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans [x, y] = go.GetComponent<Chessman> ();
		Chessman debug = go.GetComponent<Chessman> ();
        Chessmans [x, y].SetPosition(x, y);

        if (y == 6 || y == 7) {
            Chessmans[x, y].isWhite = false;
        } else
        {
            Chessmans[x, y].isWhite = true;
        }
        activeChessman.Add (go);
    }
	
    private void UpdateSelection()
    {
        if(!Camera.main)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isWhiteTurn == true)
        {
            UpdateSelection();
            DrawChessboard();

            if (Input.GetMouseButtonDown(0))
            {
                if (selectionX >= 0 && selectionY >= 0)
                {
                    if (selectedChessman == null)
                    {
                        //Select the chessman
                        SelectChessman(selectionX, selectionY);
                    }
                    else
                    {
                        //Move the Chessman
                        MoveChessman(selectedChessman, selectionX, selectionY);
                    }
                }
            }
        }
        else
        {
            List<Move> moves = IterateMoves();
            Move m = GetRandomMove(moves);
            MoveChessman(m.GetChessman(), m.GetNewX(), m.GetNewY());
        }
	}

    private void SelectChessman(int x, int y)
    {
        if(Chessmans[x,y] == null)
            return;

        if (Chessmans[x, y].isWhite != isWhiteTurn)
            return;

        bool hasAtleastOneMove = false;
        bool[,] allowedMoves = Chessmans[x, y].PossibleMove();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (allowedMoves[i, j])
                    hasAtleastOneMove = true;

        if(!hasAtleastOneMove)
        {
            return;
        }

        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }
    
    private void MoveChessman(Chessman pieceToMove, int x, int y)
    {
        bool[,] allowedMoves = pieceToMove.PossibleMove();

        if (allowedMoves[x,y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                //Capture a piece

                //If it is King
                if(c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if(x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if(isWhiteTurn)
                {
                    c = Chessmans[x, y - 1];
                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                }
                else
                {
                    c = Chessmans[x, y + 1];
                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                }
            }
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;

            if(c != null && c.GetType() == typeof(Pawn))
            {
                if(y == 7)
                {
                    activeChessman.Remove(c.gameObject);                    
                    Destroy(c.gameObject);
                    SpawnChessman(1, x, y);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0)
                {
                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                    SpawnChessman(7, x, y);
                    selectedChessman = Chessmans[x, y];
                }

                if (selectedChessman.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y - 1;
                }
                else if (selectedChessman.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[0] = x;
                    EnPassantMove[1] = y + 1;
                }
            }

            Chessmans [pieceToMove.CurrentX, pieceToMove.CurrentY] = null;
            pieceToMove.transform.position = GetTileCenter(x, y);
            pieceToMove.SetPosition(x, y);
            Chessmans [x, y] = pieceToMove;
            isWhiteTurn = !isWhiteTurn;
        }

        if(isWhiteTurn == true)
        {
            selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
            BoardHighlights.Instance.HideHighlights();
            selectedChessman = null;
        }
        
    }

    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White team wins");
        else
            Debug.Log("Black team wins");

        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessmans();
    }

    private List<Move> IterateMoves()
    {
        List<Move> moves = new List<Move>();

        foreach(Chessman c in Chessmans) {

            if(c != null && c.isWhite == false)
            {
                bool[,] _moves = c.PossibleMove();

                for(int i = 0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        if(_moves[i,j] == true)
                        {

                            moves.Add(new Assets.Move(c, i, j));
                        }
                    }                
                }
            }
        }

        return moves;
    }


    private Move GetRandomMove(List<Move> moves)
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, moves.Count);

        Move moveToMake = moves[randomNumber];

        return moveToMake;
        

    }
}

