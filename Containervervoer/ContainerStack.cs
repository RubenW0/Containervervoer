using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Containervervoer
{
    public class ContainerStack
    {
        public List<Container> containers;

        public int StackWeight { get; set; }
        public int StackCount { get; set; }

        private int totalWeight = 0;

        public ContainerStack()
        {
            containers = new List<Container>();
        }
        public ContainerStack(int stackWeight, int stackCount)
        {
            containers = new List<Container>();
            StackWeight = stackWeight;
            StackCount = stackCount;
        }

        public bool Add(Container container)
        {
            if (IsValidToAdd(container))
            {
                StackCount++;
                // Als de container waardevol is, voeg deze dan toe aan het einde van de lijst
                if (container.ContainerType == Container.Type.Valuable || container.ContainerType == Container.Type.CoolableValuable)
                {
                    containers.Add(container);
                }
                else
                {
                    // Voeg niet-waardevolle containers toe aan het begin van de lijst
                    containers.Insert(0, container);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsValidToAdd(Container container)
        {
            int totalWeight = GetTotalContainerWeight() + container.ContainerWeight;

            if (containers.Count > 0)
            {
                totalWeight -= containers[0].ContainerWeight;
            }

            if (totalWeight > 120)
            {
                return false;
            }

            if (containers.Count > 0)
            {
                Container.Type type = containers[0].ContainerType;
                if (type == Container.Type.Valuable || type == Container.Type.CoolableValuable)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetTotalContainerWeight()
        {
            int totalContainerWeight = 0;

            foreach (Container container in containers)
            {
                totalContainerWeight += container.ContainerWeight;

            }
            return totalContainerWeight;
        }

        public int GetTotalWeight()
        {
            foreach (Container container in containers)
            {
                totalWeight += container.ContainerWeight;

            }
            return totalWeight;
        }



    }
}