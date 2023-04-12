//
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Persistence;
//
namespace NSG.Integration.Helpers
{
    public class DatabaseSeeder
    {
        //
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        //
        public DatabaseSeeder(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //
        public async Task<bool> Seed()
        {
            //
            Console.WriteLine("Entering Seed ...");
            //
            await SeedData.Initialize(_context, _roleManager, true);
            await SeedData.SeedEmailTemplate(_context, true);
            //
            var users = await _context.Users.ToListAsync(); // needs Microsoft.EntityFrameworkCore;
            Console.WriteLine("User count: " + users.Count.ToString());
            Console.WriteLine("NoteType count: " + (await _context.NoteTypes.CountAsync()).ToString());
            Console.WriteLine("Server count: " + (await _context.Servers.CountAsync()).ToString());
            if (users.Count == 0)
            {
                await Seed_Users();
            }
            var _incidents = await _context.Incidents.ToListAsync();
            Console.WriteLine("Incident count: " + _incidents.Count.ToString());
            if (_incidents.Count == 0)
            {
                // serverId, incidentNoteId (for ping)
                await Seed_Incidents(1, 1);
            }
            //
            return true;
        }
        //
        public async Task Seed_Users()
        {
            //
            if (_userManager.Users.Count() == 0)
            {
                try
                {
                    ApplicationUser _user1 =
                        new ApplicationUser { Email = NSG_Helpers.User_Email, UserName = NSG_Helpers.User_Name, PasswordHash = "AQAAAAEAACcQAAAAEB4oAR8WhJGi5QVXpuONJ4z69YqF/69GlCz4TtjbQVLA4ef69x0iDq5GLTzvqM2vwQ==", SecurityStamp = "VFV7PXFFMU4VZF57I3T7A6TXVF4VAY2M", ConcurrencyStamp = "24240e95-400c-434e-b498-16542c90de13", CompanyId = 1, FirstName = "Phillip", LastName = "Huhn", FullName = "Phillip Huhn", UserNicName = NSG_Helpers.User_Name, EmailConfirmed = true };
                    ApplicationUser _user2 =
                        new ApplicationUser { Email = NSG_Helpers.User_Email2, UserName = NSG_Helpers.User_Name2, PasswordHash = "AQAAAAEAACcQAAAAEGG4L+8q4FXRLAhrLWuALpnyStwzaYOaVA6LjBUrHHqs3AreDKMm9DnRa3B4PM1DYg==", SecurityStamp = "LTCQ4W2NCVQRESG6ZWMC7IBMWDZNICK7", ConcurrencyStamp = "2dab2144-81e5-4b76-a09c-c3b6c37f0f3b", CompanyId = 1, FirstName = "Author", LastName = "Huhn", FullName = "Author Huhn", UserNicName = "Art" };
                    // Add all the predefined profiles using the predefined password
                    Console.WriteLine($"User1: {NSG_Helpers.User_Name}, User2: {NSG_Helpers.User_Name2}");
                    IdentityResult _identityResults1 = await _userManager.CreateAsync(_user1, NSG_Helpers.Password);
                    if (_identityResults1.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(_user1, new string[]{ "Admin" } );
                        _context.UserServers.Add(new ApplicationUserServer() { Id = _user1.Id, ServerId = 1 });
                        _context.SaveChanges();
                    }
                    IdentityResult _identityResults2 = await _userManager.CreateAsync(_user2, NSG_Helpers.Password2);
                    if (_identityResults2.Succeeded)
                    {
                        await _userManager.AddToRolesAsync(_user2, new string[] { "User" });
                        _context.UserServers.Add(new ApplicationUserServer() { Id = _user2.Id, ServerId = 1 });
                        _context.SaveChanges();
                    }
                    foreach( ApplicationUser _usr in _context.Users )
                    {
                        Console.WriteLine($"User: {_usr.UserName}, Email: {_usr.Email}, Name: {_usr.FullName}");
                    }
                }
                catch (Exception _ex)
                {
                    foreach (ApplicationUser _u in _context.Users)
                    {
                        Console.WriteLine(_u.Id + " " + _u.UserName);
                    }
                    Console.WriteLine(_ex.Message);
                    Console.WriteLine(_ex.ToString());
                    throw new Exception($"Seed_Users: {_ex.Message}", _ex);
                }
            }
            //
        }
        //
        private async Task Seed_Incidents(int serverId, int pingNoteId)
        {
            //
            try
            {
                /*
                ** Add incident, incident note and many-to-many linkage.
                */
                Incident _inc = new Incident() { ServerId = serverId, IPAddress = "94.41.53.106", NIC_Id = "ripe.net",
                            NetworkName = "UBN Network", AbuseEmailAddress = "abuse@AbuseEmailAddress.com",
                            ISPTicketNumber = "", Mailed = false, Closed = false, Special = false, Notes = "", CreatedDate = DateTime.Now };
                _context.Incidents.Add(_inc);
                IncidentNote _incNote = new IncidentNote()
                {
                    NoteTypeId = pingNoteId,
                    Note = "Pinging 94.41.53.106 with 32 bytes of data:",
                    CreatedDate = DateTime.Now
                };
                _context.IncidentNotes.Add(_incNote);
                await _context.SaveChangesAsync(CancellationToken.None);
                //
                _context.IncidentIncidentNotes.Add(new IncidentIncidentNote() {
                    IncidentId = _inc.IncidentId, IncidentNoteId = _incNote.IncidentNoteId });
                /*
                ** NetworkLog
                */
                DateTime _dt = DateTime.Now.AddDays(-1);
                int _incTypeSql = 3;
                _context.NetworkLogs.AddRange(
                    new NetworkLog() { ServerId = serverId, IncidentId = _inc.IncidentId, IPAddress = "94.41.53.106", NetworkLogDate = _dt.AddMilliseconds(15), Log = "Fake log 1, Fake log 1, Fake log 1", IncidentTypeId = _incTypeSql },
                    new NetworkLog() { ServerId = serverId, IncidentId = _inc.IncidentId, IPAddress = "94.41.53.106", NetworkLogDate = _dt.AddMilliseconds(15), Log = "Fake log 1, Fake log 1, Fake log 1", IncidentTypeId = _incTypeSql },
                    new NetworkLog() { ServerId = serverId, IncidentId = null, IPAddress = "104.42.229.49", NetworkLogDate = _dt.AddMinutes(4), Log = "Fake log 2, Fake log 2, Fake log 2", IncidentTypeId = _incTypeSql },
                    new NetworkLog() { ServerId = serverId, IncidentId = null, IPAddress = "104.42.229.49", NetworkLogDate = _dt.AddMinutes(5), Log = "Fake log 3, Fake log 3, Fake log 3", IncidentTypeId = _incTypeSql },
                    new NetworkLog() { ServerId = serverId, IncidentId = null, IPAddress = "54.183.209.144", NetworkLogDate = _dt.AddMinutes(10), Log = "Fake log 4, Fake log 4, Fake log 4", IncidentTypeId = _incTypeSql }
                );
                //
                await _context.SaveChangesAsync(CancellationToken.None);
                //
            }
            catch (Exception _ex)
            {
                Console.WriteLine(_ex.Message);
                Console.WriteLine(_ex.ToString());
                throw new Exception( $"Seed_Incidents: {_ex.Message}", _ex );
            }
            //
        }
    }
}
