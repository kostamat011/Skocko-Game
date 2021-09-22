using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skocko
{
    public partial class MainForm : Form
    {
        Combination target_comb;
        Combination current_comb;

        Button[,] current;
        RoundButton[,] result;

        int curr_row = 0;
        int curr_pos = 0;

        int time = 100;
        int win_cnt = 0, lose_cnt = 0;
        int pct=0;
        

        public MainForm()
        {
            InitializeComponent();
            InitGame(); 
        }

        
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                string past_results;
                StreamReader sr = new StreamReader("rezultat.txt");
                past_results = sr.ReadLine();
                if (past_results != null)
                {
                    win_cnt = Int32.Parse(past_results.Split('/')[0]);
                    lose_cnt = Int32.Parse(past_results.Split('/')[1]) - win_cnt;
                    lbSuccess.Text = past_results;
                    sr.Dispose();
                    if (!(lose_cnt == 0))
                        pct = (int)Math.Round((double)(100 * win_cnt) / (win_cnt + lose_cnt));
                    lbPct.Text = pct.ToString() + "%";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            timer1.Interval = 1000;

            AssignButtons();
        }

        public void InitGame()
        {
           /*
            * -reset time and generate new random combination
            * -hide previous combination
            * -start timer again
            */

            time = 100;
            Random r = new Random();
            target_comb = new Combination(r.Next(0,6), r.Next(0, 6), r.Next(0, 6), r.Next(0, 6));
            current_comb = new Combination(9, 9, 9, 9);
            Timer();
        }

        public void ShowSolution()
        {
            /*
             * -show the target combination in result buttons
             * 
             * */

            switch (target_comb.order[0])
            {
                case 0:
                    btnSolution1.BackgroundImage = btnSquare.BackgroundImage;
                    break;
                case 1:
                    btnSolution1.BackgroundImage = btnSpade.BackgroundImage;
                    break;
                case 2:
                    btnSolution1.BackgroundImage = btnHeart.BackgroundImage;
                    break;
                case 3:
                    btnSolution1.BackgroundImage = btnClub.BackgroundImage;
                    break;
                case 4:
                    btnSolution1.BackgroundImage = btnStar.BackgroundImage;
                    break;
                case 5:
                    btnSolution1.BackgroundImage = btnSmiley.BackgroundImage;
                    break;
            }
            switch (target_comb.order[1])
            {
                case 0:
                    btnSolution2.BackgroundImage = btnSquare.BackgroundImage;
                    break;
                case 1:
                    btnSolution2.BackgroundImage = btnSpade.BackgroundImage;
                    break;
                case 2:
                    btnSolution2.BackgroundImage = btnHeart.BackgroundImage;
                    break;
                case 3:
                    btnSolution2.BackgroundImage = btnClub.BackgroundImage;
                    break;
                case 4:
                    btnSolution2.BackgroundImage = btnStar.BackgroundImage;
                    break;
                case 5:
                    btnSolution2.BackgroundImage = btnSmiley.BackgroundImage;
                    break;
            }
            switch (target_comb.order[2])
            {
                case 0:
                    btnSolution3.BackgroundImage = btnSquare.BackgroundImage;
                    break;
                case 1:
                    btnSolution3.BackgroundImage = btnSpade.BackgroundImage;
                    break;
                case 2:
                    btnSolution3.BackgroundImage = btnHeart.BackgroundImage;
                    break;
                case 3:
                    btnSolution3.BackgroundImage = btnClub.BackgroundImage;
                    break;
                case 4:
                    btnSolution3.BackgroundImage = btnStar.BackgroundImage;
                    break;
                case 5:
                    btnSolution3.BackgroundImage = btnSmiley.BackgroundImage;
                    break;
            }
            switch (target_comb.order[3])
            {
                case 0:
                    btnSolution4.BackgroundImage = btnSquare.BackgroundImage;
                    break;
                case 1:
                    btnSolution4.BackgroundImage = btnSpade.BackgroundImage;
                    break;
                case 2:
                    btnSolution4.BackgroundImage = btnHeart.BackgroundImage;
                    break;
                case 3:
                    btnSolution4.BackgroundImage = btnClub.BackgroundImage;
                    break;
                case 4:
                    btnSolution4.BackgroundImage = btnStar.BackgroundImage;
                    break;
                case 5:
                    btnSolution4.BackgroundImage = btnSmiley.BackgroundImage;
                    break;
            }
        }

        public void Check()
        {
            /*
             *-compare current combination to target combination
             *-if they are same player has won the game
             *
             */
            
            if (current_comb.order.SequenceEqual(target_comb.order))
            {
                //color all circles red
                result[curr_row, 0].BackColor = result[curr_row, 1].BackColor = result[curr_row, 2].BackColor = result[curr_row, 3].BackColor = Color.Red;

                //call winner method
                wonGame();
            }
            else
            {
    
                int correct_on_place = 0;
                int correct_total = 0;

                int[] t = new int[4], c = new int[4];
                Array.Copy(current_comb.order, c , 4);
                Array.Copy(target_comb.order, t, 4);

                //count signs that are in the right place
                if (c[0]==t[0])
                    correct_on_place++;
                if (c[1] == t[1])
                    correct_on_place++;
                if (c[2] == t[2])
                    correct_on_place++;
                if (c[3] == t[3])
                    correct_on_place++;

                //count total number of correct signs
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (t[i] == c[j])
                        {
                            correct_total++;
                            c[j] = -1;
                            t[i] = -1;
                            break;
                        }

                    }
                }

                //every correct on place gets one red circle
                for (int d = 0; d < correct_on_place; d++)
                    result[curr_row, d].BackColor = Color.Red;

                //every correct of place gets one yellow circle
                for (int d = 0; d < correct_total; d++)
                {
                    if (result[curr_row, d].BackColor != Color.Red)
                        result[curr_row, d].BackColor = Color.Yellow;
                }

            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            /*
             * -if row is not completed show message
             * -if row is completed check the answer and proceed to next row
             * -dont forget to check if game is over (out of rows)
             */
            if (curr_pos == 4)
            {
                Check();
                curr_row++;
                curr_pos = 0;
                if (curr_row == 6 && !(current_comb.order.SequenceEqual(target_comb.order)))
                {
                    lostGame();
                }
            }
            else
            {
                MessageBox.Show("Fill all 4 fields in a row");
            }
        }


        private void btnNewGame_Click(object sender, EventArgs e)
        {
            btnNewGame.Visible = false;
            NewGame();
        }

        public void lostGame()
        {
            timer1.Stop();
            MessageBox.Show("You lose!");
            ShowSolution();
            btnNewGame.Visible = true;
            lose_cnt ++;
        }

        public void wonGame()
        {
            timer1.Stop();
            MessageBox.Show("Correct Combination!");
            ShowSolution();
            btnNewGame.Visible = true;
            win_cnt++;
        }

        public void NewGame()
        {
            /*
             *-reset all combination buttons and all result circles
             *-call InitGame to initialize time and combinations
             *-update labels
             */
            for(int i=0;i<6;i++)
            {
                for(int k=0;k<4;k++)
                {
                    current[i, k].BackgroundImage = null;
                    result[i, k].BackColor = Color.White;
                }
            }
            btnSolution1.BackgroundImage = btnSolution2.BackgroundImage = btnSolution3.BackgroundImage = btnSolution4.BackgroundImage = null;
            curr_row = curr_pos = 0;
            InitGame();
            lbSuccess.Text = win_cnt.ToString() + "/" + (win_cnt + lose_cnt).ToString();
            pct = (int)Math.Round((double)(100 * win_cnt) / (win_cnt + lose_cnt));
            lbPct.Text = pct.ToString() + "%";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*
             *-decrease time
             *-update timer text
             *-check if time ran out
             */
            time--;
            lbTime.Text = time.ToString();
            if (time == 0)
            {
                lostGame();
            }
        }

        private void saveScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /**
             *-save current score 
             */
            StreamWriter sw = new StreamWriter("rezultat.txt", false);
            sw.WriteLine(lbSuccess.Text);
            sw.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I hope you enjoyed the game!");
            this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            /*
             *-delete current sign and update current position 
             */
            if (curr_pos != 0 && current[curr_row,curr_pos-1].BackgroundImage!=null)
            {
                current[curr_row, curr_pos - 1].BackgroundImage = null;
                curr_pos--;
            }
            else if (curr_pos == 0 && current[curr_row,3].BackgroundImage!=null)
            {
                current[curr_row, 3].BackgroundImage = null;
                curr_pos = 3;
            }

        }

        private void biceUSlVerzijiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                this.BackColor = cd.Color;

        }

        private void resetScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            win_cnt = 0;
            lose_cnt = 0;
            NewGame();
            pct = 0;
            File.Create("rezultat.txt").Close();
            lbPct.Text = "0%";

        }

        public void Timer()
        {
            timer1.Start();
        }

        public void Sign_click(object sender, int indeks)
        {
            if (curr_pos <= 3)
            {
                Button b = (Button)sender;
                current[curr_row, curr_pos].BackgroundImage = b.BackgroundImage;
                current_comb.order[curr_pos] = indeks;
                curr_pos++;
            }
        }

        private void AssignButtons()
        {
            /*
            * -assign buttons to button arrays to make easier getting the needed buttons in code
            */
            current = new Button[25, 25];
            result = new RoundButton[25, 25];
            current[0, 0] = button31;
            current[0, 1] = button32;
            current[0, 2] = button33;
            current[0, 3] = button34;
            current[1, 0] = button38;
            current[1, 1] = button37;
            current[1, 2] = button36;
            current[1, 3] = button35;
            current[2, 0] = button42;
            current[2, 1] = button41;
            current[2, 2] = button40;
            current[2, 3] = button39;
            current[3, 0] = button46;
            current[3, 1] = button45;
            current[3, 2] = button44;
            current[3, 3] = button43;
            current[4, 0] = button50;
            current[4, 1] = button49;
            current[4, 2] = button48;
            current[4, 3] = button47;
            current[5, 0] = button54;
            current[5, 1] = button53;
            current[5, 2] = button52;
            current[5, 3] = button51;
            result[0, 0] = roundButton1;
            result[0, 1] = roundButton2;
            result[0, 2] = roundButton3;
            result[0, 3] = roundButton4;
            result[1, 0] = roundButton8;
            result[1, 1] = roundButton7;
            result[1, 2] = roundButton6;
            result[1, 3] = roundButton5;
            result[2, 0] = roundButton12;
            result[2, 1] = roundButton11;
            result[2, 2] = roundButton10;
            result[2, 3] = roundButton9;
            result[3, 0] = roundButton16;
            result[3, 1] = roundButton15;
            result[3, 2] = roundButton14;
            result[3, 3] = roundButton13;
            result[4, 0] = roundButton20;
            result[4, 1] = roundButton19;
            result[4, 2] = roundButton18;
            result[4, 3] = roundButton17;
            result[5, 0] = roundButton24;
            result[5, 1] = roundButton23;
            result[5, 2] = roundButton22;
            result[5, 3] = roundButton21;
        }

        #region event_handleri_znakovi

        private void button25_Click(object sender, EventArgs e)
        {
            Sign_click(sender, 0);
        }
        private void button26_Click(object sender, EventArgs e)
        {
            Sign_click(sender, 1);
        }


        private void button27_Click(object sender, EventArgs e)
        {
            Sign_click(sender, 2);
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Sign_click(sender, 3);
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Sign_click(sender, 4);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Sign_click(sender, 5);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button f = (Button)sender;
            if (f.BackColor == Color.Red)
                MessageBox.Show("Pogodjen na mestu");
            else if (f.BackColor == Color.Yellow)
                MessageBox.Show("Pogodjen na pogresnom mestu");
            else
                MessageBox.Show("Nema pogotka");
        }

        private void obrisi(object sender, MouseEventArgs e)
        {
            /*TODO*/
        }

        #endregion
    }
}
