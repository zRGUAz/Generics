using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Generics
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dicionario = new Dictionary<string, string>();

            if (!File.Exists("C:\\Dicionario.bin")) //Cria o Arquivo se Não Existe
            {
                using (FileStream fs = File.Create("C:\\Dicionario.bin")) { }
            }
            else //Pega o Dicionario Existente no Arquivo da Maquina
            {
                try
                {
                    using (Stream stream = File.Open("Dicionario.bin", FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        Dictionary<string, string> dicionarioCarregado = (Dictionary<string, string>)bin.Deserialize(stream);
                        dicionario = dicionarioCarregado;
                    }
                }
                catch //Se já existe um arquivo Dicionario.bin, ele limpa esse arquivo
                {
                    File.WriteAllBytes("Dicionario.bin", new byte[0]);
                }

                
            }

            char opcao;
            do
            {
                Console.WriteLine("Let's Speak\n");

                Console.WriteLine("1 - Adicionar termo no dicionario");
                Console.WriteLine("2 - Buscar termos");
                Console.WriteLine("S - Sair");
                Console.Write("\nDigite uma das opções acima: ");

                do
                {
                    char.TryParse(Console.ReadLine().ToUpper(), out opcao);

                    if (opcao != '1' && opcao != '2' && opcao != 'S')
                    {
                        Console.WriteLine("Opção digitada inexistente\n");
                        Console.Write("Digite 1 para Adicionar Termo, 2 para Buscar e S para Sair: ");
                    }
                } while (opcao != '1' && opcao != '2' && opcao != 'S');

                Console.Clear();

                switch (opcao)
                {
                    case '1':
                        Console.WriteLine("Adicionar Termo\n");

                        bool wildcards;
                        string termo;
                        do
                        {
                            Console.Write("Digite o Termo que Deseja Adicionar: ");
                            termo = Console.ReadLine();

                           wildcards = ValidarWildcards(termo);

                        } while (wildcards);

                        Console.Write("Digite o Significado de {0}: ", termo);
                        string significado = Console.ReadLine();

                        dicionario.Add(termo, significado);

                        Console.WriteLine("\nDigite qualquer tecla para continuar");
                        Console.ReadKey();
                        break;

                    case '2':
                        Console.WriteLine("Buscar Termo\n");

                        do
                        {
                            Console.Write("Digite o Termo que Deseja Buscar: ");
                            termo = Console.ReadLine();

                            wildcards = ValidarWildcards(termo);

                        } while (wildcards);

                        Console.WriteLine("\nRESULTADOS");

                        bool encontrou = false;
                        foreach (KeyValuePair<string, string> palavra in dicionario)
                        {
                            if (palavra.Key.Contains(termo, StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"- {palavra.Key}: {palavra.Value}");
                                encontrou = true;
                            }
                        }

                        if (!encontrou)
                        {
                            Console.WriteLine("Nenhum termo encontrado");
                        }

                        Console.WriteLine("\nDigite qualquer tecla para continuar");
                        Console.ReadKey();
                        break;
                }

                Console.Clear();

            } while (opcao != 'S');

            //Limpa o Arquivo e Salva o Dicionario Atualizado
            File.WriteAllBytes("Dicionario.bin", new byte[0]);
            using (Stream stream = File.Open("Dicionario.bin", FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, dicionario);
            }

            Console.WriteLine("Obrigado por utilizar a Let's Speak\n");
            Console.WriteLine("(O arquivo do dicionario esta localizado em C: na sua maquina)");
        }

        public static bool ValidarWildcards(string termo)
        {
            string wildcards = "*?";
            char[] chars = termo.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (wildcards.Contains(chars[i]))
                {
                    Console.WriteLine("Os wildcards (* e ?) não são aceitos no dicionario\n");
                    return true;
                }
            }

            return false;
        }

    }
}