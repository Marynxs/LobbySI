using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagicTrickServer;

namespace lobby
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lblVersao.Text = Jogo.Versao;
        }

        private void btnPartidas_Click(object sender, EventArgs e)
        {
            string partidasList = Jogo.ListarPartidas("T");
            partidasList = partidasList.Replace("\r", "");
            partidasList = partidasList.Substring(0, partidasList.Length - 1);

            string[] nomePartidas = partidasList.Split('\n');

            lstPartsEncontradas.Items.Clear();

            
            for (int i = 0; i < nomePartidas.Length; i++)
            {
                lstPartsEncontradas.Items.Add(nomePartidas[i]);

            }



        }

    
        private void lstPartsEncontradas_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string partidaList = lstPartsEncontradas.SelectedItem.ToString();
            string[] dadosPartida = partidaList.Split(',');

            int idPart = Convert.ToInt32(dadosPartida[0]);

            lblId.Text = idPart.ToString();
            lblNome.Text = dadosPartida[1];
            lblData.Text = dadosPartida[2];
        }

        private void btnListJogadores_Click(object sender, EventArgs e)
        {

            if (lstPartsEncontradas.SelectedItem != null) // Verifica se há uma partida selecionada na primeira ListBox
            {
                string partidaList = lstPartsEncontradas.SelectedItem.ToString();
                string[] partidInfo = partidaList.Split(',');

                int idPartida = Convert.ToInt32(partidInfo[0]);

                string jogadoresList = Jogo.ListarJogadores(idPartida);
                lstJogadores.Items.Clear();

                jogadoresList = jogadoresList.Replace("\r", "");
            
                string[] nomeJogadores = jogadoresList.Split('\n');

                for (int i = 0; i < nomeJogadores.Length; i++)
                {
                    lstJogadores.Items.Add(nomeJogadores[i]);
                }
            }
            else
            {
                MessageBox.Show(" selecione uma partida antes de listar os jogadores" , "MagicTrick" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnCriarPartida_Click(object sender, EventArgs e)
        {
            string nomePartida = txtNomePartida.Text;
            string senha = txtSenha.Text;
            string grupo = txtGrupo.Text;


            Jogo.CriarPartida(nomePartida, senha, grupo);
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string nomeDoJogador = txtPlayerName.Text;
            string senhaEntrar = txtSenhaLogIn.Text;
            int id = Convert.ToInt32(txtID.Text);

            Jogo.EntrarPartida(id, nomeDoJogador, senhaEntrar);
        }
    }
}
