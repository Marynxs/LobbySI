using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MagicTrickServer;

namespace lobby
{
    public partial class Lobby : Form
    {
        //variaveis globais
        List<string> dadosJogador = new List<string>();


        public Lobby()
        {
            InitializeComponent();
            lblVersao.Text = Jogo.Versao;
        }

        private void btnPartidas_Click(object sender, EventArgs e)
        {
            string partidasList = Jogo.ListarPartidas("T");
            partidasList = partidasList.Replace("\r", "");
            if (partidasList.Length > 0)
                partidasList = partidasList.Substring(0, partidasList.Length - 1);

            string[] nomePartidas = partidasList.Split('\n');

            lstPartsEncontradas.Items.Clear();

            
            for (int i = 0; i < nomePartidas.Length; i++)
            {
                lstPartsEncontradas.Items.Add(nomePartidas[i]);

            }

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
                if (jogadoresList.Length > 0)
                    jogadoresList = jogadoresList.Substring(0, jogadoresList.Length - 1);

                string[] nomeJogadores = jogadoresList.Split('\n');

                for (int i = 0; i < nomeJogadores.Length; i++)
                {
                    lstJogadores.Items.Add(nomeJogadores[i]);
                }
            }
            else
            {
                MessageBox.Show("selecione uma partida antes de listar os jogadores" , "MagicTrick" , MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnCriarPartida_Click(object sender, EventArgs e)
        {
            string nomePartida = txtNomePartida.Text;
            string senha = txtSenha.Text;
            string grupo = "Helsinque";

            

            string retornoCriacao =  Jogo.CriarPartida(nomePartida, senha, grupo);

             
            if (retornoCriacao.Length > 4 && retornoCriacao.Substring(0,4) == "ERRO")
            {
                MessageBox.Show($"Ocorreu um erro ao criar a partida:\n{retornoCriacao.Substring(5)}", "MagicTrick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtID.Text = retornoCriacao;


        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (txtPlayerName.Text != "" && txtSenhaLogIn.Text != "" && txtID.Text != "")
            {
                string nomeDoJogador = txtPlayerName.Text;
                string senhaEntrar = txtSenhaLogIn.Text;
                int idPartida = Convert.ToInt32(txtID.Text);
                string retornoEntrar = Jogo.EntrarPartida(idPartida, nomeDoJogador, senhaEntrar) + ',' + nomeDoJogador;
                if (retornoEntrar.Length > 4 && retornoEntrar.Substring(0, 4) == "ERRO")
                {
                    MessageBox.Show($"Ocorreu um erro ao entrar a partida:\n{retornoEntrar.Substring(5)}", "MagicTrick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dadosJogador.Add(retornoEntrar);
                btnListJogadores_Click(sender, e);
            }
            else
            {
                MessageBox.Show($"Ocorreu um erro ao entrar na partida:\nUm ou mais campos estão vazios", "MagicTrick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnStartMatch_Click(object sender, EventArgs e)
        {
            string[] dados = dadosJogador.ToArray();
            //Ve na lista de jogadores e separa cada informação de cada jogador em uma matriz [jogador][informação] ex. pega id do primeiro jogador: [0][0]
            string[][] dadosJogadores = new string[dados.Length][];
            for (int i = 0; i < dados.Length; i++)
            {
                string[] atual = dados[i].Split(',');
                dadosJogadores[i] = new string[atual.Length];
                for (int j = 0; j < atual.Length; j++)
                {
                    dadosJogadores[i][j] = atual[j];
                }

            }

            Random random = new Random();

            int randomJogador = random.Next(dados.Length);
            lblNome.Text = dadosJogadores[randomJogador][2];
            lblSenha.Text = dadosJogadores[randomJogador][1];

            string retornoIniciar = Jogo.IniciarPartida(Convert.ToInt32(dadosJogadores[randomJogador][0]), dadosJogadores[randomJogador][1]);
            if (retornoIniciar.Length > 4 && retornoIniciar.Substring(0, 4) == "ERRO")
            {
                MessageBox.Show($"Ocorreu um erro ao iniciar a partida:\n{retornoIniciar.Substring(5)}", "MagicTrick", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblId.Text = retornoIniciar;
        }

    }
}
