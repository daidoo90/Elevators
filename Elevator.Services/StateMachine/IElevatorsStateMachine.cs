using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Elevator.Services.StateMachine
{
    /// <summary>
    /// Interface that gives information about state of elevators and active calls for elevator.
    /// </summary>
    public interface IElevatorsStateMachine
    {
        /// <summary>
        /// Create new elevator.
        /// </summary>
        void CreateElevator();

        /// <summary>
        /// Holds information about the created elevators. Instances are live throught the whole application
        /// lifetime. Only their state is being changed.
        /// </summary>
        IList<Models.Elevator> Elevators { get; }

        /// <summary>
        /// Holds information about request calls for elevator.
        /// Key: integer - Floor number.
        /// Value: true - Request call is being processed now.
        /// Value: false - Request call is not processed yet. It will be processed soon.
        /// </summary>
        ConcurrentDictionary<int, bool> Requests { get; }
    }
}
