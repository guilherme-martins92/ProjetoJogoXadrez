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

        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            this.Tab = new Tabuleiro(8, 8);
            this.Turno = 1;
            this.JogadorAtual = Cor.Branca;
            this.Terminada = false;
            this.Pecas = new HashSet<Peca>();
            this.Capturada = new HashSet<Peca>();
            this.xeque = false;
            this.ColocarPecas();
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmCheque(Cor cor)
        {
            Peca R = Rei(cor);

            if (R == null)
            {
                throw new TabuleiroException("Não tem Rei da cor " + cor + " no tabuleiro"!);
            }
            foreach (Peca x in pecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();

                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }

            return false;
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQtdMovimento();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);

            if (pecaCapturada != null)
            {
                Capturada.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQtdMovimento();

            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturada.Remove(pecaCapturada);
            }

            Tab.ColocarPeca(p, origem);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = this.ExecutaMovimento(origem, destino);

            if (estaEmCheque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em cheque!");
            }
            if (estaEmCheque(Adversaria(JogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TestarXequeMate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                this.Turno++;
                this.MudaJogador();
            }
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

        public bool TestarXequeMate(Cor cor)
        {
            if (!estaEmCheque(cor))
            {
                return false;
            }

            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = estaEmCheque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
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
            if (!Tab.peca(origem).MovimentoPossivel(destino))
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
            this.ColocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branca));
            this.ColocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branca));
            this.ColocarNovaPeca('d', 1, new Dama(Tab, Cor.Branca));
            this.ColocarNovaPeca('e', 1, new Rei(Tab, Cor.Branca));
            this.ColocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branca));
            this.ColocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branca));
            this.ColocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));
            this.ColocarNovaPeca('a', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('b', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('c', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('d', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('e', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('f', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('g', 2, new Peao(Tab, Cor.Branca));
            this.ColocarNovaPeca('h', 2, new Peao(Tab, Cor.Branca));

            this.ColocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preta));
            this.ColocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preta));
            this.ColocarNovaPeca('d', 8, new Dama(Tab, Cor.Preta));
            this.ColocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta));
            this.ColocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preta));
            this.ColocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preta));
            this.ColocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
            this.ColocarNovaPeca('a', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('b', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('c', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('d', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('e', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('f', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('g', 7, new Peao(Tab, Cor.Preta));
            this.ColocarNovaPeca('h', 7, new Peao(Tab, Cor.Preta));


        }
    }
}
