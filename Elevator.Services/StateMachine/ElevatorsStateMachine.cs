using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Elevator.Services.StateMachine
{
    /// <summary>
    /// Singleton instance that will hold information about state of elevators and active calls for elevator.
    /// </summary>
    public class ElevatorsStateMachine : IElevatorsStateMachine
    {
        /// <summary>
        /// Holds information about the created elevators. Instances are live throught the whole application
        /// lifetime. Only their state is being changed.
        /// </summary>
        public IList<Models.Elevator> Elevators { get; private set; }

        /// <summary>
        /// Holds information about request calls for elevator.
        /// Key: integer - Floor number.
        /// Value: true - Request call is being processed now.
        /// Value: false - Request call is not processed yet. It will be processed soon.
        /// </summary>
        public ConcurrentDictionary<int, bool> Requests { get; private set; }

        public ElevatorsStateMachine()
        {
            this.Elevators = new List<Models.Elevator>();
            this.Requests = new ConcurrentDictionary<int, bool>();
        }

        /// <summary>
        /// Create new elevator.
        /// </summary>
        public void CreateElevator()
        {
            this.Elevators.Add(new Models.Elevator());
        }
    }
}
