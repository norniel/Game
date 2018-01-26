#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using MonoBrJozik;
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
