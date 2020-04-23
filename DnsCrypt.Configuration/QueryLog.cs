using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class QueryLog : PropertyChangedBase
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

		private string _file;
		private string _format;
		private BindableCollection<string> _ignored_qtypes;

		/// <summary>
		/// Path to the query log file (absolute, or relative to the same directory as the config file)
		/// On non-Windows systems, can be /dev/stdout to log to the standard output (also set log_files_max_size to 0)
		/// </summary>
		[TomlComment("# Path to the query log file (absolute, or relative to the same directory as the config file)", CommentLocation.Prepend)]
		[TomlComment("# On non-Windows systems, can be /dev/stdout to log to the standard output (also set log_files_max_size to 0)", CommentLocation.Prepend)]
		public string file
		{
			get => _file;
			set => SetAndNotify(ref _file, value);
		}

		/// <summary>
		/// Query log format (currently supported: tsv and ltsv)
		/// Log format (SimpleDnsCrypt: ltsv).
		/// </summary>
		[TomlComment("# Query log format (currently supported: tsv and ltsv)", CommentLocation.Prepend)]
		public string format
		{
			get => _format;
			set => SetAndNotify(ref _format, value);
		}

		/// <summary>
		/// Do not log these query types, to reduce verbosity. Keep empty to log everything.
		/// </summary>
		[TomlComment("# Do not log these query types, to reduce verbosity. Keep empty to log everything.", CommentLocation.Prepend)]
		public BindableCollection<string> ignored_qtypes
		{
			get => _ignored_qtypes;
			set => SetAndNotify(ref _ignored_qtypes, value);
		}
	}
}
