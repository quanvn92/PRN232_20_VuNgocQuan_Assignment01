using _20_VuNgocQuan_Assignment01.DAOs;
using _20_VuNgocQuan_Assignment01.Models;
using _20_VuNgocQuan_Assignment01.Repositories;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// ── Database (Singleton DbContext as required by assignment) ──────────────
builder.Services.AddDbContext<FUNewsManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Singleton);

// ── Repositories (Singleton to match DbContext lifetime) ─────────────────
builder.Services.AddSingleton<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddSingleton<ITagRepository, TagRepository>();

// ── OData EDM model ───────────────────────────────────────────────────────
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<SystemAccount>("SystemAccounts");
odataBuilder.EntitySet<Category>("Categories");
odataBuilder.EntitySet<NewsArticle>("NewsArticles");
odataBuilder.EntitySet<Tag>("Tags");

// ── Controllers + OData ───────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddOData(opt => opt
        .AddRouteComponents("odata", odataBuilder.GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(100));

// ── Swagger ───────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FUNewsManagement API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// ── CORS ──────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
