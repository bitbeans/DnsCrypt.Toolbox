using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using DnsCrypt.Models;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class Source : PropertyChangedBase
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

		private BindableCollection<Stamp> _stamps;
		public string[] urls { get; set; }
		public string minisign_key { get; set; }
		public string cache_file { get; set; }
		public string format { get; set; }
		public int refresh_delay { get; set; }
		public string prefix { get; set; }

		[TomlIgnore]
		public BindableCollection<Stamp> Stamps
		{
			get => _stamps;
			set => SetAndNotify(ref _stamps, value);
		}
	}
}
