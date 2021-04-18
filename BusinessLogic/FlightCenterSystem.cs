﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAO;

namespace BusinessLogic
{
    public class FlightCenterSystem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static FlightCenterSystem m_Instance;
        static object key = new object();
        static LoginService loginService = new LoginService();
        public static FlightCenterSystem Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (key)
                    {
                        if (m_Instance == null)
                            m_Instance = new FlightCenterSystem();
                    }
                }
                return m_Instance;
            }
        }
        private FlightCenterSystem()
        {
            //AutoResetEvent auto = new AutoResetEvent(false);
            //Thread thread = new Thread(() =>
            //{
            //    while (true)
            //    {
            //        if (DateTime.Now.TimeOfDay == AppConfig.Instance.WakingUpTime.TimeOfDay)
            //        {

            //        }
            //    }
            //});
            //thread.IsBackground = true;
            //thread.Start();
            
        }
        public FacadeBase GetFacade(out ILoginToken loginToken, string username, string password)
        {
            try
            {
                FacadeBase facade;
                bool res = loginService.TryLogin(out facade, out loginToken, username, password);
                return facade;
            }
            catch (WrongCredentialsException ex)
            {
                throw;
            }
        }        
    }
}
