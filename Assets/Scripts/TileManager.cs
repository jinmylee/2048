using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileManager : MonoBehaviour
{
    [Header("���� ����")]
    public RectTransform gameBoard;
    public GameObject nodePiece;
    public TextMeshProUGUI text;

    public int pieceCount = 2;

    Node[,] Board;
    int height = 4;
    int width = 4;

    List<NodePiece> update;
    List<NodePiece> dead;
    int spawnCount = 0;
    int turn = 0;
    int number = 0;

    int PlayerHp = 50;
    int EnemyHp = 50;

    enum KEY
    {
        up,
        left,
        down,
        right
    }

    int IsSwiping = 0;

    void Start()
    {
        Set();
    }

    void Set()
    {
        InitializedBoard();
        InstantiateBoard();
    }

    void InitializedBoard()
    {
        Board = new Node[width, height];
        update = new List<NodePiece>();
        dead = new List<NodePiece>();
        spawnCount = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Board[x, y] = new Node(new Point(x, y), 0, 0);
            }
        }
    }

    void InstantiateBoard()
    {
        for (int i = 0; i < pieceCount; i++)
        {
            RandSpawn();
        }

    }

    void Update()
    {
        //if (spawnCount == 25) over
        //else
        if (number >= 10) TurnChange();
        KeyDown();
        updatePiece();

        text.text = "SpawnCount: " + spawnCount + "\nMyhp" + PlayerHp + "\nEnemyHP" + EnemyHp;
    }

    void updatePiece()
    {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        for (int i = 0; i < update.Count; i++)  //������Ʈ ī��Ʈ�� �����
        {
            NodePiece piece = update[i];
            if (!piece.StartUpdate()) finishedUpdating.Add(piece);  //�Ϸ� ������Ʈ�� �ѱ�
        }
        for (int i = 0; i < finishedUpdating.Count; i++)   //�Ϸ� ������Ʈ ī��Ʈ ��ŭ
        {
            NodePiece piece = finishedUpdating[i];
            piece.EndUpdate();
            update.Remove(piece);
            for (int j = 0; j < dead.Count; j++)
            {
                NodePiece p = dead[j];
                Node node = getNodeAtPoint(p.index);
                p.SetState(0);
                Destroy(p.th(), 0.5f);
                node.SetPiece(null);
                dead.Remove(p);
            }
        }
        if (dead.Count == 0 && IsSwiping == 2)
        {
            IsSwiping = 3;
            Invoke("RandSpawn", 0.5f);
        }
    }

    void KeyDown()
    {
        if (Input.GetKeyUp(KeyCode.W)) Slide(KEY.up);
        if (Input.GetKeyUp(KeyCode.A)) Slide(KEY.left);
        if (Input.GetKeyUp(KeyCode.S)) Slide(KEY.down);
        if (Input.GetKeyUp(KeyCode.D)) Slide(KEY.right);
    }

    void Slide(KEY key)
    {
        if (IsSwiping != 0) return;
        IsSwiping = 1;

        switch (key)
        {
            case KEY.up:
                for (int y = 0; y <= height - 1; y++)
                {
                    for (int x = 0; x <= width - 1; x++)
                    {
                        for (int n = y; n <= height - 1; n++)
                        {
                            if (Board[x, n].state == 0) continue;
                            else
                            {
                                if(!plusPieces(new Point(x, n), new Point(x, y - 1))) continue; 
                                flipPieces(new Point(x, n), new Point(x, y));
                                break;
                            }
                        }
                    }
                }
                break;

            case KEY.left:
                for (int x = 0; x <= width - 1; x++)
                {
                    for (int y = 0; y <= height - 1; y++)
                    {
                        for (int n = x; n <= width - 1; n++)
                        {
                            if (Board[n, y].state == 0) continue;
                            else
                            {
                                if(!plusPieces(new Point(n, y), new Point(x - 1, y))) continue; 
                                flipPieces(new Point(n, y), new Point(x, y));
                                break;
                            }
                        }
                    }
                }
                break;

            case KEY.down:
                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x <= width - 1; x++)
                    {
                        for (int n = y; n >= 0; n--)
                        {
                            if (Board[x, n].state == 0) continue;
                            else
                            {
                                if(!plusPieces(new Point(x, n), new Point(x, y + 1))) continue; 
                                flipPieces(new Point(x, n), new Point(x, y));
                                break;
                            }
                        }
                    }
                }
                break;

            case KEY.right:
                for (int x = width - 1; x >= 0; x--)
                {
                    for (int y = 0; y <= height - 1; y++)
                    {
                        for (int n = x; n >= 0; n--)
                        {
                            if (Board[n, y].state == 0) continue;
                            else
                            {
                                if(!plusPieces(new Point(n, y), new Point(x + 1, y))) continue; 
                                flipPieces(new Point(n, y), new Point(x, y));
                                break;
                            }
                        }
                    }
                }
                break;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y <= height- 1; y++)
            {
                if (Board[x, y].state == 2) Board[x, y].SetState(1);
            }
        }

        number++;
        IsSwiping = 2;
    }

    void TurnChange()
    {

    }

    bool Spawn(Point p)
    {
        if (getPieceAtPoint(p) != null)
            return true;

        Node node = getNodeAtPoint(p);
        GameObject n = Instantiate(nodePiece, gameBoard);
        NodePiece piece = n.GetComponent<NodePiece>();
        piece.Init(p, 2, 1);
        node.SetPiece(piece);
        node.getPiece().EndUpdate();


        return false;
    }

    void RandSpawn()
    {
        bool on = true;
        while (on)
        {
            if (spawnCount >= height*width) break;

            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            on = Spawn(new Point(x, y));
        }
        IsSwiping = 0;
        spawnCount++;
    }

    void flipPieces(Point one, Point two)
    {
        if (one.x < 0 || one.x >= width || one.y < 0 || one.y >= height) return;
        if (two.x < 0 || two.x >= width || two.y < 0 || two.y >= height) return;
        //if (GetValueAtPoint(one) < 0 || GetValueAtPoint(two) < 0) return;
        NodePiece pieceOne = Board[one.x, one.y].getPiece();
        NodePiece pieceTwo = Board[two.x, two.y].getPiece();
        Board[one.x, one.y].SetPiece(pieceTwo);
        Board[two.x, two.y].SetPiece(pieceOne);
        Board[two.x, two.y].getPiece().ResetPosition();

        update.Add(Board[two.x, two.y].getPiece());
    }

    bool plusPieces(Point one, Point two)
    {
        if (one.x < 0 || one.x >= width || one.y < 0 || one.y >= height) return true;
        if (two.x < 0 || two.x >= width || two.y < 0 || two.y >= height) return true;
        if (Board[one.x, one.y].state == 2 || Board[two.x, two.y].state == 2) return true;
        if (Board[one.x, one.y].value == Board[two.x, two.y].value)
        {
            NodePiece pieceOne = Board[one.x, one.y].getPiece();
            NodePiece pieceTwo = Board[two.x, two.y].getPiece();
            pieceTwo.zero();
            pieceOne.mult();
            Board[one.x, one.y].SetPiece(pieceTwo);
            Board[two.x, two.y].SetPiece(pieceOne);
            Board[one.x, one.y].getPiece().ResetPosition();
            Board[two.x, two.y].getPiece().ResetPosition();

            update.Add(Board[two.x, two.y].getPiece());
            dead.Add(Board[one.x, one.y].getPiece());
            spawnCount--;
            return false;
        }
        return true;
    }

    public int GetValueAtPoint(Point P)
    {
        if (P.x < 0 || P.x >= width || P.y < 0 || P.y >= height) return -1;
        return Board[P.x, P.y].value;
    }

    Node getNodeAtPoint(Point p)
    {
        return Board[p.x, p.y];
    }

    NodePiece getPieceAtPoint(Point p)
    {
        return Board[p.x, p.y].getPiece();
    }
}

[System.Serializable]
public class Node
{
    NodePiece piece = null;
    public Point index;
    public int value;
    public int state;

    public Node(Point id, int v, int s)
    {
        index = id;
        value = v;
        state = s;
    }

    public void SetPiece(NodePiece p)
    {
        piece = p;
        state = (piece == null) ? 0 : piece.state;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index, value, state);
    }

    public void SetState(int s)
    {
        state = s;
        piece.SetState(s);
    }

    public NodePiece getPiece()
    {
        return piece;
    }
}