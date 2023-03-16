//using Hangfire;
//using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add Services
/*builder.Services.AddMvc(config=>{
    var policy=new AuthorizationPolicyBuilder().
    RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});*/ // To apply authorization globally

//Add HangFire Services
 /*builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));
          // Add the processing server as IHostedService
    builder.Services.AddHangfireServer();*/
builder.Services.AddMvc(options=>{
});

builder.Services.AddDbContext<ProducerContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"))
);
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ProducerContext>();
builder.Services.AddScoped<IGiftItemService,GiftItemService>();
builder.Services.AddScoped<IProducerRepository, ProducerRepository>();

//For External Login Provider
builder.Services.AddAuthentication().AddGoogle(options=>{
   options.ClientId="503113822475-uepbu0u2nm0pi0ok8gncq9mo610mokak.apps.googleusercontent.com";
   options.ClientSecret="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";
}).AddFacebook(options=>{
      options.AppId="560002395971519";
      options.AppSecret="a449697fd34108fab68d70f312ac0e84";
});


var app = builder.Build();

//Http request pipeline
app.UseStaticFiles();
    //app.UseHangfireDashboard();
  //Hangfire.BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
app.UseRouting();
/*app.UseMvc(routes=>{
    routes.MapRoute("default","{controller=Gift}/{action=GetGiftItems}/{id?}");
    routes.MapRoute("AddToCart","{controller}/{action}/{id}/{quantity}");
});*/

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints=>{
      endpoints.MapControllerRoute("default","{controller}/{action}/{id}/{quantity}",new{
    controller="Gift",
    action="GetGiftItems",
    id="",
    quantity=""
});
//endpoints.MapHangfireDashboard();
});
/*app.MapControllerRoute("default","{controller}/{action}/{id}/{quantity}",new{
    controller="Gift",
    action="GetGiftItems",
    id="",
    quantity=""
});*/

//app.MapGet("/", () => "Hello World!");

app.Run();
