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
        /// Rempli le combo avec les lettres de l'alphabet
        /// </summary>
        private void RemplirCboLettres()
        {
            // vide le combo
            cboLettre.Items.Clear();
            // rempli le combo avec les lettres de l'alphabet
            for (int k = 0; k < 26; k++)
            {
                cboLettre.Items.Add((char)('A' + k));
            }
            cboLettre.SelectedIndex = 0;
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
            // vide le label des lettres
            lblLettres.Text = "";
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
            // active la zone de la phase 2 (proposition de lettres)
            grpTestLettres.Enabled = true;
            // désactive la zone de texte contenant le mot
            txtMot.Enabled = false;
            // remplit le combo des lettres
            RemplirCboLettres();
            // donne le focus au bouton test
            btnTest.Focus();
        }

        /// <summary>
        /// Affiche une image d'une numéro précis
        /// </summary>
        /// <param name="num"></param>
        private void AfficheImage(int num)
        {
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
        /// Traitement de la lettre récupérée
        /// </summary>
        /// <param name="lettre"></param>
        private void TraitementLettre(char lettre)
        {
            // cherche si la lettre est présente dans le mot
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPendu_Load(object sender, EventArgs e)
        {
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

        /// <summary>
        /// Evénement clic sur le bouton test
        /// recherche de la lettre dans le mot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                // récupération de la lettre, affichage et suppression dans le combo
                char lettre = char.Parse(cboLettre.SelectedItem.ToString());
                lblLettres.Text += lettre;
                cboLettre.Items.RemoveAt(cboLettre.SelectedIndex);
                // traite la lettre (recherche si elle est présente dans le mot)
                TraitementLettre(lettre);
                // se positionne sur la 1ère lettre du combo
                cboLettre.SelectedIndex = 0;
            }
            catch
            {
                // cas d'une tentative de suppression de lettre alors que le combo est vide
                // normalement ce cas ne doit pas arriver car le mot doit être trouvé avant
                grpTestLettres.Enabled = false;
            }
        }

        /// <summary>
        /// Evénement choix d'une lettre dans le combo
        /// position du focus sur le bouton  test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboLettre_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTest.Focus();
        }
    }
}
