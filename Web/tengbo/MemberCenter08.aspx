<%@ Page Title="" Language="C#" MasterPageFile="~/MemberCenter.master" AutoEventWireup="true" Inherits="SitePage" %>

<%@ Register Src="~/MemberCenter08.ascx" TagPrefix="uc1" TagName="MemberCenter08" %>

<asp:Content runat="server" ContentPlaceHolderID="user_center_content"><uc1:MemberCenter08 runat="server" css_display="block" /></asp:Content>