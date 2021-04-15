using ProjetoXadrez.tabuleiro;


namespace ProjetoXadrez.xadrez
{
    class PosicaoXadrez
    {
        public char Coluna { get; set; }
        public int Linha { get; set; }

        public PosicaoXadrez(char coluna, int linha)
        {
            this.Coluna = coluna;
            this.Linha = linha;
        }

        public Posicao toPosicao()
        {
            return new Posicao(8 - this.Linha, this.Coluna - 'a');
        }
        public override string ToString()
        {
            return "" + this.Coluna + this.Linha;
        }



    }
}
