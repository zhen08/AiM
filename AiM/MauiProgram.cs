using AiM.Data;
using AiM.Views;
using Microsoft.Extensions.Logging;

namespace AiM;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<HomePage>();
        builder.Services.AddTransient<ChatPage>();
        builder.Services.AddTransient<SettingsPage>();

		builder.Services.AddSingleton<AiMDatabase>();
#if DEBUG
		builder.Logging.AddDebug();
#endif
        return builder.Build();
	}
}

