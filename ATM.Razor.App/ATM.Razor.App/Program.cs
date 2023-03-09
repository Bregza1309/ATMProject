using AtmAspRazorApp;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuiler =>
    {
        webBuiler.UseStartup<Startup>();
    }).Build().Run(); 