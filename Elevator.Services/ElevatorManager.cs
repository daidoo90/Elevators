using Elevator.Services.Extensions.cs;
using Elevator.Services.StateMachine;
using System.Linq;
using System.Threading.Tasks;

namespace Elevator.Services
{
    public class ElevatorManager : IElevatorManager
    {
        private readonly IElevatorsStateMachine _elevatorStateMachine;

        public ElevatorManager(IElevatorsStateMachine elevatorsStateMachine)
        {
            this._elevatorStateMachine = elevatorsStateMachine;
        }

        /// <summary>
        /// Call elevator to a specific floor.
        /// </summary>
        /// <param name="targetFloor">Floor number, where elevator must come.</param>
        /// <returns>Task</returns>
        public async Task Call(int targetFloor)
        {
            bool doesRequestExists = this._elevatorStateMachine.Requests.TryGetValue(targetFloor, out var beingProcessed);

            if (doesRequestExists)
            {
                // Request for that floor already exists => no need to store a new request.
                return;
            }

            // New call for elevator => checking for available elevators
            // TODO: More optimized strategy could exists for choosing free elevator.
            // Example: Calculate closest elevator in case more than 1 elevator is available.
            var freeElevator = this._elevatorStateMachine.Elevators
                                                         .FirstOrDefault(e => e.Status == Models.Status.Stopped);

            // TODO: Most probably additional synchronization between threads is needed because, here we could have race condition. Locking could be used?!?
            bool willBeProcessedNow = freeElevator != null;

            // Store information about new floor request
            this._elevatorStateMachine.Requests.TryAdd(targetFloor, willBeProcessedNow);

            // Start processing the request by one of the elevators in a new thread
            if (freeElevator != null)
            {
                await Task.Run(() => freeElevator.Run(targetFloor, _elevatorStateMachine));
            }
        }
    }
}
