using System;

namespace Andreas.Scripts
{
    public interface IInteractor
    {
        public event Action OnInteract;
    }
}