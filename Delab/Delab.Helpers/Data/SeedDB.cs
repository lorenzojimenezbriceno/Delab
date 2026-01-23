using Delab.AccessData.Data;
using Delab.Shared.Entities;
using Delab.Shared.Enum;

namespace Delab.Helpers.Data;

public class SeedDB
{
    private readonly DataContext _context;
    private readonly IUserHelper _userHelper;

    public SeedDB(DataContext context, IUserHelper userHelper)
    {
        _context = context;
        _userHelper = userHelper;
    }

    public async Task SeedAsync()
    {
        // Asegurarse que la base de datos esté creada
        await _context.Database.EnsureCreatedAsync();

        // Inserta los países, estados y ciudades
        await CheckCountriesAsync();

        // Inserta los roles (sin esos no se pueden insertar los usuarios)
        await CheckRolesAsync();

        // Inserta los planes
        await CheckSoftPlan();

        // Inserta el administrador
        await CheckUserAsync("Nexxtplanet",  "SPI",  "soporte@nexxtplanet.net",  "+1 786 503", UserType.Admin);
        await CheckUserAsync("Nexxtplanet1", "SPI1", "soporte1@nexxtplanet.net", "+1 786 504", UserType.User);
        await CheckUserAsync("Nexxtplanet2", "SPI2", "soporte2@nexxtplanet.net", "+1 786 505", UserType.UserAux);
        await CheckUserAsync("Nexxtplanet3", "SPI3", "soporte3@nexxtplanet.net", "+1 786 506", UserType.Cachier);
        await CheckUserAsync("Nexxtplanet4", "SPI4", "soporte4@nexxtplanet.net", "+1 786 507", UserType.Storage);
    }

    private async Task CheckRolesAsync()
    {
        await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
        await _userHelper.CheckRoleAsync(UserType.User.ToString());
        await _userHelper.CheckRoleAsync(UserType.UserAux.ToString());
        await _userHelper.CheckRoleAsync(UserType.Cachier.ToString());
        await _userHelper.CheckRoleAsync(UserType.Storage.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email,
                string phone, UserType userType)
    {
        User user = await _userHelper.GetUserAsync(email);
        if (user == null)
        {
            user = new()
            {
                FirstName = firstName,
                LastName = lastName,
                FullName = $"{firstName} {lastName}",
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                JobPosition = userType.ToString("g"),
                UserFrom = "SeedDb",
                UserRoleDetails = new List<UserRoleDetails> { new UserRoleDetails { UserType = userType } },
                Active = true,
            };

            await _userHelper.AddUserAsync(user, "123456");
            await _userHelper.AddUserToRoleAsync(user, userType.ToString());

            // Para Confirmar automaticamente el Usuario y activar la cuenta
            string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            await _userHelper.ConfirmEmailAsync(user, token);
            await _userHelper.AddUserClaims(userType, email);
        }
        return user;
    }

    /*
     * Alimenta la base de datos con los planes disponibles
     */

    private async Task CheckSoftPlan()
    {
        if (!_context.SoftPlans.Any())
        {
            _context.SoftPlans.Add(new SoftPlan
            {
                Name = "Plan 1 Mes",
                Price = 50,
                Meses = 1,
                ClinicsCount = 2,
                Active = true
            });
            _context.SoftPlans.Add(new SoftPlan
            {
                Name = "Plan 6 Mes",
                Price = 300,
                Meses = 6,
                ClinicsCount = 10,
                Active = true
            });
            _context.SoftPlans.Add(new SoftPlan
            {
                Name = "Plan 12 Mes",
                Price = 600,
                Meses = 12,
                ClinicsCount = 100,
                Active = true
            });
            await _context.SaveChangesAsync();
        }
    }

    /*
     * Inserta los países, estados y ciudades
     */

    private async Task CheckCountriesAsync()
    {
       if (!_context.Countries.Any())
       {
            _context.Countries.Add(new Country()
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
                            new City { Name = "Soacha"},
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

            _context.Countries.Add(new Country()
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

            _context.Countries.Add(new Country
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

            await _context.SaveChangesAsync();
       }
    }
}
