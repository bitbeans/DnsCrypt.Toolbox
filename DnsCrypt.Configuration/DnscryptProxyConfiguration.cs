using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Nett;
using Stylet;

namespace DnsCrypt.Configuration
{
	/// <summary>
	/// Class to serialize the dnscrypt-proxy.toml configuration file.
	/// </summary>
	[SuppressMessage("ReSharper", "IdentifierTypo")]
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class DnscryptProxyConfiguration : PropertyChangedBase
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

		private BindableCollection<string> _server_names;
		private BindableCollection<string> _listen_addresses;
		private int _max_clients;
		private string _user_name;
		private bool _ipv4_servers;
		private bool _ipv6_servers;
		private bool _dnscrypt_servers;
		private bool _doh_servers;
		private bool _require_dnssec;
		private bool _require_nolog;
		private bool _require_nofilter;
		private BindableCollection<string> _disabled_server_names;
		private bool _force_tcp;
		private string _proxy;
		private string _http_proxy;
		private int _timeout;
		private int _keepalive;
		private string _blocked_query_response;
		private string _lb_strategy;
		private bool? _lb_estimator;
		private int? _log_level;
		private string _log_file;
		private bool? _use_syslog;
		private int _cert_refresh_delay;
		private bool? _dnscrypt_ephemeral_keys;
		private bool? _tls_disable_session_tickets;
		private BindableCollection<int> _tls_cipher_suite;
		private BindableCollection<string> _fallback_resolvers;
		private string _fallback_resolver;
		private bool _ignore_system_dns;
		private int _netprobe_timeout;
		private string _netprobe_address;
		private bool? _offline_mode;
		private BindableCollection<string> _query_meta;
		private int _log_files_max_size;
		private int _log_files_max_age;
		private int _log_files_max_backups;
		private bool _block_ipv6;
		private bool _block_unqualified;
		private bool _block_undelegated;
		private int _reject_ttl;
		private string _forwarding_rules;
		private string _cloaking_rules;
		private int? _cloak_ttl;
		private bool _cache;
		private int _cache_size;
		private int _cache_min_ttl;
		private int _cache_max_ttl;
		private int _cache_neg_min_ttl;
		private int _cache_neg_max_ttl;

		private Dictionary<string, Schedule> _schedules;
		private Dictionary<string, Source> _sources;
		private Dictionary<string, Static> _static;

		/// <summary>
		///List of servers to use
		///
		///Servers from the "public-resolvers" source (see down below) can
		///be viewed here: https://dnscrypt.info/public-servers
		///
		///The proxy will automatically pick working servers from this list.
		///Note that the require_* filters do NOT apply when using this setting.
		///
		///By default, this list is empty and all registered servers matching the
		///require_* filters will be used instead.
		/// </summary>
		[TomlComment("# List of servers to use", CommentLocation.Prepend)]
		[TomlComment("#", CommentLocation.Prepend)]
		[TomlComment("# Servers from the 'public-resolvers' source (see down below) can", CommentLocation.Prepend)]
		[TomlComment("# be viewed here: https://dnscrypt.info/public-servers", CommentLocation.Prepend)]
		[TomlComment("# The proxy will automatically pick working servers from this list.", CommentLocation.Prepend)]
		[TomlComment("# Note that the require_* filters do NOT apply when using this setting.", CommentLocation.Prepend)]
		[TomlComment("#", CommentLocation.Prepend)]
		[TomlComment("# By default, this list is empty and all registered servers matching the", CommentLocation.Prepend)]
		[TomlComment("# require_* filters will be used instead.", CommentLocation.Prepend)]
		public BindableCollection<string> server_names
		{
			get => _server_names;
			set => SetAndNotify(ref _server_names, value);
		}

		/// <summary>
		/// List of local addresses and ports to listen to. Can be IPv4 and/or IPv6.
		/// Example with both IPv4 and IPv6:
		/// listen_addresses = ['127.0.0.1:53', '[::1]:53']
		/// </summary>
		[TomlComment("# List of local addresses and ports to listen to. Can be IPv4 and/or IPv6.", CommentLocation.Prepend)]
		[TomlComment("# Example with both IPv4 and IPv6:", CommentLocation.Prepend)]
		[TomlComment("# listen_addresses = ['127.0.0.1:53', '[::1]:53']", CommentLocation.Prepend)]
		public BindableCollection<string> listen_addresses
		{
			get => _listen_addresses;
			set => SetAndNotify(ref _listen_addresses, value);
		}

		/// <summary>
		/// Maximum number of simultaneous client connections to accept.
		/// </summary>
		[TomlComment("# Maximum number of simultaneous client connections to accept", CommentLocation.Prepend)]
		public int max_clients
		{
			get => _max_clients;
			set => SetAndNotify(ref _max_clients, value);
		}

