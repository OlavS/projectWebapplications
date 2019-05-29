using DAL.DALModels;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    /// <summary>
    /// Initialiserer databasen med mange pregenererte rader i tabellene.
    /// </summary>
    public class DBinit : DropCreateDatabaseAlways<VideoDB>
    {
        protected override void Seed(VideoDB context)
        {
            var customerRepository = new UserRepository();
            byte[] oleSalt = customerRepository.CreateSalt();
            byte[] olePassord = customerRepository.CreateHashedPassword("olaola", oleSalt);
            byte[] kariSalt = customerRepository.CreateSalt();
            byte[] kariPassord = customerRepository.CreateHashedPassword("karikari", kariSalt);
            byte[] adminSalt = customerRepository.CreateSalt();
            byte[] adminPassord = customerRepository.CreateHashedPassword("admadm", adminSalt);
            byte[] perSalt = customerRepository.CreateSalt();
            byte[] perPassord = customerRepository.CreateHashedPassword("perper", perSalt);
            byte[] thomasSalt = customerRepository.CreateSalt();
            byte[] thomasPassord = customerRepository.CreateHashedPassword("thomasthomas", thomasSalt);
            byte[] bobSalt = customerRepository.CreateSalt();
            byte[] bobPassord = customerRepository.CreateHashedPassword("bobbob", bobSalt);
            byte[] trineSalt = customerRepository.CreateSalt();
            byte[] trinePassord = customerRepository.CreateHashedPassword("trinetrine", trineSalt);

            IList<User> defaultKunder = new List<User>
            {
                new User()
                {
                    FirstName = "Admin",
                    SurName = "Adminsen",
                    Address = "Pilestredet 32",
                    PhoneNr = "67235000",
                    Email = "admin@holbergs.no",
                    Salt = adminSalt,
                    PassWord = adminPassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "0130",
                        Postal = "Oslo"
                    },
                    Admin = true
                },

                new User()
                {
                     FirstName = "Ola",
                    SurName = "Nordmann",
                    Address = "Portveien 2",
                    PhoneNr = "92233444",
                    Email = "Ola@hotmail.com",
                    Salt = oleSalt,
                    PassWord = olePassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "1234",
                        Postal = "Oslo"
                    }
                },

                new User()
                {
                    FirstName = "Kari",
                    SurName = "Nordmann",
                    Address = "Kongensgate 43",
                    PhoneNr = "92233555",
                    Email = "Kari@hotmail.com",
                    Salt = kariSalt,
                    PassWord = kariPassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "3456",
                        Postal = "Kongsberg"
                    }
                },

                new User()
                {
                    FirstName = "Per",
                    SurName = "Nordmann",
                    Address = "Kongensgate 43",
                    PhoneNr = "92233556",
                    Email = "per@hotmail.com",
                    Salt = perSalt,
                    PassWord = perPassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "3121",
                        Postal = "Nøtterøy"
                    }
                },

                new User()
                {
                    FirstName = "Thomas",
                    SurName = "Thomasen",
                    Address = "Kongensgate 49",
                    PhoneNr = "92233557",
                    Email = "thomas@hotmail.com",
                    Salt = thomasSalt,
                    PassWord = thomasPassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "3101",
                        Postal = "Tønsberg"
                    }
                },

                 new User()
                {
                    FirstName = "Bob",
                    SurName = "Bobsen",
                    Address = "Kongensgate 50",
                    PhoneNr = "92233558",
                    Email = "bob@hotmail.com",
                    Salt = bobSalt,
                    PassWord = bobPassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "3009",
                        Postal = "Horten"
                    }
                },

                 new User()
                {
                    FirstName = "Trine",
                    SurName = "Trinesen",
                    Address = "Kongensgate 51",
                    PhoneNr = "92233559",
                    Email = "trine@hotmail.com",
                    Salt = trineSalt,
                    PassWord = trinePassord,
                    PostalAddress = new PostalAddress()
                    {
                        PostalCode = "2009",
                        Postal = "Drammen"
                    }
                }
            };

            context.Users.AddRange(defaultKunder);

            IList<Genre> defaultSjangere = new List<Genre>
            {
                new Genre() { Name = "Krim"},
                new Genre() { Name = "Drama"},
                new Genre() { Name = "Thriller"},
                new Genre() { Name = "Skrekk"},
                new Genre() { Name = "Action"},
                new Genre() { Name = "Komedie"},
                new Genre() { Name = "Sci-Fi"},
                new Genre() { Name = "Adventure"},
                new Genre() { Name = "Historisk"},
                new Genre() { Name = "Western"},
                new Genre() { Name = "Fantasy"},
                new Genre() { Name = "Romanse"},
                new Genre() { Name = "Familie" },
                new Genre() { Name = "Sport" }
            };

            context.Genres.AddRange(defaultSjangere);

            IList<PriceClass> defaultPriser = new List<PriceClass>
            {
                new PriceClass(){Price = 29},
                new PriceClass(){Price = 49},
                new PriceClass(){Price = 79},
                new PriceClass(){Price = 99},
                new PriceClass(){Price = 129}
            };

            context.PriceClasses.AddRange(defaultPriser);
            context.SaveChanges();

            IList<Film> defaultFilmer = new List<Film>
            {
                new Film()
                {
                    Title = "Mad Max",
                    Description = "I en selvdestruerende verden, setter en hevnlystig politimann seg som mål å stoppe en voldelig motorsykkelgjeng.",
                    ImgURL = "/Content/Images/Filmer/MadMax.jpg",
                    Genres = new List<Genre>
                    {
                        context.Genres.FirstOrDefault(s => s.Name == "Action"),
                        context.Genres.FirstOrDefault(s => s.Name == "Adventure"),
                        context.Genres.FirstOrDefault(s => s.Name == "Sci-Fi")
                    },
                    PriceClasses = context.PriceClasses.Find(2),
                    PriceClassId = 2,
                    GenreIds =  "5 7 8"
                },

                new Film()
                {
                    Title = "Mad Max: The Roadwarrior",
                    Description = "In the post-apocalyptic Australian wasteland, a cynical drifter agrees to help a small, gasoline rich, community escape a band of bandits.",
                    ImgURL = "/Content/Images/Filmer/MadMaxTheRoadwarrior.jpg",
                    Genres = new List<Genre>
                    {
                        context.Genres.FirstOrDefault(s => s.Name == "Action"),
                        context.Genres.FirstOrDefault(s => s.Name == "Adventure"),
                        context.Genres.FirstOrDefault(s => s.Name == "Sci-Fi")
                    },
                    PriceClasses = context.PriceClasses.Find(2),
                    PriceClassId = 2,
                    GenreIds =  "5 7 8"
                },

                new Film()
                {
                    Title = "Mad Max: Beyond Thunderdome",
                    Description = "After being exiled from the most advanced town in post apocalyptic Australia, a drifter travels with a group of abandoned children to rebel against the town's queen.",
                    ImgURL = "/Content/Images/Filmer/MadMaxBeyondThunderdome.jpg",
                    Genres = new List<Genre>
                    {
                        context.Genres.FirstOrDefault(s => s.Name == "Action"),
                        context.Genres.FirstOrDefault(s => s.Name == "Adventure"),
                        context.Genres.FirstOrDefault(s => s.Name == "Sci-Fi")
                    },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "5 7 8"
                },

                new Film()
                {
                    Title = "Blade Runner",
                    Description = "A blade runner must pursue and terminate four replicants who stole a ship in space, and have returned to Earth to find their creator.",
                    ImgURL = "/Content/Images/Filmer/BladeRunner.jpg",
                    Genres = new List<Genre>
                    {
                        context.Genres.FirstOrDefault(s => s.Name == "Thriller"),
                        context.Genres.FirstOrDefault(s => s.Name == "Action"),
                        context.Genres.FirstOrDefault(s => s.Name == "Sci-Fi")
                      },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "3 5 7"
                },

                new Film()
                {
                    Title = "Blade Runner 2049",
                    Description = "A young blade runner's discovery of a long-buried secret leads him to track down former blade runner Rick Deckard, who's been missing for thirty years.",
                    ImgURL = "/Content/Images/Filmer/BladeRunner2049.jpg",
                    Genres = new List<Genre>
                    {
                        context.Genres.FirstOrDefault(s => s.Name == "Thriller"),
                        context.Genres.FirstOrDefault(s => s.Name == "Action"),
                        context.Genres.FirstOrDefault(s => s.Name == "Sci-Fi")
                    },
                    PriceClasses = context.PriceClasses.Find(4),
                    PriceClassId = 4,
                     GenreIds =  "3 5 7"
                },

                new Film()
                {
                    Title = "Nacho Libre",
                    Description = "Berated all his life by those around him, a monk follows his dream and dons a mask to moonlight as a Luchador (Mexican wrestler).",
                    ImgURL = "/Content/Images/Filmer/NachoLibre.jpg",
                    Genres = new List<Genre>
                    {
                        context.Genres.FirstOrDefault(s => s.Name == "Komedie"),
                        context.Genres.FirstOrDefault(s => s.Name == "Familie"),
                        context.Genres.FirstOrDefault(s => s.Name == "Sport")
                    },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "6 13 14"
                },

                new Film
                {
                    Title ="Jurassic World: Fallen Kingdom",
                    Description ="When the island's dormant volcano begins roaring to life, Owen and Claire mount a campaign to rescue the remaining dinosaurs from this extinction-level event.",
                    ImgURL ="/Content/Images/Filmer/JurassicWorldFallenKingdom.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Action"),
                        context.Genres.FirstOrDefault(f=> f.Name=="Sci-Fi"),
                        context.Genres.FirstOrDefault(f=> f.Name=="Adventure")
                    },
                     PriceClasses = context.PriceClasses.Find(5),
                     PriceClassId = 5,
                     GenreIds =  "5 7 8"
                },

                new Film
                    {
                    Title ="Fahrenheit 451",
                    Description ="In a terrifying care-free future, a young man, Guy Montag, whose job as a fireman is to burn all books, questions his actions after meeting a young woman...and begins to rebel against society.",
                    ImgURL ="/Content/Images/Filmer/Fahrenheit451.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Sci-Fi"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Thriller")
                    },
                     PriceClasses = context.PriceClasses.Find(5),
                     PriceClassId = 5,
                     GenreIds =  "2 3 7"
                },

                new Film
                {
                    Title ="Ocean's Eight",
                    Description ="Debbie Ocean gathers an all-female crew to attempt an impossible heist at New York City's yearly Met Gala.",
                    ImgURL ="/Content/Images/Filmer/OceansEight.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Action"),
                        context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Krim")
                    },
                     PriceClasses = context.PriceClasses.Find(1),
                     PriceClassId = 1,
                     GenreIds =  "1 2 5"
                },

                new Film
                {
                    Title ="The Watcher in the Woods",
                    Description ="Mrs. Aylwood is a distraught mother since her daughter, Karen, vanished in the Welsh countryside 30 years ago. When the Carstairs family move into the Aylwood manor for the summer., strange...                ",
                    ImgURL ="/Content/Images/Filmer/TheWatcherInTheWoods.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Skrekk")
                    },
                     PriceClasses = context.PriceClasses.Find(4),
                     PriceClassId = 4,
                     GenreIds =  "4"
                },

                new Film
                {
                    Title ="SuperFly",
                    Description ="With retirement on his mind, a successful young drug dealer sets up one last big job, while dealing with trigger-happy colleagues and the police.",
                    ImgURL ="/Content/Images/Filmer/SuperFly.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Action"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Krim"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Thriller")
                    },
                     PriceClasses = context.PriceClasses.Find(5),
                     PriceClassId = 5,
                     GenreIds =  "1 3 5"
                },

                new Film
                {
                    Title ="Western",
                    Description ="A group of German construction workers start a tough job at a remote site in the Bulgarian countryside. The foreign land awakens the men's sense of adventure, but they are also confronted ...                ",
                    ImgURL ="/Content/Images/Filmer/Western.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Drama")
                    },
                     PriceClasses = context.PriceClasses.Find(5),
                     PriceClassId = 5,
                     GenreIds =  "2"
                },

                new Film
                {
                    Title ="The Shawshank Redemption ",
                    Description ="Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                    ImgURL ="/Content/Images/Filmer/TheShawshankRedemption.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Drama")
                    },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "2"
                },

                new Film
                {
                    Title ="The Godfather",
                    Description ="The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    ImgURL ="/Content/Images/Filmer/TheGodfather.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                        context.Genres.FirstOrDefault(f=> f.Name=="Krim")
                    },
                     PriceClasses = context.PriceClasses.Find(1),
                     PriceClassId = 1,
                     GenreIds =  "1 2"
                },

                new Film
                {
                    Title ="The Godfather: Part II",
                    Description ="The early life and career of Vito Corleone in 1920s New York City is portrayed, while his son, Michael, expands and tightens his grip on the family crime syndicate.",
                    ImgURL ="/Content/Images/Filmer/TheGodfatherPartII.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Action"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Krim"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Drama")
                    },
                     PriceClasses = context.PriceClasses.Find(1),
                     PriceClassId = 1,
                     GenreIds =  "1 2 5"
                },

                new Film
                {
                    Title ="The Godfather: Part III",
                    Description ="In the midst of trying to legitimize his business dealings in New York City and Italy in 1979, aging Mafia Don Michael Corleone seeks to avow for his sins, while taking his nephew Vincent Mancini under his wing.",
                    ImgURL ="/Content/Images/Filmer/TheGodfatherPartIII.jpg",
                    Genres = new List<Genre>()
                    {
                         context.Genres.FirstOrDefault(f=> f.Name=="Krim"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Drama")
                    },
                     PriceClasses = context.PriceClasses.Find(1),
                     PriceClassId = 1,
                     GenreIds =  "1 2"
                },

                new Film
                {
                    Title ="The Dark Knight",
                    Description ="When the menace known as the Joker emerges from his mysterious past, he wreaks havoc and chaos on the people of Gotham. The Dark Knight must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                    ImgURL ="/Content/Images/Filmer/TheDarkKnight.jpg",
                    Genres = new List<Genre>()
                    {
                        context.Genres.FirstOrDefault(f=> f.Name=="Action"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Krim"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Drama")
                    },
                     PriceClasses = context.PriceClasses.Find(3),
                     PriceClassId = 3,
                     GenreIds =  "1 2 5"
                },

                new Film
                {
                    Title ="Schindler's List",
                    Description ="In German-occupied Poland during World War II, Oskar Schindler gradually becomes concerned for his Jewish workforce after witnessing their persecution by the Nazi Germans.",
                    ImgURL ="/Content/Images/Filmer/SchindlersList.jpg",
                    Genres = new List<Genre>()
                   {
                        context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Historisk")
                    },
                     PriceClasses = context.PriceClasses.Find(1),
                     PriceClassId = 1,
                     GenreIds =  "2 9"
                },

                new Film
                {
                    Title ="The Lord of the Rings: The Return of the King",
                    Description ="Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.",
                    ImgURL ="/Content/Images/Filmer/TheLordOfTheRingsTheReturnOfTheKing.jpg",
                    Genres = new List<Genre>()
                    {
                     context.Genres.FirstOrDefault(f=> f.Name=="Adventure"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Fantasy")
                    },
                     PriceClasses = context.PriceClasses.Find(3),
                     PriceClassId = 3,
                     GenreIds =  "2 8 11"
                },

                new Film
                {
                    Title ="Pulp Fiction",
                    Description ="The lives of two mob hitmen, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                    ImgURL ="/Content/Images/Filmer/PulpFiction.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Krim"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Drama")
                    },
                     PriceClasses = context.PriceClasses.Find(3),
                     PriceClassId = 3,
                     GenreIds =  "1 2"
                },

                new Film
                {
                    Title ="The Good, the Bad and the Ugly ",
                    Description ="A bounty hunting scam joins two men in an uneasy alliance against a third in a race to find a fortune in gold buried in a remote cemetery.",
                    ImgURL ="/Content/Images/Filmer/TheGoodTheBadAndTheUgly.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Western"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Action")
                    },
                     PriceClasses = context.PriceClasses.Find(1),
                     PriceClassId = 1,
                     GenreIds =  "2 5 10"
                },

                new Film
                {
                    Title ="Fight Club",
                    Description ="An insomniac office worker and a devil-may-care soapmaker form an underground fight club that evolves into something much, much more.",
                    ImgURL ="/Content/Images/Filmer/FightClub.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                         context.Genres.FirstOrDefault(f=> f.Name=="Thriller")
                    },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "2 3"
                },

                new Film
                {
                    Title ="The Lord of the Rings: The Fellowship of the Ring",
                    Description ="A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring and save Middle-earth from the Dark Lord Sauron.",
                    ImgURL ="/Content/Images/Filmer/TheLordOfTheRingsTheFellowshipOfTheRing.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Adventure"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Fantasy")
                    },
                     PriceClasses = context.PriceClasses.Find(3),
                     PriceClassId = 3,
                     GenreIds =  "2 8 11"
                },

                new Film
                {
                    Title ="Forrest Gump",
                    Description ="The presidencies of Kennedy and Johnson, Vietnam, Watergate, and other history unfold through the perspective of an Alabama man with an IQ of 75.",
                    ImgURL ="/Content/Images/Filmer/ForrestGump.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Drama"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Romanse")
                    },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "2 12"
                },

                new Film
               {
                    Title ="Star Wars: Episode V - The Empire Strikes Back",
                    Description ="After the rebels are brutally overpowered by the Empire on the ice planet Hoth, Luke Skywalker begins Jedi training with Yoda, while his friends are pursued by Darth Vader.",
                    ImgURL ="/Content/Images/Filmer/StarWarsEpisodeVTheEmpireStrikesBack.jpg",
                    Genres = new List<Genre>()
                    {
                      context.Genres.FirstOrDefault(f=> f.Name=="Action"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Adventure"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Sci-Fi"),
                      context.Genres.FirstOrDefault(f=> f.Name=="Fantasy")
                    },
                     PriceClasses = context.PriceClasses.Find(2),
                     PriceClassId = 2,
                     GenreIds =  "5 7 8 11"
                }
            };

            context.Films.AddRange(defaultFilmer);
            context.SaveChanges();

            User kunde = context.Users.Find(2);
            var ordreListe = new List<Order>
            {
                new Order()
                {
                    User = kunde,
                    Date = DateTime.Now.AddDays(-1),
                    OrderLines = new List<OrderLine>
                    {
                            new OrderLine()
                            {
                                Film = context.Films.Find(1),
                                Price = context.Films.Find(1).PriceClasses.Price
                            },
                             new OrderLine()
                            {
                                Film = context.Films.Find(2),
                                Price = context.Films.Find(2).PriceClasses.Price
                            },
                              new OrderLine()
                            {
                                Film = context.Films.Find(3),
                                Price = context.Films.Find(3).PriceClasses.Price
                            },
                               new OrderLine()
                            {
                                Film = context.Films.Find(4),
                                Price = context.Films.Find(4).PriceClasses.Price
                            }
                    }
                },
                new Order()
                {
                    User = kunde,
                    Date = DateTime.Now,
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine()
                        {
                            Film = context.Films.Find(5),
                            Price = context.Films.Find(5).PriceClasses.Price
                        },
                         new OrderLine()
                        {
                            Film = context.Films.Find(6),
                            Price = context.Films.Find(6).PriceClasses.Price
                        },
                          new OrderLine()
                        {
                            Film = context.Films.Find(7),
                            Price = context.Films.Find(7).PriceClasses.Price
                        },
                           new OrderLine()
                        {
                            Film = context.Films.Find(8),
                            Price = context.Films.Find(8).PriceClasses.Price
                        }
                    }
                }
            };

            User kunde2 = context.Users.Find(3);
            var ordreListe2 = new List<Order>
            {
                new Order()
                {
                    User = kunde2,
                    Date = DateTime.Now.AddDays(-1),
                    OrderLines = new List<OrderLine>
                    {
                            new OrderLine()
                            {
                                Film = context.Films.Find(1),
                                Price = context.Films.Find(1).PriceClasses.Price
                            },
                             new OrderLine()
                            {
                                Film = context.Films.Find(2),
                                Price = context.Films.Find(2).PriceClasses.Price
                            },
                              new OrderLine()
                            {
                                Film = context.Films.Find(3),
                                Price = context.Films.Find(3).PriceClasses.Price
                            },
                               new OrderLine()
                            {
                                Film = context.Films.Find(4),
                                Price = context.Films.Find(4).PriceClasses.Price
                            }
                    }
                },
                new Order()
                {
                    User = kunde2,
                    Date = DateTime.Now,
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine()
                        {
                            Film = context.Films.Find(5),
                            Price = context.Films.Find(5).PriceClasses.Price
                        },
                         new OrderLine()
                        {
                            Film = context.Films.Find(6),
                            Price = context.Films.Find(6).PriceClasses.Price
                        },
                          new OrderLine()
                        {
                            Film = context.Films.Find(7),
                            Price = context.Films.Find(7).PriceClasses.Price
                        },
                           new OrderLine()
                        {
                            Film = context.Films.Find(8),
                            Price = context.Films.Find(8).PriceClasses.Price
                        }
                    }
                }
            };
            context.Orders.AddRange(ordreListe);
            context.Orders.AddRange(ordreListe2);
            context.SaveChanges();

            var error = new Error()
            {
                Message = "Her skal metodens path vises inkl paramter",
                Parameter = "Her putter man toStringene til parametrene",
                StackTrace = "stacktrace"
            };
            context.ErrorLogs.Add(error);

            base.Seed(context);
        }
    }
}