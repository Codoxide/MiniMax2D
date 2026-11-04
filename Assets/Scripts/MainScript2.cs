using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainScript2 : MonoBehaviour
{
    
    public Button[] SideButtons;
    public int AI_Depth;
    public GameObject[] Circles_Img;
    public Slider depthSlider;
    public Text DepthText;
    public GameObject PlayerWin;
    public GameObject PlayerLose;
    char[] ButtonsList = new char[42];
    bool isPlayerTurn = true;
    int height = 6;
    int width = 7;
    int WinNum = 4;
    int[] SideNum = new int[7];
    bool GameOver = false;
    int[] EdgeIndices = new int[]
    {
    
        // Left edge (excluding corners)
        7, 14, 21, 28,
    
        // Right edge (excluding corners)
        13, 20, 27, 34,
    
        // Bottom row
        35, 36, 37, 38, 39, 40, 41
    };
        

    // ' ' is Empty
    // 'B' is Bot 
    // 'P' is Player
    // Start is called before the first frame update
    void Start()
    {
        SetBoardEmpty();
        ResetColors();  
    }
    public void SliderChange()
    {
        DepthText.text = "Depth: "+depthSlider.value;
        AI_Depth = (int)depthSlider.value;

    }
    void Update()
    {
        
        if(CheckWinner(true) && !GameOver)
        {
            PlayerLose.SetActive(false);
            PlayerWin.SetActive(true);
            QuitAllButtons();
            GameOver = true;

        }
        if(CheckWinner(false) && !GameOver)
        {
            PlayerLose.SetActive(true);
            PlayerWin.SetActive(false);
            QuitAllButtons();
            GameOver = true;
        }
        
    }

    void SetBoardEmpty()
    {
        for(int i = 0; i < ButtonsList.Length; i++)
        {
            ButtonsList[i] = ' ';
        }
    }
    void ResetColors()
    {
        for(int i = 0; i < Circles_Img.Length; i++)
        {
            Circles_Img[i].GetComponent<Image>().color = Color.clear;
        }
    }
    void QuitAllButtons()
    {
        for(int i =0; i < SideButtons.Length; i++)
        {
            SideButtons[i].interactable = false;
        }

    }
    void ComputerMove()
    {
        if(!GameOver)
        {
            int bestIndex = BestMove();
            Debug.Log(bestIndex);
            if(bestIndex != -1)
            {

                ButtonsList[bestIndex+width*(height-SideNum[bestIndex]-1)] = 'B';
                Circles_Img[bestIndex+width*(height-SideNum[bestIndex]-1)].GetComponent<Image>().color = Color.red;
                SideNum[bestIndex]++;
                isPlayerTurn = true;
            }
            else
            {
                Debug.Log("Board is Full!");
                
            }
        }

    }
    int EvaluateBoard()
    {
        int score = 0;
        score += CountLines('B', 3) * 50;
        score += CountLines('B', 2) * 10;

        score -= CountLines('P', 3) * 60;
        score -= CountLines('P', 2) * 12;

        score += CenterControl();

        return score;
        
    } 

    int CenterControl()
    {
        int score = 0;
        int[] PositionWeights = new int[]
        {
            3, 4, 5, 7, 5, 4, 3,
            4, 6, 8, 10, 8, 6, 4,
            5, 7, 11, 13, 11, 7, 5,
            5, 7, 11, 13, 11, 7, 5,
            4, 6, 8, 10, 8, 6, 4,
            3, 4, 5, 7, 5, 4, 3
        };

        for(int i = 0; i < PositionWeights.Length; i++)
        {
            if(ButtonsList[i]=='P')
            {
                score-=PositionWeights[i]*3;
            }
            else if(ButtonsList[i]=='B')
            {
                score+=PositionWeights[i]*3;
            }
        }
        return score;
    }
    int BestMove()
    {
        int bestScore = int.MinValue;
        int bestMoveIndex = 0;
        int score = 0;
        if(isBoardFull() == false)
        {

            for(int i = 0; i < SideButtons.Length; i++)
            {
                if(isSideFull(i) == false && SideNum[i] <= 5)
                {


                    int index = i+width*(height-SideNum[i]-1);
                    ButtonsList[index] = 'B';
                    SideNum[i]++;
                    score = Minimax(true, AI_Depth-1);
                    ButtonsList[index] = ' ';
                    SideNum[i]--;

                    if(bestScore < score)
                    {
                        bestScore = score;
                        bestMoveIndex = i;
                    }                    
                }
                Debug.Log($"Column {i} -> score {score}");

            }
            return bestMoveIndex;
        }
        else
        {
            return -1;            
        }
    }

    int Minimax(bool isPlayer, int depth)
    {
        if(CheckWinner(false)) return 100000 + depth;
        if(CheckWinner(true))  return -100000 - depth;

        if(depth == 0 || isBoardFull())
            return EvaluateBoard();

        if(isPlayer)
        {
            int best = int.MaxValue;
            for(int i = 0; i < 7; i++)
            {
                if(!isSideFull(i))
                {
                    int index = i + width * (height - SideNum[i] - 1);
                    ButtonsList[index] = 'P';
                    SideNum[i]++;

                    best = Math.Min(best, Minimax(false, depth - 1));

                    ButtonsList[index] = ' ';
                    SideNum[i]--;
                }
            }
            return best;
        }
        else
        {
            int best = int.MinValue;
            for(int i = 0; i < 7; i++)
            {
                if(!isSideFull(i))
                {
                    int index = i + width * (height - SideNum[i] - 1);
                    ButtonsList[index] = 'B';
                    SideNum[i]++;

                    best = Math.Max(best, Minimax(true, depth - 1));

                    ButtonsList[index] = ' ';
                    SideNum[i]--;
                }
            }
            return best;
        }
    }

    public void Side0()
    {
        if(SideNum[0] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[width*(height-SideNum[0]-1)] = 'P';
                Circles_Img[width*(height-SideNum[0]-1)].GetComponent<Image>().color = Color.blue;
                SideNum[0]++;
                isPlayerTurn = false;
                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    public void Side1()
    {
        if(SideNum[1] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[1+(width*(height-SideNum[1]-1))] = 'P';
                Circles_Img[1+(width*(height-SideNum[1]-1))].GetComponent<Image>().color = Color.blue;
                SideNum[1]++;
                isPlayerTurn = false;
                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    public void Side2()
    {
        if(SideNum[2] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[2+(width*(height-SideNum[2]-1))] = 'P';
                Circles_Img[2+(width*(height-SideNum[2]-1))].GetComponent<Image>().color = Color.blue;
                SideNum[2]++;
                isPlayerTurn = false;
                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    public void Side3()
    {
        if(SideNum[3] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[3+(width*(height-SideNum[3]-1))] = 'P';
                Circles_Img[3+(width*(height-SideNum[3]-1))].GetComponent<Image>().color = Color.blue;
                SideNum[3]++;
                isPlayerTurn = false;
                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    public void Side4()
    {
        if(SideNum[4] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[4+(width*(height-SideNum[4]-1))] = 'P';
                Circles_Img[4+(width*(height-SideNum[4]-1))].GetComponent<Image>().color = Color.blue;
                SideNum[4]++;
                isPlayerTurn = false;

                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    public void Side5()
    {
        if(SideNum[5] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[5+(width*(height-SideNum[5]-1))] = 'P';
                Circles_Img[5+(width*(height-SideNum[5]-1))].GetComponent<Image>().color = Color.blue;
                SideNum[5]++;
                isPlayerTurn = false;
                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    public void Side6()
    {
        if(SideNum[6] <= 5)
        {
            if(isPlayerTurn)
            {
                ButtonsList[6+(width*(height-SideNum[6]-1))] = 'P';
                Circles_Img[6+(width*(height-SideNum[6]-1))].GetComponent<Image>().color = Color.blue;
                SideNum[6]++;
                isPlayerTurn = false;
                ComputerMove();
            }
            else
            {
                Debug.Log("Not your turn yet.");

            }

        }
        else
        {
            Debug.Log("No Available Spaces.");
        }
    }
    bool isSideFull(int intSide)
    {
        int[] triggerPoints = new int[6];
        if(intSide == 0)
        {
            triggerPoints = new int[] {0, 7, 14, 21, 28, 35};
        }
        else if(intSide == 1)
        {
            triggerPoints = new int[] {1, 8, 15, 22, 29, 36};
        }
        else if(intSide == 2)
        {
            triggerPoints = new int[] {2, 9, 16, 23, 30, 37};
        }
        else if(intSide == 3)
        {
            triggerPoints = new int[] {3, 10, 17, 24, 31, 38};
        }       
        else if(intSide == 4)
        {
            triggerPoints = new int[] {4, 11, 18, 25, 32, 39};
        }  
        else if(intSide == 5)
        {
            triggerPoints = new int[] {5, 12, 19, 26, 33, 40};
        }   
        else if(intSide == 6)
        {
            triggerPoints = new int[] {6, 13, 20, 27, 34, 41};
        }                            

        for(int i = 0; i < triggerPoints.Length; i++)
        {
            if(ButtonsList[triggerPoints[i]] == ' ')
            {
                return false;
            }
        }
        return true;
    }
    bool isBoardFull()
    {
        for(int i = 0; i < ButtonsList.Length; i++)
        {
            if(ButtonsList[i] == ' ')
            {
                return false;
            }
        }
        return true;
    }
    bool CheckWinner(bool isPlayer)
    {
        char piece = isPlayer ? 'P' : 'B';
        // HORIZONTAL
        if(CheckRow(piece))
        {
            return true;
        }

        // VERTICAL
        if(CheckColumn(piece))
        {
            return true;
        }

        // Diagonal Decrease
        if(CheckDiagonalDecrease(piece))
        {
            return true;
        }

        // Diagonal Increase
        if(CheckDiagonalIncrease(piece))
        {
            return true;
        }
    

        return false;
    }
    bool CheckRow(char piece)
    {
        for(int y = 0; y < height; y++)
        {
            int count = 0;
            for(int x = 0; x < width; x++)
            {
                int i = x + y * width;

                if(ButtonsList[i] == piece)
                {
                    count++;
                    if(count == WinNum)
                        return true;
                }
                else
                {
                    count = 0;
                }
            }
        }
        return false;
    }

    bool CheckColumn(char piece)
    {
        
        for (int x = 0; x < width; x++)
        {
            int count = 0;
            for (int y = 0; y < height; y++)
            {
                int i = x + (y*width);

                if (ButtonsList[i] == piece)
                {
                    count++;
                    if (count == WinNum)
                    {
                        return true;
                    }
                }
                else 
                {
                    count = 0;
                }

            }
        }
        return false;
    }
    bool CheckDiagonalDecrease(char piece)
    {
        int count;
        int loops;
        int i;
        for(int t = 0; t < ButtonsList.Length; t++)
        {
            i = t;          
            count = 0;
            loops = 0;

            while(loops < WinNum)
            {
                if(ButtonsList[i] == piece)
                {
                    count++;
                }
                else{
                    break;
                }
                if(count == WinNum)
                {
                    return true;
                }
                if(!IsEdge(i))
                {
                    i+=8;
                }
                else{
                    break;
                }
                loops++;
            }
            


        }
        return false;
    }
    bool CheckDiagonalIncrease(char piece)
    {
        int count;
        int loops;
        int i;
        for(int t = 0; t < ButtonsList.Length; t++)
        {
            i = t;          
            count = 0;
            loops = 0;

            while(loops < WinNum)
            {
                if(ButtonsList[i] == piece)
                {
                    count++;
                }
                else{
                    break;
                }
                if(count == WinNum)
                {
                    return true;
                }
                if(!IsEdge(i))
                {
                    i+=6;
                }
                else{
                    break;
                }
                loops++;
            }
            


        }
        return false;
    }
    int CountLines(char piece, int intLength)
    {
        int connections = 0;
        connections += CountLinesHor(piece, intLength);
        connections += CountLinesVer(piece, intLength);
        connections += CountLinesDiaI(piece, intLength);
        connections += CountLinesDiaD(piece, intLength);

        return connections;
    }         
    int CountLinesHor(char piece, int intLength)
    {
        int count = 0;
        int connections = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int x = 0; x < 7; x++)
            {
                if(ButtonsList[x+i*7] == piece)
                {
                    count++;
                }
                else{
                    count = 0;
                }
                if(count == intLength)
                {
                    count = 0;
                    if(x+1 < 7)
                    {
                        if(ButtonsList[(x+1)+i*7] == ' ')
                        {
                            connections++;
                        }
                    }
                    if(x-intLength >= 0)
                    {
                        if(ButtonsList[(x-intLength)+i*7] == ' ')
                        {
                            connections++;
                        }
                    } 
                }
            }
        }     
        return connections;
    }    
    int CountLinesVer(char piece, int intLength)
    {
        int count = 0;
        int connections = 0;
        
        for (int i = 0; i < 7; i++)
        {
            for (int x = 0; x < 6; x++)
            {
                if(ButtonsList[i+x*7] == piece)
                {
                    count++;
                }
                else{
                    count = 0;
                }                  
                
                if(count == intLength)
                {
                    count = 0;
                    if(i+(x+1)*7 <= i+35)
                    {
                        if(ButtonsList[(i+(x+1)*7)] == ' ')
                        {
                            connections++;
                        }
                    }
                    if(i+(x-intLength)*7 >= i)
                    {
                        if(ButtonsList[(i+(x-intLength)*7)] == ' ')
                        {
                            connections++;
                        }
                    } 
                }
            }
        }
        return connections;            
    }         
    int CountLinesDiaD(char piece, int intLength)
    {
        int count = 0;
        int i;
        int loops = 0;
        int connections = 0;
        for(int t = 0; t < ButtonsList.Length; t++)
        {
            i = t;          
            count = 0;
            if(ButtonsList[t] == piece && t != 6)
            {
                while(loops < intLength  && !IsEdge(i))
                {
                    if(ButtonsList[i] == piece)
                    {
                        count++;
                    }
                    else{
                        count = 0;
                    }
                    if(count == intLength)
                    {
                        Console.Write(t+": ");
                        Console.WriteLine(count);
                        count = 0;
                        if(!IsEdge(i))
                        {
                            if(ButtonsList[i+8] == ' ')
                            {
                                connections++;
                            }
                        }
                        if(t > 7)
                        {
                            if(ButtonsList[t-8] == ' ')
                            {
                                connections++;
                            }
                        }      
                        
                    }
                    
                    i+=8;
                    loops++;
                    
                }
            }


        }
        return connections;
    }        
    int CountLinesDiaI(char piece, int intLength)
    {
        int count;
        int loops;
        int i;
        int connections = 0;
        for(int t = 0; t < ButtonsList.Length; t++)
        {
            i = t;          
            count = 0;
            loops = 0;
            if(ButtonsList[t] == piece && t != 0)
            {
                while(loops < intLength && !IsEdge(i))
                {
                    if(ButtonsList[i] == piece)
                    {
                        count++;
                        Console.WriteLine(count+": "+ButtonsList[i]+":"+i+":"+t);
                        
                    }
                    else{
                        count = 0;
                    }
                    if(count == intLength)
                    {

                        count = 0;
                        if(!IsEdge(i))
                        {
                            if(ButtonsList[i+6] == ' ')
                            {
                                connections++;
                            }                                
                        }

                        if(t > 7 && t != 13)
                        {
                            if(ButtonsList[t-6] == ' ')
                            {
                                connections++;
                            }
                        }      

                        
                    }
                    
                    i+=6;
                    loops++;
                }
            }


        }
        return connections;
    }         
    bool IsEdge(int num)
    {
        for(int i = 0; i < EdgeIndices.Length; i++)
        {
            if(num == EdgeIndices[i])
            {
                return true;
            }
        }
        return false;
    }
    public void Retry()
    {
        SceneManager.LoadScene(1);
    }
}