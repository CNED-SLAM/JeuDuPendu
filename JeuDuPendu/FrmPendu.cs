using System;
using System.Drawing;
using System.Windows.Forms;

namespace JeuDuPendu
{
    public partial class frmPendu : Form
    {
        /// <summary>
        /// mot à rechercher
        /// </summary>
        private string mot;
        /// <summary>
        /// étape d'affichage du pendu
        /// </summary>
        private int etapePendu;
        /// <summary>
        /// maximum d'étapes du pendu
        /// </summary>
        private int maxPendu = 10;

        /// <summary>
        /// Initialisation des objets graphiques
        /// </summary>
        public frmPendu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Permet de créer les boutons de l'alphabet
        /// </summary>
        private void CreeBoutons()
        {
            // variables locales
            char[] tAlpha = new char[26]; // tableau des lettres
            int sizeButton = 30; // taille du côté d'un bouton de lettre
            int posXButton = 6; // position du 1er bouton à partir de la gauche 
            int posYButton = 18; // position du 1er bouton à partir du haut
            int nbLettreParLigne = 9; // détermine le nombre de boutons de lettres par ligne
            // remplissage du tableau de lettres
            for (int k = 0; k < tAlpha.Length; k++)
            {
                tAlpha[k] = (char)('A' + k);
            }

            // création et positionnement des boutons
            int line = -1, col = 0;
            for (int k = 0; k < tAlpha.Length; k++)
            {
                // création du bouton
                Button btn = new Button();
                // ajout du bouton dans le groupe de boutons pour l'affichage
                grpTestLettres.Controls.Add(btn);
                // Ajoute la lettre correspondante au bouton
                btn.Text = tAlpha[k].ToString();
                // fixe la taille du bouton
                btn.Size = new Size(sizeButton, sizeButton);
                // ajout d'une écoute sur le clic du bouton
                btn.Click += new System.EventHandler(btnAlpha_Click);
                // changement de ligne au bout d'un certain nombre de boutons affichés
                col++;
                if (k % nbLettreParLigne == 0)
                {
                    line++;
                    col = 0;
                }
                // positionne le bouton dans le groupe
                btn.Location = new Point(posXButton + sizeButton * col, posYButton + sizeButton * line);
            }
        }

        /// <summary>
        /// Active les boutons des lettres
        /// </summary>
        private void ActiveBoutons()
        {
            // active les boutons
            for (int k = 0; k < grpTestLettres.Controls.Count; k++)
            {
                grpTestLettres.Controls[k].Enabled = true;
            }
            grpTestLettres.Controls[0].Focus();
        }

        /// <summary>
        /// Rend le focus au bouton de la première lettre non désactivée
        /// </summary>
        private void GestionFocusBoutonLettre()
        {
            int k = 0;
            while (!grpTestLettres.Controls[k].Enabled)
            {
                k++;
            }
            grpTestLettres.Controls[k].Focus();
        }

        /// <summary>
        /// Evénement clic sur un des boutons de l'alphabet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlpha_Click(object sender, EventArgs e)
        {
            // récupération du bouton concerné par l'événement
            Button btnLettre = ((Button)sender);
            // désactive le bouton et donne le focus au premier bouton accessible
            btnLettre.Enabled = false;
            GestionFocusBoutonLettre();
            // recherche la lettre
            char lettre = char.Parse(btnLettre.Text);
            if (!RechercheLettreDansMot(lettre))
            {
                // lettre non trouvée : affichage du pendu
                if (AffichePendu())
                {
                    // si totalement pendu, perdu et fin du jeu
                    FinJeu("PERDU");
                }
            }
            else
            {
                // il n'y a plus de lettre à trouver
                if (txtMot.Text.IndexOf('-') == -1)
                {
                    // s'il n'y a plus de tiret (toutes les lettres trouvées) c'est gagné
                    FinJeu("GAGNE");
                }
            }
        }

        /// <summary>
        /// Préparation de la phase 1 (saisie du mot à chercher)
        /// </summary>
        private void PreparationPhase1()
        {
            // réinitialise l'étape du pendu et affiche l'image vide
            etapePendu = 0;
            AfficheImage(etapePendu);
            // désactive la zone de la phase 2 (proposition de lettres)
            grpTestLettres.Enabled = false;
            // supprime le message
            lblResultat.Text = "";
            // active, vide et se positionne sur la zone de saisie du mot
            txtMot.Enabled = true;
            txtMot.Text = "";
            txtMot.Focus();
        }

