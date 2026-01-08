using Daleb.Shared.Entities;

namespace Delab.AccessData.Data;

public class SeedDB
{
    private readonly DataContext context;

    public SeedDB(DataContext context)
    {
        this.context = context;
    }

    public async Task SeedAsync()
    {
        // Asegurarse que la base de datos esté creada
        await context.Database.EnsureCreatedAsync();

        // Inserta los países, estados y ciudades
        await CheckCountriesAsync();
    }

    private async Task CheckCountriesAsync()
    {
       if (!context.Countries.Any())
       {
            context.Countries.Add(new Country()
            {
                Name = "Colombia",
                CodPhone = "+57",
                States = new List<State>()
                {
                    new State {
                        Name = "Antioquia",
                        Cities = new List<City>()
                        {
                            new City { Name = "Medellin" },
                            new City { Name = "Itagui" },
                            new City { Name = "Envigado" },
                            new City { Name = "Bello" },
                            new City { Name = "Rionegro" },
                        }
                    },
                    new State {
                        Name = "Bogota",
                        Cities = new List<City>()
                        {
                            new City { Name = "Usaquen" },
                            new City { Name = "Champineo" },
                            new City { Name = "Santa fe" },
                            new City { Name = "Useme" },
                            new City { Name = "Bosa" },
                        }
                    },
                    new State {
                        Name = "Cundinamarca",
                        Cities = new List<City>()
                        {
                            new City{ Name = "Soacha"},
                            new City { Name = "Facatativa"},
                            new City { Name = "Fusagasuga"},
                            new City { Name = "Chia"},
                            new City { Name = "Zipaquira"}
                        }
                    },
                    new State {
                        Name = "Atlántico",
                        Cities = new List<City>()
                        {
                            new City { Name = "Baranoa" },
                            new City { Name = "Barranquilla" },
                            new City { Name = "Campo de la Cruz" },
                            new City { Name = "Candelaria" },
                            new City { Name = "Galapa" },
                        }
                    },
                    new State {
                        Name = "Bolívar",
                        Cities = new List<City>()
                        {
                            new City { Name = "El Carmen de Bolívar" },
                            new City { Name = "El Guamo" },
                            new City { Name = "El Peñón" },
                            new City { Name = "Hatillo de Loba" },
                            new City { Name = "Cartagena de Indias" },
                        }
                    }
                }
            });

            context.Countries.Add(new Country()
            {
                Name = "Estados Unidos",
                CodPhone = "+1",
                States = new List<State>()
                {
                    new State {
                        Name = "Florida",
                        Cities = new List<City>()
                        {
                            new City { Name = "Orlando" },
                            new City { Name = "Miami" },
                            new City { Name = "Tampa" },
                            new City { Name = "Fort Lauderdale" },
                            new City { Name = "Key West" },
                        }
                    },
                    new State {
                        Name = "Texas",
                        Cities = new List<City>()
                        {
                            new City { Name = "Houston" },
                            new City { Name = "San Antonio" },
                            new City { Name = "Dallas" },
                            new City { Name = "Austin" },
                            new City { Name = "El Paso" },
                        }
                    },
                    new State {
                        Name = "California",
                        Cities = new List<City>()
                        {
                            new City { Name = "Los Angeles" },
                            new City { Name = "San Diego" },
                            new City { Name = "San Jose" },
                            new City { Name = "San Francisco" },
                            new City { Name = "Sacramento" },
                        }
                    },
                    new State {
                        Name = "Virginia",
                        Cities = new List<City>()
                        {
                            new City { Name = "Virginia Beach" },
                            new City { Name = "Chesapeake" },
                            new City { Name = "Richmond" },
                            new City { Name = "Arlington" },
                            new City { Name = "Newport News" },
                        }
                    },
                    new State {
                        Name = "Ohio",
                        Cities = new List<City>()
                        {
                            new City { Name = "Columbus" },
                            new City { Name = "Cleveland" },
                            new City { Name = "Cincinnati" },
                            new City { Name = "Toledo" },
                            new City { Name = "Akron" },
                        }
                    },
                    new State {
                        Name = "Michigan",
                        Cities = new List<City>()
                        {
                            new City { Name = "Detroit" },
                            new City { Name = "Grand Rapids" },
                            new City { Name = "Warren" },
                            new City { Name = "Sterling Heights" },
                            new City { Name = "Lansing" },
                        }
                    },
                    new State {
                        Name = "Arizona",
                        Cities = new List<City>()
                        {
                            new City { Name = "Phoenix" },
                            new City { Name = "Tucson" },
                            new City { Name = "Mesa" },
                            new City { Name = "Chandler" },
                            new City { Name = "Scottsdale" },
                        }
                    },
                }
            });

            context.Countries.Add(new Country
            {
                Name = "Mexico",
                CodPhone = "+57",
                States = new List<State>()
                {
                    new State
                    {
                        Name = "Chiapas",
                        Cities = new List<City>()
                        {
                            new City { Name = "Tuctla"},
                            new City { Name = "Tapachula"},
                            new City { Name = "San Cristobal"},
                            new City { Name = "Comitan"},
                            new City { Name = "Cintalapa"}
                        }
                    },
                    new State
                    {
                        Name = "Colima",
                        Cities = new List<City>()
                        {
                            new City { Name = "Manzanillo"},
                            new City { Name = "Queseria"},
                            new City { Name = "El Colomo"},
                            new City { Name = "Comala"},
                            new City { Name = "Armeria"}
                        }
                    }
                }
            });

            await context.SaveChangesAsync();
       }
    }
}