		/// <summary>
		/// Switch to a different system user after listening sockets have been created.
		/// Note (1): this feature is currently unsupported on Windows.
		/// Note (2): this feature is not compatible with systemd socket activation.
		/// Note (3): when using -pidfile, the PID file directory must be writable by the new user
		/// </summary>
		[TomlComment("# Switch to a different system user after listening sockets have been created.", CommentLocation.Prepend)]
		[TomlComment("# Note (1): this feature is currently unsupported on Windows.", CommentLocation.Prepend)]
		[TomlComment("# Note (2): this feature is not compatible with systemd socket activation.", CommentLocation.Prepend)]
		[TomlComment("# Note (3): when using -pidfile, the PID file directory must be writable by the new user", CommentLocation.Prepend)]
		public string user_name
		{
			get => _user_name;
			set => SetAndNotify(ref _user_name, value);
		}

		/// <summary>
		/// Use servers reachable over IPv4
		/// </summary>
		[TomlComment("# Require servers (from static + remote sources) to satisfy specific properties", CommentLocation.Prepend)]
		[TomlComment("Use servers reachable over IPv4", CommentLocation.Prepend)]
		public bool ipv4_servers
		{
			get => _ipv4_servers;
			set => SetAndNotify(ref _ipv4_servers, value);
		}

		/// <summary>
		/// Use servers reachable over IPv6 -- Do not enable if you don't have IPv6 connectivity
		/// </summary>
		[TomlComment("Use servers reachable over IPv6 -- Do not enable if you don't have IPv6 connectivity", CommentLocation.Prepend)]
		public bool ipv6_servers
		{
			get => _ipv6_servers;
			set => SetAndNotify(ref _ipv6_servers, value);
		}

		/// <summary>
		/// Use servers implementing the DNSCrypt protocol
		/// </summary>
		[TomlComment("Use servers implementing the DNSCrypt protocol", CommentLocation.Prepend)]
		public bool dnscrypt_servers
		{
			get => _dnscrypt_servers;
			set => SetAndNotify(ref _dnscrypt_servers, value);
		}

		/// <summary>
		///	Use servers implementing the DNS-over-HTTPS protocol
		/// </summary>
		[TomlComment("Use servers implementing the DNS-over-HTTPS protocol", CommentLocation.Prepend)]
		public bool doh_servers
		{
			get => _doh_servers;
			set => SetAndNotify(ref _doh_servers, value);
		}

		/// <summary>
		/// Server must support DNS security extensions (DNSSEC)
		/// </summary>
		[TomlComment("# Require servers defined by remote sources to satisfy specific properties", CommentLocation.Prepend)]
		[TomlComment("Server must support DNS security extensions (DNSSEC)", CommentLocation.Prepend)]
		public bool require_dnssec
		{
			get => _require_dnssec;
			set => SetAndNotify(ref _require_dnssec, value);
		}

		/// <summary>
		/// Server must not log user queries (declarative)
		/// </summary>
		[TomlComment("Server must not log user queries (declarative)", CommentLocation.Prepend)]
		public bool require_nolog
		{
			get => _require_nolog;
			set => SetAndNotify(ref _require_nolog, value);
		}

		/// <summary>
		/// Server must not enforce its own blacklist (for parental control, ads blocking...)
		/// </summary>
		[TomlComment("Server must not enforce its own blacklist (for parental control, ads blocking...)", CommentLocation.Prepend)]
		public bool require_nofilter
		{
			get => _require_nofilter;
			set => SetAndNotify(ref _require_nofilter, value);
		}

		/// <summary>
		///  Server names to avoid even if they match all criteria
		/// </summary>
		[TomlComment("Server names to avoid even if they match all criteria", CommentLocation.Prepend)]
		public BindableCollection<string> disabled_server_names
		{
			get => _disabled_server_names;
			set => SetAndNotify(ref _disabled_server_names, value);
		}

		/// <summary>
		/// Always use TCP to connect to upstream servers.
		/// This can be useful if you need to route everything through Tor.
		/// Otherwise, leave this to `false`, as it doesn't improve security
		/// (dnscrypt-proxy will always encrypt everything even using UDP), and can
		/// only increase latency.
		/// </summary>
		[TomlComment("Always use TCP to connect to upstream servers.", CommentLocation.Prepend)]
		[TomlComment("This can be useful if you need to route everything through Tor.", CommentLocation.Prepend)]
		[TomlComment("Otherwise, leave this to `false`, as it doesn't improve security", CommentLocation.Prepend)]
		[TomlComment("(dnscrypt-proxy will always encrypt everything even using UDP), and can", CommentLocation.Prepend)]
		[TomlComment("only increase latency.", CommentLocation.Prepend)]
		public bool force_tcp
		{
			get => _force_tcp;
			set => SetAndNotify(ref _force_tcp, value);
		}

		/// <summary>
		/// SOCKS proxy
		/// Uncomment the following line to route all TCP connections to a local Tor node
		/// Tor doesn't support UDP, so set `force_tcp` to `true` as well.
		/// </summary>
		[TomlComment("SOCKS proxy", CommentLocation.Prepend)]
		[TomlComment("Uncomment the following line to route all TCP connections to a local Tor node", CommentLocation.Prepend)]
		[TomlComment("Tor doesn't support UDP, so set `force_tcp` to `true` as well.", CommentLocation.Prepend)]
		public string proxy
		{
			get => _proxy;
			set => SetAndNotify(ref _proxy, value);
		}

