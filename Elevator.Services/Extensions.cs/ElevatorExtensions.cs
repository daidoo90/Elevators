using Elevator.Services.Constants;
using Elevator.Services.Models;
using Elevator.Services.StateMachine;
using System;
using System.Linq;
using System.Threading;

namespace Elevator.Services.Extensions.cs
{
    /// <summary>
    /// Additional methods that update elevator's state.
    /// </summary>
    internal static class ElevatorExtensions
    {
        static bool isBookRequestingInitiated = false;
        static Object bookNewRequestLock = new Object();

        internal static void Run(this Models.Elevator elevator, int? targetFloor, IElevatorsStateMachine elevatorsStateMachine)
        {
            elevator.SetMoving();

            while (true)
            {
                if (targetFloor.HasValue &&
                    targetFloor.Value != elevator.CurrentFloor)
                {
                    elevator.Move(targetFloor.Value);

                    if (targetFloor.Value == elevator.CurrentFloor)
                    {
                        // Destination reached
                        elevatorsStateMachine.Requests.TryRemove(elevator.CurrentFloor, out bool floorRequestRemoved);

                        var floorRequest = TryBookNewFloorRequest(elevatorsStateMachine);
                        if (floorRequest.HasValue)
                        {
                            elevator.Run(floorRequest, elevatorsStateMachine);
                        }
                        else
                        {
                            // No more floor requests
                            elevator.SetStopped();
                            return;
                        }
                    }
                    else
                    {
                        // We still have more floors to go through before reaching target floor
                        // Let's see if we need to stop on this one as well
                        var currentFloorRequested = elevatorsStateMachine.Requests.TryGetValue(elevator.CurrentFloor, out bool isProcessed);
                        if (currentFloorRequested &&
                           !isProcessed)
                        {
                            elevatorsStateMachine.Requests.TryRemove(elevator.CurrentFloor, out bool floorRequestRemoved);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Move elevator up or down.
        /// </summary>
        /// <param name="elevator">Elevator instance.</param>
        /// <param name="targetFloor">Floor where elevator must go.</param>
        private static void Move(this Models.Elevator elevator, int targetFloor)
        {
            if (targetFloor > elevator.CurrentFloor)
            {
                elevator.MoveUp();
            }
            else
            {
                elevator.MoveDown();
            }
        }

        /// <summary>
        /// Move elevator 1 floor up.
        /// </summary>
        /// <param name="elevator">Elevator instance</param>
        private static void MoveUp(this Models.Elevator elevator)
        {
            // Simulating move between floors.
            Thread.Sleep(2000);

            int newFloor = elevator.CurrentFloor + 1;
            elevator.SetCurrentFloor(newFloor);
        }

        /// <summary>
        /// Move elevator 1 floor down.
        /// </summary>
        /// <param name="elevator">Elevator instance</param>
        private static void MoveDown(this Models.Elevator elevator)
        {
            // Simulating move between floors.
            Thread.Sleep(2000);

            int newFloor = elevator.CurrentFloor - 1;
            elevator.SetCurrentFloor(newFloor);
        }

        /// <summary>
        /// Check if someone is waiting for elevator.
        /// </summary>
        /// <param name="elevatorsStateMachine"></param>
        /// <returns></returns>
        private static int? TryBookNewFloorRequest(IElevatorsStateMachine elevatorsStateMachine)
        {
            if (!isBookRequestingInitiated)
            {
                lock (bookNewRequestLock)
                {
                    if (!isBookRequestingInitiated)
                    {
                        var nextRequests = elevatorsStateMachine.Requests.Where(req => req.Value == false);
                        if (!nextRequests.Any())
                            return null;

                        var floorReqest = nextRequests.First().Key;

                        // Mark floor request as being processed
                        elevatorsStateMachine.Requests.TryUpdate(floorReqest, true, false);

                        isBookRequestingInitiated = true;

                        return floorReqest;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Mark elevator as moving.
        /// </summary>
        /// <param name="elevator">Elevator instance.</param>
        private static void SetMoving(this Models.Elevator elevator)
        {
            elevator.Status = Status.Moving;
        }

        /// <summary>
        /// Mark elevator as stopped.
        /// </summary>
        /// <param name="elevator">Elevator instance.</param>
        private static void SetStopped(this Models.Elevator elevator)
        {
            elevator.Status = Status.Stopped;
        }
    }
}
