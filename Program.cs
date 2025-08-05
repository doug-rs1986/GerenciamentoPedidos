var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona suporte à leitura do appsettings.json e injeção de IConfiguration
// (Já é feito por padrão, mas vamos garantir a injeção dos repositórios)
builder.Services.AddScoped<GerenciamentoPedidos.Data.IClienteRepository, GerenciamentoPedidos.Data.ClienteRepository>();
builder.Services.AddScoped<GerenciamentoPedidos.Data.IPedidoRepository, GerenciamentoPedidos.Data.PedidoRepository>();
// Injeção do repositório de produtos
builder.Services.AddScoped<GerenciamentoPedidos.Data.IProdutoRepository, GerenciamentoPedidos.Data.ProdutoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
    