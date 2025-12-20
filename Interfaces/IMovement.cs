using PlantsVsZombies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies
{
    public interface IMovement
    {
        void Move(GameObject obj, GameTime gameTime);
    }
}
