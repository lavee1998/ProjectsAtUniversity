using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    /*
     Regisztrált felhasználók modellje. A legalapvetőbb tulajdonságokat
     (pl.: név jelszó) az IdentityUser már tartalmazza, de ebből leszármaztatva
     plusz mezőkkel egészíthetjük ezt.
     */
    public class ApplicationUser : IdentityUser
    {
    }
}