using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPAccounts.DataAccessObject.DTO
{
    public class Common
    {
    }

    public class Settings
    {

        public string dateformat = "dd-MMM-yyyy";
    }


    public class Const
    {
        #region Messages

        private List<ConstMessage> ConstMessage = new List<ConstMessage>();

        public Const()
        {
            ConstMessage.Add(new ConstMessage("Test message", "DF8D1", "ERROR"));
             
            //
        }

        public string LoginAndEmailExist
        {
            get { return "Login or Email Exist! "; }
        }
        public string InsertFailure
        {
            get { return "Insertion Not Successfull! "; }
        }

        public string InsertSuccess
        {
            get { return "Values Saved Successfully ! "; }
        }

        public string UpdateFailure
        {
            get { return "Updation Not Successfull! "; }
        }

        public string UpdateSuccess
        {
            get { return "Updation Successfull! "; }
        }

        public string DeleteFailure
        {
            get { return "Deletion Not Successfull! "; }
        }
        public string DeleteSuccess
        {
            get { return "Deletion Successfull! "; }
        }
        public string FKviolation
        {
            get { return "Deletion Not Successfull!-Already In Use"; }
        }
        public string Duplicate
        {
            get { return "Allready Exist.."; }
        }
        
        public string NoItems
        {
            get { return "No items"; }
        }
        
        public string PasswordError
        {
            get { return "Password is wrong"; }
        }
        public ConstMessage GetMessage(string MsgCode)
        {
            ConstMessage result = new ConstMessage(MsgCode, "", "ERROR");

            try
            {
                foreach (ConstMessage c in ConstMessage)
                {
                    if (c.Code == MsgCode)
                    {
                        result = c;
                        break;
                    }

                }

            }
            catch (Exception)
            {


            }
            return result;



        }


        #endregion Messages

        #region Strings
        public string AppUser
        {
            get { return "App User"; }
        }
        #endregion
    }

    public class ConstMessage
    {
        public string Message;
        public string Code;
        public string type;
        public ConstMessage(string msg, string cd, string typ)
        {
            Message = (cd == "" ? "" : cd + "-") + msg;
            Code = cd;
            type = typ;

        }
    }
}