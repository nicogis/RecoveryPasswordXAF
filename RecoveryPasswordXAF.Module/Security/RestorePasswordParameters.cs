using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using RecoveryPasswordXAF.Module.BusinessObjects;
using RecoveryPasswordXAF.Module.BusinessObjects.Database;
using System;
using System.ComponentModel;

namespace RecoveryPasswordXAF.Module.Security
{


    [DomainComponent]
    public abstract class LogonActionParametersBase
    {
       
        public const string ValidationContext = "RestorePasswordContext";

        public string Email { get; set; }

        public abstract void ExecuteBusinessLogic(IObjectSpace objectSpace);
    }

    

    [DomainComponent]
    [ModelDefault("Caption", "Reset password")]
    [ImageName("Action_ResetPassword")]
    public class RestorePasswordParameters : LogonActionParametersBase
    {


        [Browsable(false)]
        [RuleFromBoolProperty("VRFromBooleanIsMailValidEmailRestorePasswordParameters", ValidationContext, CustomMessageTemplate = "Format email non valid!", SkipNullOrEmptyValues = true, UsedProperties = nameof(Email))]
        public bool IsEmailValid
        {
            get
            {
                System.ComponentModel.DataAnnotations.EmailAddressAttribute emailAddressValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                return emailAddressValidator.IsValid(Email);
            }
        }

        public override void ExecuteBusinessLogic(IObjectSpace objectSpace)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    throw new ArgumentException("Email doesn't set!");
                }

                ApplicationUser user = objectSpace.FindObject<ApplicationUser>(CriteriaOperator.Parse("Email = ? And IsActive = 1", Email));
                
                if (user == null)
                {
                    throw new ArgumentException("User not found!");
                }

                ChangePassword changePassword = objectSpace.CreateObject<ChangePassword>();
                changePassword.User = user.UserName;
                Guid token = Guid.NewGuid();
                changePassword.Token = token;
                
                objectSpace.CommitChanges();

                string urlApp = ""; // set https://miodominio/myapp


                string body = "A password reset was requested:<br>" +
                $"Click on the link {urlApp}/ChangePassword.aspx?tk={token} to reset it<br><br><br>" +
                "<strong>Warning: if the password reset request has not been made, ignore the message!</strong><br>" +
                "<strong>>Warning: the link is only available for 10 minutes then it is no longer valid!</strong>";

                string error = null;
                if (!SendEmail($"Recovery password", body, user.Email, ref error))
                {
                    throw new Exception(error);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool SendEmail(string v, string body, string email, ref string error)
        {
            // set your method to send email
            bool ok = false;
            try
            {

                ok = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return ok;
        }

        

    }


}
