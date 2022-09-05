using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Validation;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using RecoveryPasswordXAF.Module.BusinessObjects.Database;
using System;

namespace RecoveryPasswordXAF.Module.Security
{

    public class ManageUsersOnLogonController : ViewController<DetailView>
    {
        protected const string LogonActionParametersActiveKey = "Active for ILogonActionParameters only";
        public SimpleAction RestorePasswordAction { get; private set; }
        public SimpleAction AcceptLogonParametersAction { get; private set; }
        public SimpleAction CancelLogonParametersAction { get; private set; }
        public ManageUsersOnLogonController()
        {
            
            string defaultCategory = PredefinedCategory.PopupActions.ToString();
            
            RestorePasswordAction = CreateLogonParametersAction("RestorePassword", defaultCategory, "Reset password", "RestorePassword", "Reset password", typeof(RestorePasswordParameters));
            AcceptLogonParametersAction = new SimpleAction(this, "AcceptLogonParameters", defaultCategory, (s, e) =>
            {
                AcceptParameters(e.CurrentObject as LogonActionParametersBase);
            })
            { Caption = "OK" };

            CancelLogonParametersAction = new SimpleAction(this, "CancelLogonParameters", defaultCategory, (s, e) =>
            {
                CancelParameters(e.CurrentObject as LogonActionParametersBase);
            })
            { Caption = "Cancel" };
        }
        
        protected override void OnViewChanging(View view)
        {
            base.OnViewChanging(view);
            Active[ControllerActiveKey] = !SecuritySystem.IsAuthenticated;
        }
        
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            
            bool isLogonParametersActionView = (View != null) && (View.ObjectTypeInfo != null) && View.ObjectTypeInfo.Implements<LogonActionParametersBase>();
            LogonController lc = Frame.GetController<LogonController>();
            if (lc != null)
            {
                lc.AcceptAction.Active[LogonActionParametersActiveKey] = !isLogonParametersActionView;
                lc.CancelAction.Active[LogonActionParametersActiveKey] = !isLogonParametersActionView;
            }

            AcceptLogonParametersAction.Active[LogonActionParametersActiveKey] = isLogonParametersActionView;
            CancelLogonParametersAction.Active[LogonActionParametersActiveKey] = isLogonParametersActionView;
            
            RestorePasswordAction.Active[LogonActionParametersActiveKey] = !isLogonParametersActionView;
        }
        
        private SimpleAction CreateLogonParametersAction(string id, string category, string caption, string imageName, string toolTip, Type parametersType)
        {
            SimpleAction action = new SimpleAction(this, id, category)
            {
                Caption = caption,
                ImageName = imageName,
                PaintStyle = ActionItemPaintStyle.Image,
                ToolTip = toolTip
            };
            action.Execute += (s, e) => CreateParametersViewCore(e);
            action.Tag = parametersType;
            return action;
        }
        
        
        protected virtual void CreateParametersViewCore(SimpleActionExecuteEventArgs e)
        {
            ValidationModule module = Application.Modules.FindModule<ValidationModule>();
            if (module != null) module.InitializeRuleSet();
            
            Type parametersType = e.Action.Tag as Type;
            Guard.ArgumentNotNull(parametersType, "parametersType");
            object logonActionParameters = Activator.CreateInstance(parametersType);
            DetailView dv = Application.CreateDetailView(ObjectSpaceInMemory.CreateNew(), logonActionParameters);
            dv.ViewEditMode = ViewEditMode.Edit;
            Frame.SetView(dv);
        }
        protected virtual void AcceptParameters(LogonActionParametersBase parameters)
        {
            try
            {
                Guard.ArgumentNotNull(parameters, "parameters");

                Validator.RuleSet.Validate(null, parameters, LogonActionParametersBase.ValidationContext);

                DevExpress.ExpressApp.Xpo.XPObjectSpaceProvider directProvider = new DevExpress.ExpressApp.Xpo.XPObjectSpaceProvider(ConnectionHelper.ConnectionString, null);
                IObjectSpace directObjectSpace = directProvider.CreateObjectSpace();

                parameters.ExecuteBusinessLogic(directObjectSpace);
                CloseParametersView();
            }
            catch
            {
                throw;
            }
        }
        protected virtual void CancelParameters(LogonActionParametersBase parameters)
        {
            CloseParametersView();
        }
        protected virtual void CloseParametersView()
        {
            View.Close(false);
            Application.LogOff();
        }
    }
}