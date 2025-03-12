using System;

namespace Main
{

    abstract class Media
    {
        public string Title;
        public double Duration;
        public abstract void SongDetails();
    }

    class Song : Media
    {
        private string album;

        public Song(string title, double duration)
        {
            base.Title = title;
            base.Duration = duration;
        }

        public string GetAlbum()
        {
            return album;
        }

        public void SetAlbum(string album)
        {
            this.album = album;
        }

        public override void SongDetails()
        {
            Console.WriteLine("Song: "+base.Title+" of Duration "+base.Duration+" from the "+album+" ablum");
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            
            //Taking Input from User
            string songTitle;
            int songDuration = 0;
            Console.WriteLine("Enter Song Title: ");
            songTitle = Console.ReadLine();
            Console.WriteLine("Enter Song Duration: ");
            songDuration = Convert.ToInt32(Console.ReadLine());

            //Creating Object for Derived Class
            Song song = new Song(title: songTitle, duration: songDuration);
            song.SetAlbum("Favourite Album");
            song.SongDetails();

        }
    }

}
