using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class ScheduleDay : PropertyChangedBase
	{
		#region Ignore for serialization
		private Action<Action> _propertyChangedDispatcher = Execute.DefaultPropertyChangedDispatcher;

		[TomlIgnore]
		[XmlIgnore]
		public virtual Action<Action> PropertyChangedDispatcher
		{
			get => _propertyChangedDispatcher;
			set => _propertyChangedDispatcher = value;
		}

		/// <summary>
		/// Occurs when a property value changes
		/// </summary>
		[TomlIgnore]
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		private string _after;
		private string _before;

		public string after
		{
			get => _after;
			set => SetAndNotify(ref _after, value);
		}

		public string before
		{
			get => _before;
			set => SetAndNotify(ref _before, value);
		}
	}

	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class Schedule : PropertyChangedBase
	{
		#region Ignore for serialization
		private Action<Action> _propertyChangedDispatcher = Execute.DefaultPropertyChangedDispatcher;

		[TomlIgnore]
		[XmlIgnore]
		public virtual Action<Action> PropertyChangedDispatcher
		{
			get => _propertyChangedDispatcher;
			set => _propertyChangedDispatcher = value;
		}

		/// <summary>
		/// Occurs when a property value changes
		/// </summary>
		[TomlIgnore]
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		public BindableCollection<ScheduleDay> mon { get; set; }
		public BindableCollection<ScheduleDay> tue { get; set; }
		public BindableCollection<ScheduleDay> wed { get; set; }
		public BindableCollection<ScheduleDay> thu { get; set; }
		public BindableCollection<ScheduleDay> fri { get; set; }
		public BindableCollection<ScheduleDay> sat { get; set; }
		public BindableCollection<ScheduleDay> sun { get; set; }

	}
}
