using Elevator.Services;
using Elevator.Services.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Elevator.Controllers
{
    /// <summary>
    /// Endpoints exposing different elevator operations
    /// </summary>
    public class ElevatorController : Controller
    {
        private readonly IElevatorManager elevatorManager;

        public ElevatorController(IElevatorManager elevatorManager)
        {
            this.elevatorManager = elevatorManager;
        }

        /// <summary>
        /// Call elevator for a specific floor.
        /// </summary>
        /// <param name="floor"></param>
        /// <returns></returns>
        public async Task<IActionResult> Call(int id)
        {
            if (id < GeneralConstants.MIN_VALID_FLOOR || id > GeneralConstants.MAX_VALID_FLOOR)
            {
                return NotFound();
            }

            await this.elevatorManager.Call(targetFloor: id);

            return Ok();
        }
    }
}
