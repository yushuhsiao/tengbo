<%@ Page Title="" Language="C#" MasterPageFile="MemberCenter.master" AutoEventWireup="true" Inherits="SitePage" %>

<%@ Register Src="05.ascx" TagPrefix="uc1" TagName="MemberCenter05" %>

<asp:Content runat="server" ContentPlaceHolderID="user_center_content"><uc1:MemberCenter05 runat="server" css_display="block" /></asp:Content>