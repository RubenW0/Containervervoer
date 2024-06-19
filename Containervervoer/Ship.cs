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
        public int X { get; set; } = 4;
        public int MaxWeight { get; set; } = 300;

        public int ContainerFAIL { get; set; }

        private int currentMiddle = 1;
        private int totalWeightLeft = 0;
        private int totalWeightRight = 0;

        private int RowsLeft = 1;
        private int RowsRight = 0;
        private int SideWidth;

        private int Result;

        private int lastAddedSide = 0; // 0 voor rechts, 1 voor links

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

        public void PrintLayoutandCheckWeight()
        {
            if (!IsAtLeastHalfWeight())
            {
                throw new Exception("Minder dan 50% van het maximumgewicht is benut.");
            }


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




        Retry:
            int Ypoint = DecideY(container);
            int Xpoint = DecideX(container, Failed);


            bool succeed = Layout[Ypoint, Xpoint].Add(container);
            if (!succeed)
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

                if (!isEven) // oneven
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

                    if (Side == 1) // links toevoegen
                    {
                        if (ContainerFAIL >= Y)
                        {
                            RowsLeft++;
                        }
                        return SideWidth - RowsLeft; // links
                    }

                    if (ContainerFAIL >= Y) // rechts toevoegen
                    {
                        RowsRight++;
                    }
                    return SideWidth + RowsRight; // rechts
                }

                else // even
                {
                    SideWidth = X / 2;
                    int Side = DecideSide(X, container);
                    int attempts = 0;

                    while (true)
                    {
                        if (attempts++ > X * Y) 
                        {
                            throw new Exception("Het was niet mogelijk om de container toe te voegen.");
                        }

                        if (Side == 1) //links toevoegen
                        {
                            if (ContainerFAIL >= Y)
                            {
                                RowsLeft = (X / 2) - RowsLeft; 
                                RowsLeft++;
                            }
                            int Result = SideWidth - RowsLeft;
                            if (Result < 0 || Result >= X)
                            {
                                Side = 0; // Probeer aan de rechterkant toe te voegen
                                continue;
                            }
                            else
                            {
                                return Result; // links
                            }
                        }

                        if (Side == 0) // rechts toevoegen
                        {
                            if (ContainerFAIL >= Y)
                            {
                                RowsRight = (X / 2) + RowsRight; 
                                RowsRight++;
                            }
                            int Result = SideWidth + RowsRight;
                            if (Result < 0 || Result >= X)
                            {
                                Side = 1; // Probeer aan linkerkant toe te voegen
                                continue;
                            }
                            else
                            {
                                return Result; // rechts
                            }
                        }
                    }

                }
            }
        }

        public int DecideSide(int x, Container container)
        {

            totalWeightLeft = CalculateTotalWeight(0, x / 2);
            totalWeightRight = CalculateTotalWeight((x / 2) + 1, x);

            // voeg de container dan aan de linkerkant toe
            if (totalWeightLeft < totalWeightRight)
            {
                return 1;
            }
            // voeg de container aan de rechterkant toe
            else if (totalWeightRight < totalWeightLeft) { }
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
            int totalWeightContainers = CalculateTotalWeight(0, X);

            return totalWeightContainers >= (0.5 * MaxWeight);


        }


    }
}
