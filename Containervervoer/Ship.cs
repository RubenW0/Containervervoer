    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Containervervoer
{
    public class Ship
    {

        public ContainerStack[,] Layout { get; set; }
        public int Y { get; set; } = 4;
        public int X { get; set; } = 3;
        public int MaxWeight { get; set; } = 300;

        private int currentMiddle = 1;
        private int totalWeightLeft = 0;
        private int totalWeightRight = 0;

        public Ship() 
        { 

        }

        public void CreateStacks() 
        {
            Layout = new ContainerStack[Y, X];
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    Layout[i, j] = new ContainerStack();
                }
            }
        }
        
        public void PrintLayout()
        {
            for (int yy = 0; yy < Y; yy++)
            {
                for (int xx = 0; xx < X; xx++)
                {
                    Console.Write(Layout[yy, xx].StackCount + "\t");
                }
                Console.WriteLine();
            }
        }


        public int DecideY(int y, Container container)
        {
            // kijken of het gekoeld of waardevol is
            if (container.ContainerType == Container.Type.Coolable || container.ContainerType == Container.Type.CoolableValuable)
            {
                return 0;
            }
            if (container.ContainerType == Container.Type.Valuable)
            {
                return y - 1;
            }
            if (currentMiddle >= y - 1)
            {
                currentMiddle = 1;
            }
            return currentMiddle++;



        }

        public int DecideX(int x, Container container)
        {

            // kijken of x even of oneven is (Wel of geen middenrij)

            // verschil gewicht dat je hebt x0,2 gewicht recht + container gewicht - gewicht links kleiner dan getal x0.2
            // als kleiner gewicht rechts, anders links
            // evenwicht

            {
                // Controleer of x even of oneven is
                bool isEven = x % 2 == 0;

                // Als x oneven is, betekent dit dat er een middelste rij is
                if (!isEven)
                {
                    // Controleer of de middelste rij vol is
                    if (Layout[Y / 2, x / 2].IsValidToAdd(container))
                    {
                        // Als de middelste rij niet vol is, voeg de container dan toe aan de middelste rij
                        return x / 2;
                    }
                    else
                    {
                        // Als de middelste rij vol is, voeg de container dan toe aan de lichtere kant (links of rechts)
                        return DecideSide(x, container);
                    }
                }
                // Als x even is, betekent dit dat er geen middelste rij is
                else
                {
                    // Voeg de container toe aan de lichtere kant (links of rechts)
                    return DecideSide(x, container);
                }
            }
        }

        public int DecideSide(int x, Container container)
        {
            // Bereken het totale gewicht aan elke kant
            totalWeightLeft = CalculateTotalWeight(0, x / 2);
            totalWeightRight = CalculateTotalWeight((x / 2) + 1, x);

            // Als het gewicht aan de linkerkant minder is, voeg de container dan aan de linkerkant toe
            if (totalWeightLeft <= totalWeightRight)
            {
                return 0;
            }
            // Anders, voeg de container aan de rechterkant toe
            else
            {
                return x - 1;
            }
        }

        public int CalculateTotalWeight(int startX, int endX)
        {
            int totalWeight = 0;

            for (int i = 0; i < Y; i++)
            {
                for (int j = startX; j < endX; j++)
                {
                    totalWeight += Layout[i, j].GetTotalWeight();
                }
            }

            return totalWeight;
        }


        public void AddContainer(Container container)
        {
            int Ypoint = DecideY(Y, container);
            int Xpoint = DecideX(X, container);

            Layout[Ypoint, Xpoint].Add(container);
        }


        //public bool IsBalanced()
        //{
        //    int totalWeightLeft = 0;
        //    int totalWeightRight = 0;

        //}
    }
}
