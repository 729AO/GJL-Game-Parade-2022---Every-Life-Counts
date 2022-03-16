using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectInterfaces
{
    interface IConsumable
    {
        public Vector2 starting_position { get; set; }
        public Vector2 starting_velocity { get; set; }
        public GameObject consumableObject { get; set; }
    }
}
