using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Speech;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Threading;

namespace MusicPlayer
{
    class Program
    {
        public enum Moods
        {
            HAPPY,
            MELANCHOLY,
            INSPIRED,
            CALM,
            EXIT
        }
        


        

        static void Main(string[] args)
        {

            TheCoreProgram();       
            
        }
        


        //Main Application methods

        /// <summary>
        /// Contains the menuing system
        /// </summary>
        static void TheCoreProgram()
        {
                Moods userMood;
                bool exiting = false;

            do
            {
                userMood = DisplayGetMood();
                if (userMood != Moods.EXIT)
                {
                    GetPlaylist(userMood);
                }
                else
                {
                    exiting = true;
                }
                        
               
            } while (!exiting);
                
        }

        /// <summary>
        /// Displays the home screen to user 
        /// </summary>
        /// <returns></returns>
        static Moods DisplayGetMood()
        {
            Moods userMood;

            DisplayHeader(" Welcome to the Mood Radio.");

            Console.WriteLine("\t\t\tSelect a mood from the list and we'll make you a soundtrack.\n\n");
            Console.WriteLine("\t\t\t\t\t   Melancholy | Happy");
            Console.WriteLine("\t\t\t\t\t   Inspired   | Calm");
            Console.WriteLine("\t\t\t\t\t   To exit enter: Exit");
            Console.Write("\n\t\t\t\t\t   ");

            while (!Enum.TryParse<Moods>(Console.ReadLine().ToUpper(), out userMood))
            {
                DisplayHeader(" Welcome to the Mood Radio.");

                Console.WriteLine("\t\t\tSelect a mood from the list and we'll make you a soundtrack.\n\n");
                Console.WriteLine("\t\t\t\t\t   Melancholy | Happy");
                Console.WriteLine("\t\t\t\t\t   Inspired   | Calm");
                Console.WriteLine("\t\t\t\t\t   To exit enter: Exit");
                Console.Write("\n\t\t\t\t\t   ");  
            }
            return userMood;
        }

            /// <summary>
        /// Evaluates which playlist should be played according to the user's input
        /// </summary>
        /// <param name="userMood"></param>
            static void GetPlaylist(Moods userMood)
        {
            // instanciate Lists
            List<SoundPlayer> songlist = new List<SoundPlayer>();
            List<Discography> SonglistInfo = new List<Discography>();
            List<String> title = new List<String>();
            List<String> album = new List<String>();
            List<String> artist  = new List<String>();
            List<int> year = new List<int>();

            //Loading of data
            songlist = ListOfSongs(songlist);

            SonglistInfo = InitalizeSongInformation(SonglistInfo);

            title = SongTitleInformation(SonglistInfo, title);

            album = SongAlbumTitleInformation(SonglistInfo, album);

            artist = SongArtistInformation(SonglistInfo, artist);

            year = AlbumYearInformation(SonglistInfo, year);


            switch (userMood)
            {
                case Moods.HAPPY:
                    PlayHappy(userMood,songlist, title, album, artist, year, SonglistInfo);
                    break;
                case Moods.MELANCHOLY:
                    PlayMelancholy(userMood,songlist, title, album, artist, year, SonglistInfo);
                    break;
                case Moods.INSPIRED:
                    PlayInspired(userMood,songlist, title, album, artist, year, SonglistInfo);
                    break;
                case Moods.CALM:
                    PlayCalm(userMood,songlist, title, album, artist, year, SonglistInfo);
                    break;
                case Moods.EXIT:
                    break;
            }

        }

