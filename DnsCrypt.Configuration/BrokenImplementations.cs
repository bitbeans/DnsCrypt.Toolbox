using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class BrokenImplementations : PropertyChangedBase
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

		private List<string> _broken_query_padding;
		private List<string> _fragments_blocked;

		public List<string> broken_query_padding
		{
			get => _broken_query_padding;
			set => SetAndNotify(ref _broken_query_padding, value);
		}

		[TomlComment("# The list below enables workarounds to make non-relayed usage more reliable", CommentLocation.Prepend)]
		[TomlComment("# until the servers are fixed.", CommentLocation.Prepend)]
		public List<string> fragments_blocked
		{
			get => _fragments_blocked;
			set => SetAndNotify(ref _fragments_blocked, value);
		}
	}
}
