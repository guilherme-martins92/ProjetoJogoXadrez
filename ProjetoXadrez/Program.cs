using System;
using ProjetoXadrez.tabuleiro;
using ProjetoXadrez.xadrez;

namespace ProjetoXadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.ColocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
                tab.ColocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 9));
                tab.ColocarPeca(new Rei(tab, Cor.Preta), new Posicao(0, 0));



                Tela.ImprimirTabuleiro(tab);           
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            */
            PosicaoXadrez pos = new PosicaoXadrez('a', 1);
            Console.WriteLine(pos);
            Console.WriteLine(pos.toPosicao());
            Console.ReadLine();

        }
    }
}