                /// <summary>
                /// Plays the Melancholy Playlist for user
                /// </summary>
                /// <param name="userMood"></param>
                /// <param name="songlist"></param>
                static void PlayMelancholy(Moods userMood, List<SoundPlayer> songlist,List<String> title, List<String> album, List<String> artist, List<int> year, List<Discography> songlistInfo)
        {
            int standardLimit = 0;
            int songDirection = 0;
            int songChangeLimit1 = 2;
            int songChangeLimit2 = 4;
            bool exiting = false;
            int index;

            

           

            do
            {
                while (songDirection == standardLimit || songDirection == 1)
                {
                    index = 3;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection = UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 1:
                            songDirection = PlaylistLoopToLast(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            break;
                    }
                }

                Console.Clear();

                while (songDirection == standardLimit || songDirection == songChangeLimit1)
                {
                    index = 4;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection += UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 12:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }

                Console.Clear();

                while (songDirection == standardLimit || songDirection == songChangeLimit2)
                {
                    index = 5;
                    songDirection = 0;

                    songlist[index].Play();

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songDirection = UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 2:
                            songDirection = PlaylistLoopToFirst(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }


            } while (standardLimit == 0 && !exiting);

        }

                /// <summary>
                /// Plays the Happy playlist for user
                /// </summary>
                /// <param name="userMood"></param>
                /// <param name="songlist"></param>
                static void PlayHappy(Moods userMood, List<SoundPlayer> songlist, List<String> title, List<String> album, List<String> artist, List<int> year, List<Discography> songlistInfo)
        {
            int standardLimit = 0;
            int songDirection = 0;
            int songChangeLimit1 = 2;
            int songChangeLimit2 = 4;
            bool exiting = false;
            int index = 0;

            

            

            

            do
            {
                while (songDirection == standardLimit || songDirection == 1)
                {
                    index = 0;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection = UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 1:
                            songDirection = PlaylistLoopToLast(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            break;
                    }

                }

                while (songDirection == standardLimit || songDirection == songChangeLimit1)
                {
                    index = 1;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection += UserSongChange(userMood,title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 12:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }

                while (songDirection == standardLimit || songDirection == songChangeLimit2)
                {
                    index = 2;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();


                    songDirection = 0;

                    songlist[index].Play();

                    songDirection = UserSongChange(userMood,title, album, artist, year, songlistInfo, index);
                    
                    switch (songDirection)
                    {
                        case 2:
                            songDirection = PlaylistLoopToFirst(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }


            } while (standardLimit == 0 && !exiting);

        }

                /// <summary>
                /// Plays the Inspired playlist for user
                /// </summary>
                /// <param name="userMood"></param>
                /// <param name="songlist"></param>
                static void PlayInspired(Moods userMood, List<SoundPlayer> songlist, List<String> title, List<String> album, List<String> artist, List<int> year, List<Discography> songlistInfo)
        {
            int standardLimit = 0;
            int songDirection = 0;
            int songChangeLimit1 = 2;
            int songChangeLimit2 = 4;
            bool exiting = false;
            int index = 0;
            

            do
            {

                while (songDirection == standardLimit || songDirection == 1)
                {
                    index = 6;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();
                    

                    songDirection = UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                   
                    switch (songDirection)
                    {
                        case 1:
                            songDirection = PlaylistLoopToLast(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            break;
                    }

                }

                while (songDirection == standardLimit || songDirection == songChangeLimit1)
                {
                    index = 7;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection += UserSongChange(userMood,title, album, artist, year, songlistInfo, index);
                    
                    switch (songDirection)
                    {
                        case 12:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }

                while (songDirection == standardLimit || songDirection == songChangeLimit2)
                {
                    songDirection = 0;
                    index = 8;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection = UserSongChange(userMood,title, album, artist, year, songlistInfo, index);
                    
                    switch (songDirection)
                    {
                        case 2:
                            songDirection = PlaylistLoopToFirst(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }


            } while (standardLimit == 0 && !exiting);
        }

                /// <summary>
                /// Plays the Calm Playlist for user
                /// </summary>
                /// <param name="userMood"></param>
                /// <param name="songlist"></param>
                static void PlayCalm(Moods userMood, List<SoundPlayer> songlist, List<String> title, List<String> album, List<String> artist, List<int> year, List<Discography> songlistInfo)
        {
            int standardLimit = 0;
            int songDirection = 0;
            int songChangeLimit1 = 2;
            int songChangeLimit2 = 4;
            bool exiting = false;
            int index = 0;

            

            do
            {
                while (songDirection == standardLimit || songDirection == 1)
                {
                    index = 9;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection = UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 1:
                            songDirection = PlaylistLoopToLast(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            break;
                    }

                }

                while (songDirection == standardLimit || songDirection == songChangeLimit1)
                {
                    index = 10;

                    DisplayHeader($"Now Playing: {userMood} Playlist");
                    DisplayDiscography(songlistInfo, title, album, artist, year, index);
                    DisplayUserDirections();

                    songlist[index].Play();

                    songDirection += UserSongChange(userMood, title, album, artist, year, songlistInfo, index);
                    
                   

                    switch (songDirection)
                    {
                        case 12:
                            exiting = true;
                            break;
                        default:
                        songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }

                }

                while (songDirection == standardLimit || songDirection == songChangeLimit2)
                    {
                            songDirection = 0;
                            index = 11;

                            DisplayHeader($"Now Playing: {userMood} Playlist");
                            DisplayDiscography(songlistInfo, title, album, artist, year, index);
                            DisplayUserDirections();

                    songlist[index].Play();

                            songDirection = UserSongChange(userMood, title, album, artist, year, songlistInfo, index);

                    switch (songDirection)
                    {
                        case 2:
                            songDirection = PlaylistLoopToFirst(songDirection);
                            break;
                        case 10:
                            exiting = true;
                            break;
                        default:
                            songDirection = BackwardLoopConversion(songDirection);
                            break;
                    }
                }
                    
                
            } while (standardLimit == 0 && !exiting);
            
        }
        

 
        // conversion Methods

        /// <summary>
        /// Contains the conversions for the user's choice
        /// </summary>
        /// <param name="userMood"></param>
        /// <returns>int</returns>
        static int UserSongChange(Moods userMood, List<String> title, List<String> album, List<String> artist, List<int> year, List<Discography> songlistInfo, int index)

        {
            int userChoice;

            int songDirection = 0;

            int backwards = 1;

            int forward = 2;

             userChoice = RetriveUserChoice(userMood,title,album,artist,year,songlistInfo,index);

                if (userChoice == 1)
                {
                    songDirection = backwards;
                }
                else if (userChoice == 2)
                {
                    songDirection = forward;
                }
                else if (userChoice == 10)
                {
                    songDirection = 10;
                }
            

            return songDirection;
        }

            /// <summary>
        /// Retrives user's input for song change
        /// </summary>
        /// <param name="userMood"></param>
        /// <returns>int</returns>
            static int RetriveUserChoice (Moods userMood, List<String> title, List<String> album, List<String> artist, List<int> year, List<Discography> songlistInfo, int index)
        {
            int userChoice;

            while (!int.TryParse(Console.ReadLine(), out userChoice))
            {
                Console.Clear();
                DisplayHeader($"Now Playing: {userMood} Playlist");
                DisplayDiscography(songlistInfo, title, album, artist, year, index);
                DisplayUserDirections();
            }

            return userChoice;
        }

        /// <summary>
        /// Allows the first song to access the last song of the playlist
        /// </summary>
        /// <param name="songDirection"></param>
        /// <returns>int</returns>
        static int PlaylistLoopToLast(int songDirection)
        {
            if (songDirection == 1)
            {
                songDirection = songDirection + 3;
                
            }
            else
            {
                
            }

            return songDirection;
        }

        /// <summary>
        /// Allows the last song to access the first song of the playlist
        /// </summary>
        /// <param name="songDirection"></param>
        /// <returns>int</returns>
        static int PlaylistLoopToFirst(int songDirection)
        {
            if (songDirection == 2)
            {
                songDirection = 1;
            }
            else
            {

            }

            return songDirection;
        }

        /// <summary>
        /// Converts users choice into a correct choice for pervious song's parameter
        /// </summary>
        /// <param name="songDirection"></param>
        /// <returns>int</returns>
        static int BackwardLoopConversion(int songDirection)
        {
            if (songDirection == 3)
            {
                songDirection = 1;

            }
            else if (songDirection == 1)
            {
                songDirection = 2;
            }
            else
            {
                    
            }

            return songDirection;
        }



        // Styling Methods

        /// <summary>
        /// Displays the playlist controls for the user
        /// </summary>
        static void DisplayUserDirections ()
        {
            Console.WriteLine("\n\n\t\t\t\t   0. Restart Song | 1. Back | 2. Forward");
        }

        /// <summary>
        /// display header
        /// </summary>
        static void DisplayHeader(string headerTitle)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t       " + headerTitle);
            Console.WriteLine();

          
        }

        /// <summary>
        /// Displays the song information to user
        /// </summary>
        /// <param name="songlistInfo"></param>
        /// <param name="title"></param>
        /// <param name="album"></param>
        /// <param name="artist"></param>
        /// <param name="year"></param>
        static void DisplayDiscography(List<Discography> songlistInfo, List<String> title, List<String> album, List<String> artist, List<int> year, int index)
        {
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("\t Song:   " + songlistInfo[index].Title);
                
                Console.WriteLine("\t Album:  " + songlistInfo[index].Album);
                
                Console.WriteLine("\t Artist: " + songlistInfo[index].Artist);
                
                Console.WriteLine("\t Year:   " + songlistInfo[index].Year);
            }

        }

       

        //Storing Methods

        /// <summary>
        /// List of all the songs in the music playlist
        /// </summary>
        /// <param name="songlist"></param>
        /// <returns>List<SoundPlayer></returns>
        static List<SoundPlayer> ListOfSongs (List<SoundPlayer> songlist)
        {
            SoundPlayer colors = new SoundPlayer(@"Media\Colors.wav");
            SoundPlayer celeste = new SoundPlayer(@"Media\Celeste.wav");
            SoundPlayer newYearsDay = new SoundPlayer(@"Media\NewYearsDay.wav");
            SoundPlayer autumnLeaves = new SoundPlayer(@"Media\AutumnLeaves.wav");
            SoundPlayer everglow = new SoundPlayer(@"Media\Everglow(RadioEdit).wav");
            SoundPlayer twentysix = new SoundPlayer(@"Media\26.wav");
            SoundPlayer kingsAndQueens = new SoundPlayer(@"Media\Kings and Queens.wav");
            SoundPlayer paradise = new SoundPlayer(@"Media\Paradise.wav");
            SoundPlayer superheroes = new SoundPlayer(@"Media\Superheroes.wav");
            SoundPlayer better = new SoundPlayer(@"Media\Better.wav");
            SoundPlayer warmWhispers = new SoundPlayer(@"Media\Warm Whispers.wav");
            SoundPlayer fireside = new SoundPlayer(@"Media\Fireside.wav");




            songlist.Add(colors);
            songlist.Add(celeste);
            songlist.Add(newYearsDay);
            songlist.Add(autumnLeaves);
            songlist.Add(everglow);
            songlist.Add(twentysix);
            songlist.Add(kingsAndQueens);
            songlist.Add(paradise);
            songlist.Add(superheroes);
            songlist.Add(better);
            songlist.Add(warmWhispers);
            songlist.Add(fireside);


            return songlist;
        }

        /// <summary>
        /// Initializes Discography for each song.
        /// </summary>
        /// <param name="songlistInfo"></param>
        /// <returns>List<String></returns>
        static List<Discography> InitalizeSongInformation(List<Discography> songlistInfo)
        {
            Discography colors = new Discography();
            Discography celeste = new Discography();
            Discography newYearsDay = new Discography();
            Discography autumnLeaves = new Discography();
            Discography everglow = new Discography();
            Discography twentysix = new Discography();
            Discography kingsAndQueens = new Discography();
            Discography paradise = new Discography();
            Discography superheroes = new Discography();
            Discography better = new Discography();
            Discography warmWhispers = new Discography();
            Discography fireside = new Discography();

            songlistInfo.Add(colors);
            songlistInfo.Add(celeste);
            songlistInfo.Add(newYearsDay);
            songlistInfo.Add(autumnLeaves);
            songlistInfo.Add(everglow);
            songlistInfo.Add(twentysix);
            songlistInfo.Add(kingsAndQueens);
            songlistInfo.Add(paradise);
            songlistInfo.Add(superheroes);
            songlistInfo.Add(better);
            songlistInfo.Add(warmWhispers);
            songlistInfo.Add(fireside);

            return songlistInfo;
        }

            /// <summary>
        /// Holds the data for each song's title
        /// </summary>
        /// <param name="songlistInfo"></param>
        /// <param name="title"></param>
        /// <returns>List<string></returns>
            static List<String> SongTitleInformation(List<Discography> songlistInfo, List<String> title)
        {
            
            songlistInfo[0].Title = "Colors";
            songlistInfo[1].Title = "Celeste";
            songlistInfo[2].Title = "New Year's Day";
            songlistInfo[3].Title = "Autumn Leaves";
            songlistInfo[4].Title = "Everglow";
            songlistInfo[5].Title = "26";
            songlistInfo[6].Title = "Kings and Queens";
            songlistInfo[7].Title = "Paradise";
            songlistInfo[8].Title = "Superheroes";
            songlistInfo[9].Title = "Better";
            songlistInfo[10].Title = "Warm Whispers";
            songlistInfo[11].Title = "Fireside";

            title.Add(songlistInfo[0].Title);
            title.Add(songlistInfo[1].Title);
            title.Add(songlistInfo[2].Title);
            title.Add(songlistInfo[3].Title);
            title.Add(songlistInfo[4].Title);
            title.Add(songlistInfo[5].Title);
            title.Add(songlistInfo[6].Title);
            title.Add(songlistInfo[7].Title);
            title.Add(songlistInfo[8].Title);
            title.Add(songlistInfo[9].Title);
            title.Add(songlistInfo[10].Title);
            title.Add(songlistInfo[11].Title);


            return title;
        }

            /// <summary>
        /// Holds the data for each song's album 
        /// </summary>
        /// <param name="songlistInfo"></param>
        /// <param name="album"></param>
        /// <returns>List<String></returns>
            static List<String> SongAlbumTitleInformation(List<Discography> songlistInfo, List<String> album)
        {
            songlistInfo[0].Album = "When I Get It Right";
            songlistInfo[1].Album = "Celeste EP";
            songlistInfo[2].Album = "Reputation";
            songlistInfo[3].Album = "+";
            songlistInfo[4].Album = "Everglow";
            songlistInfo[5].Album = "After Laughter";
            songlistInfo[6].Album = "This Is War";
            songlistInfo[7].Album = "Mylo Xyloto";
            songlistInfo[8].Album = "No Sound Without Silence";
            songlistInfo[9].Album = "Bird On A Wire";
            songlistInfo[10].Album = "On A Clear Night";
            songlistInfo[11].Album = "City Lights";

            album.Add(songlistInfo[0].Album);
            album.Add(songlistInfo[1].Album);
            album.Add(songlistInfo[2].Album);
            album.Add(songlistInfo[3].Album);
            album.Add(songlistInfo[4].Album);
            album.Add(songlistInfo[5].Album);
            album.Add(songlistInfo[6].Album);
            album.Add(songlistInfo[7].Album);
            album.Add(songlistInfo[8].Album);
            album.Add(songlistInfo[9].Album);
            album.Add(songlistInfo[10].Album);
            album.Add(songlistInfo[11].Album);


            return album;
        }

            /// <summary>
        /// Holds the data for each song's artist
        /// </summary>
        /// <param name="songlistInfo"></param>
        /// <param name="artist"></param>
        /// <returns>List<String></returns>
            static List<String> SongArtistInformation(List<Discography> songlistInfo, List<String> artist)
        {
                songlistInfo[0].Artist = "Michael Blume";
                songlistInfo[1].Artist = "Ezra Vine";
                songlistInfo[2].Artist = "Taylor Swift";
                songlistInfo[3].Artist = "Ed Sheeran";
                songlistInfo[4].Artist = "Coldplay";
                songlistInfo[5].Artist = "Paramore";
                songlistInfo[6].Artist = "Thirty Seconds To Mars";
                songlistInfo[7].Artist = "Coldplay";
                songlistInfo[8].Artist = "The Script";
                songlistInfo[9].Artist = "Toby Lightman";
                songlistInfo[10].Artist = "Missy Higgins";
                songlistInfo[11].Artist = "Brett Bixby";


                artist.Add(songlistInfo[0].Artist);
                artist.Add(songlistInfo[1].Artist);
                artist.Add(songlistInfo[2].Artist);
                artist.Add(songlistInfo[3].Artist);
                artist.Add(songlistInfo[4].Artist);
                artist.Add(songlistInfo[5].Artist);
                artist.Add(songlistInfo[6].Artist);
                artist.Add(songlistInfo[7].Artist);
                artist.Add(songlistInfo[8].Artist);
                artist.Add(songlistInfo[9].Artist);
                artist.Add(songlistInfo[10].Artist);
                artist.Add(songlistInfo[11].Artist);


            return artist;
        }

            /// <summary>
        /// Holds the data for each song's year
        /// </summary>
        /// <param name="songlistInfo"></param>
        /// <param name="year"></param>
        /// <returns></returns>
            static List<int> AlbumYearInformation(List<Discography> songlistInfo, List<int> year)
        {
            songlistInfo[0].Year = 2016;
            songlistInfo[1].Year = 2014;
            songlistInfo[2].Year = 2017;
            songlistInfo[3].Year = 2011;
            songlistInfo[4].Year = 2017;
            songlistInfo[5].Year = 2017;
            songlistInfo[6].Year = 2009;
            songlistInfo[7].Year = 2011;
            songlistInfo[8].Year = 2014;
            songlistInfo[9].Year = 2006;
            songlistInfo[10].Year = 2007;
            songlistInfo[11].Year = 2006;

            year.Add(songlistInfo[0].Year);
            year.Add(songlistInfo[1].Year);
            year.Add(songlistInfo[2].Year);
            year.Add(songlistInfo[3].Year);
            year.Add(songlistInfo[4].Year);
            year.Add(songlistInfo[5].Year);
            year.Add(songlistInfo[6].Year);
            year.Add(songlistInfo[7].Year);
            year.Add(songlistInfo[8].Year);
            year.Add(songlistInfo[9].Year);
            year.Add(songlistInfo[10].Year);
            year.Add(songlistInfo[11].Year);


            return year;
        }

    }

    
}
