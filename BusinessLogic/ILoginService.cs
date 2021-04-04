using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    interface ILoginService
    {
        bool TryLogin(out ILoginToken loginToken, string userName, string password);
    }
}
