using Timer = System.Windows.Forms.Timer;

namespace APMCalculator
{
	public class Application
	{
		public static void Main(string[] args)
		{
			APMCalculator calc = new APMCalculator();
			calc.OnTimerTick((_, _) =>
			{
				Console.WriteLine($"Current APM is: {calc.GetApm()} (Keypresses: {calc.totalKeypresses} over {calc.GetMsElapsedSinceStart() / 1000}s)");
			});

			KeyboardHandler.Listen();
			KeyboardHandler.OnKeyPress(calc.Handler);

			calc.StartTimer();
	
			System.Windows.Forms.Application.Run();
		}
	}
}
