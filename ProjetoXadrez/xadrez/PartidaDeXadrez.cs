using System;
using System.Collections.Generic;
using ProjetoXadrez.tabuleiro;

namespace ProjetoXadrez.xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturada;

        public PartidaDeXadrez()
        {
            this.Tab = new Tabuleiro(8, 8);
            this.Turno = 1;
            this.JogadorAtual = Cor.Branca;
            this.Terminada = false;
            this.Pecas = new HashSet<Peca>();
            this.Capturada = new HashSet<Peca>();

            this.ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdMovimento();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);

            if (pecaCapturada != null)
            {
                Capturada.Add(pecaCapturada);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            this.ExecutaMovimento(origem, destino);
            this.Turno++;
            this.MudaJogador();
        }

        private void MudaJogador()
        {
            if (this.JogadorAtual == Cor.Branca)
            {
                this.JogadorAtual = Cor.Preta;
            }
            else
            {
                this.JogadorAtual = Cor.Branca;
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida");
            }
            if (this.JogadorAtual != Tab.peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!Tab.peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça escolhida");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.peca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição destino inválida!");
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturada)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            this.ColocarNovaPeca('c', 1, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('c', 2, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('d', 2, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('e', 2, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('e', 1, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('d', 1, new Rei(Tab, Cor.Branca));

            this.ColocarNovaPeca('c', 7, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('c', 8, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('d', 7, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('e', 7, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('e', 8, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('d', 8, new Rei(Tab, Cor.Preta));

        }
    }
}
