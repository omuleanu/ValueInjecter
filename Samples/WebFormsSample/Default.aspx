<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsSample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <fieldset>
    <legend>Fields</legend>
    <p>
    <asp:Label ID="Label1" runat="server" Text="First Name"></asp:Label>
    <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
    </p>
    <p>
    <asp:Label ID="Label2" runat="server" Text="Last Name"></asp:Label>
    <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
    </p>
    <p>
    <asp:Label ID="Label3" runat="server" Text="Address"></asp:Label>
    <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>        
        
    </p>
        <p>
    <asp:Label ID="Label4" runat="server" Text="not in entity"></asp:Label>
    <asp:TextBox ID="SomeElse" runat="server"></asp:TextBox>        
        
    </p>
    <asp:Button ID="btnSumbit" runat="server" Text="Submit" onclick="btnSumbit_Click" />
    </fieldset>
    </div>
    </form>
    <p>
        Change the values and click submit</p>
</body>
</html>
