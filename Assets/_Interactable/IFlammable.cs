using Randolph.Interactable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Interactable
{
    interface IFlammable
    {
        InventoryItem GetBurningVersion();
        void Ignite();
    }
}