		/// <summary>
		/// HTTP/HTTPS proxy
		/// Only for DoH servers
		/// </summary>
		[TomlComment("HTTP/HTTPS proxy", CommentLocation.Prepend)]
		[TomlComment("Only for DoH servers", CommentLocation.Prepend)]
		public string http_proxy
		{
			get => _http_proxy;
			set => SetAndNotify(ref _http_proxy, value);
		}

		/// <summary>
		/// How long a DNS query will wait for a response, in milliseconds.
		/// If you have a network with *a lot* of latency, you may need to
		/// increase this. Startup may be slower if you do so.
		/// Don't increase it too much. 10000 is the highest reasonable value.
		/// </summary>
		[TomlComment("How long a DNS query will wait for a response, in milliseconds.", CommentLocation.Prepend)]
		[TomlComment("If you have a network with *a lot* of latency, you may need to", CommentLocation.Prepend)]
		[TomlComment("increase this. Startup may be slower if you do so.", CommentLocation.Prepend)]
		[TomlComment("Don't increase it too much. 10000 is the highest reasonable value.", CommentLocation.Prepend)]
		public int timeout
		{
			get => _timeout;
			set => SetAndNotify(ref _timeout, value);
		}

		/// <summary>
		/// Keepalive for HTTP (HTTPS, HTTP/2) queries, in seconds
		/// </summary>
		[TomlComment("Keepalive for HTTP (HTTPS, HTTP/2) queries, in seconds", CommentLocation.Prepend)]
		public int keepalive
		{
			get => _keepalive;
			set => SetAndNotify(ref _keepalive, value);
		}

		/// <summary>
		/// Response for blocked queries.  Options are `refused`, `hinfo` (default) or
		///	an IP response.  To give an IP response, use the format `a:<IPv4>,aaaa:<IPv6>`.
		///	Using the `hinfo` option means that some responses will be lies.
		///	Unfortunately, the `hinfo` option appears to be required for Android 8+
		/// </summary>
		[TomlComment("# Response for blocked queries.  Options are `refused`, `hinfo` (default) or", CommentLocation.Prepend)]
		[TomlComment("# an IP response.  To give an IP response, use the format `a:<IPv4>,aaaa:<IPv6>`.", CommentLocation.Prepend)]
		[TomlComment("# Using the `hinfo` option means that some responses will be lies.", CommentLocation.Prepend)]
		[TomlComment("# Unfortunately, the `hinfo` option appears to be required for Android 8+", CommentLocation.Prepend)]
		public string blocked_query_response
		{
			get => _blocked_query_response;
			set => SetAndNotify(ref _blocked_query_response, value);
		}

		/// <summary>
		/// Load-balancing strategy: 'p2' (default), 'ph', 'first' or 'random'
		/// </summary>
		[TomlComment("# Load-balancing strategy: 'p2' (default), 'ph', 'first' or 'random'", CommentLocation.Prepend)]
		public string lb_strategy
		{
			get => _lb_strategy;
			set => SetAndNotify(ref _lb_strategy, value);
		}

		/// <summary>
		/// Set to `true` to constantly try to estimate the latency of all the resolvers
		/// and adjust the load-balancing parameters accordingly, or to `false` to disable.
		/// </summary>
		[TomlComment("# Set to `true` to constantly try to estimate the latency of all the resolvers", CommentLocation.Prepend)]
		[TomlComment("# and adjust the load-balancing parameters accordingly, or to `false` to disable.", CommentLocation.Prepend)]
		public bool? lb_estimator
		{
			get => _lb_estimator;
			set => SetAndNotify(ref _lb_estimator, value);
		}

		/// <summary>
		/// Log level (0-6, default: 2 - 0 is very verbose, 6 only contains fatal errors).
		/// </summary>
		[TomlComment("# Log level (0-6, default: 2 - 0 is very verbose, 6 only contains fatal errors).", CommentLocation.Prepend)]
		public int? log_level
		{
			get => _log_level;
			set => SetAndNotify(ref _log_level, value);
		}

		/// <summary>
		/// log file for the application.
		/// </summary>
		[TomlComment("# log file for the application", CommentLocation.Prepend)]
		public string log_file
		{
			get => _log_file;
			set => SetAndNotify(ref _log_file, value);
		}

		/// <summary>
		/// Use the system logger (syslog on Unix, Event Log on Windows)
		/// </summary>
		[TomlComment("# Use the system logger (syslog on Unix, Event Log on Windows)", CommentLocation.Prepend)]
		public bool? use_syslog
		{
			get => _use_syslog;
			set => SetAndNotify(ref _use_syslog, value);
		}

