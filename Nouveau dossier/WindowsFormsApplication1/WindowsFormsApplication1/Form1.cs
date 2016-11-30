using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // Représentation visuelles du jeu en mémoire.
        PictureBox[,] toutesImagesVisuelles = null;
        //Nombre de lignes dans le jeu
        int nbLignesJeu = 20;
        //Nombre de colonnes dans le jeu
        int nbColonnesJeu = 10;
        //Énumération des différents types de blocs dans le jeu
        enum TypeBloc { None, Gelé, Carré, Ligne, T, L, J, S, Z }
        TypeBloc[] tabBlocPossible = new TypeBloc[9] { TypeBloc.None, TypeBloc.Gelé, TypeBloc.Carré, TypeBloc.Ligne, TypeBloc.T, TypeBloc.L, TypeBloc.J, TypeBloc.S, TypeBloc.Z };
        TypeBloc[,] grilleJeu;
        // Y et X position initiale.
        int _yPosition = 0;
        int _xPosition = 10 / 2;
        TypeBloc pieceGenerer;




        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitialiserSurfaceVisuelle(nbLignesJeu, nbColonnesJeu);
            grilleJeu = InitialiserSurfaceDeJeu();
            pieceGenerer = GenererPieceAJouer();
            TypeBloc[,] TabChoixPiece = ChoixPiece(pieceGenerer);
            PositionnerBlocSurGrille(pieceGenerer);
        }

        /*-------------------Initialisation du jeu---------------------*/
        /// <summary>
        /// Fonction fait par le professeur pour initialiser les picturesBox dans le form.
        /// </summary>
        /// <param name="nbLignes"></param>
        /// <param name="nbCols"></param>
        private void InitialiserSurfaceVisuelle(int nbLignes, int nbCols)
        {
            // Création d'une surface de jeu 10 colonnes x 20 lignes
            toutesImagesVisuelles = new PictureBox[nbLignes, nbCols];
            tableauJeu.Controls.Clear();
            tableauJeu.ColumnCount = toutesImagesVisuelles.GetLength(1);
            tableauJeu.RowCount = toutesImagesVisuelles.GetLength(0);
            for (int i = 0; i < tableauJeu.RowCount; i++)
            {
                tableauJeu.RowStyles[i].Height = tableauJeu.Height / tableauJeu.RowCount;
                for (int j = 0; j < tableauJeu.ColumnCount; j++)
                {
                    tableauJeu.ColumnStyles[j].Width = tableauJeu.Width / tableauJeu.ColumnCount;
                    // Création dynamique des PictureBox qui contiendront les pièces de jeu
                    PictureBox newPictureBox = new PictureBox();
                    newPictureBox.Width = tableauJeu.Width / tableauJeu.ColumnCount;
                    newPictureBox.Height = tableauJeu.Height / tableauJeu.RowCount;
                    newPictureBox.BackColor = Color.Black;
                    newPictureBox.Margin = new Padding(0, 0, 0, 0);
                    newPictureBox.BorderStyle = BorderStyle.FixedSingle;
                    newPictureBox.Dock = DockStyle.Fill;

                    // Assignation de la représentation visuelle.
                    toutesImagesVisuelles[i, j] = newPictureBox;
                    // Ajout dynamique du PictureBox créé dans la grille de mise en forme.
                    // A noter que l' "origine" du repère dans le tableau est en haut à gauche.
                    tableauJeu.Controls.Add(newPictureBox, j, i);
                }
            }
        }
        /// <summary>
        /// Crée un tableau de la meme grandeur que le nombre de picturebox et initialise au valeur 0 = libre, 1 = occupé par une piece gelée.
        /// </summary>
        TypeBloc[,] InitialiserSurfaceDeJeu()
        {
            grilleJeu = new TypeBloc[nbLignesJeu, nbColonnesJeu];
            int iPos;
            int jPos;
            for(iPos =0; iPos< nbLignesJeu; iPos++)
            {
                for(jPos =0; jPos< nbColonnesJeu; jPos++)
                {
                    grilleJeu[iPos, jPos] = TypeBloc.None;
                }
            }
            return grilleJeu;
        }

        /*-------------------Generation piece aléatoire-------------------------*/
        /// <summary>
        /// Permet de choisir aléatoirement la piece qui va etre jouer.
        /// </summary>
        /// <returns>BlocChoisis</returns>
        /// C'est le la variable en TypeBloc qui a ete choisis.
        TypeBloc GenererPieceAJouer()
        {
            Random rnd = new Random();
            int pieceChoisis = rnd.Next(2, 7 + 1);
            //int pieceChoisis = 5;
            TypeBloc blocChoisis = tabBlocPossible[pieceChoisis];
            return blocChoisis;
        }
        /// <summary>
        /// Place bloc dans le tableau
        /// </summary>
        /// <param name="BlocChoisis"></param>
        /// <returns></returns>
        TypeBloc[,] ChoixPiece(TypeBloc BlocChoisis)
        {
            TypeBloc[,] tabRotationVide = CreationTableauRotation(BlocChoisis);
            switch (BlocChoisis)
            {
                case TypeBloc.Carré:
                    tabRotationVide[0,0] = TypeBloc.Carré;
                    tabRotationVide[0,1] = TypeBloc.Carré;
                    tabRotationVide[1,0] = TypeBloc.Carré;
                    tabRotationVide[1,1] = TypeBloc.Carré;

                    break;
                case TypeBloc.J:
                    tabRotationVide[0,1] = TypeBloc.J;
                    tabRotationVide[1,1] = TypeBloc.J;
                    tabRotationVide[2,1] = TypeBloc.J;
                    tabRotationVide[0,2] = TypeBloc.J;
                    break;
                case TypeBloc.L:
                    tabRotationVide[0,1] = TypeBloc.L;
                    tabRotationVide[1,1] = TypeBloc.L;
                    tabRotationVide[2,1] = TypeBloc.L;
                    tabRotationVide[2,2] = TypeBloc.L;
                    break;
                case TypeBloc.Ligne:
                    tabRotationVide[0,3] = TypeBloc.Ligne;
                    tabRotationVide[1,3] = TypeBloc.Ligne;
                    tabRotationVide[2,3] = TypeBloc.Ligne;
                    tabRotationVide[3,3] = TypeBloc.Ligne;
                    break;
                case TypeBloc.S:
                    tabRotationVide[0,2] = TypeBloc.S;
                    tabRotationVide[1,1] = TypeBloc.S;
                    tabRotationVide[1,2] = TypeBloc.S;
                    tabRotationVide[2,1] = TypeBloc.S;
                    break;
                case TypeBloc.T:
                    tabRotationVide[0,1] = TypeBloc.T;
                    tabRotationVide[1,1] = TypeBloc.T;
                    tabRotationVide[1,2] = TypeBloc.T;
                    tabRotationVide[2,1] = TypeBloc.T;
                    break;
                case TypeBloc.Z:
                    tabRotationVide[0,0] = TypeBloc.Z;
                    tabRotationVide[0,1] = TypeBloc.Z;
                    tabRotationVide[1,1] = TypeBloc.Z;
                    tabRotationVide[1,2] = TypeBloc.Z;
                    break;
            }
            return tabRotationVide;
        }
        
        TypeBloc[,] PositionnerBlocSurGrille(TypeBloc pieceChoisis)
        {
            PictureBox pic = toutesImagesVisuelles[_yPosition, _xPosition];
            TypeBloc[,] grilleJeu = InitialiserSurfaceDeJeu();
            TypeBloc[,] grilleAvecBloc = ChoixPiece(pieceChoisis);
            _xPosition = _xPosition - grilleAvecBloc.GetLength(1)/2;
            if (grilleAvecBloc.GetLength(1) == 3)
            {
                _xPosition -= 1;
            }
            else if (grilleAvecBloc.GetLength(1) == 4)
            {
                _xPosition -= 1;
            }
            for ( int iPos =0; iPos < grilleAvecBloc.GetLength(0); iPos++)
            {
                for ( int jPos =0; jPos<grilleAvecBloc.GetLength(1); jPos++)
                {
                    if (grilleAvecBloc[iPos, jPos] != TypeBloc.None)
                    {
                        grilleJeu[_yPosition + iPos, _xPosition + jPos] = pieceChoisis;
                        pic = toutesImagesVisuelles[_yPosition + iPos, _xPosition + jPos];
                        pic.BackColor = Color.Green;
                    }
                }
            }
            return grilleJeu;
        }

        /*------------------------Déplacement piece-----------------------*/
        /// <summary>
        /// Permet de déplacer le bloc sur la grille de jeu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            grilleJeu = InitialiserSurfaceDeJeu();
            DeplacementWASD(e, grilleJeu);
        }

        /*------------------Rotation Bloc-----------------------*/
        /// <summary>
        /// Permet de tourner les blocs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    break;
                case Keys.Left:
                    break;
                case Keys.Right:
                    break;
            }
        }

        /*---------------------------Deplacement Automatique------------------*/
        /// <summary>
        /// Permet de faire bouger la piece à chaque seconde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Fonction qui fait bouger les pieces.
            BougerPieceSurLaSurface();
        }

        /*---------------------------Fonctions Diverses----------------------------------*/

        void DeplacementPiece()
        {
            PictureBox pic = toutesImagesVisuelles[_yPosition, _xPosition];
            TypeBloc[,] grilleJeu = InitialiserSurfaceDeJeu();
            TypeBloc[,] grilleAvecBloc = ChoixPiece(pieceGenerer);
            for (int iPos = 0; iPos < grilleJeu.GetLength(0); iPos++)
            {
                for (int jPos = 0; jPos < grilleJeu.GetLength(1); jPos++)
                {
                    if (grilleJeu[iPos, jPos] != TypeBloc.Gelé)
                    {
                        grilleJeu[iPos, jPos] = TypeBloc.None;
                        pic = toutesImagesVisuelles[iPos,jPos];
                        pic.BackColor = Color.Black;
                    }
                }
            }

            for (int iPos = 0; iPos < grilleAvecBloc.GetLength(0); iPos++)
            {
                for (int jPos = 0; jPos < grilleAvecBloc.GetLength(1); jPos++)
                {
                    if (_yPosition + iPos < 20 && grilleAvecBloc[iPos , jPos] != TypeBloc.None)
                    {
                        grilleJeu[_yPosition + iPos, _xPosition + jPos] = pieceGenerer;
                        pic = toutesImagesVisuelles[_yPosition + iPos, _xPosition + jPos];
                        pic.BackColor = Color.Green;
                    }
                }
            }
        }
        //Déplacement

        /// <summary>
        /// Fait descendre la piece à chaque tick (1000 millisecondes)
        /// </summary>
        void BougerPieceSurLaSurface()
        {
            bool peutDescendre = true;
            TypeBloc pieceChoisis = GenererPieceAJouer();
            TypeBloc[,] grilleAvecBloc = ChoixPiece(pieceChoisis);
            PictureBox pic = toutesImagesVisuelles[_yPosition, _xPosition];
            if (_yPosition < grilleJeu.GetLength(0))
            {
                if (_yPosition == grilleJeu.GetLength(0) - 1)
                {
                    //pic.BackColor = Color.White;
                    grilleJeu[_yPosition, _xPosition] = TypeBloc.Gelé;
                    _yPosition = 0;
                }
                else if (grilleJeu[_yPosition + 1, _xPosition] == TypeBloc.Gelé)
                {
                    //pic.BackColor = Color.DarkGoldenrod;
                    grilleJeu[_yPosition, _xPosition] = TypeBloc.Gelé;
                    _yPosition = 0;
                }
                else
                {
                    for (int iPos = 0; iPos < grilleAvecBloc.GetLength(0); iPos++)
                    {
                        for (int jPos = 0; jPos < grilleAvecBloc.GetLength(1); jPos++)
                        {
                            if ( _yPosition > 19 && grilleJeu[_yPosition + iPos, _xPosition + jPos] != TypeBloc.None && _yPosition + iPos == nbLignesJeu - 1)
                            {
                                peutDescendre = false;
                                _yPosition = 0;
                            }
                        }
                    }
                    if ( peutDescendre == true)
                    {
                        _yPosition++;
                    }
                    DeplacementPiece();
                }
            }
        }

        /// <summary>
        /// Fonction qui prend comme input ASD / gauche bas droit pour bouger le bloc.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="grilleJeu"></param>
        void DeplacementWASD(KeyPressEventArgs e, TypeBloc[,] grilleJeu)
        {
            TypeBloc[,] grilleAvecBloc = ChoixPiece(pieceGenerer);
            // UP
            if (e.KeyChar == 'w')
            {
                if (_yPosition - 1 != 0)
                {
                    _yPosition--;
                }
            }
            // Down
            if (e.KeyChar == 's')
            {
                if (_yPosition + 1 < grilleJeu.GetLength(0))
                {
                    if (grilleJeu[_yPosition + 1, _xPosition] != TypeBloc.Gelé)
                    {
                        _yPosition++;

                    }

                }
            }
            // Right
            else if (e.KeyChar == 'd')
            {
                int iPos = grilleAvecBloc.GetLength(0);
                if (_xPosition + iPos < grilleJeu.GetLength(1) && grilleJeu[_yPosition, _xPosition + 1] != TypeBloc.Gelé)
                {
                    _xPosition++;
                }
            }
            // Left
            else if (e.KeyChar == 'a')
            {
                if (_xPosition != 0)
                {
                    if (grilleJeu[_yPosition, _xPosition - 1] != TypeBloc.Gelé)
                    {
                        _xPosition--;
                    }
                }
            }
        }


        /// <summary>
        /// Crée le tableau dans lequel les pieces vont être générées.
        /// </summary>
        /// <returns>tabRotation</returns>
        /// C'est le tableau vide dans lequel une piece va être générée.
        TypeBloc[,] CreationTableauRotation(TypeBloc PieceChoisis)
        {
            int taille;
            if ( PieceChoisis == TypeBloc.Ligne)
            {
                taille = 4;
            }
            else if (PieceChoisis == TypeBloc.Carré)
            {
                taille = 2;
            }
            else
            {
                taille = 3;
            }

            TypeBloc[,] tabRotationVide = new TypeBloc[taille,taille];
            int i;
            int j;
            for (i = 0; i < taille; i++)
            {
                for (j = 0; j < taille; j++)
                {
                    tabRotationVide[i, j] = TypeBloc.None;
                }
            }
            return tabRotationVide;
        }

        /// <summary>
        /// Permet de quitter le jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitterMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
