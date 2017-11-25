# M3UPlaylistExporter
Reads your *.m3u playlist and export songs/audio to some other folder, created this utility to export songs from *.m3u playlists exported by foobar2000. Plan was to directly copy the songs to the smartphone but for some reason running <code>DriveInfo.GetDrives()</code> won't show my OnePlus One storage.

# Usage
<code>-copy [SOURCE] [DESTIANTION]</code> Performs a normal copy funcation, you can use the Windows copy instead.

<code>-plcopy [PLAYLIST_FOLDER] [DESTIANTION]</code> Finds all the *.m3u files in the folder and copy all (newly added) files to specified destination folder

# Compilation
Using Visual Studio 2017 Communtity Edition to compile.
