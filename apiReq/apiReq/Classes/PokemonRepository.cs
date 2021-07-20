using System;
using System.Collections.Generic;
using projectDIO.Interfaces;


namespace apiReq
{
    public class PokemonRepository : IRepository<Pokemon>


    {

        private List<Pokemon> listPokemon =  new List<Pokemon>();
        public void Exclude(int id)
        {
            listPokemon[id].Delete();
        }

        public void Insert(Pokemon entity)
        {
            listPokemon.Add(entity);
        }

        public List<Pokemon> List()
        {
            return listPokemon;
        }

        public int NextId()
        {
            return listPokemon.Count;
        }

        public Pokemon ReturnById(int id)
        {
            return listPokemon[id];
        }

        public void Update(int id, Pokemon entity)
        {
            listPokemon[id] = entity;
        }
    }


}