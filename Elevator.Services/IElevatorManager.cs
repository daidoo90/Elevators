using System.Threading.Tasks;

namespace Elevator.Services
{
    public interface IElevatorManager
    {
        /// <summary>
        /// Call elevator to a specific floor.
        /// </summary>
        /// <param name="targetFloor">Floor number, where elevator must come.</param>
        /// <returns>Task</returns>
        Task Call(int targetFloor);
    }
}
