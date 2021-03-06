﻿using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    interface ILoginService
    {
        bool TryLogin(out FacadeBase facade, out ILoginToken loginToken, string userName, string password);
    }
}
