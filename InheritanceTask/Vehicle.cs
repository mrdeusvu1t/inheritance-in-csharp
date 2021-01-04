using System;
using System.Collections.Generic;
using System.Text;

namespace InheritanceTask
{
	abstract class Vehicle
	{
		private string name;
		private readonly int maxSpeed;

		public Vehicle(string name, int maxSpeed)
		{
			Name = name;
		}

		protected string Name 
		{
			get => name;
			set => name = value;
		}

		public int MaxSpeed
		{
			get => maxSpeed;
		}
	}

	class Car : Vehicle
	{
		public Car(string name, int maxSpeed)
			: base(name, maxSpeed) { }

		public void SetName(string newName)
		{
			Name = newName;
		}

		public virtual string GetName()
		{
			return Name;
		}
	}
}
