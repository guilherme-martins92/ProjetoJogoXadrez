
namespace ProjetoXadrez.tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QuantidadeDeMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab)
        {
            this.Posicao = null;
            this.Tab = tab;
            this.Cor = cor;
            this.QuantidadeDeMovimentos = 0;
        }

        public void IncrementarQtdMovimento()
        {
            this.QuantidadeDeMovimentos++;
        }

        public abstract bool[,] MovimentosPossiveis();

    }
}
