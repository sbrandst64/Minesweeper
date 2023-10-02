using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        static int spielfeldgrößeX = 20;
        static int spielfeldgrößeY = 10;

        Label[,] Feld = new Label[spielfeldgrößeX, spielfeldgrößeY];
        int feldabstand = 22;
        int currentBombs = 50;
        public Form1()
        {
            int bombsNear = 0;
            InitializeComponent();
            
            var path = new System.Drawing.Drawing2D.GraphicsPath();

            Rectangle myRec = new Rectangle(20, 20, 20, 20);
            path.AddRectangle(myRec);
            this.BackColor = Color.Black;
            for (int X = 0; X < spielfeldgrößeX; X++)
            {
                for (int Y = 0; Y < spielfeldgrößeY; Y++)
                {
                                     
                    Feld[X, Y] = new System.Windows.Forms.Label();
                    Feld[X, Y].BackColor = Color.Gray;
                    Feld[X, Y].AutoSize = false;
                    Feld[X, Y].Location = new System.Drawing.Point(X * feldabstand, Y * feldabstand);
                    Feld[X, Y].Size = new System.Drawing.Size(40, 40);
                    Feld[X, Y].Text = " ";
                    Feld[X, Y].AccessibleDescription = "";
                    Feld[X, Y].TextAlign = ContentAlignment.BottomRight;
                    Feld[X, Y].Region = new Region(path);
                    Feld[X, Y].MouseClick += new MouseEventHandler(this.label1_Click);
                    

                    this.Controls.Add(Feld[X, Y]);
                   
                }
            }
            place_Bombs();

            //initialize all fields
            for (int X = 0; X < spielfeldgrößeX; X++)
            {
                for (int Y = 0; Y < spielfeldgrößeY; Y++)
                {
                    if (Feld[X,Y].AccessibleDescription != "bomb")
                    {
                        bombsNear = checkNearBombs(X, Y);
                        Feld[X, Y].AccessibleDescription = bombsNear.ToString();
                    }

                }
            }


        }

        private void label1_Click(object sender, MouseEventArgs e)
        {
            
          Label clickedField = sender as Label;
                if (e.Button == MouseButtons.Left){

                if (clickedField.BackColor == Color.Gray)
                {
                    clickedField.BackColor = Color.White;
                    clickedField.ForeColor = Color.Blue;
                    clickedField.Text = clickedField.AccessibleDescription;

                }
                //Wenn 0 dann decke alle anliegenden Felder auf
                if(clickedField.Text == "0")
                {
                    checkSurroundingZeros(clickedField.Location.X / feldabstand, clickedField.Location.Y / feldabstand);
                }

                //check if Death
                checkIfDeath(clickedField.Location.X / feldabstand, clickedField.Location.Y / feldabstand);
                }

                //Place flag
            if (e.Button == MouseButtons.Right)
            {
                placeFlag(clickedField.Location.X / feldabstand, clickedField.Location.Y / feldabstand);
            }
            checkIfWon();
        }
        public void placeFlag(int X, int Y)
        {
            if (Feld[X, Y].BackColor != Color.White)
            {
                if (Feld[X, Y].Text == "F")
                {
                    if (Feld[X, Y].AccessibleDescription == "bomb")
                    {
                        Feld[X, Y].BackColor = Color.Red;
                        Feld[X, Y].Text = " ";
                    }
                    else
                    {
                        Feld[X, Y].Text = " ";
                        Feld[X, Y].BackColor = Color.Gray;
                    }
                }
                else
                {
                    Feld[X, Y].Text = "F";
                    Feld[X, Y].BackColor = Color.Yellow;
                }
            }
        }
        public void checkIfDeath(int X , int Y)
        {
            if (Feld[X,Y].AccessibleDescription == "bomb")
            {
                for (int x = 0; x < spielfeldgrößeX; x++)
                {
                    for (int y = 0; y < spielfeldgrößeY; y++)
                    {
                        if (Feld[X, Y].AccessibleDescription == "bomb")
                        {
                            Feld[X, Y].BackColor = Color.Red;
                            Feld[X, Y].Text = " ";
                        }
                    }
                }
                MessageBox.Show("Du host oag vergackt");
                reset();
            }
        }
        public void checkSurroundingZeros(int X, int Y)
        {
            var zerolist = new List<Tuple<int, int>>();
            int zeroCounter = 0;
            for (int sx = -1; sx <= 1; sx++)
            {
                for (int sy = -1; sy <= 1; sy++)
                {
                    if (X + sx >= 0
                       && X + sx < spielfeldgrößeX
                       && Y + sy >= 0
                       && Y + sy < spielfeldgrößeY)
                    {
                        checkNearBombs(X + sx, Y + sy);
                        //speichere alle nuller in eine Liste
                        if(Feld[X + sx, Y + sy].AccessibleDescription == "0")
                        {
                            zerolist.Add(new Tuple<int, int>(X+sx, Y+sy));

                        }
                        Feld[X + sx, Y + sy].BackColor = Color.White;
                        Feld[X + sx, Y + sy].Text = Feld[X + sx, Y + sy].AccessibleDescription;
                        
                    }
                }
            }
            foreach(Tuple <int,int> oasch in zerolist)
            {
                checkSurroundingZeros(oasch.Item1, oasch.Item2);
            }
        }
      
     
    
        public void reset()
        {
            for (int X = 0; X < spielfeldgrößeX; X++)
            {
                for (int Y = 0; Y < spielfeldgrößeY; Y++)
                {

                    Feld[X, Y].BackColor = Color.Gray;
                    Feld[X, Y].Text = " ";
                    Feld[X, Y].AccessibleDescription = "";
                }
            }
            place_Bombs();
        }
        public void checkIfWon()
        {
            
        }
        public int checkNearBombs(int X,int Y)
        {                   
            int bombcounter = 0;
            for (int sx = -1; sx <= 1; sx++)
            {
                for (int sy = -1; sy <= 1; sy++)
                {
                    if (X + sx >= 0
                        && X + sx < spielfeldgrößeX
                        && Y + sy >= 0
                        && Y + sy < spielfeldgrößeY)
                    {
                        if (Feld[X + sx, Y + sy].AccessibleDescription == "bomb")
                        {

                            bombcounter++;

                        }
                    }
                }
            }


            return bombcounter;
        }
        public void place_Bombs()
        {
            
            int bombX;
            int bombY;
            Random rnd = new Random();
            for (int i = 0; i<=currentBombs; i++)
            {

                bombX = rnd.Next(0,spielfeldgrößeX);
                bombY = rnd.Next(0,spielfeldgrößeY);

                if (Feld[bombX, bombY].Text == " ")
                {
                    Feld[bombX, bombY].AccessibleDescription="bomb";
                    Feld[bombX, bombY].BackColor = Color.Red;
                    
                }
                else
                {
                    i--;
                }
            }
        }

    }
}
