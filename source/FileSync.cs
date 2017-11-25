using System;
using System.IO;

namespace PlaylistSync
{
    class FileSync
    {
        public FileSync( string[] args )
        {
            if ( args == null || args.Length == 0 )
            {
                PrintListOfCommads();
            }
            if ( args[0].Equals( "-copy" ) )
            {
                if ( args.Length >= 3 )
                    CopyCommand( args[1], args[2] );
            }
            if ( args[0].Equals( "-plcopy" ) )
            {
                if ( args.Length >= 3 )
                    PlaylistCopyCommad( args[1], args[2] );
            }
            if ( args[0].Equals( "-help" ) )
            {
                PrintListOfCommads();
            }
        }

        private void PrintListOfCommads()
        {
            Console.WriteLine();
            Console.WriteLine( "-copy [SOURCE] [DESTINATION]\ncopies a source folder to a specified destination folder\n" );
            Console.WriteLine( "-plclean [PLAYLIST] [DESTINATION]\nread a .m3u files and copy the diff to specified destination folder\n" );
            Console.WriteLine( "-help\nthis help\n" );
        }

        private void CopyCommand( string source, string destination )
        {
            Console.WriteLine( "Performs a normal copy funcation, you can use the Windows copy instead." );

            if ( !Directory.Exists( source ) )
            {
                throw new Exception( "Source directory does not exist." );
            }

            PerformCopy( source, destination );
        }

        private void PlaylistCopyCommad( string playlistPath, string destination )
        {
            Console.WriteLine( "playlist " + playlistPath );
            Console.WriteLine( "destination " + destination );

            if ( !Directory.Exists( playlistPath ) )
            {
                throw new Exception( "Invalid path to playlist directory." );
            }

            CopyFilesFromM3UPlaylists( playlistPath, destination );
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

        public void PerformCopy( string source, string destination )
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

                    Console.WriteLine( $"Copying {destPath}" );
                    File.Copy( file, destPath );
                }
            }
        }

        private void CopyFilesFromM3UPlaylists( string playlistPath, string destination )
        {
            foreach ( var file in Directory.GetFiles( playlistPath, "*.m3u" ) )
            {
                CopyFiles( Path.GetFileNameWithoutExtension( file ), GetListOfFilesToCopy( file ), destination );
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

                var dest = Path.Combine( path, Path.GetFileName( src ) );

                if ( !File.Exists( src ) || File.Exists( dest ) )
                    continue;

                Console.WriteLine( $"Copying {src}" );
                File.Copy( src, dest );
            }
        }
    }
}