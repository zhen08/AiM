using AiM.Data;
using AiM.Services;
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

		builder.Services.AddHttpClient();

        builder.Services.AddSingleton<AiMDatabase>();
		builder.Services.AddSingleton<Settings>();

		builder.Services.AddSingleton<ChatService>();

		builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<ChatPage>();
        builder.Services.AddSingleton<SettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
        return builder.Build();
	}
}

