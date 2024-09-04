<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Pages_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WERP</title>
    <meta charset="UTF-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" type="text/css" href="../res/css/login.css" />
    <link rel="stylesheet" type="text/css" href="../res/css/util.css" />
</head>
<body>
    <form id="form1" runat="server">        
        <div class="limiter">
		<div >
			<div class="wrap-login100">
				<div class="login100-form validate-form p-l-55 p-r-55 p-t-178">
                    
                    <span class="login100-form-title">
						WERP Login
					</span>
					<div class="text-center" style="margin-top:-50px;"><img src="../res/images/AdaroLogo.jpg" width="180" height="105" /></div>

					<div class="wrap-input100 validate-input m-b-16" data-validate="Please enter username">						
                        <asp:TextBox runat="server" ID="tbUserName" CssClass="input100" placeholder="User Name" />                        
						<span class="focus-input100"></span>
					</div>

					<div class="wrap-input100 validate-input" data-validate = "Please enter password">						
                        <asp:TextBox runat="server" ID="tbPassword" CssClass="input100" TextMode="Password" placeholder="Password" />                        
						<span class="focus-input100"></span>
					</div>
                    
                    <div >
						<span class="txt1">
							<asp:CheckBox runat="server" ID="cbSave" Text=" Save User Name and Password" />
						</span>						
					</div>

                    <div class="text-right p-t-13 p-b-23">
						<span class="txt1">
							<asp:Label runat="server" ID="lblError" ForeColor="Red" Visible="false" />
						</span>						
					</div>
                    

					<div class="container-login100-form-btn">
                      <asp:Button runat="server" id="btnLogin" Text="Login" OnClick="btn_Click" CssClass="login100-form-btn"/>						
					</div>	
                    
                    <div class="flex-col-c p-t-170 p-b-40">
						<span class="txt1 p-b-9">
							
						</span>

						<a href="#" class="txt3">
							
						</a>
					</div>

				</div>
			</div>
		</div>
	</div>
    </form>
</body>
</html>
