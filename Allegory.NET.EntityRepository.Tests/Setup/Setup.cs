using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Allegory.NET.EntityRepository.Tests.Setup
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            SetPrincipal();
        }
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {

        }
        private static void SetPrincipal()
        {
            ClaimsIdentity userIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,"10")
            });
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
            Thread.CurrentPrincipal = claimsPrincipal;
        }
    }
}
