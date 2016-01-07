using System;

namespace Engine.Interfaces
{
    interface IRemovableObject
    {
        Action RemoveFromContainer { get; set; }
    }
}
