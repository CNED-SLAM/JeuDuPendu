using System;
using System.Windows.Forms;

namespace JeuDuPendu
{
    public partial class frmPendu : Form
    {
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
                //   if (char.Parse(unMot.Substring(k, 1)) < 'A' || char.Parse(unMot.Substring(k, 1)) > 'Z')
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
        /// si validation, début de la phase 2
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
