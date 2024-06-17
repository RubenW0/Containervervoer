using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

        public int ContainerFAIL { get; set; }

        private int currentMiddle = 1;
        private int totalWeightLeft = 0;
        private int totalWeightRight = 0;

        private int RowsLeft = 1;
        private int RowsRight = 1;
        private int SideWidth;

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

        public void AddContainer(Container container)
        {
            bool Failed = false;
            ContainerFAIL = 0;
            RowsLeft = 0;
            RowsRight = 0;

        Retry:
            int Ypoint = DecideY(container);
            int Xpoint = DecideX(container, Failed);

            bool gelukt = Layout[Ypoint, Xpoint].Add(container);
            if (!gelukt)
            {
                Failed = true;
                ContainerFAIL++;
                goto Retry;
            }
        }

        public int DecideY(Container container)
        {
            // kijken of het gekoeld of waardevol is
            if (container.ContainerType == Container.Type.Coolable || container.ContainerType == Container.Type.CoolableValuable)
            {
                return 0;
            }
            if (container.ContainerType == Container.Type.Valuable)
            {
                return Y - 1;
            }
            if (currentMiddle >= Y - 1)
            {
                currentMiddle = 1;
            }
            return currentMiddle++;

        }

        public int DecideX(Container container, bool Failed)
        {
            {
                bool isEven = X % 2 == 0;

                if (!isEven)
                {
                    if (Failed == false)
                    {
                        if (Layout[Y / 2, X / 2].IsValidToAdd(container))
                        {
                            return X / 2;
                        }
                    }

                    SideWidth = (X - 1) / 2;

                    int Side = DecideSide(X, container);

                    if (Side == 1)
                    {
                        if (ContainerFAIL >= Y)
                        {
                            RowsLeft++;
                        }
                        return SideWidth - RowsLeft; // links
                    }

                    if (ContainerFAIL >= Y)
                    {
                        RowsRight++;
                    }
                    return SideWidth + RowsRight; // rechts
                }

                else
                {
                    SideWidth = X / 2;
                    int Side = DecideSide(X, container);

                    if (Side == 1)
                    {
                        if (ContainerFAIL >= Y)
                        {
                            RowsLeft++;
                        }
                        return SideWidth - RowsLeft; // links
                    }

                    if (ContainerFAIL >= Y)
                    {
                        RowsRight++;
                    }
                    return SideWidth + (RowsRight - 1); // rechts
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
                return 1;
            }
            // Anders, voeg de container aan de rechterkant toe
            else
            {
                return 0;
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

        public bool IsAtLeastHalfWeight()
        {
            int totalWeight = CalculateTotalWeight(0, X);

            return totalWeight >= (0.5 * MaxWeight);
        }


    }
}
