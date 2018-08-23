using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        int num = 0;
        static int Horizontal = 18, Vertical = 18, Bomb = 20; //橫向縱向方格數量
        bool isStart = true;                                       //是否開始點擊方塊 
        bool isLost = false;                                       //是否輸掉遊戲
        bool[,] isClicked = new bool[Horizontal, Vertical];        //判斷是否被點擊
        bool[,] isLabeled = new bool[Horizontal, Vertical];        //判斷是否被標記
        int[,] isBump = new int[Horizontal, Vertical];             //方塊的資訊
        Button[,] gameBomp = new Button[Horizontal, Vertical];     //按鈕方塊


        public Form1() //窗體構造
        {
            InitializeComponent();
            InitlizeGame();
        }

        public void InitlizeGame()
        {
            //窗體
            this.Text = "踩地雷";                           //視窗名稱
            this.Width = 60 + Horizontal * 30;                  //視窗寬度
            this.Height = 60 + Vertical * 30;                   //視窗高度
            this.MaximizeBox = false;                       //不可最大化
            this.StartPosition = FormStartPosition.CenterScreen;   //起始出現位置-中心
            this.FormBorderStyle = FormBorderStyle.FixedSingle;    //不可調整大小

            //按鈕的添加
            for (int i = 0; i < Horizontal; i++)
            {
                for (int j = 0; j < Vertical; j++)
                {
                    isBump[i, j] = 0;                               //開場沒地雷
                    isLabeled[i, j] = false;                        //初始狀態沒有點擊沒有標記
                    isClicked[i, j] =  false;
                    gameBomp[i, j] = new Button();
                    gameBomp[i, j].BackColor = Color.LightSkyBlue;  //按鈕顏色
                    gameBomp[i, j].Width = 30;                      //按鈕寬
                    gameBomp[i, j].Height = 30;                     //按鈕高
                    gameBomp[i, j].Top = i * 30 + 18;               //按鈕距離上面間距
                    gameBomp[i, j].Left = j * 30 + 18;              //按鈕距離左邊間距 
                    gameBomp[i, j].Tag = i * Horizontal + j + 1;    //按鈕的標籤
                    gameBomp[i, j].Text = "";                       //按鈕的文字
                    gameBomp[i, j].Click += new EventHandler(OnclickedGameBump);           // 回調函數-左鍵
                    gameBomp[i, j].MouseUp += new MouseEventHandler(OnRightClicked);       // 回調函數-右鍵
                    this.Controls.Add(gameBomp[i, j]);                                      // 回傳回窗體

                }

            }
        }


        private void OnRightClicked(object sender, MouseEventArgs e) //標記事件
        {
            Button thisBump = sender as Button;
            int i = (int.Parse(thisBump.Tag.ToString()) % Horizontal == 0 ? int.Parse(thisBump.Tag.ToString()) / Horizontal - 1 : int.Parse(thisBump.Tag.ToString()) / Horizontal), j = int.Parse(thisBump.Tag.ToString()) - i * Horizontal - 1;
            if (e.Button == MouseButtons.Right && !isClicked[i, j])
            {
                isLabeled[i, j] = !isLabeled[i, j];
                if (isLabeled[i, j])
                { thisBump.BackColor = Color.Red; }
                else
                { thisBump.BackColor = Color.LightSkyBlue; }

            }
        }

        private void OnclickedGameBump(object sender, EventArgs e) //按鈕事件
        {
            Button thisBump = sender as Button;
            int i = (int.Parse(thisBump.Tag.ToString()) % Horizontal == 0 ? int.Parse(thisBump.Tag.ToString()) / Horizontal - 1 : int.Parse(thisBump.Tag.ToString()) / Horizontal), j = int.Parse(thisBump.Tag.ToString()) - i * Horizontal - 1;
            if (isStart)   //首次點擊
            {
                SetBump(i, j);
                isStart = false;
            }
            if (isLabeled[i, j])
            { return; }
            if (isClicked[i, j] || isLost)    //是否點擊過
            { return; }
            if (isBump[i,j]==-1)              //是否踩到地雷
            {
                isLost = true;
                DiscoverAllBomb();
                MessageBox.Show("你死了","遊戲失敗",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ReStartGame();
            }
            else
            {
                gameBomp[i, j].BackColor = Color.LightGreen;
                isClicked[i, j] = true;
                if (isBump[i,j]>0)    //周圍有地雷顯示地雷數量
                {
                    thisBump.Text = isBump[i, j].ToString();
                }
                else                  //沒有則九宮格擴充
                {
                    if (i - 1 > -1 && j - 1 > -1 && isBump[i - 1, j - 1] > -1)
                    { OnclickedGameBump(gameBomp[i - 1, j - 1], null); }
                    if (i - 1 > -1 && isBump[i - 1, j ] > -1)
                    { OnclickedGameBump(gameBomp[i - 1, j], null); }
                    if (i - 1 > -1 && j + 1 < Horizontal && isBump[i - 1, j + 1] > -1)
                    { OnclickedGameBump(gameBomp[i - 1, j + 1], null); }
                    if (j - 1 > -1 && isBump[i , j - 1] > -1)
                    { OnclickedGameBump(gameBomp[i, j - 1], null); }
                    if (j + 1 <Horizontal && isBump[i , j+1] > -1)
                    { OnclickedGameBump(gameBomp[i, j+1], null); }
                    if (i + 1 <Vertical && j - 1 > -1 && isBump[i + 1, j - 1] > -1)
                    { OnclickedGameBump(gameBomp[i + 1, j - 1], null); }
                    if (i + 1 <Vertical  && isBump[i + 1, j ] > -1)
                    { OnclickedGameBump(gameBomp[i + 1, j ], null); }
                    if (i + 1 <Vertical && j + 1 < Horizontal && isBump[i + 1, j + 1] > -1)
                    { OnclickedGameBump(gameBomp[i + 1, j + 1], null); }
                }
            }
            num++;
            if (num==Horizontal*Vertical-Bomb)
            {
                DiscoverAllBomb();
                MessageBox.Show("你贏了", "遊戲勝利", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReStartGame();
            }
            
        }

        //顯示所有地雷
        private void DiscoverAllBomb()
        {
            for ( int i = 0;  i < Horizontal;  i++)
            {
                for (int j = 0; j < Vertical; j++)
                {
                    if (isBump[i, j] == -1)
                        gameBomp[i, j].Text = "● ";
                }
            }
        }
        //設置地雷
        private void SetBump (int clickedI,int clickedJ)
        {
            int i,j, k;
            Random random = new Random();
            for ( i = 0; i < Bomb; i++)
            {
                j = random.Next(Horizontal);
                k = random.Next(Vertical);
                if(isBump[j, k] ==-1||((j == clickedI)&&(k == clickedJ)))
                {
                    i--;
                    continue;
                }
                isBump[j, k] = -1;
            }
            //方塊周圍地雷的統計
            for ( i = 0; i < Horizontal; i++)
            {
                for (j = 0; j < Vertical; j++)
                {
                    k = 0;
                    if (isBump[i, j] == -1) continue;
                    if (i - 1 > -1 && j - 1 > -1 && isBump[i-1, j-1] == -1) k++;
                    if (i - 1 > -1 && isBump[i - 1, j ] == -1) k++;
                    if (i - 1 > -1 && j + 1 <Horizontal && isBump[i - 1, j + 1] == -1) k++;
                    if (j - 1 > -1 && isBump[i , j - 1] == -1) k++;
                    if (j + 1 <Horizontal && isBump[i, j + 1] == -1) k++;
                    if (i + 1 <Vertical && j - 1 > -1 && isBump[i + 1, j - 1] == -1) k++;
                    if (i + 1 <Vertical && isBump[i + 1, j]  == -1) k++;
                    if (i + 1 <Vertical && j + 1 < Horizontal && isBump[i + 1, j + 1] == -1) k++;
                    isBump[i, j] = k;
                }

            }

        }


        //重新開始遊戲
        private void ReStartGame()
        {
            num = 0;
            isLost = false;
            isStart = true;
            for (int i = 0; i < Horizontal; i++)
            {
                for (int j = 0; j < Vertical; j++)
                {
                    isBump[i, j] = 0;                                 //無地雷
                    isClicked[i,j] = false;                           //不點擊
                    gameBomp[i, j].BackColor = Color.LightSkyBlue;    //重製背景色
                    gameBomp[i, j].Text = "";                         //按鈕文本
                }
            }
        }


    }
}
