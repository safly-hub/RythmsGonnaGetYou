using System;

namespace RhythmsGonnaGetYou
{
    class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsExplicit { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int BandId { get; set; }
        public Band Band { get; set; }
    }
    class Band
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryOfOrigin { get; set; }
        public int NumberOfMembers { get; set; }
        public string Website { get; set; }
        public string Style { get; set; }
        public bool IsSigned { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
    }
    class RhythmsGonnaGetYouContext : DbContext
    {
        private DbSet<Album> albums;

        public DbSet<Album> Albums { get => Albums1; set => Albums1 = value; }
        public DbSet<Band> Bands { get; set; }
        internal DbSet<Album> Albums1 { get => albums; set => albums = value; }
        internal DbSet<Album> Albums2 { get => albums; set => albums = value; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("server=localhost;database=Rhythm");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var context = new RhythmsGonnaGetYouContext();
            var albums = context.Albums.Include(album => album.Band);
            var bands = context.Bands;
            var quit = false;
            while (quit != true)
            {
                Console.WriteLine("Please choose one of the following");
                Console.WriteLine();
                Console.WriteLine("Add : Add a new band");
                Console.WriteLine("View: View all the bands");
                Console.WriteLine("New: Add an album for a band");
                Console.WriteLine("Let: Let a band go");
                Console.WriteLine("Resign: Resign a band");
                Console.WriteLine("Prompt: Prompt for a band name and view all their albums");
                Console.WriteLine("VD: View all albums ordered by ReleaseDate");
                Console.WriteLine("VA: View all bands that are signed");
                Console.WriteLine("VNS: View all bands that are not signed");
                Console.WriteLine("Quit: Quit the program");
                var choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    case "quit":
                        Console.WriteLine("===Good bye===");
                        quit = true;
                        break;
                    case "add":
                        Console.Write("Band name: ");
                        var bandName = Console.ReadLine();
                        Console.Write("Country of origin: ");
                        var origin = Console.ReadLine();
                        Console.Write("Number of band members: ");
                        var membersNumber = int.Parse(Console.ReadLine());
                        Console.Write("Band's website: ");
                        var website = Console.ReadLine();
                        Console.Write("Band's Style: ");
                        var style = Console.ReadLine();
                        Console.Write("Is the band signed? True|False: ");
                        var isSigned = Boolean.Parse(Console.ReadLine().ToLower());
                        Console.Write("Is the band has a contact name? True|False : ");
                        var response = Console.ReadLine().ToLower();
                        string contactName = null;
                        string contactPhone = null;
                        if (response == "yes")
                        {
                            Console.Write("Contact name: ");
                            contactName = Console.ReadLine();
                            Console.Write("Contact phone: ");
                            contactPhone = Console.ReadLine();
                        }
                        var newBand = new Band
                        {
                            Name = bandName,
                            CountryOfOrigin = origin,
                            NumberOfMembers = membersNumber,
                            Website = website,
                            Style = style,
                            IsSigned = isSigned,
                            ContactName = contactName,
                            ContactPhoneNumber = contactPhone
                        };
                        bands.Add(newBand);
                        context.SaveChanges();
                        Console.WriteLine();
                        Console.WriteLine("Your band has been added successfully");
                        break;
                    case "view":
                        foreach (var band in bands)
                        {
                            Console.WriteLine(band.Name);
                        }
                        break;
                    case "new":
                        Console.Write("What is the album name? Answer: ");
                        var albumName = Console.ReadLine();
                        Console.Write("what is the band Id? Answer: ");
                        var bandId = int.Parse(Console.ReadLine());
                        Console.Write("Is this album explicit? True|False: ");
                        var isExplicit = Boolean.Parse(Console.ReadLine());
                        Console.Write("What is the release date of that album? example format: Jan 1, 2005 Answer: ");
                        var date = DateTime.Parse(Console.ReadLine());
                        var newAlbum = new Album
                        {
                            Title = albumName,
                            IsExplicit = isExplicit,
                            ReleaseDate = date,
                            BandId = bandId
                        };
                        context.Albums.Add(newAlbum);
                        context.SaveChanges();
                        Console.WriteLine("Your album has been Added successfully");
                        break;
                    case "let":
                        Console.Write("What is the band name you want to let go? Answer: ");
                        var bandNameToLet = Console.ReadLine();
                        var bandToLet = context.Bands.FirstOrDefault(band => band.Name == bandNameToLet);
                        if (bandToLet != null)
                        {
                            bandToLet.IsSigned = false;
                            context.SaveChanges();
                            Console.WriteLine("The band you entered has been let go successfully");
                        }
                        else
                        {
                            Console.WriteLine("There is no such band in the database");
                        }
                        break;
                    case "Resign":
                        Console.Write("What is the band name you want to resign? Answer: ");
                        var bandNameToResign = Console.ReadLine();
                        var bandToResign = context.Bands.FirstOrDefault(band => band.Name == bandNameToResign);
                        if (bandToResign != null)
                        {
                            bandToResign.IsSigned = true; ;
                            context.SaveChanges();
                            Console.WriteLine("The band you entered has been Resigned successfully");
                        }
                        else
                        {
                            Console.WriteLine("There is no such band in the database");
                        }
                        break;
                    case "prompt":
                        Console.Write("What is the band Id? Answer: ");
                        var bandIdToView = int.Parse(Console.ReadLine());
                        var albumsToView = albums.Where(album => album.Band.Id == bandIdToView);
                        foreach (var album in albumsToView)
                        {
                            Console.WriteLine(album.Title);
                        }
                        break;
                    case "vd":
                        var orderedAlbums = albums.OrderBy(album => album.ReleaseDate);
                        foreach (var album in orderedAlbums)
                        {
                            Console.WriteLine($"Album {album.Title} was released on {album.ReleaseDate}");
                        }
                        break;
                    case "va":
                        var signedBands = bands.Where(band => band.IsSigned == true);
                        foreach (var signedBand in signedBands)
                        {
                            Console.WriteLine(signedBand.Name);
                        }
                        break;
                    case "vns":
                        var nonSignedBands = bands.Where(band => band.IsSigned == false);
                        foreach (var nonSignedBand in nonSignedBands)
                        {
                            Console.WriteLine(nonSignedBand.Name);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid Entry, please try again");
                        break;
                }
            }
        }
    }
}