
namespace ProjetoXadrez.tabuleiro
{
    class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QuantidadeDeMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Posicao posicao, Cor cor, Tabuleiro tab)
        {
            this.Posicao = posicao;
            this.Tab = tab;
            this.Cor = cor;
            this.QuantidadeDeMovimentos = 0;
        }
    }
}
