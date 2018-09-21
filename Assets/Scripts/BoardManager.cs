using ChessGame.AI.Interface;
using ChessGame.AI;
using ChessGame.Pieces;
using ChessGame.Pieces.Interface;
using System.Collections.Generic;
using UnityEngine;
using ChessGame;

namespace ChessGame
{
    public class BoardManager : MonoBehaviour
    {
        public static BoardManager Instance { set; get; }
        public Piece[,] CurrentBoard { set; get; }
        public List<GameObject> piecePrefabs;
        public Material selectedMat;
        public int[] EnPassantMove { set; get; }
        public bool isWhiteTurn = true;

        private const float TILE_SIZE = 1.0f;
        private const float TILE_OFFSET = 0.5f;
        private Piece selectedPiece;
        private int selectionX = -1;
        private int selectionY = -1;        
        private List<GameObject> activePiece;
        private Material previousMat;        
        private Quaternion orientation = Quaternion.Euler(0, 180, 0);        

        private void Start()
        {
            Instance = this;
            SpawnAllPieces();
        }

        private void SpawnAllPieces()
        {
            activePiece = new List<GameObject>();
            CurrentBoard = new Piece[8, 8];
            EnPassantMove = new int[2] { -1, -1 };

            // spawn the white team

            //king 
            SpawnPiece(0, 3, 0);

            //queen
            SpawnPiece(1, 4, 0);

            //rooks
            SpawnPiece(2, 0, 0);
            SpawnPiece(2, 7, 0);

            //bishops
            SpawnPiece(3, 2, 0);
            SpawnPiece(3, 5, 0);

            //knights
            SpawnPiece(4, 1, 0);
            SpawnPiece(4, 6, 0);

            //pawns
            for (int i = 0; i < 8; i++)
            {
                SpawnPiece(5, i, 1);
            }

            //spawn the black team

            //king 
            SpawnPiece(6, 4, 7);

            //queen
            SpawnPiece(7, 3, 7);

            //rooks
            SpawnPiece(8, 0, 7);
            SpawnPiece(8, 7, 7);

            //bishops
            SpawnPiece(9, 2, 7);
            SpawnPiece(9, 5, 7);

            //knights
            SpawnPiece(10, 1, 7);
            SpawnPiece(10, 6, 7);

            //pawns
            for (int i = 0; i < 8; i++)
            {
                SpawnPiece(11, i, 6);
            }
        }
        // Use this for initialization
        private void DrawChessboard()
        {
            Vector3 widthLine = Vector3.right * 8;
            Vector3 heightLine = Vector3.forward * 8;

            for (int i = 0; i <= 8; i++)
            {
                Vector3 start = Vector3.forward * i;
                Debug.DrawLine(start, start + widthLine);
                for (int j = 0; j <= 8; j++)
                {
                    start = Vector3.right * j;
                    Debug.DrawLine(start, start + heightLine);
                }
            }

            //draw the selection
            if (selectionX >= 0 && selectionY >= 0)
            {
                Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                               Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

                Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                               Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
            }
        }

        private void SpawnPiece(int index, int x, int y)
        {
            GameObject go = Instantiate(piecePrefabs[index], GetTileCenter(x, y), orientation) as GameObject;
            go.transform.SetParent(transform);
            CurrentBoard[x, y] = go.GetComponent<Piece>();
            CurrentBoard[x, y].SetPosition(x, y);

            if (y == 6 || y == 7)
            {
                CurrentBoard[x, y].isWhite = false;
            }
            else
            {
                CurrentBoard[x, y].isWhite = true;
            }

            activePiece.Add(go);
        }

