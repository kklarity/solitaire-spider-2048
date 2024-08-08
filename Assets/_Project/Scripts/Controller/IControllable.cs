using UnityEngine;

namespace _Project.Scripts.Controller
{
    public interface IControllable
    {
        public void Move(Vector3 direction);
    }
}