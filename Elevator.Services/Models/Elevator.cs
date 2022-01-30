using Elevator.Services.Constants;
using System;

namespace Elevator.Services.Models
{
    public class Elevator
    {
        public string Id { get; private set; }

        public Status Status { get; internal set; }

        public int CurrentFloor { get; private set; }

        public Elevator()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Status = Status.Stopped;
            this.CurrentFloor = GeneralConstants.GROUND_FLOOR;
        }

        public void SetCurrentFloor(int currentFloor)
        {
            if (currentFloor < GeneralConstants.MIN_VALID_FLOOR ||
                currentFloor > GeneralConstants.MAX_VALID_FLOOR)
            {
                throw new ArgumentOutOfRangeException(nameof(currentFloor));
            }

            this.CurrentFloor = currentFloor;
        }

        public override string ToString()
            => $"Elevator {this.Id} is at {this.CurrentFloor} floor. Status is {this.Status}.";
    }
}
