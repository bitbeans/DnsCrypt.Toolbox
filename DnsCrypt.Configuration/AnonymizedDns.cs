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
	public class AnonymizedDns : PropertyChangedBase
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

		private List<Route> _routes;
		private bool? _skip_incompatible;

		[TomlComment("# Routes are indirect ways to reach DNSCrypt servers.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# A route maps a server name (\"server_name\") to one or more relays that will be", CommentLocation.Prepend)]
		[TomlComment("# used to connect to that server.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# A relay can be specified as a DNS Stamp (either a relay stamp, or a", CommentLocation.Prepend)]
		[TomlComment("# DNSCrypt stamp), an IP:port, a hostname:port, or a server name.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# Review the list of available relays from the \"relays.md\" file, and, for each", CommentLocation.Prepend)]
		[TomlComment("# server you want to use, define the relays you want connections to go through.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# Carefully choose relays and servers so that they are run by different entities.", CommentLocation.Prepend)]
		[TomlComment("# \"server_name\" can also be set to \"*\" to define a default route, but this is not", CommentLocation.Prepend)]
		[TomlComment("# recommended. If you do so, keep \"server_name\" short and distinct from relays.", CommentLocation.Prepend)]
		public List<Route> routes
		{
			get => _routes;
			set => SetAndNotify(ref _routes, value);
		}

		/// <summary>
		/// skip resolvers incompatible with anonymization instead of using them directly
		/// </summary>
		[TomlComment("# skip resolvers incompatible with anonymization instead of using them directly", CommentLocation.Prepend)]
		public bool? skip_incompatible
		{
			get => _skip_incompatible;
			set => SetAndNotify(ref _skip_incompatible, value);
		}
	}
}