		/// <summary>
		/// Delay, in minutes, after which certificates are reloaded.
		/// </summary>
		[TomlComment("# Delay, in minutes, after which certificates are reloaded.", CommentLocation.Prepend)]
		public int cert_refresh_delay
		{
			get => _cert_refresh_delay;
			set => SetAndNotify(ref _cert_refresh_delay, value);
		}

		/// <summary>
		/// DNSCrypt: Create a new, unique key for every single DNS query
		/// This may improve privacy but can also have a significant impact on CPU usage
		/// Only enable if you don't have a lot of network load
		/// </summary>
		[TomlComment("# DNSCrypt: Create a new, unique key for every single DNS query", CommentLocation.Prepend)]
		[TomlComment("# This may improve privacy but can also have a significant impact on CPU usage", CommentLocation.Prepend)]
		[TomlComment("# Only enable if you don't have a lot of network load", CommentLocation.Prepend)]
		public bool? dnscrypt_ephemeral_keys
		{
			get => _dnscrypt_ephemeral_keys;
			set => SetAndNotify(ref _dnscrypt_ephemeral_keys, value);
		}

		/// <summary>
		/// DoH: Disable TLS session tickets - increases privacy but also latency.
		/// </summary>
		[TomlComment("# DoH: Disable TLS session tickets - increases privacy but also latency", CommentLocation.Prepend)]
		public bool? tls_disable_session_tickets
		{
			get => _tls_disable_session_tickets;
			set => SetAndNotify(ref _tls_disable_session_tickets, value);
		}

