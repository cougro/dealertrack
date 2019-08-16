<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CS.aspx.cs" Inherits="CS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            border: 1px solid #ccc;
            border-collapse: collapse;
            background-color: #fff;
        }
        table th
        {
            background-color: #ff7f00;
            color: #fff;
            font-weight: bold;
        }
        table th, table td
        {
            padding: 5px;
            border: 1px solid #ccc;
        }
        table, table table td
        {
            border: 0px solid #ccc;
        }
        .button {
    background-color: #0094ff; /* Blue */
    border: none;
    color: white;
    padding: 15px 32px;
    text-align: center;
    text-decoration: none;
    display: inline-block;
    font-size: 16px;
}

.content {
  max-width: 960px;
  margin: auto;
}
</style>
</head>
<body>
    <div class="content" >
    <img src="dealertrack.png" width="960px"/>
     <hr />
    <form id="form1" runat="server">
    <asp:FileUpload ID="FileUpload1"  CssClass="button"  runat="server" />
    <asp:Button ID="btnImport" CssClass="button" runat="server" Text="Import CSV File" OnClick="ImportCSV" accept=".csv" />
       
    <hr />
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    </form>
            
    </div>
</body>
</html>
