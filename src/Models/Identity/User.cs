using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace bom.Models.Identity
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser, IIdentifiable<string>
    {

    }
}
