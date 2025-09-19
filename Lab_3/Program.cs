using MyFunctions;
using System.Drawing;
using static MyFunctions.Tools;

namespace Lab_3
{
    internal class Program
    {
        static Car[] cars = new Car[0];
        static int maxCapacity;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            maxCapacity = InputInt("Enter the maximum number of cars this storage can hold (1-100): ", InputType.With, 1, 100);

            do
            {
                try
                {
                    MessageBox.BoxItem("   Menu   ");
                    Console.WriteLine("1. Add car");
                    Console.WriteLine("2. Show all cars");
                    Console.WriteLine("3. Search car");
                    Console.WriteLine("4. Demonstrate behaviour");
                    Console.WriteLine("5. Delete car");
                    Console.WriteLine("0. Exit");

                    int choice = InputInt("MAIN MENU: Choose an option: ", InputType.With, -1, 5); //-1 to add seed data

                    switch (choice)
                    {
                        case 1:
                            MenuAddCar();
                            break;
                        case 2:
                            ShowAllCars();
                            break;
                        case 3:
                            SearchCars();
                            break;
                        case 4:
                            DemonstrateBehaviour();
                            break;
                        case 5:
                            RemoveCar();
                            break;

                        case -1:
                            AddSeedData();
                            break;
                        case 0:
                            Console.WriteLine("Goodbye! :)");
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            } while (true);
        }

        static public void AddSeedData() //cheat code
        {
            if (cars.Length >= maxCapacity)
            {
                Console.WriteLine("Storage is full, cannot add seed data.");
                return;
            }

            var seedDataItems = new[]
            {
                new { Mark = "Audi", Model = "A4", Color = Color.Red, HorsePower = 150f, Weight = 1550m, Milage = 12000.0, FuelConsumptionPer100km = 10.0, FuelCapacity = 60.0, ProductionDate = new DateTime(2022, 1, 1) },
                new { Mark = "Audi", Model = "A6", Color = Color.Black, HorsePower = 250f, Weight = 1800m, Milage = 0.0, FuelConsumptionPer100km = 14.0, FuelCapacity = 70.0, ProductionDate = new DateTime(2020, 6, 12) },
                new { Mark = "BMW", Model = "M3", Color = Color.Blue, HorsePower = 420f, Weight = 1600m, Milage = 85000.0, FuelConsumptionPer100km = 16.0, FuelCapacity = 63.0, ProductionDate = new DateTime(2008, 5, 17) },
                new { Mark = "Mini", Model = "Cooper", Color = Color.Green, HorsePower = 40f, Weight = 650m, Milage = 0.0, FuelConsumptionPer100km = 6.0, FuelCapacity = 30.0, ProductionDate = new DateTime(1995, 3, 4) },
                new { Mark = "Ford", Model = "F-150", Color = Color.Black, HorsePower = 400f, Weight = 2500m, Milage = 25000.0, FuelConsumptionPer100km = 24.0, FuelCapacity = 120.0, ProductionDate = new DateTime(2021, 1, 1) }
            };

            int initialCarsCount = cars.Length;
            int carsAddedCount = 0;

            foreach (var item in seedDataItems)
            {
                if (cars.Length < maxCapacity)
                {
                    AddCar(item.Mark, item.Model, item.Color, item.HorsePower, item.Weight, item.Milage, item.FuelConsumptionPer100km, item.FuelCapacity, item.ProductionDate);
                    carsAddedCount++;
                }
                else
                {
                    Console.WriteLine("Storage became full. Not all seed data cars were added.");
                    break;
                }
            }

            if (carsAddedCount > 0)
            {
                Console.WriteLine($"{carsAddedCount} cars added from seed data.");
                Console.WriteLine("CHEAT CODE ACTIVATED: Seed data added.");
            }
            else if (initialCarsCount == cars.Length)
            {
                Console.WriteLine("No seed data cars could be added due to storage capacity.");
            }
        }

        static void DemonstrateBehaviour()
        {
            if (cars.Length == 0)
            {
                Console.WriteLine("No cars yet.");
                return;
            }

            ShowAllCars();

            int selectedCarIndex;

            do
            {
                int userInputIndex = InputInt("Select car number to interact (0 to back to main menu): ", InputType.With, 0, cars.Length);
                if (userInputIndex == 0) return;
                selectedCarIndex = userInputIndex - 1;
                break;
            } while (true);

            InteractWithCar(cars[selectedCarIndex]);
        }

        static void MenuAddCar()
        {
            if (cars.Length >= maxCapacity)
            {
                Console.WriteLine("The object storage is full. Cannot add more cars.");
                return;
            }

            Array.Resize(ref cars, cars.Length + 1);
            cars[cars.Length - 1] = new Car();

            while (true)
            {
                string mark = InputString("Enter the cars mark: ");

                try
                {
                    cars[cars.Length - 1].Mark = mark;
                    break;

                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].Model = InputString("Enter the cars model: ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].Color = (Color)InputInt("Choose the cars color:\n0. Red\n1. Blue\n2. Green\n3. Black\n4. White\n5. Grey\nYour choice: ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                int numberOfDoors = InputInt("Enter the number of doors: ");
                try
                {
                    cars[cars.Length - 1].NumberOfDoors = numberOfDoors;
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].HorsePower = (float)InputDouble("Enter the car's horse power: ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].Weight = (decimal)InputDouble("Enter the car's weight (kg): ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].Milage = InputDouble("Enter the car's milage (km): ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].FuelConsumptionPer100km = (double)InputDouble("Enter the car's fuel consumption (l/100km): ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].FuelCapacity = InputDouble("Enter the car's fuel capacity (l): ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true)
            {
                try
                {
                    cars[cars.Length - 1].ProductionDate = InputDateTime("Enter the cars production date: ");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine($"Car {cars[cars.Length - 1].MarkAndModel} added successfully.");
        }

        static string AddCar(string mark, string model, Color color, float horsePower, decimal weight, double milage, double fuelConsumption, double fuelCapacity, DateTime productiDate)
        {
            Array.Resize(ref cars, cars.Length + 1);

            cars[cars.Length - 1] = new Car
            {
                Mark = mark,
                Model = model,
                Color = color,
                HorsePower = horsePower,
                Weight = weight,
                Milage = milage,
                FuelConsumptionPer100km = fuelConsumption,
                FuelCapacity = fuelCapacity,
                ProductionDate = productiDate
            };

            return "Car added successfully";
        }


        static void ShowAllCars()
        {
            if (cars.Length == 0)
            {
                Console.WriteLine("No cars yet...");
                return;
            }

            PrintHeader();
            for (int i = 0; i < cars.Length; i++)
            {
                PrintCarLine(i + 1, cars[i]);
            }
        }

        static void SearchCars()
        {
            if (cars.Length == 0)
            {
                Console.WriteLine("No cars yet...");
                return;
            }

            int choose = InputInt("Search by:\n1. Mark and Model\n2. Color\nYour choice: ", InputType.With, 1, 2);

            bool anyFound = false;

            if (choose == 1)
            {
                string text = InputString("Enter part of mark/model: ");
                PrintHeader();
                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i].MarkAndModel.ToLower().Contains(text.ToLower()))
                    {
                        PrintCarLine(i + 1, cars[i]);
                        anyFound = true;
                    }
                }
            }
            else
            {
                int colorVal = InputInt("Choose color:\n0. Red\n1. Blue\n2. Green\n3. Black\n4. White\n5. Grey\nYour choice: ", InputType.With, 0, 5);
                Color searchColor = (Color)colorVal;
                PrintHeader();
                for (int i = 0; i < cars.Length; i++)
                {
                    if (cars[i].Color == searchColor)
                    {
                        PrintCarLine(i + 1, cars[i]);
                        anyFound = true;
                    }
                }
            }

            if (!anyFound)
            {
                Console.WriteLine("No cars found...");
            }
        }

        static void RemoveCar()
        {
            if (cars.Length == 0)
            {
                Console.WriteLine("No cars yet...");
                return;
            }

            string removedNamesString = "";
            int itemsRemovedCount = 0;

            int choose = InputInt("Remove by:\n1. Mark and Model\n2. Color\n3. Index\nYour choice: ", InputType.With, 1, 3);

            switch (choose)
            {
                case 1:
                    string searchText = InputString("Enter search prompt of mark/model: ");
                    for (int i = 0; i < cars.Length - itemsRemovedCount; i++)
                    {
                        if (cars[i].MarkAndModel.ToLower().Contains(searchText.ToLower()))
                        {
                            if (removedNamesString == "") removedNamesString = cars[i].MarkAndModel;
                            else removedNamesString += ", " + cars[i].MarkAndModel;

                            itemsRemovedCount++;

                            for (int j = i; j < cars.Length - 1; j++)
                            {
                                cars[j] = cars[j + 1];
                            }
                            i--;
                        }
                    }
                    break;

                case 2:
                    int colorVal = InputInt("Choose color:\n0. Red\n1. Blue\n2. Green\n3. Black\n4. White\n5. Grey\nYour choice: ", InputType.With, 0, 5);
                    Color searchColor = (Color)colorVal;
                    for (int i = 0; i < cars.Length - itemsRemovedCount; i++)
                    {
                        if (cars[i].Color == searchColor)
                        {
                            if (removedNamesString == "") removedNamesString = cars[i].MarkAndModel;
                            else removedNamesString += ", " + cars[i].MarkAndModel;

                            itemsRemovedCount++;

                            for (int j = i; j < cars.Length - 1; j++)
                            {
                                cars[j] = cars[j + 1];
                            }
                            i--;
                        }
                    }
                    break;

                case 3:
                    ShowAllCars();
                    int indexToDelete = InputInt("Enter index of car to remove (or 0 to cancel): ", InputType.With, 0, cars.Length);
                    if (indexToDelete == 0)
                    {
                        Console.WriteLine("Removal cancelled");
                        return;
                    }

                    removedNamesString = cars[indexToDelete - 1].MarkAndModel;
                    itemsRemovedCount = 1;

                    for (int i = indexToDelete - 1; i < cars.Length - 1; i++)
                    {
                        cars[i] = cars[i + 1];
                    }
                    break;
            }

            if (itemsRemovedCount == 0)
            {
                Console.WriteLine("No cars found matching the criteria for removal...");
                return;
            }

            Array.Resize(ref cars, cars.Length - itemsRemovedCount);

            MessageBox.Show($"Removed the following cars: {removedNamesString}. Total cars remaining: {cars.Length}");
        }


        static void InteractWithCar(Car carSel)
        {
            do
            {
                try
                {
                    MessageBox.BoxItem("   Behaviour Menu   ");
                    PrintHeader();
                    PrintCarLine(0, carSel);
                    Console.WriteLine("1. Start engine");
                    Console.WriteLine("2. Stop engine");
                    Console.WriteLine("3. Speed up");
                    Console.WriteLine("4. Slow down");
                    Console.WriteLine("5. Ride the car");
                    Console.WriteLine("6. Refuel\n");
                    Console.WriteLine("0. Main menu\n");

                    int action = InputInt("BEHAVIOUR MENU: Choose how to interact: ", InputType.With, 0, 7);

                    switch (action)
                    {
                        case 1:
                            Console.WriteLine(carSel.StartEnige());
                            break;
                        case 2:
                            Console.WriteLine(carSel.StopEngine());
                            break;
                        case 3:
                            double inc = InputDouble("Speed increment (km/h): ", InputType.With, 0, 300);
                            Console.WriteLine(carSel.SpeedUp(inc));
                            break;
                        case 4:
                            double dec = InputDouble("Speed decrement (km/h): ", InputType.With, 0, 300);
                            Console.WriteLine(carSel.SlowDown(dec));
                            break;
                        case 5:
                            double distance = InputDouble("Distance to drive (km): ", InputType.With, 0, 10000);
                            Console.WriteLine(carSel.RideCar(distance));
                            break;
                        case 6:
                            if (carSel.CurrentFuel >= carSel.FuelCapacity)
                            {
                                Console.WriteLine("Tank is full.");
                                break;
                            }
                            double maxAdd = carSel.FuelCapacity - carSel.CurrentFuel;
                            double fuel = InputDouble($"Fuel to add (max {maxAdd:F1}): ", InputType.With, 0, maxAdd);
                            Console.WriteLine(carSel.Refuel(fuel));
                            break;
                        case 0:
                            Console.WriteLine("Returning to main menu...");
                            return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);
        }


        static void PrintHeader()
        {
            DrawLine(151);
            Console.WriteLine("Index| Mark&Model           | Color  | Doors | HP   | Weight | Milage     | Cap    | Fuel   | Fuel per 100km | Speed  | Max speed | Date of production");
            DrawLine(151);
        }

        static void PrintCarLine(int index, Car car)
        {
            Console.WriteLine(
                $"{index,4} | " +
                $"{car.MarkAndModel,-20} | " +
                $"{car.Color,-6} | " +
                $"{car.NumberOfDoors,5} | " +
                $"{car.HorsePower,4} | " +
                $"{car.Weight,6} | " +
                $"{car.Milage,10:F1} | " +
                $"{car.FuelCapacity,6:F1} | " +
                $"{car.CurrentFuel,6:F1} | " +
                $"{car.FuelConsumptionPer100km,14:F1} | " +
                $"{car.CurrentSpeed,6:F1} | " +
                $"{car.MaxSpeed,9:F1} | " +
                $"{car.ProductionDate:yyyy-MM-dd}"
                );
            DrawLine(151);
        }
    }
}