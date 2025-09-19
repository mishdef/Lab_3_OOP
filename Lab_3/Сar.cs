namespace Lab_3
{
    internal class Car
    {
        string _mark;
        string _model;

        Color _color;
        float _horsePower;
        decimal _weight;
        double _milage;
        double _fuelCapacity;
        double _currentFuel;
        DateTime _productionDate;
        double _fuelConsumptionPer100km;
        int _numberOfDoors;

        bool _isStarted = false;
        double _currentSpeed = 0;

        public int NumberOfDoors 
        {
            get { return _numberOfDoors; }
            set 
            {
                if (value < 2 || value > 5) throw new ArgumentException("Number of doors must be between 2 and 5.");
                _numberOfDoors = value; 
            }
        }

        public string Mark
        {
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Mark cannot be empty.");
                if (value.Length < 1) throw new ArgumentException("Mark cannot be shorter than 1 character.");
                if (value.Length > 10) throw new ArgumentException("Mark cannot be longer than 10 characters.");
                if (!char.IsLetter(value[0])) throw new ArgumentException("Mark must start with a letter.");

                _mark = value;
            }
            private get { return _mark; }
        }

        public string Model
        {
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Mark cannot be empty.");
                if (value.Length < 1) throw new ArgumentException("Mark cannot be shorter than 1 character.");
                if (value.Length > 10) throw new ArgumentException("Mark cannot be longer than 10 characters.");
                if (!char.IsLetter(value[0])) throw new ArgumentException("Mark must start with a letter.");

                _model = value;
            }
            private get { return _model; }
        }

        public Color Color
        {
            set
            {
                if ((int)value < 0 || (int)value > 5) throw new ArgumentException("Color must be between 0 and 5.");
                _color = value;
            }
            get { return _color; }
        }

        public float HorsePower
        {
            set
            {
                if (value < 20 || value > 2000) throw new ArgumentException("Horse power must be between 20 and 2000.");
                _horsePower = value;
                MaxSpeed = _horsePower / (float)_weight * 1000;
            }
            get { return _horsePower; }
        }

        public double CurrentSpeed
        {
            set
            {
                if (value < 0) throw new ArgumentException("Speed cannot be negative.");
                if (value > MaxSpeed) throw new ArgumentException("Speed cannot exceed maximum speed.");
                _currentSpeed = value;
            }
            get { return _currentSpeed; }
        }

        public double MaxSpeed { get; private set; } = 0;  //aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

        public decimal Weight
        {
            set
            {
                if (value < 400 || value > 8000) throw new ArgumentException("Weight must be between 400 and 8000.");
                _weight = value;
                MaxSpeed = _horsePower / (float)_weight * 1000;
            }
            get { return _weight; }
        }

        public double Milage
        {
            set
            {
                if (value < 0) throw new ArgumentException("Milage cannot be negative.");
                if (value > 3000000) throw new ArgumentException("How your junk even works?..."); //рофл
                _milage = value;
            }
            get { return _milage; }
        }

        public double FuelCapacity
        {
            set
            {
                if (value < 20) throw new ArgumentException("Fuel capacity cannot be less than 20 liters.");
                if (value > 200) throw new ArgumentException("Fuel capacity cannot exceed 200 liters.");

                _currentFuel = value;
                _fuelCapacity = value;
            }
            get { return _fuelCapacity; }
        }

        public double CurrentFuel
        {
            set
            {
                if (value < 0) throw new ArgumentException("Current fuel cannot be negative.");
                if (value > _fuelCapacity) throw new ArgumentException("Current fuel cannot exceed fuel capacity.");
                _currentFuel = value;
            }
            get { return _currentFuel; }
        }

        public DateTime ProductionDate
        {
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Production date cannot be in the future.");
                _productionDate = value;
            }
            get { return _productionDate; }
        }

        public double FuelConsumptionPer100km
        {
            set
            {
                if (value < 0) throw new ArgumentException("Fuel consumption cannot be negative.");
                _fuelConsumptionPer100km = value;
            }
            get { return _fuelConsumptionPer100km; }
        }

        public bool IsStarted
        {
            get { return _isStarted; }
        }

        public string MarkAndModel
        {
            get { return Mark + " " + Model; }
        }

        public string StartEnige()
        {
            if (_isStarted)
            {
                return "The car is already started.";
            }
            if (_currentFuel <= 0)
            {
                return "Cannot start engine. The fuel tank is empty.";
            }
            _isStarted = true;
            return "The car has started.";
        }

        public string StopEngine()
        {
            if (_isStarted)
            {
                _isStarted = false;
                _currentSpeed = 0;
                return "The car has stopped.";
            }
            else
            {
                return "The car is already stopped.";
            }
        }

        public string SpeedUp(double increment)
        {
            if (!_isStarted)
            {
                return "The car is not started. Please start the engine first.";
            }
            if (_currentFuel <= 0)
            {
                StopEngine();
                return "Out of fuel! The car stopped. Please refuel.";
            }
            if (increment < 0)
            {
                return "Increment must be a positive value.";
            }

            _currentSpeed += increment;
            if (_currentSpeed > MaxSpeed)
            {
                _currentSpeed = MaxSpeed;
                return $"The car has reached its maximum speed of {MaxSpeed:F2} km/h.";
            }
            return $"The car's current speed is {_currentSpeed:F2} km/h.";
        }

        public string SlowDown(double decrement)
        {
            if (!_isStarted)
            {
                return "The car is not started. Please start the engine first.";
            }
            if (decrement < 0)
            {
                return "Decrement must be a positive value.";
            }
            _currentSpeed -= decrement;
            if (_currentSpeed < 0)
            {
                _currentSpeed = 0;
                return "The car has come to a complete stop.";
            }
            return $"The car's current speed is {_currentSpeed:F2} km/h.";
        }

        public string Refuel(double amount)
        {
            if (amount <= 0)
            {
                return "Refuel amount must be positive.";
            }

            if (_currentFuel == _fuelCapacity)
            {
                return "Tank is already full.";
            }

            double fuelAdded = 0;
            if (_currentFuel + amount > _fuelCapacity)
            {
                fuelAdded = _fuelCapacity - _currentFuel;
                _currentFuel = _fuelCapacity;
                return $"Refueled {fuelAdded:F2} liters. The tank is now full: {_currentFuel:F2} / {_fuelCapacity} liters.";
            }
            else
            {
                _currentFuel += amount;
                fuelAdded = amount;
                return $"Refueled {fuelAdded:F2} liters. Current fuel: {_currentFuel:F2} / {_fuelCapacity} liters.";
            }
        }

        private double CalculateFuelConsumption(double distanceDrivenKM) //private new
        {
            double litersPerKM = _fuelConsumptionPer100km / 100.0;
            return litersPerKM * distanceDrivenKM;
        }

        public string RideCar(double distanceDrivenKM)
        {
            if (!_isStarted)
            {
                return "Cannot ride. The car is not started. Please start the engine first.";
            }
            if (_currentSpeed <= 0)
            {
                return "Cannot ride. The car is not moving. Increase speed first.";
            }
            if (distanceDrivenKM <= 0)
            {
                return "Driving distance must be positive.";
            }
            if (_currentFuel <= 0)
            {
                StopEngine();
                return "Out of fuel! The car has stopped. Please refuel.";
            }

            double fuelConsumed = CalculateFuelConsumption(distanceDrivenKM); //new

            double timeInMinutes = (distanceDrivenKM / _currentSpeed) * 60.0; //new


            if (fuelConsumed > _currentFuel)
            {
                double actualDistancePossible = _currentFuel / (_fuelConsumptionPer100km / 100.0);

                double actualTimePossible = (actualDistancePossible / _currentSpeed) * 60.0;

                _milage += actualDistancePossible;
                _currentFuel = 0;
                StopEngine();
                return $"Ran out of fuel after driving for {actualTimePossible:F2} minutes and {actualDistancePossible:F2} km. " +
                       $"The car stopped. Total milage: {_milage:F2} km. Please refuel.";
            }
            else
            {
                _currentFuel -= fuelConsumed;
                _milage += distanceDrivenKM;
                return $"Drove for {timeInMinutes:F2} minutes ({distanceDrivenKM:F2} km) at {_currentSpeed:F2} km/h. " +
                       $"Fuel consumed: {fuelConsumed:F2} liters. Remaining fuel: {_currentFuel:F2} / {_fuelCapacity} liters. " +
                       $"Total milage: {_milage:F2} km.";
            }
        }

        public override string ToString()
        {
            return $"Car: {MarkAndModel}, Color: {Color}, HorsePower: {HorsePower}, Weight: {Weight}, Milage: {Milage:F2} km, CurrentSpeed: {CurrentSpeed:F2} km/h ," +
                  $"MaxSpeed: {MaxSpeed:F2} km/h, Fuel: {CurrentFuel:F2}/{FuelCapacity} liters. Fuel per 100km: {FuelConsumptionPer100km:F2} liters.";
        }
    }
}