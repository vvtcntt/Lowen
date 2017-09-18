﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LOWEN.Models;
namespace LOWEN.Models
{
    public class Updatehistoty
    {
        public LOWENContext db = new LOWENContext();
        public static void UpdateHistory(string task,string FullName,string UserID)
        {

            LOWENContext db = new LOWENContext();
            tblHistoryLogin tblhistorylogin = new tblHistoryLogin();
            tblhistorylogin.FullName = FullName;
            tblhistorylogin.Task = task;
            tblhistorylogin.idUser = int.Parse(UserID);
            tblhistorylogin.DateCreate = DateTime.Now;
            tblhistorylogin.Active = true;
            
            db.tblHistoryLogins.Add(tblhistorylogin);
            db.SaveChanges();
           
        }
    }
}