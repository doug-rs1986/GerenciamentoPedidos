var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona suporte � leitura do appsettings.json e inje��o de IConfiguration
// (J� � feito por padr�o, mas vamos garantir a inje��o dos reposit�rios)
builder.Services.AddScoped<GerenciamentoPedidos.Data.IClienteRepository, GerenciamentoPedidos.Data.ClienteRepository>();
builder.Services.AddScoped<GerenciamentoPedidos.Data.IPedidoRepository, GerenciamentoPedidos.Data.PedidoRepository>();
// Inje��o do reposit�rio de produtos
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
    