#region Using Statements

#endregion

namespace MonoBrJozik
{
	public static class Program
	{
		static void Main (string [] args)
		{
			using (var game = new Game1 ()) {
				game.Run ();
			}
		}
	}
}
