using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base.Security;
using DevExpress.Web;
using RecoveryPasswordXAF.Module.BusinessObjects.Database;
using System;
using System.Web;

namespace RecoveryPasswordXAF.Web
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void NuovaPasswordTextBox_Validation(object sender, ValidationEventArgs e)
        {
            ASPxTextBox txt = sender as ASPxTextBox;

            
            if ((!string.IsNullOrWhiteSpace(this.ConfermaNuovaPasswordTextBox.Text)) && (!string.IsNullOrWhiteSpace(txt.Text)))
            {
                if (string.Compare(this.ConfermaNuovaPasswordTextBox.Text, txt.Text,false) != 0)
                {
                    e.IsValid = false;
                    e.ErrorText = "New password and confirm new password are different!";
                    return;
                }
            }


            //e.IsValid = Helpers.CheckPassword(txt.Text); //method check complex password
            //e.ErrorText = "Password isn't complex!";

        }

        protected void ConfermaNuovaPasswordTextBox_Validation(object sender, ValidationEventArgs e)
        {
            ASPxTextBox txt = sender as ASPxTextBox;

            
            if ((!string.IsNullOrWhiteSpace(this.NuovaPasswordTextBox.Text)) && (!string.IsNullOrWhiteSpace(txt.Text)))
            {
                if (string.Compare(this.NuovaPasswordTextBox.Text, txt.Text, false) != 0)
                {
                    e.IsValid = false;
                    e.ErrorText = "New password and confirm new password are different!";
                    return;
                }
            }

            //e.IsValid = Helpers.CheckPassword(txt.Text); method check complex password
            //e.ErrorText = "Password isn't complex!";

        }

        protected void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            if (!(this.NuovaPasswordTextBox.IsValid && this.ConfermaNuovaPasswordTextBox.IsValid))
            {
                return;
            }


            try
            {
                string tk = HttpContext.Current.Request.Params["tk"];

                if (!string.IsNullOrEmpty(tk))
                {
                    if (Guid.TryParse(tk, out var tkGuid))
                    {


                        DevExpress.ExpressApp.Xpo.XPObjectSpaceProvider directProvider = new DevExpress.ExpressApp.Xpo.XPObjectSpaceProvider(ConnectionHelper.ConnectionString, null);
                        IObjectSpace objectSpace = directProvider.CreateObjectSpace();

                        Module.BusinessObjects.Database.ChangePassword changePassword = objectSpace.GetObjectByKey<Module.BusinessObjects.Database.ChangePassword>(tkGuid);
                        if (changePassword != null)
                        {
                            if (DateTime.Now.Subtract(changePassword.DateRequest).TotalMinutes < 10)
                            {
                                IAuthenticationStandardUser user = (IAuthenticationStandardUser)objectSpace.FindObject(WebApplication.Instance.Security.UserType, new BinaryOperator("UserName", changePassword.User));
                                if (user != null)
                                {
                                    user.SetPassword(this.NuovaPasswordTextBox.Text);
                                    user.ChangePasswordOnFirstLogon = false;
                                    objectSpace.CommitChanges();
                                    WebApplication.Redirect("Default.aspx");
                                }
                                else
                                {
                                    ASPxPopupControl1.ShowOnPageLoad = true;
                                    lblPopup.Text = "User not found!";
                                }
                            }
                            else
                            {
                                ASPxPopupControl1.ShowOnPageLoad = true;
                                lblPopup.Text = "The following link has expired: it is no longer possible to reset your password!";
                            }
                        }
                        else
                        {
                            ASPxPopupControl1.ShowOnPageLoad = true;
                            lblPopup.Text = "Page link not found: contact the administrator!";
                        }
                    }
                    else
                    {
                        ASPxPopupControl1.ShowOnPageLoad = true;
                        lblPopup.Text = "Error in the link to the page: contact the administrator!";
                    }
                }
                else
                {
                    ASPxPopupControl1.ShowOnPageLoad = true;
                    lblPopup.Text = "Invalid page link: contact administrator!";
                }
            }
            catch
            {
                ASPxPopupControl1.ShowOnPageLoad = true;
                lblPopup.Text = "Unexpected error: Please contact your administrator!";
            }
        }
    }
}