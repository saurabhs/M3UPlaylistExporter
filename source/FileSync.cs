using System;
using System.IO;

namespace PlaylistSync
{
    class FileSync
    {
        public FileSync( string[] args )
        {
            //if ( args[0].Equals( "-copy" ) )
            {
                Console.Write( "Enter the source playlist path : " );
                var source = ""; //Console.ReadLine();

                if ( source.Equals( string.Empty ) )
                {
                    source = @"C:\Users\Saurabh S\Music\James Blunt";
                    Console.WriteLine( source );
                }

                Console.Write( "Enter the destination path : " );
                var destination = ""; //Console.ReadLine();

                if ( destination.Equals( string.Empty ) )
                {
                    destination = @"D:\Dev\PlaylistSync\bin\Music";
                    Console.Write( destination + "\n\n" );
                }

                Console.Write( "Enter the playlist fullpath : " );
                var playlistPath = string.Empty; //Console.ReadLine();

                if ( playlistPath.Equals( string.Empty ) )
                {
                    playlistPath = @"C:\Users\Saurabh S\Music\foobar2000";
                    Console.Write( playlistPath + "\n\n" );
                }

                Console.WriteLine( $"Total No. of files present in the folder is {GetFilesCount( source )}" );

                if ( !Directory.Exists( source ) )
                {
                    throw new Exception( "Source directory does not exist." );
                }

                foreach ( var file in Directory.GetFiles( playlistPath, "*.m3u" ) )
                {
                    CopyFiles( Path.GetFileNameWithoutExtension( file ), GetListOfFilesToCopy( file ), destination );
                }
            }
        }

        private string[] GetListOfFilesToCopy( string playlistPath )
        {
            if ( !File.Exists( playlistPath ) )
                throw new Exception( "Playlist file does not exist." );

            return File.ReadAllText( playlistPath ).Split( new[] { '\r', '\n' } );
        }

        private string GetLastFolderName( string path )
        {
            var ss = path.Split( '\\' );
            return ss[ss.Length - 1];
        }

        private int GetFilesCount( string source )
        {
            return Directory.GetFiles( source, "*", SearchOption.TopDirectoryOnly ).Length;
        }

        public void PerformCopy( string source, string destination, int filesCount = 0 )
        {
            var destPath = Path.Combine( destination, GetLastFolderName( source ) );
            CopyFiles( source, destPath );

            //check for subfolders within the current folder
            foreach ( var sourcefolder in Directory.GetDirectories( source ) )
            {
                //move into subfolder
                PerformCopy( sourcefolder, destPath );
            }
        }

        private void CopyFiles( string source, string destination )
        {
            var files = Directory.GetFiles( source, "*.mp3" );
            if ( files.Length > 0 )
            {
                if ( !Directory.Exists( destination ) )
                    Directory.CreateDirectory( destination );

                foreach ( var file in files )
                {
                    var destPath = Path.Combine( destination, Path.GetFileName( file ) );
                    if ( File.Exists( destPath ) )
                        continue;

                    Print( $"Copying {destPath}..." );
                    File.Copy( file, destPath );
                }
            }
        }

        private void CopyFiles( string playlistName, string[] source, string destination )
        {
            var path = Path.Combine( destination, playlistName );
            if ( !Directory.Exists( path ) )
                Directory.CreateDirectory( path );

            foreach ( var src in source )
            {
                if ( src.Equals( string.Empty ) )
                    continue;

                var filename = Path.GetFileName( src );
                var dest = Path.Combine( path, filename );

                if ( !File.Exists( src ) || File.Exists( dest ) )
                    continue;

                Console.WriteLine( $"Copying {src}..." );
                File.Copy( src, dest );
            }
        }

        public void Clear( string destination )
        {
        }

        public void ClearAll( string destination )
        {
        }

        private void Print( string value )
        {
            Console.WriteLine( value );
        }
    }
}