using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Containervervoer
{
    public class Container
    {
        public enum Type
        {
            Normal,
            Valuable,
            Coolable,
            CoolableValuable
        }

        public Type ContainerType { get; set; }
        public int ContainerWeight { get; set; }

        public Container(Type containerType, int containerWeight)
        {
            ContainerType = containerType;
            ContainerWeight = containerWeight;
        }





    }
}