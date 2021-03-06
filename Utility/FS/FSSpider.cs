﻿using System;
using System.Collections.Generic;
using System.IO;
#if DEBUG
using System.Diagnostics;
#endif

namespace Utility.FS {
	public static class FSSpider {
		private static Loggers.Logger staticClassLogger = new Loggers.Logger( "FSSpider" );

		public static List<string> GetAllSubFiles( string path, string[] ignoreList, bool verbose = false, string extLookFor = "*" ) {
			List<string> files = new List<string>();
			staticClassLogger.Verbose = verbose;

			foreach(FileSystemInfo fsObject in new DirectoryInfo( path ).EnumerateFileSystemInfos() ) {
				bool skip = false;

				foreach(string str in ignoreList ) {
					if ( fsObject.Name.Contains( str ) ) {
						if ( verbose ) {
							staticClassLogger.WriteLineToLog( $"'{fsObject.FullName}' contains '{str}' which is on the ignore list." );
							#if DEBUG
							Debug.WriteLine( $"'{fsObject.FullName}' contains '{str}' which is on the ignore list." );
							#endif
						}
						skip = true;
						break;
					}
				}

				if ( !skip ) {
					if ( fsObject.Attributes.HasFlag( FileAttributes.Directory ) ) {
						if ( verbose ) {
							staticClassLogger.WriteLineToLog( $"Found directory {fsObject.FullName}." );
							#if DEBUG
							Debug.WriteLine( $"Found directory {fsObject.FullName}." );
							#endif
						}
						try {
							if ( verbose ) {
								staticClassLogger.WriteLineToLog( $"Looking into sub directory '{fsObject.FullName}'." );
								#if DEBUG
								Debug.WriteLine( $"Looking into sub directory '{fsObject.FullName}'." );
								#endif
							}
							files.AddRange( GetAllSubFiles( fsObject.FullName, ignoreList, verbose, extLookFor ) );
						} catch (Exception e) {
							if ( verbose ) {
								staticClassLogger.WriteLineToLog( $"An error occuered: {e.Message}" );
								#if DEBUG
								Debug.WriteLine( $"An error occuered: {e.Message}" );
								#endif
							}
						}
					} else {
						if ( verbose ) {
							staticClassLogger.WriteLineToLog( $"Found file '{fsObject.FullName}'" );
							#if DEBUG
							Debug.WriteLine( $"Found file '{fsObject.FullName}'" );
							#endif
						}
						if ( extLookFor == "*" || fsObject.Extension.Equals( extLookFor ) ) {
							if ( verbose ) {
								staticClassLogger.WriteLineToLog( $"Adding file '{fsObject.FullName}' to the list." );
								#if DEBUG
								Debug.WriteLine( $"Adding file '{fsObject.FullName}' to the list." );
								#endif
							}
							files.Add( fsObject.FullName );
						}
					}
				}
			}

			return files;
		}
	}
}
