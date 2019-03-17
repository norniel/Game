#region Using Statements

#endregion

using Serilog;

namespace MonoBrJozik
{
	public static class Program
	{
		static void Main (string [] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.CreateLogger();
			
			
			using (var game = new Game1 ()) {
				game.Run ();
			}
		}
	}
}
