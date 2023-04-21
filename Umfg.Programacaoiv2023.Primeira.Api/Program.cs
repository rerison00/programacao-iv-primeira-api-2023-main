using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Umfg.Programacaoiv2023.Primeira.Api
{
    public class Program
    {
        private static List<Cliente> _lista = new List<Cliente>()
        {
            new Cliente("Teste Um"),
            new Cliente("Teste Dois"),
        };

        private static List<Produto> _listaproduto = new List<Produto>()
        {
            new Produto("Teste Produto 1", "12345", 14),
            new Produto("Teste Produto 2", "56789", 11),
        };

        public static void Main(string[] args)
        {
            var app = WebApplication.Create(args);

            app.MapGet("api/v1/Cliente", ObterTodosClientes);
            app.MapGet("api/v1/Cliente/{id}", ObterCliente);
            app.MapPost("api/v1/Cliente", CadastrarClienteAsync);
            app.MapPut("api/v1/Cliente/{id}", AtualizarClienteAsync);
            app.MapDelete("api/v1/Cliente", RemoverTodosClientesAsync);
            app.MapDelete("api/v1/Cliente/{id}", RemoverClientesAsync);
            app.MapGet("api/v1/Produto", ObterTodosProdutos);
            app.MapGet("api/v1/Produto/{id}", ObterProduto);
            app.MapPost("api/v1/Produto", CadastrarProdutoAsync);
            app.MapPut("api/v1/Produto/{id}", AtualizarProdutoAsync);
            app.MapDelete("api/v1/Produto", RemoverTodosProdutosAsync);
            app.MapDelete("api/v1/Produto/{id}", RemoverProdutosAsync);

            app.Run();
        }

        public static async Task ObterTodosProdutos(HttpContext context)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(_listaproduto);
        }

        public static async Task ObterProduto(HttpContext context)
        {
            if (!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode = 400;
                return;
            }

            var produto = _listaproduto.FirstOrDefault(x => x.Id == id);

            if (produto == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(produto);
        }

        public static async Task CadastrarProdutoAsync(HttpContext context)
        {
            var produto = await context.Request.ReadFromJsonAsync<Produto>();

            if (produto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Nao foi possivel cadastrar o produto! Verifique.");
                return;
            }

            _listaproduto.Add(produto);
            context.Response.StatusCode = (int)HttpStatusCode.Created;
            await context.Response.WriteAsJsonAsync(produto);

        }

        public static async Task AtualizarProdutoAsync(HttpContext context)
        {
            if (!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Parametro id nao foi enviado! Verifique");
            }

            var produto = _listaproduto.FirstOrDefault(x => x.Id == id);

            if (produto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Produto nao encontrado para o id: {id}. Verifique");

                return;
            }

            _listaproduto.Remove(produto);
            var produtoRecebido = await context.Request.ReadFromJsonAsync<Produto>();
            produto.Descricao = produtoRecebido.Descricao;
            produto.CodigoBarra = produtoRecebido.CodigoBarra;
            produto.Valor = produtoRecebido.Valor;

            _listaproduto.Add(produto);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(produto);
        }

        public static async Task RemoverTodosProdutosAsync(HttpContext context)
        {
            _listaproduto.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync("Todos os produtos foram removidos com sucesso!");
        }

        public static async Task RemoverProdutosAsync(HttpContext context)
        {
            if (!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Parametro id nao foi enviado! Verifique");

                return;
            }

            var produto = _listaproduto.FirstOrDefault(x => x.Id == id);
            if (produto == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Produto nao encontrado para o id{id}. Verifique");
                return;
            }

            _listaproduto.Remove(produto);
            await context.Response.WriteAsync("produto removido com sucesso");
        }

        public static async Task ObterTodosClientes(HttpContext context)
        {
            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(_lista);
        }

        public static async Task ObterCliente(HttpContext context)
        {
            if (!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode = 400;
                return;
            }

            var cliente = _lista.FirstOrDefault(x => x.Id == id);

            if (cliente == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            context.Response.StatusCode = 200;
            await context.Response.WriteAsJsonAsync(cliente);
        }

        public static async Task CadastrarClienteAsync(HttpContext context)
        {
            var cliente = await context.Request.ReadFromJsonAsync<Cliente>();

            if(cliente == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Nao foi possivel cadastrar o cliente! Verifique.");
                return;
            }

            _lista.Add(cliente);
            context.Response.StatusCode = (int)HttpStatusCode.Created;
            await context.Response.WriteAsJsonAsync(cliente);
            
        }

        public static async Task AtualizarClienteAsync(HttpContext context)
        {
            if(!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode= (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Parametro id nao foi enviado! Verifique");
            }

            var cliente = _lista.FirstOrDefault(x => x.Id == id);

            if(cliente == null)
            {
                context.Response.StatusCode=(int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Cliente nao encontrado para o id: {id}. Verifique");

                return;
            }

            _lista.Remove(cliente);
            cliente.Nome = (await context.Request.ReadFromJsonAsync<Cliente>()).Nome;

            _lista.Add(cliente);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsJsonAsync(cliente);
        }

        public static async Task RemoverTodosClientesAsync(HttpContext context)
        {
            _lista.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync("Todos os clientes foram removidos com sucesso!");
        }

        public static async Task RemoverClientesAsync(HttpContext context)
        {
            if(!context.Request.RouteValues.TryGet("id", out Guid id))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Parametro id nao foi enviado! Verifique");

                return;
            }

            var cliente = _lista.FirstOrDefault(x => x.Id == id);
            if(cliente == null)
            {
                context.Response.StatusCode= (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync($"Cliente nao encontrado para o id{id}. Verifique");
                return;
            }

            _lista.Remove(cliente);
            await context.Response.WriteAsync("Cliente removido com sucesso");
        }
    }
}