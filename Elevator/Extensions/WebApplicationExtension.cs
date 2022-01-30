using Elevator.Services.StateMachine;

namespace Elevator.Extensions
{
    internal static class WebApplicationExtension
    {
        /// <summary>
        /// Add new elevator instance. Instance will be live through the whole application lifetime.
        /// </summary>
        /// <param name="application">WebApplication</param>
        /// <returns>WebApplication</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static WebApplication AddElevator(this WebApplication application)
        {
            var elevatorsStateMachine = application.Services.GetService<IElevatorsStateMachine>();
            if (elevatorsStateMachine == null)
            {
                throw new ArgumentNullException(nameof(elevatorsStateMachine));
            }

            elevatorsStateMachine.CreateElevator();

            return application;
        }
    }
}
