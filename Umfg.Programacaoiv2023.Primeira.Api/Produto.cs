using System.Text.Json.Serialization;
using System;

namespace Umfg.Programacaoiv2023.Primeira.Api
{
    public sealed class Produto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("codigobarra")]
        public string CodigoBarra { get; set; }

        [JsonPropertyName("valor")]
        public double Valor { get; set; }

        private Produto() { }

        public Produto(string descricao, string codigobarra, double valor)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            CodigoBarra = codigobarra;
            Valor = valor;
        }
    }
}