        private void UpdateSelection()
        {
            if (!Camera.main)
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
                        if (selectedPiece == null)
                        {
                            //Select the chessman
                            SelectPiece(selectionX, selectionY);
                        }
                        else
                        {
                            //Move the Chessman
                            MovePiece(selectedPiece, selectionX, selectionY);
                        }
                    }
                }
            }
            else
            {
                // black turn == false, white turn == true

                string selector = "minimax";
                if (selector.Equals("rando"))
                {
                    RandomMoveGenerator deepBlue = new RandomMoveGenerator();
                    List<Move> moves = deepBlue.IterateMoves(CurrentBoard, false);
                    Move m = deepBlue.GetRandomMove(moves);
                    MovePiece(m.GetChessman(), m.GetNewX(), m.GetNewY());
                }
                else if (selector.Equals("minimax"))
                {
                    MiniMax minimax = new MiniMax();
                    Move m = minimax.FindMove(CurrentBoard);
                    MovePiece(m.GetChessman(), m.GetNewX(), m.GetNewY());
                }
            }
        }

        private void SelectPiece(int x, int y)
        {
            if (CurrentBoard[x, y] == null)
                return;

            if (CurrentBoard[x, y].isWhite != isWhiteTurn)
                return;

            bool hasAtleastOneMove = false;
            bool[,] allowedMoves = CurrentBoard[x, y].PossibleMove();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (allowedMoves[i, j])
                        hasAtleastOneMove = true;

            if (!hasAtleastOneMove)
            {
                return;
            }

            selectedPiece = CurrentBoard[x, y];
            previousMat = selectedPiece.GetComponent<MeshRenderer>().material;
            selectedMat.mainTexture = previousMat.mainTexture;
            selectedPiece.GetComponent<MeshRenderer>().material = selectedMat;
            BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
        }

        private void MovePiece(Piece pieceToMove, int x, int y)
        {
            bool[,] allowedMoves = pieceToMove.PossibleMove();

            if (allowedMoves[x, y])
            {
                Piece c = CurrentBoard[x, y];

                if (c != null && c.isWhite != isWhiteTurn)
                {
                    //Capture a piece

                    //If it is King
                    if (c.GetType() == typeof(King))
                    {
                        EndGame();
                        return;
                    }

                    activePiece.Remove(c.gameObject);
                    Destroy(c.gameObject);
                }

                if (x == EnPassantMove[0] && y == EnPassantMove[1])
                {
                    if (isWhiteTurn)
                    {
                        c = CurrentBoard[x, y - 1];
                        activePiece.Remove(c.gameObject);
                        Destroy(c.gameObject);
                    }
                    else
                    {
                        c = CurrentBoard[x, y + 1];
                        activePiece.Remove(c.gameObject);
                        Destroy(c.gameObject);
                    }
                }
                EnPassantMove[0] = -1;
                EnPassantMove[1] = -1;

                if (c != null && c.GetType() == typeof(Pawn))
                {
                    if (y == 7)
                    {
                        activePiece.Remove(c.gameObject);
                        Destroy(c.gameObject);
                        SpawnPiece(1, x, y);
                        selectedPiece = CurrentBoard[x, y];
                    }
                    else if (y == 0)
                    {
                        activePiece.Remove(c.gameObject);
                        Destroy(c.gameObject);
                        SpawnPiece(7, x, y);
                        selectedPiece = CurrentBoard[x, y];
                    }

                    if (selectedPiece.CurrentY == 1 && y == 3)
                    {
                        EnPassantMove[0] = x;
                        EnPassantMove[1] = y - 1;
                    }
                    else if (selectedPiece.CurrentY == 6 && y == 4)
                    {
                        EnPassantMove[0] = x;
                        EnPassantMove[1] = y + 1;
                    }
                }

                CurrentBoard[pieceToMove.CurrentX, pieceToMove.CurrentY] = null;
                pieceToMove.transform.position = GetTileCenter(x, y);
                pieceToMove.SetPosition(x, y);
                CurrentBoard[x, y] = pieceToMove;
                isWhiteTurn = !isWhiteTurn;
            }

            if (isWhiteTurn == true)
            {
                selectedPiece.GetComponent<MeshRenderer>().material = previousMat;
                BoardHighlights.Instance.HideHighlights();
                selectedPiece = null;
            }

        }

        private void EndGame()
        {
            if (isWhiteTurn)
                Debug.Log("White team wins");
            else
                Debug.Log("Black team wins");

            foreach (GameObject go in activePiece)
                Destroy(go);

            isWhiteTurn = true;
            BoardHighlights.Instance.HideHighlights();
            SpawnAllPieces();
        }
    }
}