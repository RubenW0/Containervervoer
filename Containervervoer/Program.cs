using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Containervervoer;

namespace Containervervoer
{
    internal class Program
    {
        static void Main()
        {
            {
                List<Container> containers = new List<Container>();
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.Coolable, 30));
                containers.Add(new Container(Container.Type.Valuable, 30));
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.CoolableValuable, 30));
                containers.Add(new Container(Container.Type.Coolable, 30));
                containers.Add(new Container(Container.Type.Normal, 30));

                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.Coolable, 30));
                containers.Add(new Container(Container.Type.Valuable, 30));
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.Normal, 30));
                containers.Add(new Container(Container.Type.CoolableValuable, 30));
                containers.Add(new Container(Container.Type.Coolable, 30));
                containers.Add(new Container(Container.Type.Normal, 30));



                Ship ship = new Ship();

                ship.CreateStacks();

                foreach (Container container in containers)
                {
                    ship.AddContainer(container);
                }

                ship.PrintLayout();


            }
        }
    }
}