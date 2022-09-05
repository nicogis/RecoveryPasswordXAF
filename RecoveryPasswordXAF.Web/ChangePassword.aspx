<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="RecoveryPasswordXAF.Web.ChangePassword" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
        html, body, form {
            height: 100%;
            margin: 0;
            padding: 0;
            overflow: hidden;
        }

        .centered {
            margin: 0 auto;
            position: relative;
            top: 30%;
        }
    </style>

    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        


        <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout" Width="500px" ClientInstanceName="FormLayout" CssClass="centered" EnableTheming="true" Theme="XafTheme">
            <Items>
                <dx:LayoutGroup Width="100%" Caption="Recovery password" ColumnCount="1" AlignItemCaptions="true">

                    <Items>
                        <dx:LayoutItem Caption="New password" VerticalAlign="Middle">

                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="NuovaPasswordTextBox" runat="server" Width="100%"  Password="true" OnValidation="NuovaPasswordTextBox_Validation" EnableTheming="true" Theme="XafTheme">
                                        <ValidationSettings ValidationGroup="gruppoValidazione" ValidateOnLeave="True" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="True" ErrorText="Required field" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Confirm new password" VerticalAlign="Middle">

                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="ConfermaNuovaPasswordTextBox" runat="server" Width="100%"  Password="true" OnValidation="ConfermaNuovaPasswordTextBox_Validation" EnableTheming="true" Theme="XafTheme">
                                        <ValidationSettings ValidationGroup="gruppoValidazione" ValidateOnLeave="True" ErrorDisplayMode="ImageWithTooltip">

                                            <RequiredField IsRequired="True" ErrorText="Required field" />

                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>






                        <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right" VerticalAlign="Middle" Paddings-PaddingTop="20px" CssClass="lastItem">

                            <Paddings PaddingTop="20px"></Paddings>

                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton runat="server" ID="ChangePasswordButton" Text="Confirm" AutoPostBack="true"  ValidationGroup="gruppoValidazione" OnClick="ChangePasswordButton_Click" EnableTheming="true" Theme="XafTheme">
                                        
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>

        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="popupControl" Height="83px" Modal="True" CloseAction="CloseButton" Width="300px" AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableTheming="true" Theme="XafTheme" HeaderText="Warning">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    
                        <dx:ASPxLabel runat="server" Width="250px" ClientInstanceName="lblPopup" ID="lblPopup" Wrap="True" Height ="100px" EnableTheming="true" Theme="XafTheme"></dx:ASPxLabel> 
                                
                                <br/><br/>
                    <table style="border:none">
                        <tr>
                            
                            <td style="align-content:center">
                                <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel"
                                    Text="OK" Width="80px" EnableTheming="true" Theme="XafTheme">
                                    <ClientSideEvents Click="function(s, e) {
	popupControl.Hide();
}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </form>
</body>
</html>
