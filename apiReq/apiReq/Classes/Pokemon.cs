using apiReq.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace apiReq
{
    public class Pokemon:AbstractClass
    {
    
        

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("weight")]

        public string Weight{ get; set; }

        private bool Exclude { get; set; }

        private Tipo Tipo { get; set; }

        private string Name { get; set; }

        public Pokemon(Tipo tipo,int id,string name,string weight,string height)
        {
            this.Id = id;
            this.Tipo = tipo;
            this.Exclude = false;
            this.Name = name;
            this.Height = height;
            this.Weight = weight;
        }

        public bool Delete()
        {
            return this.Exclude = true;
        }

        public int retornaId()
        {
            return this.Id;
        }
        public string retornaNome()
        {
            return this.Name;
        }
        public bool retornaExcluid()
        {
            return this.Exclude;
        }
        public override string ToString()
        {
            string retorno = "";
            retorno += "Wheight:" +this.Weight + Environment.NewLine;
            retorno += "Height: " + this.Height + Environment.NewLine;
            retorno += "Type of: " + this.Tipo + Environment.NewLine;
            retorno += "Name: " + this.Name + Environment.NewLine;
            retorno += "Deleted:" + this.Exclude;

            return retorno;
        }
    }


}