		/// <summary>
		/// DoH: Use a specific cipher suite instead of the server preference
		/// 49199 = TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256
		/// 49195 = TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256
		/// 52392 = TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305
		/// 52393 = TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305
		///  4865 = TLS_AES_128_GCM_SHA256
		///  4867 = TLS_CHACHA20_POLY1305_SHA256
		///
		/// On non-Intel CPUs such as MIPS routers and ARM systems (Android, Raspberry Pi...),
		/// the following suite improves performance.
		/// This may also help on Intel CPUs running 32-bit operating systems.
		///
		/// Keep tls_cipher_suite empty if you have issues fetching sources or
		/// connecting to some DoH servers. Google and Cloudflare are fine with it.
		/// </summary>
		[TomlComment("# DoH: Use a specific cipher suite instead of the server preference", CommentLocation.Prepend)]
		[TomlComment("# 49199 = TLS_ECDHE_RSA_WITH_AES_128_GCM_SHA256", CommentLocation.Prepend)]
		[TomlComment("# 49195 = TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256", CommentLocation.Prepend)]
		[TomlComment("# 52392 = TLS_ECDHE_RSA_WITH_CHACHA20_POLY1305", CommentLocation.Prepend)]
		[TomlComment("# 52393 = TLS_ECDHE_ECDSA_WITH_CHACHA20_POLY1305", CommentLocation.Prepend)]
		[TomlComment("#  4865 = TLS_AES_128_GCM_SHA256", CommentLocation.Prepend)]
		[TomlComment("#  4867 = TLS_CHACHA20_POLY1305_SHA256", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# On non-Intel CPUs such as MIPS routers and ARM systems (Android, Raspberry Pi...),", CommentLocation.Prepend)]
		[TomlComment("# the following suite improves performance.", CommentLocation.Prepend)]
		[TomlComment("# This may also help on Intel CPUs running 32-bit operating systems.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# Keep tls_cipher_suite empty if you have issues fetching sources or", CommentLocation.Prepend)]
		[TomlComment("# connecting to some DoH servers. Google and Cloudflare are fine with it.", CommentLocation.Prepend)]
		public BindableCollection<int> tls_cipher_suite
		{
			get => _tls_cipher_suite;
			set => SetAndNotify(ref _tls_cipher_suite, value);
		}

		[Obsolete("2.0.38")]
		public string fallback_resolver
		{
			get => _fallback_resolver;
			set => SetAndNotify(ref _fallback_resolver, value);
		}

		/// <summary>
		/// Fallback resolvers
		/// These are normal, non-encrypted DNS resolvers, that will be only used
		/// for one-shot queries when retrieving the initial resolvers list, and
		/// only if the system DNS configuration doesn't work.
		/// No user application queries will ever be leaked through these resolvers,
		/// and they will not be used after IP addresses of resolvers URLs have been found.
		/// They will never be used if lists have already been cached, and if stamps
		/// don't include host names without IP addresses.
		/// They will not be used if the configured system DNS works.
		/// Resolvers supporting DNSSEC are recommended.
		/// 
		/// People in China may need to use 114.114.114.114:53 here.
		/// Other popular options include 8.8.8.8 and 1.1.1.1.
		/// 	
		/// If more than one resolver is specified, they will be tried in sequence.
		/// </summary>
		[TomlComment("# Fallback resolvers", CommentLocation.Prepend)]
		[TomlComment("# These are normal, non-encrypted DNS resolvers, that will be only used", CommentLocation.Prepend)]
		[TomlComment("# for one-shot queries when retrieving the initial resolvers list, and", CommentLocation.Prepend)]
		[TomlComment("# only if the system DNS configuration doesn't work.", CommentLocation.Prepend)]
		[TomlComment("# No user application queries will ever be leaked through these resolvers,", CommentLocation.Prepend)]
		[TomlComment("# and they will not be used after IP addresses of resolvers URLs have been found.", CommentLocation.Prepend)]
		[TomlComment("# They will never be used if lists have already been cached, and if stamps", CommentLocation.Prepend)]
		[TomlComment("# don't include host names without IP addresses.", CommentLocation.Prepend)]
		[TomlComment("# They will not be used if the configured system DNS works.", CommentLocation.Prepend)]
		[TomlComment("# Resolvers supporting DNSSEC are recommended.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# People in China may need to use 114.114.114.114:53 here.", CommentLocation.Prepend)]
		[TomlComment("# Other popular options include 8.8.8.8 and 1.1.1.1.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# If more than one resolver is specified, they will be tried in sequence.", CommentLocation.Prepend)]
		public BindableCollection<string> fallback_resolvers
		{
			get => _fallback_resolvers;
			set => SetAndNotify(ref _fallback_resolvers, value);
		}

		/// <summary>
		/// Always use the fallback resolver before the system DNS settings.
		/// </summary>
		[TomlComment("# Always use the fallback resolver before the system DNS settings.", CommentLocation.Prepend)]
		public bool ignore_system_dns
		{
			get => _ignore_system_dns;
			set => SetAndNotify(ref _ignore_system_dns, value);
		}

		/// <summary>
		/// Maximum time (in seconds) to wait for network connectivity before
		/// initializing the proxy.
		/// Useful if the proxy is automatically started at boot, and network
		/// connectivity is not guaranteed to be immediately available.
		/// Use 0 to not test for connectivity at all (not recommended),
		/// and -1 to wait as much as possible.
		/// </summary>
		[TomlComment("# Maximum time (in seconds) to wait for network connectivity before", CommentLocation.Prepend)]
		[TomlComment("# initializing the proxy.", CommentLocation.Prepend)]
		[TomlComment("# Useful if the proxy is automatically started at boot, and network", CommentLocation.Prepend)]
		[TomlComment("# connectivity is not guaranteed to be immediately available.", CommentLocation.Prepend)]
		[TomlComment("# Use 0 to not test for connectivity at all (not recommended),", CommentLocation.Prepend)]
		[TomlComment("# and -1 to wait as much as possible.", CommentLocation.Prepend)]
		public int netprobe_timeout
		{
			get => _netprobe_timeout;
			set => SetAndNotify(ref _netprobe_timeout, value);
		}

		/// <summary>
		/// Address and port to try initializing a connection to, just to check
		/// if the network is up. It can be any address and any port, even if
		/// there is nothing answering these on the other side. Just don't use
		/// a local address, as the goal is to check for Internet connectivity.
		/// On Windows, a datagram with a single, nul byte will be sent, only
		/// when the system starts.
		/// On other operating systems, the connection will be initialized
		/// but nothing will be sent at all.
		/// </summary>
		[TomlComment("# Address and port to try initializing a connection to, just to check", CommentLocation.Prepend)]
		[TomlComment("# if the network is up. It can be any address and any port, even if", CommentLocation.Prepend)]
		[TomlComment("# there is nothing answering these on the other side. Just don't use", CommentLocation.Prepend)]
		[TomlComment("# a local address, as the goal is to check for Internet connectivity.", CommentLocation.Prepend)]
		[TomlComment("# On Windows, a datagram with a single, nul byte will be sent, only", CommentLocation.Prepend)]
		[TomlComment("# when the system starts.", CommentLocation.Prepend)]
		[TomlComment("# On other operating systems, the connection will be initialized", CommentLocation.Prepend)]
		[TomlComment("# but nothing will be sent at all.", CommentLocation.Prepend)]
		public string netprobe_address
		{
			get => _netprobe_address;
			set => SetAndNotify(ref _netprobe_address, value);
		}

		/// <summary>
		/// Offline mode - Do not use any remote encrypted servers.
		/// The proxy will remain fully functional to respond to queries that
		/// plugins can handle directly (forwarding, cloaking, ...)
		/// </summary>
		[TomlComment("# Offline mode - Do not use any remote encrypted servers.", CommentLocation.Prepend)]
		[TomlComment("# The proxy will remain fully functional to respond to queries that", CommentLocation.Prepend)]
		[TomlComment("# plugins can handle directly (forwarding, cloaking, ...)", CommentLocation.Prepend)]
		public bool? offline_mode
		{
			get => _offline_mode;
			set => SetAndNotify(ref _offline_mode, value);
		}

		/// <summary>
		/// Additional data to attach to outgoing queries.
		/// These strings will be added as TXT records to queries.
		/// Do not use, except on servers explicitly asking for extra data
		/// to be present.
		/// encrypted-dns-server can be configured to use this for access control
		/// in the [access_control] section
		/// </summary>
		[TomlComment("# Additional data to attach to outgoing queries.", CommentLocation.Prepend)]
		[TomlComment("# These strings will be added as TXT records to queries.", CommentLocation.Prepend)]
		[TomlComment("# Do not use, except on servers explicitly asking for extra data", CommentLocation.Prepend)]
		[TomlComment("# to be present.", CommentLocation.Prepend)]
		[TomlComment("# encrypted-dns-server can be configured to use this for access control", CommentLocation.Prepend)]
		[TomlComment("#  in the [access_control] section", CommentLocation.Prepend)]
		public BindableCollection<string> query_meta
		{
			get => _query_meta;
			set => SetAndNotify(ref _query_meta, value);
		}

		/// <summary>
		/// Automatic log files rotation
		/// Maximum log files size in MB - Set to 0 for unlimited.
		/// </summary>
		[TomlComment("# Automatic log files rotation", CommentLocation.Prepend)]
		[TomlComment("Maximum log files size in MB - Set to 0 for unlimited.", CommentLocation.Prepend)]
		public int log_files_max_size
		{
			get => _log_files_max_size;
			set => SetAndNotify(ref _log_files_max_size, value);
		}

		/// <summary>
		/// How long to keep backup files, in days
		/// </summary>
		[TomlComment("How long to keep backup files, in days", CommentLocation.Prepend)]
		public int log_files_max_age
		{
			get => _log_files_max_age;
			set => SetAndNotify(ref _log_files_max_age, value);
		}

		/// <summary>
		/// Maximum log files backups to keep (or 0 to keep all backups)
		/// </summary>
		[TomlComment("Maximum log files backups to keep (or 0 to keep all backups)", CommentLocation.Prepend)]
		public int log_files_max_backups
		{
			get => _log_files_max_backups;
			set => SetAndNotify(ref _log_files_max_backups, value);
		}

		/// <summary>
		/// Immediately respond to IPv6-related queries with an empty response
		/// This makes things faster when there is no IPv6 connectivity, but can
		/// also cause reliability issues with some stub resolvers
		/// </summary>
		[TomlComment("# Immediately respond to IPv6-related queries with an empty response", CommentLocation.Prepend)]
		[TomlComment("# This makes things faster when there is no IPv6 connectivity, but can", CommentLocation.Prepend)]
		[TomlComment("# also cause reliability issues with some stub resolvers.", CommentLocation.Prepend)]
		public bool block_ipv6
		{
			get => _block_ipv6;
			set => SetAndNotify(ref _block_ipv6, value);
		}

		/// <summary>
		/// Immediately respond to A and AAAA queries for host names without a domain name
		/// </summary>
		[TomlComment("# Immediately respond to A and AAAA queries for host names without a domain name", CommentLocation.Prepend)]
		public bool block_unqualified
		{
			get => _block_unqualified;
			set => SetAndNotify(ref _block_unqualified, value);
		}

		/// <summary>
		/// Immediately respond to queries for local zones instead of leaking them to
		/// upstream resolvers (always causing errors or timeouts).
		/// </summary>
		[TomlComment("# Immediately respond to queries for local zones instead of leaking them to", CommentLocation.Prepend)]
		[TomlComment("# upstream resolvers (always causing errors or timeouts).", CommentLocation.Prepend)]
		public bool block_undelegated
		{
			get => _block_undelegated;
			set => SetAndNotify(ref _block_undelegated, value);
		}

		/// <summary>
		/// TTL for synthetic responses sent when a request has been blocked (due to IPv6 or blacklists).
		/// </summary>
		[TomlComment("# TTL for synthetic responses sent when a request has been blocked (due to", CommentLocation.Prepend)]
		[TomlComment("# IPv6 or blacklists).", CommentLocation.Prepend)]
		public int reject_ttl
		{
			get => _reject_ttl;
			set => SetAndNotify(ref _reject_ttl, value);
		}

		/// <summary>
		///     Forwarding rule file.
		/// </summary>
		[TomlComment("# Route queries for specific domains to a dedicated set of servers", CommentLocation.Prepend)]
		[TomlComment("# See the `example-forwarding-rules.txt` file for an example", CommentLocation.Prepend)]
		public string forwarding_rules
		{
			get => _forwarding_rules;
			set => SetAndNotify(ref _forwarding_rules, value);
		}

		/// <summary>
		///     Cloaking rule file.
		/// </summary>
		[TomlComment("# Cloaking returns a predefined address for a specific name.", CommentLocation.Prepend)]
		[TomlComment("# In addition to acting as a HOSTS file, it can also return the IP address", CommentLocation.Prepend)]
		[TomlComment("# of a different name. It will also do CNAME flattening.", CommentLocation.Prepend)]
		[TomlComment("#", CommentLocation.Prepend)]
		[TomlComment("# See the `example-cloaking-rules.txt` file for an example", CommentLocation.Prepend)]
		public string cloaking_rules
		{
			get => _cloaking_rules;
			set => SetAndNotify(ref _cloaking_rules, value);
		}

		/// <summary>
		/// TTL used when serving entries in cloaking-rules.txt
		/// </summary>
		[TomlComment("# TTL used when serving entries in cloaking-rules.txt", CommentLocation.Prepend)]
		public int? cloak_ttl
		{
			get => _cloak_ttl;
			set => SetAndNotify(ref _cloak_ttl, value);
		}

		/// <summary>
		///     Enable a DNS cache to reduce latency and outgoing traffic.
		/// </summary>
		[TomlComment("# Enable a DNS cache to reduce latency and outgoing traffic", CommentLocation.Prepend)]
		public bool cache
		{
			get => _cache;
			set => SetAndNotify(ref _cache, value);
		}

		/// <summary>
		///     Cache size.
		/// </summary>
		[TomlComment("# Cache size", CommentLocation.Prepend)]
		public int cache_size
		{
			get => _cache_size;
			set => SetAndNotify(ref _cache_size, value);
		}

		/// <summary>
		///     Minimum TTL for cached entries.
		/// </summary>
		[TomlComment("# Minimum TTL for cached entries", CommentLocation.Prepend)]
		public int cache_min_ttl
		{
			get => _cache_min_ttl;
			set => SetAndNotify(ref _cache_min_ttl, value);
		}

		/// <summary>
		///     Maxmimum TTL for cached entries.
		/// </summary>
		[TomlComment("# Maximum TTL for cached entries", CommentLocation.Prepend)]
		public int cache_max_ttl
		{
			get => _cache_max_ttl;
			set => SetAndNotify(ref _cache_max_ttl, value);
		}

		/// <summary>
		///     Minimum TTL for negatively cached entries.
		/// </summary>
		[TomlComment("# Minimum TTL for negatively cached entries", CommentLocation.Prepend)]
		public int cache_neg_min_ttl
		{
			get => _cache_neg_min_ttl;
			set => SetAndNotify(ref _cache_neg_min_ttl, value);
		}

		/// <summary>
		///     Maximum TTL for negatively cached entries
		/// </summary>
		[TomlComment("# Maximum TTL for negatively cached entries", CommentLocation.Prepend)]
		public int cache_neg_max_ttl
		{
			get => _cache_neg_max_ttl;
			set => SetAndNotify(ref _cache_neg_max_ttl, value);
		}


		/// <summary>
		/// Local DoH server
		/// </summary>
		[TomlComment("# Local DoH server", CommentLocation.Prepend)]
		[TomlComment("# dnscrypt-proxy can act as a local DoH server. By doing so, web browsers", CommentLocation.Prepend)]
		[TomlComment("# requiring a direct connection to a DoH server in order to enable some", CommentLocation.Prepend)]
		[TomlComment("# features will enable these, without bypassing your DNS proxy.", CommentLocation.Prepend)]
		public LocalDoh local_doh { get; set; }

		/// <summary>
		/// Log client queries to a file.
		/// </summary>
		[TomlComment("# Query logging", CommentLocation.Prepend)]
		[TomlComment("# Log client queries to a file", CommentLocation.Prepend)]
		public QueryLog query_log { get; set; }

		/// <summary>
		/// Log queries for nonexistent zones.
		/// </summary>
		[TomlComment("# Suspicious queries logging", CommentLocation.Prepend)]
		[TomlComment("# Log queries for nonexistent zones", CommentLocation.Prepend)]
		[TomlComment("# These queries can reveal the presence of malware, broken/obsolete applications,", CommentLocation.Prepend)]
		[TomlComment("# and devices signaling their presence to 3rd parties.", CommentLocation.Prepend)]
		public NxLog nx_log { get; set; }

		/// <summary>
		/// Pattern-based blocking (blacklists)
		/// </summary>
		[TomlComment("# Pattern-based blocking (blacklists)", CommentLocation.Prepend)]
		[TomlComment("# Blacklists are made of one pattern per line. Example of valid patterns:", CommentLocation.Prepend)]
		[TomlComment("#      example.com", CommentLocation.Prepend)]
		[TomlComment("#      =example.com", CommentLocation.Prepend)]
		[TomlComment("#      *sex*", CommentLocation.Prepend)]
		[TomlComment("#      ads.*", CommentLocation.Prepend)]
		[TomlComment("#      ads*.example.*", CommentLocation.Prepend)]
		[TomlComment("#      ads*.example[0-9]*.com", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# Example blacklist files can be found at https://download.dnscrypt.info/blacklists/", CommentLocation.Prepend)]
		[TomlComment("# A script to build blacklists from public feeds can be found in the", CommentLocation.Prepend)]
		[TomlComment("# `utils/generate-domains-blacklists` directory of the dnscrypt-proxy source code.", CommentLocation.Prepend)]
		public Blacklist blacklist { get; set; }

		/// <summary>
		/// Pattern-based IP blocking (IP blacklists)
		/// </summary>
		[TomlComment("# Pattern-based IP blocking (IP blacklists)", CommentLocation.Prepend)]
		[TomlComment("# IP blacklists are made of one pattern per line. Example of valid patterns:", CommentLocation.Prepend)]
		[TomlComment("#", CommentLocation.Prepend)]
		[TomlComment("#      127.*", CommentLocation.Prepend)]
		[TomlComment("#      fe80:abcd:*", CommentLocation.Prepend)]
		[TomlComment("#      192.168.1.4", CommentLocation.Prepend)]
		public Blacklist ip_blacklist { get; set; }

		/// <summary>
		/// Pattern-based whitelisting (blacklists bypass)
		/// </summary>
		[TomlComment("# Pattern-based whitelisting (blacklists bypass)", CommentLocation.Prepend)]
		[TomlComment("# Whitelists support the same patterns as blacklists", CommentLocation.Prepend)]
		[TomlComment("# If a name matches a whitelist entry, the corresponding session", CommentLocation.Prepend)]
		[TomlComment("# will bypass names and IP filters.", CommentLocation.Prepend)]
		[TomlComment("", CommentLocation.Prepend)]
		[TomlComment("# Time-based rules are also supported to make some websites only accessible at specific times of the day.", CommentLocation.Prepend)]
		public Whitelist whitelist { get; set; }

		/// <summary>
		/// TLS Client Authentication
		/// </summary>
		[TomlComment("# This is only useful if you are operating your own, private DoH server(s).", CommentLocation.Prepend)]
		[TomlComment("# (for DNSCrypt, see the `query_meta` feature instead)", CommentLocation.Prepend)]
		public TlsClientAuth tls_client_auth { get; set; }

		public AnonymizedDns anonymized_dns { get; set; }

		/// <summary>
		/// Servers with known bugs 
		/// </summary>
		[TomlComment("# Cisco servers currently cannot handle queries larger than 1472 bytes, and don't", CommentLocation.Prepend)]
		[TomlComment("# truncate reponses larger than questions as expected by the DNSCrypt protocol.", CommentLocation.Prepend)]
		[TomlComment("# This prevents large responses from being received over UDP and over relays.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# The `dnsdist` server software drops client queries larger than 1500 bytes.", CommentLocation.Prepend)]
		[TomlComment("# They are aware of it and are working on a fix.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		public BrokenImplementations broken_implementations { get; set; }

		/// <summary>
		/// Time access restrictions 
		/// </summary>
		[TomlComment("# One or more weekly schedules can be defined here.", CommentLocation.Prepend)]
		[TomlComment("# Patterns in the name-based blocklist can optionally be followed with @schedule_name", CommentLocation.Prepend)]
		[TomlComment("# to apply the pattern 'schedule_name' only when it matches a time range of that schedule.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# For example, the following rule in a blacklist file:", CommentLocation.Prepend)]
		[TomlComment("# *.youtube.* @time-to-sleep", CommentLocation.Prepend)]
		[TomlComment("# would block access to YouTube during the times defined by the 'time-to-sleep' schedule.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# {after='21:00', before= '7:00'} matches 0:00-7:00 and 21:00-0:00", CommentLocation.Prepend)]
		[TomlComment("# {after= '9:00', before='18:00'} matches 9:00-18:00", CommentLocation.Prepend)]
		public Dictionary<string, Schedule> schedules
		{
			get => _schedules;
			set => SetAndNotify(ref _schedules, value);
		}

		/// <summary>
		/// Servers
		/// </summary>
		[TomlComment("# Remote lists of available servers", CommentLocation.Prepend)]
		[TomlComment("# Multiple sources can be used simultaneously, but every source", CommentLocation.Prepend)]
		[TomlComment("# requires a dedicated cache file.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# Refer to the documentation for URLs of public sources.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# A prefix can be prepended to server names in order to", CommentLocation.Prepend)]
		[TomlComment("# avoid collisions if different sources share the same for", CommentLocation.Prepend)]
		[TomlComment("# different servers. In that case, names listed in `server_names`", CommentLocation.Prepend)]
		[TomlComment("# must include the prefixes.", CommentLocation.Prepend)]
		[TomlComment("# ", CommentLocation.Prepend)]
		[TomlComment("# If the `urls` property is missing, cache files and valid signatures", CommentLocation.Prepend)]
		[TomlComment("# must already be present. This doesn't prevent these cache files from", CommentLocation.Prepend)]
		[TomlComment("# expiring after `refresh_delay` hours.", CommentLocation.Prepend)]
		public Dictionary<string, Source> sources
		{
			get => _sources;
			set => SetAndNotify(ref _sources, value);
		}

		[TomlComment("# Optional, local, static list of additional servers", CommentLocation.Prepend)]
		[TomlComment("# Mostly useful for testing your own servers.", CommentLocation.Prepend)]
		[TomlMember(Key = "static")]
		public Dictionary<string, Static> Static
		{
			get => _static;
			set => SetAndNotify(ref _static, value);
		}
	}
}
