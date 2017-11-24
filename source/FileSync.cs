using System;
using System.IO;

namespace PlaylistSync
{
    class FileSync
    {
        public FileSync( string[] args )
        {
            if ( args[0].Equals( "-copy" ) )
            {
                Console.Write( "Enter the source playlist path : " );
                var source = Console.ReadLine();

                if ( source.Equals( string.Empty ) )
                {
                    source = @"C:\Users\Saurabh S\Music\Gorillaz";
                    Console.WriteLine( source );
                }

                Console.Write( "Enter the destination path : " );
                var destination = Console.ReadLine();

                if ( destination.Equals( string.Empty ) )
                {
                    destination = @"D:\Dev\PlaylistSync\bin\foo";
                    Console.WriteLine( destination );
                }

                Console.WriteLine( $"Total No. of files present in the folder is {GetFilesCount( source )}" );

                if ( !Directory.Exists( source ) )
                {
                    throw new Exception( "Source directory does not exist." );
                }

                Copy( source, destination, GetFilesCount( source ) );
            }
        }

        private void ReadPlaylist( string playlistPath )
        {
        }

        private string GetLastFolderName( string path )
        {
            var ss = path.Split( '\\' );
            return ss[ss.Length - 1];
        }

        private int GetFilesCount( string source )
        {
            return Directory.GetFiles( source, "*", SearchOption.AllDirectories ).Length;
        }

        private void CopyFiles( string source, string destination )
        {
            if ( !Directory.Exists( destination ) )
                Directory.CreateDirectory( destination );

            var files = Directory.GetFiles( source );
            if ( files.Length > 0 )
            {
                foreach ( var file in files )
                {
                    var destPath = Path.Combine( destination, Path.GetFileName( file ) );
                    if ( File.Exists( destPath ) )
                        continue;

                    Print( $"Copying {file}..." );
                    File.Copy( file, destPath );
                }
            }
        }

        public void Copy( string source, string destination, int filesCount = 0 )
        {
            CopyFiles( source, destination );

            var folders = Directory.GetDirectories( source );

            //check for subfolders within the current folder
            foreach ( var sourcefolder in folders )
            {
                var destFolder = Path.Combine( destination, GetLastFolderName( sourcefolder ) );
                CopyFiles( sourcefolder, destFolder );

                //move into subfolder
                Copy( sourcefolder, destFolder );
            }
        }

        public void Clear( string destination )
        {
        }

        public void ClearAll( string destination )
        {
        }

        void Print( string value )
        {
            Console.WriteLine( value );
        }
    }
}