        /// <summary>
        /// Préparation de la phase 2 (recherche du mot)
        /// </summary>
        private void PreparationPhase2()
        {
            // active les boutons et la zone de la phase 2 (proposition de lettres)
            ActiveBoutons();
            grpTestLettres.Enabled = true;
            // positionne le focus sur le premier bouton (lettre A)
            grpTestLettres.Controls[0].Focus();
            // désactive la zone de texte contenant le mot
            txtMot.Enabled = false;
        }

        /// <summary>
        /// Affiche une image d'une numéro précis
        /// </summary>
        /// <param name="num"></param>
        private void AfficheImage(int num)
        {
//            imgPendu.ImageLocation = Application.StartupPath + "/../../Resources/pendu" + num + ".png";
            imgPendu.Image = (Image)Properties.Resources.ResourceManager.GetObject("pendu" + num);
        }

        /// <summary>
        /// Afichage d'une étape du pendu
        /// </summary>
        /// <returns></returns>
        private bool AffichePendu()
        {
            etapePendu++;
            AfficheImage(etapePendu);
            return (etapePendu == maxPendu);
        }

        /// <summary>
        /// Recherche la lettre dans le mot et remplace le tiret par la lettre
        /// retourne vrai si la lettre est trouvée au moins une fois
        /// </summary>
        /// <param name="lettre">lettre à chercher</param>
        /// <returns></returns>
        private bool RechercheLettreDansMot(char lettre)
        {
            int position = -1; // position de la lettre dans le mot
            bool trouve = false; // pour savoir si la lettre a été trouvée
            // boucle sur la recherche de la lettre (qui peut être présente plusieurs fois)
            do
            {
                // récupère la position de la lettre (ou -1)
                position = mot.IndexOf(lettre, position + 1);
                if (position != -1)
                {
                    trouve = true;
                    // remplace le tiret par la lettre dans la zone de texte
                    txtMot.Text = txtMot.Text.Remove(position, 1);
                    txtMot.Text = txtMot.Text.Insert(position, lettre.ToString());
                }
            } while (position != -1);
            return trouve;
        }

        /// <summary>
        /// Fin du jeu (gagné ou perdu)
        /// </summary>
        /// <param name="message"></param>
        private void FinJeu(string message)
        {
            // affichage du message (gagné ou perdu)
            lblResultat.Text = message;
            // affiche le mot correct
            txtMot.Text = mot;
            // désactive la zone de proposition de lettre
            grpTestLettres.Enabled = false;
            // se positionne sur le bouton pour recommencer
            btnRejouer.Focus();
        }

        /// <summary>
        /// Contrôle si un mot est bien constitué uniquement de lettres
        /// </summary>
        /// <param name="unMot"></param>
        /// <returns></returns>
        private bool MotCorrect(string unMot)
        {
            unMot = unMot.ToUpper();
            bool correct = true;
            for (int k = 0; k < unMot.Length; k++)
            {
                if (unMot[k] < 'A' || unMot[k] > 'Z')
                {
                    correct = false;
                }
            }
            return correct;
        }

        /// <summary>
        /// Evénement chargement de la fenêtre
        /// création des boutons des lettres de l'alphabet
        /// et démarrage du jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPendu_Load(object sender, EventArgs e)
        {
            // création des boutons des lettres
            CreeBoutons();
            // préparation des objets graphiques pour la phase 1 (saisie du mot)
            PreparationPhase1();
        }

        /// <summary>
        /// Evénement touche enfoncée dans la zone du mot
        /// si validation, enregistrement du mot et début du jeu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMot_KeyPress(object sender, KeyPressEventArgs e)
        {
            // validation donc fin de la saisie du mot
            if (e.KeyChar == (char)Keys.Enter)
            {
                // vérifie qu'un mot a été tapé et qu'il est correct (uniquement lettres)
                if (!txtMot.Text.Equals("") && MotCorrect(txtMot.Text))
                {
                    // met le mot en majuscule et le mémorise dans une propriété
                    mot = txtMot.Text.ToUpper();
                    // remplit la zone de tirets à la place des lettres
                    txtMot.Text = "";
                    for (int k = 0; k < mot.Length; k++)
                    {
                        txtMot.Text += "-";
                    }
                    // préparation des objets graphiques pour la phase 2 (recherche du mot)
                    PreparationPhase2();
                }
                else
                {
                    // le mot n'est pas correct : efface la zone
                    MessageBox.Show("Le mot ne doit comporter que des lettres alphabétiques (pas d'espace, pas d'accent)");
                    txtMot.Text = "";
                    txtMot.Focus();
                }
            }
        }

        /// <summary>
        /// Evénement clic sur le bouton rejouer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRejouer_Click(object sender, EventArgs e)
        {
            // préparation des objets graphiques pour la phase 1 (saisie du mot)
            PreparationPhase1();
        }
    }
}
