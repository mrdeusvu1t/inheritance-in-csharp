using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace InheritanceTask.Tests
{
    [TestFixture]
    public class CarTests
    {
        private const string CarClassName = "car";
        private const string VehicleClassName = "vehicle";
        private const int ConstructorParamsCount = 2;

        private readonly string[] _fields = {"name", "maxSpeed"};
        private Type _carType;

        [SetUp]
        public void Initialize()
        {
            var assembly = Assembly.Load("InheritanceTask");
            _carType = assembly.GetTypes().FirstOrDefault(
                t => t.Name.Equals(CarClassName, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Car_Class_Is_Created()
        {
            Assert.IsNotNull(_carType, "'Car' class is not created.");
        }

        [Test]
        public void Car_Inherits_Vehicle()
        {
            var carInstance = Activator.CreateInstance(_carType, string.Empty, 0);
            var vehicleType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(VehicleClassName, StringComparison.OrdinalIgnoreCase));

            Assert.IsInstanceOf(vehicleType, carInstance, "'Car' type does NOT inherit 'Vehicle' type.");
        }

        [Test]
        public void Set_Name_Method_Is_Defined()
        {
            var method = _carType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    if (m.ReturnType == typeof(void) && parameters?.FirstOrDefault()?.ParameterType == typeof(string))
                    {
                        return true;
                    }

                    return false;
                });

            Assert.IsNotNull(method,
                "Method which changes 'Car' name is NOT define or it does NOT contain correct parameters.");
        }

        [Test]
        public void Get_Name_Method_Is_Defined()
        {
            var method = _carType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    if (m.ReturnType == typeof(string) && m.GetParameters().Length == 0)
                    {
                        return true;
                    }

                    return false;
                });

            Assert.IsNotNull(method,
                "Method which retrieves 'Car' name is NOT define or it is's return type is NOT correct.");
        }

        [Test]
        public void Get_Car_Name()
        {
            var name = "Toyota";
            var newName = "BMW";
            var age = 5;
            var carInstance = Activator.CreateInstance(_carType, name, age);

            var setNameMethod = _carType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    var parameters = m.GetParameters();
                    if (m.ReturnType == typeof(void) && parameters?.FirstOrDefault()?.ParameterType == typeof(string))
                    {
                        return true;
                    }

                    return false;
                });

            var getNameMethod = _carType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(m =>
                {
                    if (m.ReturnType == typeof(string) && m.GetParameters().Length == 0)
                    {
                        return true;
                    }

                    return false;
                });

            setNameMethod.Invoke(carInstance, new[] {newName});

            var carName = getNameMethod.Invoke(carInstance, new object[] { });

            Assert.AreEqual(
                newName,
                carName,
                $"'{getNameMethod.Name}' method does NOT return correct value or '{setNameMethod.Name}' method does NOT change car name correctly.");
        }
    }
}