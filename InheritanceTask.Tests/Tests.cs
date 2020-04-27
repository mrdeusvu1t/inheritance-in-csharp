using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace InheritanceTask.Tests
{
    [TestFixture]
    public class PublicTest
    {
        private const string CarClassName = "car";
        private const string VehicleClassName = "vehicle";
        private const int ConstructorParamsCount = 2;

        private readonly string[] _fields = {"name", "maxSpeed"};
        private Type _carType;

        [SetUp]
        public void Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();
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

    [TestFixture]
    public class VehicleTests
    {
        private const string VehicleClassName = "vehicle";
        private const int ConstructorParamsCount = 2;
        private readonly string[] _fields = {"name", "maxspeed"};
        private Type _vehicleType;

        [SetUp]
        public void Initialize()
        {
            var assembly = Assembly.GetExecutingAssembly();

            _vehicleType = assembly.GetTypes().FirstOrDefault(
                t => t.Name.Equals(VehicleClassName, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void Vehicle_Class_Is_Created()
        {
            Assert.IsNotNull(_vehicleType, "'Vehicle' class is not created.");
        }

        [Test]
        public void All_Fields_Are_Defined()
        {
            var notDefinedFields = new List<string>();
            var vehicleFields = _vehicleType.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (var field in _fields)
            {
                var vField = vehicleFields.FirstOrDefault(f => f.Name.ToLowerInvariant().Contains(field));
                if (vField == null)
                {
                    notDefinedFields.Add(field);
                }
            }

            if (notDefinedFields.Count == 0)
            {
                notDefinedFields = null;
            }

            Assert.IsNull(
                notDefinedFields,
                $"Some field(s) is(are) not define: {notDefinedFields?.Aggregate((previous, next) => $"'{previous}', '{next}'")}");
        }

        [Test]
        public void MaxSpeed_Field_Is_Type_Of_Integer()
        {
            var vehicleFields = _vehicleType.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            var field = vehicleFields
                .FirstOrDefault(f => f.Name.ToLowerInvariant().Contains(_fields[1]));

            Assert.True(field.FieldType == typeof(int), $"'{field.Name}' field must be a type of INT.");
        }

        [Test]
        public void Name_Field_Is_Type_Of_String()
        {
            var vehicleFields = _vehicleType.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            var field = vehicleFields
                .FirstOrDefault(f => f.Name.ToLowerInvariant().Contains(_fields[0]));

            Assert.True(field.FieldType == typeof(string), $"'{field.Name}' field must be a type of STRING.");
        }

        [Test]
        public void Parametrized_Vehicle_Constructor_Is_Created()
        {
            var paramsConstructor = _vehicleType.GetConstructors(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .FirstOrDefault(c =>
                {
                    var parameters = c.GetParameters();
                    if (parameters.Length == ConstructorParamsCount)
                    {
                        if (parameters[0].ParameterType == typeof(string) &&
                            parameters[1].ParameterType == typeof(int) ||
                            parameters[0].ParameterType == typeof(int) && parameters[1].ParameterType == typeof(string))
                        {
                            return true;
                        }

                        return false;
                    }

                    return false;
                });

            Assert.IsNotNull(paramsConstructor,
                "'Vehicle' parametrized constructor is not defined or it does NOT contain appropriate parameters.");
        }

        [Test]
        public void All_Properties_Are_Defined()
        {
            var notDefinedProperties = new List<string>();
            var vehicleProperties = _vehicleType.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (var field in _fields)
            {
                var property = vehicleProperties.FirstOrDefault(f => f.Name.ToLowerInvariant().Contains(field));
                if (property == null)
                {
                    notDefinedProperties.Add(field);
                }
            }

            if (notDefinedProperties.Count == 0)
            {
                notDefinedProperties = null;
            }

            Assert.IsNull(
                notDefinedProperties,
                $"Some property(ies) is(are) not define: {notDefinedProperties?.Aggregate((previous, next) => $"'{previous}', '{next}'")}");
        }

        [Test]
        public void Name_Property_Is_Type_Of_String()
        {
            var nonPublicProperties = _vehicleType.GetProperties(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            var property = nonPublicProperties.FirstOrDefault(p => p.Name.ToLowerInvariant().Contains(_fields[0]));

            Assert.True(property.PropertyType == typeof(string),
                $"'{property.Name}' property must be a type of STRING.");
        }

        [Test]
        public void MaxSpeed_Property_Is_Type_Of_Integer()
        {
            var publicProperties = _vehicleType.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            var property = publicProperties.FirstOrDefault(p => p.Name.ToLowerInvariant().Contains(_fields[1]));

            Assert.True(property.PropertyType == typeof(int), $"'{property.Name}' property must be a type of INT.");
        }
    }
}