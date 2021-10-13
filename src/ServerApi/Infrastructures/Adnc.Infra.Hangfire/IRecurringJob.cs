using Hangfire.Server;

namespace Hangfire
{
    /// <summary>
    /// Provides a unified interface to build hangfire <see cref="RecurringJob"/>, similar to quartz.net.
    /// </summary>
    public interface IRecurringJob
	{
		/// <summary>
		/// Execute the <see cref="RecurringJob"/>.
		/// </summary>
		/// <param name="context">The context to <see cref="PerformContext"/>.</param>
		void Execute(PerformContext context);
	}
}
