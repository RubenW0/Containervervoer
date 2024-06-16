using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Containervervoer
{
    public class ContainerStack
    {
        private List<Container> containers;

        public int StackWeight { get; set; }
        public int StackCount { get; set; }
        private int totalWeight = 0;

        public ContainerStack() { }
        public ContainerStack(int stackWeight, int stackCount)
        {
            containers = new List<Container>();
            StackWeight = stackWeight;
            StackCount = stackCount;
        }

        public void Add(Container container)
        {
            if (IsValidToAdd(container))
            {
                StackCount++;
                containers.Add(container);
            }
            else
            {
                throw new Exception("Kan de container niet toevoegen: de stack zou ongeldig worden.");
            }
        }
        public bool IsValidToAdd(Container container)
        {
            StackWeight = GetTotalContainerWeight() + container.ContainerWeight;
             
            if (StackWeight > 120)
            {
                return false;
            }
            if (containers.Count > 0 && containers[0].ContainerType == Container.Type.Valuable)
            {
                return false;
            }
            else if (containers.Count > 0 && containers[0].ContainerType == Container.Type.CoolableValuable)
            {
                return false;
            }

            return true;
        }

        public bool IsFull(Container container)
        {

            // Controleer of de hypothetische container kan worden toegevoegd
            return !IsValidToAdd(container);
        }

        public int GetTotalContainerWeight()
        {
            int totalContainerWeight = 0;

            foreach (Container container in containers)
            {
                totalContainerWeight += container.ContainerWeight;

                //Console.WriteLine(totalContainerWeight);
            }
            return totalContainerWeight;
        }

        public int GetTotalWeight()
        {
            foreach (Container container in containers)
            {
                totalWeight += container.ContainerWeight;

                //Console.WriteLine(totalWeight);
            }
            return totalWeight;
        }



    }
}
