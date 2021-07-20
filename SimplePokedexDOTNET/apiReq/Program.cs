using apiReq.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace apiReq
{

    class Program
    {
        static void Main(string[] args)
        {
            string userAnswer = printOptions();
            while (userAnswer.ToUpper() != "X")
            {
                switch (userAnswer)
                {
                    case "1":
                        Console.WriteLine("\nWhat is the name of the pokemon that you wanna add?\n");

                        string pokemonName = Console.ReadLine();
                        InsertPokemon(pokemonName);
                        break;
                    case "2":
                        DeletePokemon();
                        break;
                    case "3":
                        ListPokemons();
                        break;
                    case "4":
                        UpdatePokemon();
                        break;
                    case "5":
                        VisualizePokemon();
                        break;
                    default:
                        Console.WriteLine("Please, provide a value accordingly to the pre-established answers\n");
                        throw new ArgumentOutOfRangeException();
                }
                userAnswer = Program.printOptions().ToUpper();
            }
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Quitting.....");
            Environment.Exit(0);
        }







        static PokemonRepository repository = new PokemonRepository();
        private static string printOptions()
        {
            Console.WriteLine("\nHello, im your Pokedex...Please, tell me what to do.\n");
            Console.WriteLine("1-Add Pokemon to the list");
            Console.WriteLine("2-Remove Pokemon from the list");
            Console.WriteLine("3-List everything");
            Console.WriteLine("4- Update Pokemon");
            Console.WriteLine("5- Visualize Pokemon");
            Console.WriteLine("X-Quit\n");

            string userAnswer = Console.ReadLine();
            return userAnswer;
        }

        private static void ListPokemons()
        {
            Console.WriteLine("\nListing pokemons\n");
            var list = repository.List();

            if (list.Count == 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\nYou still don't have anything in the pokedex\n");
                Console.BackgroundColor = ConsoleColor.Black;

                return;
            }
            foreach(var pokemon in list)
            {
                var deleted = pokemon.retornaExcluid();
                if (!deleted)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("#ID {0}: - {1}", pokemon.retornaId(), pokemon.retornaNome());
                    Console.BackgroundColor = ConsoleColor.Black;
                }
             }
            Console.BackgroundColor = ConsoleColor.Black;
        }




        private static void InsertPokemon(string pokemonName)
        {
            (string Height, string Weight) heightandweight;
            heightandweight = ShowData(pokemonName);

            bool error = false;
            if (heightandweight == ("null", "null")) { error = true; }

            if (error == false)
            {


                foreach (int i in Enum.GetValues(typeof(Tipo)))
                {
                  
                    Console.WriteLine("{0}.{1}", i, Enum.GetName(typeof(Tipo), i));
                 
                }
                Console.WriteLine("\nWhats the type of the pokemon given the options above?\n");
                int entryType = int.Parse(Console.ReadLine());

                string entryname = pokemonName;

                Pokemon newPokemon = new Pokemon(id: repository.NextId(),
                                      tipo: (Tipo)entryType,
                                      name: entryname,
                                      height: heightandweight.Height,
                                      weight: heightandweight.Weight);

                repository.Insert(newPokemon);
            }
            else
            {
               Console.BackgroundColor = ConsoleColor.Red;
               Console.WriteLine("\n Please, check the proper spelling of the pokemon that you are trying to add\n");
               Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private static void UpdatePokemon()
        {
            Console.WriteLine("Whats the id of the pokemon that you wanna update?");
            int indexPokemon = int.Parse(Console.ReadLine());


            foreach(int i in Enum.GetValues(typeof(Tipo))){
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("{0}.{1}", i, Enum.GetName(typeof(Tipo), i));
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine("Whats the type of the pokemon given the options above?");
            int entryType = int.Parse(Console.ReadLine());

            Console.WriteLine("What is the name of the pokemon?");
            string entryName = Console.ReadLine();
            bool error = false;
            
           (string Height, string Weight) heightandweight = ShowData(entryName);
            if (heightandweight == ("null", "null"))
            {
                error = true;
            }
           

            if (error == false)
            {
                Pokemon updatePokemon = new Pokemon(id: indexPokemon,
                                         tipo: (Tipo)entryType,
                                         name: entryName,
                                         height:heightandweight.Height,
                                         weight:heightandweight.Weight);

                repository.Update(indexPokemon, updatePokemon);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\n Please, check the proper spelling of the pokemon that you are trying to add\n");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }


        private static void DeletePokemon()
        {
            Console.WriteLine("Give the id of the pokemon that you want to erase");
            int index = int.Parse(Console.ReadLine());


            Console.WriteLine($"\nDeseja mesmo excluir o pokemon com o id {index} ?\n");
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("\n Yes(Y)\n No(N)");
            Console.BackgroundColor = ConsoleColor.Black;
            string confirmation = Console.ReadLine();
            if (confirmation.ToLower() == "y" || confirmation.ToLower()=="yes")
            {
                repository.Exclude(index);
            }
            else
            {
                return;
            }

        }

        private static void VisualizePokemon()
        {
            Console.WriteLine("Write down the id of the pokemon");
            int indexPokemon = int.Parse(Console.ReadLine());

            var pokemon = repository.ReturnById(indexPokemon);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(pokemon);
            Console.BackgroundColor = ConsoleColor.Black;
        }


        static (string Height, string Weight) ShowData(string name)
        {
            try
            {
                name.ToLower();
                var json = GetJSON(name);
                var data = JsonConvert.DeserializeObject<Pokemon>(json);
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"\n{name}:   It's Height is {data.Height} and it's Weight is {data.Weight}  ");
                Console.BackgroundColor = ConsoleColor.Black;
                string height = data.Height;
                string weight = data.Weight;
                (string Height, string Weight) heightandweight = (height, weight);
                return heightandweight;
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\nWe could not found the pokemon that you tried to add\n" + e);
                Console.BackgroundColor = ConsoleColor.Black;

            }
            return ("null", "null");



        }
        static string GetJSON(string name)
        {

            var request = WebRequest.Create($"https://pokeapi.co/api/v2/pokemon/{name}");
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var json = reader.ReadToEnd();
                    return json;
                }
            }
            else
            {
                string error = "Nothing was found on our database with that name";
                return error;
            }

        }























        
    }
  
}
